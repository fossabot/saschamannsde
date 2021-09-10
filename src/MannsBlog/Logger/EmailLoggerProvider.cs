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
using Microsoft.Extensions.Logging;
using MannsBlog.Services;

namespace MannsBlog.Logger
{
  public class EmailLoggerProvider : ILoggerProvider
  {
    private readonly Func<string, LogLevel, bool> _filter;
    private readonly IMailService _mailService;
    private readonly IHttpContextAccessor _contextAccessor;

    public EmailLoggerProvider(Func<string, LogLevel, bool> filter, IMailService mailService, IHttpContextAccessor contextAccessor)
    {
      _mailService = mailService;
      _contextAccessor = contextAccessor;
      _filter = filter;
    }

    public ILogger CreateLogger(string categoryName)
    {
      return new EmailLogger(categoryName, _filter, _mailService, _contextAccessor);
    }

    public void Dispose()
    {
    }
  }
}
