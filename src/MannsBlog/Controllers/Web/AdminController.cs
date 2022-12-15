﻿// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
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
using System.Threading.Tasks;
using MannsBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MannsBlog.Controllers.Web
{
    /// <summary>
    /// Controller for standard Admin tasks.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="username">The username.</param>
        /// <param name="oldPwd">The old password.</param>
        /// <param name="newPwd">The new password.</param>
        /// <returns>OK or BadRequest.</returns>
        [Route("changepwd")]
        public async Task<IActionResult> ChangePwd(
          [FromServices] UserManager<MannsUser> userManager,
          string username,
          string oldPwd,
          string newPwd)
        {
            var user = await userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return this.BadRequest(new { success = false });
            }

            var result = await userManager.ChangePasswordAsync(user, oldPwd, newPwd);
            if (result.Succeeded)
            {
                return this.Ok(new { success = true });
            }
            else
            {
                return this.BadRequest(new { success = false, errors = result.Errors });
            }
        }
    }
}
