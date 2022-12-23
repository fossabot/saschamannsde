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
