using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Services
{
    /// <summary>
    /// Mailservice for logging.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.IMailService" />
    public class LoggingMailService : IMailService
    {
        private readonly ILogger<LoggingMailService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMailService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoggingMailService(ILogger<LoggingMailService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sends the mail sendgrid asynchronous.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns>
        /// True or false depending on sending email success.
        /// </returns>
        public Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg, [Optional] IFormFile attachment)
        {
            _logger.LogDebug($"Email Requested from {name} subject of {subject}");
            return Task.FromResult(true);
        }
    }
}