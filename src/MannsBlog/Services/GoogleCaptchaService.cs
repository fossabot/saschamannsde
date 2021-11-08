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

using MannsBlog.Config;
using MannsBlog.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MannsBlog.Services
{
    public class GoogleCaptchaService
    {
        private readonly ILogger<GoogleCaptchaService> _logger;
        private readonly IOptions<AppSettings> _settings;

        public GoogleCaptchaService(ILogger<GoogleCaptchaService> logger,
          IOptions<AppSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task<bool> Verify(string recaptcha)
        {
            using (var client = new System.Net.WebClient())
            {
                try
                {
                    // Enter your reCAPTCHA private key here
                    string secretKey = _settings.Value.Google.CaptchaSecret;
                    var gReply = client.DownloadString(string.Format(
                        "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                        secretKey, recaptcha));

                    var jsonReturned = JsonConvert.DeserializeObject<Recaptcha>(gReply);
                    return (jsonReturned.Success.ToLower() == "true");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
