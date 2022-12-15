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
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MannsBlog.Config;
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
        /// <returns>
        /// True or false depending on sending email success.
        /// </returns>
        public async Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg)
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
