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
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MannsBlog.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MannsBlog.Services
{
    /// <summary>
    /// MailService.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.IMailService" />
    public class OutlookMailService : IMailService
    {
        private readonly IHostEnvironment env;
        private readonly IOptions<AppSettings> settings;
        private readonly ILogger<OutlookMailService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookMailService"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        public OutlookMailService(
          IHostEnvironment env,
          IOptions<AppSettings> settings,
          ILogger<OutlookMailService> logger)
        {
            this.env = env;
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Sends the mail outlook asynchronous.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="attachment">The Attachement.</param>
        /// <returns>
        /// True or false depending on sending email success.
        /// </returns>
        public async Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg, [Optional] IFormFile attachment)
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

                var body = File.ReadAllText(path);
                this.logger.LogInformation($"Read Email Body");

                var credentials = new NetworkCredential(this.settings.Value.Outlook.Mailaddress, this.settings.Value.Outlook.Password);
                var formattedMessage = string.Format(body, email, name, subject, msg);
                var mail = new MailMessage()
                {
                    From = new MailAddress(this.settings.Value.Outlook.Mailaddress),
                    Subject = "Mail from Contact form",
                    Body = body,
                };

                mail.IsBodyHtml = true;

                if (attachment.Length > 0)
                {
                    string fileName = Path.GetFileName(attachment.ToString());
                    mail.Attachments.Add(new Attachment(fileName.ToString()));
                }

                mail.To.Add(new MailAddress(this.settings.Value.Outlook.Mailaddress));

                var client = new SmtpClient()
                {
                    UseDefaultCredentials = false,
                    Credentials = credentials,
                    Host = this.settings.Value.Outlook.SMTPServer,
                    Port = this.settings.Value.Outlook.SMTPPort,
                };
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception Thrown sending message via Outlook", ex);
            }

            return false;
        }
    }
}
