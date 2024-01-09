using EmailAPI.Models;
using EmailAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace EmailAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]

    public class SendEmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IEmailBody _emailBody;
        private readonly PDFService _pdfService;
        public SendEmailController(IEmailService emailService, IEmailBody emailBody,PDFService pdfService)
        {
            _emailService = emailService;
            _emailBody = emailBody;
            this._pdfService = pdfService;

        }
        [HttpPost("TestEmail")]
        public async Task<IActionResult> TestEmail ()
        {
            string eBody = _emailBody.GetBody("Tino", 100000);
            MailData mailData = new MailData("tinotenda.nyamande4@gmail.com","Email testing", eBody, "tinotendanyamande0784@gmail.com", "Tinozz");
            string result = await _emailService.sendEmailAsync(mailData ,new CancellationToken(),"Test");
            return Ok(new
            {
                message = result
            }) ;
        }
        [HttpPost("TestPdf")]
        public async Task<IActionResult> TestPdf(string htmlContent)
        {
            try
            {
                var pdfBytes = _pdfService.GeneratePdf(htmlContent ,"Test");
                return File(pdfBytes, "application/pdf", "generated.pdf");

            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }

        }
        

        

    }
}
