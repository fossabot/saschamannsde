using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MannsBlog.Models;
using MannsBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Controllers.Web
{
    /// <summary>
    /// Controller for all Contact-Forms related stuff.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class ContactController : Controller
    {
        private readonly IMailService _mailService;
        private readonly ILogger<ContactController> _logger;
        private readonly GoogleCaptchaService _captcha;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="mailService">The mail service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="captcha">The captcha.</param>
        public ContactController(
            IMailService mailService,
            ILogger<ContactController> logger,
            GoogleCaptchaService captcha)
        {
            _mailService = mailService;
            _logger = logger;
            _captcha = captcha;
        }

        /// <summary>
        /// Returns the Index.
        /// </summary>
        /// <returns>Contact View.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>Success or failue.</returns>
        /// <exception cref="ABI.System.Exception">The submission failed the spam bot verification.</exception>
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
                    if (!(await _captcha.Verify(form.Recaptcha)))
                    {
                        return BadRequest("Failed to send email: You might be a bot...try again later.");
                    }

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
        "link building software",
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
