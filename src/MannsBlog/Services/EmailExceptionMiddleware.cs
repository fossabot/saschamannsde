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
