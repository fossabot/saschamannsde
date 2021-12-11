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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MannsBlog.Services
{
    public class EmailExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMailService _mailService;
        private readonly IHostEnvironment _env;
        private ILogger<EmailExceptionMiddleware> _logger;

        public EmailExceptionMiddleware(RequestDelegate next, IMailService mailService, IHostEnvironment env, ILogger<EmailExceptionMiddleware> logger)
        {
            _next = next;
            _mailService = mailService;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await _mailService.SendMailAsync("exceptionMessage.txt", "Sascha Manns", "Sascha.Manns@outlook.de", "[MannsBlog Exception]", ex.ToString());

                // Don't swallow the exception
                throw;
            }

        }
    }
}
