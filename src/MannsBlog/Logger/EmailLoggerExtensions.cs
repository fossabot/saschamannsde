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
    /// Logger Extension.
    /// </summary>
    public static class EmailLoggerExtensions
    {
        /// <summary>
        /// Adds the email.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>factory.</returns>
        public static ILoggerFactory AddEmail(this ILoggerFactory factory,
                                              IMailService mailService,
                                              IHttpContextAccessor contextAccessor,
                                              IOptions<AppSettings> settings,
                                              Func<string, LogLevel, bool>? filter = null)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            factory.AddProvider(new EmailLoggerProvider(filter, mailService, contextAccessor, settings));
#pragma warning restore CS8604 // Possible null reference argument.
            return factory;
        }

        /// <summary>
        /// Adds the email.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        /// <param name="minLevel">The minimum level.</param>
        /// <returns>Add Email object.</returns>
        public static ILoggerFactory AddEmail(this ILoggerFactory factory,
          IMailService mailService,
          IHttpContextAccessor contextAccessor,
          IOptions<AppSettings> settings,
          LogLevel minLevel)
        {
            return AddEmail(
                factory,
                mailService,
                contextAccessor,
                settings,
                (_, logLevel) => logLevel >= minLevel);
        }
    }
}
