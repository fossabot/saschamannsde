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
using System.Diagnostics.CodeAnalysis;
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
    public class ContactModel : PageModel
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        [AllowNull]
        private IConfiguration _configuration;

        [AllowNull]
        private GoogleCaptchaService _captcha;

        [AllowNull]
        private IMailService _mailService;

        [AllowNull]
        private ILogger<ContactModel> _logger;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string? Message { get; set; }

        /// <summary>
        /// Indexes the model.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="captcha">The captcha.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mailService">The Mailing Service.</param>
        public void IndexModel(IConfiguration configuration, GoogleCaptchaService captcha, IMailService mailService, ILogger<ContactModel> logger)
        {
            _configuration = configuration;
            _captcha = captcha;
            _mailService = mailService;
            _logger = logger;
        }

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Called when [post submit].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task OnPostSubmit(ContactFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var spamState = VerifyNoSpam(model);
                    if (!spamState.Success)
                    {
                        _logger.LogError("Spam detected. Break submitting proces..");
                        this.Message = "Spam detected. Sorry";
                    }

                    // Captcha
                    if (await _captcha.Verify(model.Recaptcha))
                    {
                        this.Message = "Recaptcha solved. That are good news.";
                        if (await _mailService.SendMailAsync("ContactTemplate.txt", model.Name, model.Email, model.Subject, model.Body, model.Attachment))
                        {
                            _logger.LogInformation("Captcha verified. Sent mail.");
                            this.Message = "Email sent.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email from contact page", ex);
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
