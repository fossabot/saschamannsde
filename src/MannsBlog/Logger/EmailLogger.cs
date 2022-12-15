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
using MannsBlog.Config;
using MannsBlog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MannsBlog.Logger
{
    /// <summary>
    /// Email Logging.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class EmailLogger : ILogger
    {
        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;
        private IMailService _mailService;
        private readonly IHttpContextAccessor _contextAccessor;
        private IOptions<AppSettings> _settings;

        public EmailLogger(string categoryName, Func<string, LogLevel, bool> filter, IMailService mailService, IHttpContextAccessor contextAccessor, IOptions<AppSettings> settings)
        {
            _categoryName = categoryName;
            _filter = filter;
            _mailService = mailService;
            _contextAccessor = contextAccessor;
            _settings = settings;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An <see cref="T:System.IDisposable" /> that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            // Not necessary
            return null!;
        }

        /// <summary>
        /// Happens if enabled.
        /// </summary>
        /// <param name="logLevel">LogLevel for apply.</param>
        /// <returns>Bool, if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || _filter(_categoryName, logLevel);
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <see cref="T:System.String" /> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        /// <exception cref="System.ArgumentNullException">formatter.</exception>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $@"<div>
  <h1>Error on MannsBlog</h1>
<p>Level: {logLevel}</p>
<p>{message}</p>";

            if (exception != null)
            {
                message += $"<p>{exception}</p>";
            }

            var url = UriHelper.GetEncodedPathAndQuery(_contextAccessor.HttpContext?.Request);
            message += $"<p>Request: {url}</p></div>";

            string user = _settings.Value.Blog.UserName;
            string email = _settings.Value.Blog.Email;

            _mailService.SendMailAsync("logmessage.txt", user, email, "[MannsBlog Log Message]", message).Wait();

        }
    }
}