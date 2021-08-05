﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using MannsBlog.Config;

namespace MannsBlog.Services
{
  public class MailService : IMailService
  {
    private readonly IHostEnvironment _env;
    private readonly IOptions<AppSettings> _settings;
    private readonly ILogger<MailService> _logger;

    public MailService(IHostEnvironment env,
      IOptions<AppSettings> settings,
      ILogger<MailService> logger)
    {
      _env = env;
      _settings = settings;
      _logger = logger;
    }

    public async Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg)
    {
      try
      {
        var path = Path.Combine(_env.ContentRootPath, "EmailTemplates", template);
        if (!File.Exists(path))
        {
          _logger.LogError($"Cannot find email templates: {path}");
          if (Directory.Exists(Path.Combine(_env.ContentRootPath, "EmailTemplates")))
          {
            _logger.LogError($"File doesn't exist but directory for templates does");
          }

          return false;
        }

        var body = await File.ReadAllTextAsync(path);
        _logger.LogInformation($"Read Email Body");

        var key = _settings.Value.MailService.ApiKey;

        var client = new SendGridClient(key);
        var formattedMessage = string.Format(body, email, name, subject, msg);

        var mailMsg = MailHelper.CreateSingleEmail(
          new EmailAddress(_settings.Value.MailService.Receiver),
          new EmailAddress(_settings.Value.MailService.Receiver),
          $"saschamanns.com Site Mail",
          formattedMessage,
          formattedMessage);

        _logger.LogInformation("Attempting to send mail via SendGrid");
        var response = await client.SendEmailAsync(mailMsg);
        _logger.LogInformation("Received response from SendGrid");

        if (response.StatusCode >= System.Net.HttpStatusCode.PartialContent) // Not 200 or 202
        {
          var result = await response.Body.ReadAsStringAsync();
          _logger.LogError($"Failed to send message via SendGrid: Status Code: {response.StatusCode}");
        }
        else
        {
          return true;
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