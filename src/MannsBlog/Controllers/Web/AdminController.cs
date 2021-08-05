﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MannsBlog.Data;

namespace MannsBlog.Controllers.Web
{
  [Route("[controller]")]
  public class AdminController : Controller
  {
    [Route("changepwd")]
    public async Task<IActionResult> ChangePwd([FromServices] UserManager<MannsUser> userManager, 
      string username, 
      string oldPwd, 
      string newPwd)
    {
      var user = await userManager.FindByEmailAsync(username);
      if (user == null) return BadRequest(new { success = false });
      var result = await userManager.ChangePasswordAsync(user, oldPwd, newPwd);
      if (result.Succeeded) return Ok(new { success = true });
      else return BadRequest(new { success = false, errors = result.Errors });
    }
  }
}
