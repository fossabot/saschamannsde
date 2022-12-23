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
