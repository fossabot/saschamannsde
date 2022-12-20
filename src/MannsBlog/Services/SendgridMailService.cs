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
