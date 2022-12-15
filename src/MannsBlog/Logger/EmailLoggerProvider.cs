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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MannsBlog.Logger
{
    /// <summary>
    /// Logging Provider.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    public class EmailLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<AppSettings> _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailLoggerProvider"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        /// <param name="settings">The settings.</param>
        public EmailLoggerProvider(Func<string, LogLevel, bool> filter, IMailService mailService, IHttpContextAccessor contextAccessor, IOptions<AppSettings> settings)
        {
            _mailService = mailService;
            _contextAccessor = contextAccessor;
            _filter = filter;
            _settings = settings;
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>
        /// The instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.
        /// </returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new EmailLogger(categoryName, _filter, _mailService, _contextAccessor, _settings);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
