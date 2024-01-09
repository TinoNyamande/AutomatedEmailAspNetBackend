using Microsoft.AspNetCore.Mvc;
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
    public class ReceiptController : Controller
    {
            private readonly ApplicationDbContext _context;
            private readonly IReceiptBody _receiptBody;
            private readonly PDFService _pdfService;
            private readonly IReceiptEmail _receiptEmail;
            private readonly IEmailService _emailService;

            public ReceiptController(ApplicationDbContext context,IReceiptBody receiptBody,PDFService pdfService,IEmailService emailService,IReceiptEmail receiptEmail)
            {
                _context = context;
                _receiptBody = receiptBody;
                _pdfService = pdfService;
                _emailService = emailService;
               _receiptEmail = receiptEmail;
            }

            [HttpPost("AddReceipt")]
            public async Task<IActionResult> AddReceipt(AddReceiptVM receiptVM)
            {
                try
                {
                    Receipt receipt = new Receipt
                    {
                        Id = Guid.NewGuid().ToString(),
                        FilePath = "",
                        HtmlContent = "",
                        ReceiptDate = DateTime.Now,
                        Status = "NEW",
                        CompanyAddress = receiptVM.CompanyAddress,
                        CompanyName = receiptVM.CompanyName,
                        CompanyEmail = receiptVM.CompanyEmail,
                        CompanyPhone = receiptVM.CompanyPhone,
                        CustomerAddress = receiptVM.CustomerAddress,
                        CustomerEmail = receiptVM.CustomerEmail,
                        CustomerName = receiptVM.CustomerName,
                        CustomerPhone = receiptVM.CustomerPhone,
                        PaymentMethod = receiptVM.PaymentMethod,
                        Items = receiptVM.Items


                    };
                    await _context.AddAsync(receipt);
                    await _context.SaveChangesAsync();
                    return Ok(new
                    {
                        message = "Saved"
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        message = ex
                    });
                }
            }
        [HttpPost("CalculateSubtotal")]
        public async Task<IActionResult> CalculateSubTotal()
        {
            var receipt = await _context.Receipts.Include(b => b.Items).FirstOrDefaultAsync(a => a.Status == "NEW");
            if (receipt == null)
            {
                return BadRequest(new
                {
                    message = "No new receipt"
                });
            }
            try
            {
                var receiptItems = receipt.Items;
                double total = 0;
                foreach (var item in receiptItems)
                {
                    var subTotal = (item.Price * item.Quantity);
                    item.Total = subTotal;
                    total += subTotal;
                    await _context.SaveChangesAsync();
                }
               receipt.Total = total;
                receipt.Status = "CALC";
                await _context.SaveChangesAsync();
                return Ok(receiptItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex
                });
            }


        }
        [HttpPost("AddHtmlContent")]
        public async Task<IActionResult> AddHtmlContent()
        {
            var receipt = await _context.Receipts.Include(a => a.Items).FirstOrDefaultAsync(b => b.Status == "CALC");
            if (receipt == null)
            {
                return BadRequest(new
                {
                    message = "No receipts to process"
                });
            }
            try
            {
                receipt.HtmlContent = _receiptBody.GetHtmlString(receipt);
                receipt.Status = "INDEXED";
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Saved"
                });

            }
            catch (Exception ex)
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
            var receipt = await _context.Receipts.FirstOrDefaultAsync(a => a.Status == "INDEXED");
            if (receipt == null)
            {
                return BadRequest(new
                {
                    message = "No data to process"
                });
            }
            try
            {
                var fileName = receipt.CustomerName + "_" + receipt.Id + "_" + DateTime.Now.ToString("yyyyMMddHHss") + ".pdf";
                var invoiceBytes = _pdfService.GeneratePdf(receipt.HtmlContent, fileName);
                var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                var filePath = Path.Combine(wwwRoot, fileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(invoiceBytes, 0, invoiceBytes.Length);
                };
                receipt.Status = "Email";
                receipt.FilePath = fileName;
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Saved"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex
                });
            }
        }
        [HttpPost("SendReceiptEmail")]
        public async Task<IActionResult> SendReceiptEmail()
        {
            var receipt = _context.Receipts.FirstOrDefault(a => a.Status == "Email");
            if (receipt == null)
            {
                return BadRequest(new
                {
                    message = "Table empty"
                });
            }
            var emailData = _receiptEmail.GetEmailHtml( receipt.CustomerName, receipt.Id);

            MailData mailData = new MailData(receipt.CustomerEmail, "Receipt Update", emailData);
            //var filePath = "C:/Users/tnyamande/source/repos/EMS/EMS/wwwroot/Images/" + order.FilePath;
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            string filePath = Path.Combine(wwwRootPath, receipt.FilePath);
            try
            {
                await _emailService.sendEmailAsync(mailData, new CancellationToken(), filePath);
                receipt.Status = "Email_Sent";
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
        [HttpGet("GetReceipts")]
        public async Task<IActionResult> GetReceipts()
        {
            var receipts = await _context.Receipts.ToListAsync();
            if (receipts == null)
            {
                return BadRequest(new
                {
                    message = "No data found"
                });
            }
            return Ok(receipts);
        }
        [HttpGet("GetReceiptById")]
        public async Task<IActionResult> GetReceiptById(string Id)
        {
            var receipt = await _context.Receipts.Include(b=>b.Items).FirstOrDefaultAsync(a => a.Id == Id);
            if (receipt == null)
            {
                return BadRequest(new
                {
                    message = "Not Found"
                });
            }
            var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            var filePath = Path.Combine(wwwRoot, receipt.FilePath);
            var pdfBytes = System.IO.File.ReadAllBytes(filePath);
            var stream = new MemoryStream(pdfBytes);
            var myFile = File(pdfBytes, "application/pdf", receipt.FilePath);
            ReceiptViewModel receiptVM = new ReceiptViewModel
            {
                CompanyAddress = receipt.CompanyAddress,
                CompanyEmail = receipt.CompanyEmail,
                CompanyName = receipt.CompanyName,
                CompanyPhone = receipt.CompanyPhone,
                CustomerAddress = receipt.CustomerAddress,
                CustomerEmail = receipt.CustomerEmail,
                CustomerName = receipt.CustomerName,
                CustomerPhone = receipt.CustomerPhone,
                Id = receipt.Id,
                Total = receipt.Total,
                Items = receipt.Items,
                ReceiptDate = receipt.ReceiptDate,
                PaymentMethod = receipt.PaymentMethod,
                Status = receipt.Status,
                PdfFile = myFile
            };
            return Ok(receiptVM);
        }


    }
}
