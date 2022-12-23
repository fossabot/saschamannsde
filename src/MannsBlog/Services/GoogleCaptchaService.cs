// MIT License
//
// Copyright (c) 2022 Sascha Manns
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MannsBlog.Config;
using MannsBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MannsBlog.Services
{
    /// <summary>
    /// Service for Googles Recaptcha.
    /// </summary>
    public class GoogleCaptchaService
    {
        private readonly ILogger<GoogleCaptchaService> _logger;
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpContextAccessor _ctxAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleCaptchaService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="ctxAccessor">The CTX accessor.</param>
        public GoogleCaptchaService(
          ILogger<GoogleCaptchaService> logger,
          IOptions<AppSettings> settings,
          IHttpContextAccessor ctxAccessor)
        {
            _logger = logger;
            _settings = settings;
            _ctxAccessor = ctxAccessor;
        }

        /// <summary>
        /// Verifies the specified recaptcha.
        /// </summary>
        /// <param name="recaptcha">The recaptcha.</param>
        /// <returns>True or False.</returns>
        public async Task<bool> Verify(string recaptcha)
        {
            var uri = "https://www.google.com/recaptcha/api/siteverify";
            var request = _ctxAccessor.HttpContext?.Request;

            if (request is not null)
            {

                // make the api call and determine validity
                using (var client = new HttpClient())
                {
#pragma warning disable SA1118 // Parameter should not span multiple lines
                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string?, string?>("secret", _settings.Value.Google.CaptchaSecret),
                         new KeyValuePair<string?, string?>("response", recaptcha),
                         new KeyValuePair<string?, string?>("remoteip", request.Headers.ContainsKey("HTTP_X_FORWARDED_FOR") ?
                             request.Headers["HTTP_X_FORWARDED_FOR"].FirstOrDefault() :
                             _ctxAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()),
                    });
#pragma warning restore SA1118 // Parameter should not span multiple lines

                    var result = await client.PostAsync(uri, content);

                    if (result.IsSuccessStatusCode)
                    {
                        var json = await result.Content.ReadAsStringAsync();
                        var verifyResponse = JsonConvert.DeserializeObject<SiteVerifyResult>(json);
                        if (verifyResponse.Success)
                        {
                            _logger.LogInformation("Verifying Google Recaptcha was successful");
                            return true;
                        }
                    }

                    _logger.LogInformation("Verifying Google Recaptcha was failed");
                    return false;
                }
            }

            return false;
        }
    }
}
