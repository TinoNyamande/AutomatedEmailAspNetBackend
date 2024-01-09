using EmailAPI.Data;
using EmailAPI.Models.ViewModels;
using EmailAPI.Models;
using EmailAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailAPI.Services.HtmlTemplates;

namespace EmailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvoiceBody _invoiceBody;
        private readonly PDFService _pdfService;
        private readonly IInvoiceEmail _invoiceEmail;
        private readonly IEmailService _emailService;
 
        public InvoiceController(ApplicationDbContext context,IInvoiceBody invoiceBody,PDFService pdfService,IInvoiceEmail invoiceEmail,IEmailService emailService)
        {
            _context = context;
            _invoiceBody = invoiceBody;
            _pdfService = pdfService;
            _invoiceEmail = invoiceEmail;
            _emailService = emailService;
        }
        [HttpPost("CalculateSubtotal")]
        public async Task<IActionResult> CalculateSubTotal ()
        {
            var invoice = await _context.InvoiceModel.Include(b=>b.Items).FirstOrDefaultAsync(a=>a.Status == "NEW");
            if (invoice == null)
            {
                return BadRequest(new
                {
                    message = "No new invoices"
                });
            }
            try
            {
                var invoiceItems = invoice.Items;
                double total = 0;
                foreach (var item in invoiceItems)
                {
                    var subTotal = (item.Price * item.Quantity);
                    item.Total = subTotal;
                    total += subTotal;
                    await _context.SaveChangesAsync();
                }
                invoice.Total = total;
                invoice.Status = "CALC";
                await _context.SaveChangesAsync();
                return Ok(invoiceItems);
            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex
                });
            }
            

        }

        [HttpPost("AddInvoice")]
        public async Task<IActionResult> AddInvoice (AddInvoiceModel invoiceVM)
        {
            try
            {
                InvoiceModel invoice = new InvoiceModel
                {
                    Id = Guid.NewGuid().ToString(),
                    FilePath = "",
                    HtmlContent = "",
                    InvoiceDate = DateTime.Now,
                    Status = "NEW",
                    CompanyAddress = invoiceVM.CompanyAddress,
                    CompanyName = invoiceVM.CompanyName,
                    CompanyEmail = invoiceVM.CompanyEmail,
                    CompanyPhone = invoiceVM.CompanyPhone,
                    CustomerAddress = invoiceVM.CustomerAddress,
                    CustomerEmail = invoiceVM.CustomerEmail,
                    CustomerName = invoiceVM.CustomerName,
                    CustomerPhone = invoiceVM.CustomerPhone,
                    PaymentMethod = invoiceVM.PaymentMethod,
                    Items = invoiceVM.Items


                };
                await _context.AddAsync(invoice);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Saved"
                });
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost("AddHtmlContent")]
        public async Task<IActionResult> AddHtmlContent()
        {
            var invoice = await _context.InvoiceModel.Include(a => a.Items).FirstOrDefaultAsync(b => b.Status == "CALC");
            if (invoice == null)
            {
                return BadRequest(new
                {
                    message = "No invoices to process"
                });
            }
            try
            {
                invoice.HtmlContent = _invoiceBody.GetHtmlTemplate(invoice);
                invoice.Status = "INDEXED";
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Saved"
                });

            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex
                });
            }


        }
        [HttpPost("GeneratePdf")]
        public async Task<IActionResult> GeneratePdf()
        {
            var invoice = await _context.InvoiceModel.FirstOrDefaultAsync(a => a.Status == "INDEXED");
            if (invoice == null)
            {
                return BadRequest(new
                {
                    message = "No data to process"
                });
            }
            try
            {
                var fileName = invoice.CustomerName + "_" + invoice.Id + "_" + DateTime.Now.ToString("yyyyMMddHHss") + ".pdf";
                var invoiceBytes = _pdfService.GeneratePdf(invoice.HtmlContent, fileName);
                var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                var filePath = Path.Combine(wwwRoot, fileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(invoiceBytes, 0, invoiceBytes.Length);
                };
                invoice.Status = "Email";
                invoice.FilePath = fileName;
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Saved"
                });

            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex
                });
            }
           }
        [HttpPost("SendInvoiceEmail")]
        public async Task<IActionResult> SendInvoiceEmail()
        {
            var invoice = _context.InvoiceModel.FirstOrDefault(a => a.Status == "Email");
            if (invoice == null)
            {
                return BadRequest(new
                {
                    message = "Table empty"
                });
            }
            var emailData = _invoiceEmail.GetEmailHtml(invoice.CustomerName, invoice.Id);

            MailData mailData = new MailData(invoice.CustomerEmail, "Invoice Update", emailData);
            //var filePath = "C:/Users/tnyamande/source/repos/EMS/EMS/wwwroot/Images/" + order.FilePath;
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            string filePath = Path.Combine(wwwRootPath, invoice.FilePath);
            try
            {
                await _emailService.sendEmailAsync(mailData, new CancellationToken(), filePath);
                invoice.Status = "Email_Sent";
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Email sent successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }

        }

        [HttpGet("GetInvoices")]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _context.InvoiceModel.ToListAsync();
            if (invoices == null)
            {
                return BadRequest(new
                {
                    message = "No data found"
                });
            }
            return Ok(invoices);
        }
        [HttpGet("GetInvoiceById")]
        public async Task<IActionResult> GetInvoiceById(string Id)
        {
            var invoice = await _context.InvoiceModel.Include(b=>b.Items).FirstOrDefaultAsync(a => a.Id == Id);
            if (invoice == null)
            {
                return BadRequest(new
                {
                    message = "Not Found"
                });
            }
            var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            var filePath = Path.Combine(wwwRoot, invoice.FilePath);
            var pdfBytes = System.IO.File.ReadAllBytes(filePath);
            var stream = new MemoryStream(pdfBytes);
            var myFile = File(pdfBytes, "application/pdf", invoice.FilePath);
            InvoiceViewModel invoiceVM = new InvoiceViewModel
            {
                CompanyAddress = invoice.CompanyAddress,
                CompanyEmail = invoice.CompanyEmail,
                CompanyName = invoice.CompanyName,
                CompanyPhone = invoice.CompanyPhone,
                CustomerAddress = invoice.CustomerAddress,
                CustomerEmail = invoice.CustomerEmail,
                CustomerName = invoice.CustomerName,
                CustomerPhone = invoice.CustomerPhone,
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                Items = invoice.Items,
                PaymentMethod = invoice.PaymentMethod,
                Status = invoice.Status,
                Total = invoice.Total,
                PdfFile = myFile
            };
            return Ok(invoiceVM);
        }

    }
}
