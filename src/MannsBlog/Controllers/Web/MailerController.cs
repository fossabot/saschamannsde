using MannsBlog.Models;
using MannsBlog.Services;
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
        private readonly ILogger<MailerController> _logger;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var spamState = VerifyNoSpam(form);
                    if (!spamState.Success)
                    {
                        _logger.LogError("Spamstate wasn't succeeded");
                        return BadRequest(new { Reason = spamState.Reason });
                    }

                    // Captcha
                    if (!(await _captcha.Verify(form.Recaptcha))) return BadRequest("Failed to send email: You might be a bot...try again later.");
                    if (await _mailService.SendMailAsync("ContactTemplate.txt", form.Name, form.Email, form.Subject, form.Message))
                    {
                        return Json(new { success = true, message = "Your message was successfully sent." });
                    }
                }
                _logger.LogError("Modelstate wasnt valid");
                return Json(new { success = false, message = "ModelState wasnt valid..." });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email from contact page", ex.Message);
                return Json(new { success = false, message = ex.Message });
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
    }
}
