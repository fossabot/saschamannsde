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
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MannsBlog.Models;
using MannsBlog.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Views.Root
{
    /// <summary>
    /// Page Model for the Contact Page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class Contact : PageModel
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string? Message { get; set; }

        private GoogleCaptchaService Captcha;

        private IMailService MailService;

        private ILogger<Contact> Logger;

        /// <summary>
        /// The configuration.
        /// </summary>
        private IConfiguration Configuration;

        /// <summary>
        /// Indexes the model.
        /// </summary>
        /// <param name="_configuration">The configuration.</param>
        public void IndexModel(IConfiguration _configuration, GoogleCaptchaService _captcha, IMailService _mailService, ILogger<Contact> _logger)
        {
            Configuration = _configuration;
            Captcha = _captcha;
            MailService = _mailService;
            Logger = _logger;
        }

        public void OnGet()
        {
        }

        public async Task OnPostSubmit(ContactFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var spamState = VerifyNoSpam(model);
                    if (!spamState.Success)
                    {
                        Logger.LogError("Spam detected. Break submitting proces..");
                        Message = "Spam detected. Sorry";
                    }

                    // Captcha
                    if (await Captcha.Verify(model.Recaptcha))
                    {
                        Message = "Recaptcha solved. That are good news.";
                        if (await MailService.SendMailAsync("ContactTemplate.txt", model.Name, model.Email, model.Subject, model.Body, model.Attachment))
                        {
                            Logger.LogInformation("Captcha verified. Sent mail.");
                            Message = "Email sent.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to send email from contact page", ex);
            }
        }

        // Brute Force getting rid of my worst emails
        private SpamState VerifyNoSpam(ContactFormModel model)
        {
            var tests = new string[]
            {
        "improve your seo",
        "improved seo",
        "generate leads",
        "viagra",
        "your team",
        "PHP Developers",
        "working remotely",
        "google search results",
        "link building software",
            };

            if (tests.Any(t =>
            {
                return new Regex(t, RegexOptions.IgnoreCase).Match(model.Body).Success;
            }))
            {
                return new SpamState() { Reason = "Spam Email Detected. Sorry." };
            }

            return new SpamState() { Success = true };
        }
    }
}
