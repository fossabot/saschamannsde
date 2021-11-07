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
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MannsBlog.Config;
using MannsBlog.Models;

namespace MannsBlog.Services
{
    public class MailService : IMailService
    {        
        private readonly ILogger<MailService> _logger;
        private readonly EmailServerConfiguration _eConfig;
        private readonly IHostEnvironment _env;        

        public MailService(ILogger<MailService> logger,
          EmailServerConfiguration config,
          IHostEnvironment env)
        {
            _logger = logger;
            _eConfig = config;
            _env = env;            
        }

        public async Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg)
        {
            try
            {                                
                _logger.LogInformation("Use SMTP");

                var path = Path.Combine(_env.ContentRootPath, "EmailTemplates", template);

                if (!File.Exists(path))
                {
                    _logger.LogError($"Cannot find email templates: {path}");
                    if (Directory.Exists(Path.Combine(_env.ContentRootPath, "EmailTemplates")))
                    {
                        _logger.LogError($"File doesn't exist but directory for templates does");
                    }
                }

                var templatebody = await File.ReadAllTextAsync(path);
                var templateout = String.Empty;
                var contentpart = String.Empty;
                
                if (template == "ContactTemplate.txt")
                {
                    templateout = string.Format(templatebody, email, name, subject, msg);
                    contentpart = "html";
                } 
                else if (template == "exceptionMessage.txt" || template == "logmessage.txt")
                { 
                    templateout = string.Format(templatebody, name, email, subject, msg);
                    contentpart = "text";
                }
                else
                {
                    templateout = string.Format(templatebody, name, email, subject, msg);
                    contentpart = "text";
                }

                // "logmessage.txt", "Sascha Manns", "Sascha.Manns@outlook.de", "[MannsBlog Exception]", message

                _logger.LogInformation($"Read Email Body");                

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sascha Manns", "Sascha.Manns@outlook.de"));
                message.To.Add(new MailboxAddress("Sascha Manns", "Sascha.Manns@outlook.de"));                

                message.Subject = "[MannsBlog Message]" + subject;

                message.Body = new TextPart(contentpart)
                {
                    Text = templateout
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_eConfig.SmtpServer, _eConfig.SmtpPort);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

                    _logger.LogInformation("Attempting to send mail via SMTP");

                    client.Send(message);

                    _logger.LogInformation("Received response from SMTP");

                    client.Disconnect(true);
                }              
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Thrown sending message via SendGrid", ex);
            }
            return false;
        }
    }
}