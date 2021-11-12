using MannsBlog.Models;
using MannsBlog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MannsBlog.Controllers.Web
{
    [Route("[controller]")]
    public class MailerController : Controller
    {
        private readonly IMailService _mailService;
        private ILogger<MailerController> _logger;
        private readonly GoogleCaptchaService _captcha;

        public MailerController(IMailService mailService,
            ILogger<MailerController> logger,
            GoogleCaptchaService captcha)
        {
            _mailService = mailService;
            _logger = logger;
            _captcha = captcha;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ContactFormModel form)
        {
            if (form == null) return BadRequest("Form wasn't filled.");
            _logger.LogInformation("Form was filled.");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model was valid.");
                try
                {
                    var spamState = VerifyNoSpam(form);
                    if (!spamState.Success)
                    {
                        return BadRequest(new { Reason = spamState.Reason });
                    }
                    _logger.LogInformation("SpamState passed.");

                    if (!(await _captcha.Verify(form.Recaptcha)))
                    {
                        _logger.LogInformation("Submission failed.");
                        throw new Exception(
                            "The submission failed the spam bot verification. If you have JavaScript disabled in your browser, please enable it and try again.");
                    }
                    else
                    {
                        _logger.LogInformation("Normal execution. Launching SendGrid.");
                        await _mailService.SendMailAsync("ContactTemplate.txt", form.Name, form.Email, form.Subject,
                            form.Message);
                    }

                    return Json(new { success = true, message = "Your message was successfully sent" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return BadRequest("The form contains corrupted data... Please retry.");
            }
        }

        // Brute Force getting rid of my worst emails
        private SpamState VerifyNoSpam(ContactFormModel model)
        {
            var tests = new string[]
            {
        "improve your seo",
        "improved seo",
        "generate leads",
        "viagra",
        "your team",
        "PHP Developers",
        "working remotely",
        "google search results",
        "link building software"
            };

            if (tests.Any(t =>
            {
                return new Regex(t, RegexOptions.IgnoreCase).Match(model.Message).Success;
            }))
            {
                return new SpamState() { Reason = "Spam Email Detected. Sorry." };
            }
            return new SpamState() { Success = true };
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = System.DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
    }
}
