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
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MannsBlog.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MannsBlog.Services
{
    /// <summary>
    /// MailService.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.IMailService" />
    public class SendgridMailService : IMailService
    {
        private readonly IHostEnvironment env;
        private readonly IOptions<AppSettings> settings;
        private readonly ILogger<SendgridMailService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendgridMailService"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        public SendgridMailService(
          IHostEnvironment env,
          IOptions<AppSettings> settings,
          ILogger<SendgridMailService> logger)
        {
            this.env = env;
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Sends the mail sendgrid asynchronous.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="attachement">The Attachment.</param>
        /// <returns>
        /// True or false depending on sending email success.
        /// </returns>
        public async Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg, [Optional] IFormFile attachement)
        {
            try
            {
                var path = Path.Combine(this.env.ContentRootPath, "EmailTemplates", template);
                if (!File.Exists(path))
                {
                    this.logger.LogError($"Cannot find email templates: {path}");
                    if (Directory.Exists(Path.Combine(this.env.ContentRootPath, "EmailTemplates")))
                    {
                        this.logger.LogError($"File doesn't exist but directory for templates does");
                    }

                    return false;
                }

                var body = await File.ReadAllTextAsync(path);
                this.logger.LogInformation($"Read Email Body");

                var key = this.settings.Value.MailService.ApiKey;

                var client = new SendGridClient(key);
                var formattedMessage = string.Format(body, email, name, subject, msg);

                var mailMsg = MailHelper.CreateSingleEmail(
                  new EmailAddress(this.settings.Value.MailService.Receiver),
                  new EmailAddress(this.settings.Value.MailService.Receiver),
                  $"saschamanns.de Site Mail",
                  formattedMessage,
                  formattedMessage);

                if (attachement.Length > 0)
                {
                    //using (FileStream fileStream = File.OpenRead(attachement))
                    //{
                    //    await mailMsg.AddAttachmentAsync(attachement, fileStream);
                    //}
                }

                this.logger.LogInformation("Attempting to send mail via SendGrid");
                var response = await client.SendEmailAsync(mailMsg);

                this.logger.LogInformation("Received response from SendGrid");

                // Not 200 or 202
                if (response.StatusCode >= System.Net.HttpStatusCode.PartialContent)
                {
                    var result = await response.Body.ReadAsStringAsync();
                    this.logger.LogError($"Failed to send message via SendGrid: Status Code: {response.StatusCode}");
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception Thrown sending message via SendGrid", ex);
            }

            return false;
        }
    }
}
