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
using System.Threading.Tasks;
using MannsBlog.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MannsBlog.Services
{
    /// <summary>
    /// Middleware for Email Exceptions.
    /// </summary>
    public class EmailExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMailService _mailService;
        private readonly IHostEnvironment _env;
        private ILogger<EmailExceptionMiddleware> _logger;
        private IOptions<AppSettings> _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="env">The env.</param>
        /// <param name="logger">The logger.</param>
        public EmailExceptionMiddleware(RequestDelegate next, IMailService mailService, IHostEnvironment env, ILogger<EmailExceptionMiddleware> logger, IOptions<AppSettings> settings)
        {
            _next = next;
            _mailService = mailService;
            _env = env;
            _logger = logger;
            _settings = settings;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The new Invoke, or if its failed, it sends a error message via mail.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                string user = _settings.Value.Blog.UserName;
                string email = _settings.Value.Blog.Email;

                await _mailService.SendMailAsync("exceptionMessage.txt", user, email, "[MannsBlog Exception]", ex.ToString());

                // Don't swallow the exception
                throw;
            }
        }
    }
}
