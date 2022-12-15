// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string?, string?>("secret", _settings.Value.Google.CaptchaSecret),
                         new KeyValuePair<string?, string?>("response", recaptcha),
                         new KeyValuePair<string?, string?>("remoteip", request.Headers.ContainsKey("HTTP_X_FORWARDED_FOR") ?
                             request.Headers["HTTP_X_FORWARDED_FOR"].FirstOrDefault() :
                             _ctxAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()),
                    });

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
