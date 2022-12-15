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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Filters
{
    /// <summary>
    /// My Exceptionfilter.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute" />
    public class MannsExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MannsExceptionFilter"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public MannsExceptionFilter(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Method for case of Exception.
        /// </summary>
        /// <param name="context">Exception Context.</param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return;
            }
        }
    }
}
