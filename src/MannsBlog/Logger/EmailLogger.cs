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