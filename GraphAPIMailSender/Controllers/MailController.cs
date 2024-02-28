using GraphAPIMailSender.Interfaces;
using GraphAPIMailSender.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraphAPIMailSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        public IMailSender _mailSender;
        private readonly ILogger<MailController> _logger;
        public MailController(IMailSender mailSender, ILogger<MailController> logger)
        {
            _mailSender = mailSender;
            _logger = logger;
        }

        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> Send([FromBody] MailSenderDetails mailSenderDetails)
        {
            try
            {
                _logger.LogInformation("API started at:" + DateTime.Now);
                await _mailSender.SendEmail(mailSenderDetails);
                return Ok("Email Sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
