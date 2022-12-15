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
using MannsBlog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Logger
{
    public class EmailLogger : ILogger
    {
        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;
        private IMailService _mailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmailLogger(string categoryName, Func<string, LogLevel, bool> filter, IMailService mailService, IHttpContextAccessor contextAccessor)
        {
            _categoryName = categoryName;
            _filter = filter;
            _mailService = mailService;
            _contextAccessor = contextAccessor;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // Not necessary
            return null!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

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


            _mailService.SendMailAsync("logmessage.txt", "Sascha Manns", "Sascha.Manns@outlook.de", "[MannsBlog Log Message]", message).Wait();

        }
    }
}