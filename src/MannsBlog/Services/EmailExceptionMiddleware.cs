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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MannsBlog.Services
{
  public class EmailExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IMailService _mailService;

    public EmailExceptionMiddleware(RequestDelegate next, IMailService mailService)
    {
      _next = next;
      _mailService = mailService;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next.Invoke(context);
      }
      catch (Exception ex)
      {
        await _mailService.SendMailAsync("exceptionmessage.txt", "Sascha Manns", "Sascha.Manns@outlook.de", "[MannsBlog Exception]", ex.ToString());

        // Don't swallow the exception
        throw;
      }

    }
  }
}
