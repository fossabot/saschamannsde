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
        public string? Message { get; set; } = "Launching Emailprocess";

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
