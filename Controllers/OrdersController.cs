using EmailAPI.Data;
using EmailAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using EmailAPI.Models.ViewModels;

namespace EmailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderEmailBody _orderEmailBody;
        private readonly PDFService _pdfService;
        private readonly IOrderMainEmailBody _orderMainEmailBody;
        private readonly IEmailService _emailService;
        public OrdersController(ApplicationDbContext context , IOrderEmailBody orderEmailBody,PDFService pDFService,
            IOrderMainEmailBody orderMainEmailBody, IEmailService emailService)
        {
            _context = context;
            _orderEmailBody = orderEmailBody;
            _pdfService = pDFService;
            _orderMainEmailBody = orderMainEmailBody;
            _emailService = emailService;
        }
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(OrderVM orderVM)
        {
            Order order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                CustomerName = orderVM.CustomerName,
                PlacedOn = orderVM.PlacedOn,
                OrderStatus = orderVM.OrderStatus,
                PickUpLocation = orderVM.PickUpLocation,
                PaymentMethod = orderVM.PaymentMethod,
                ShippingMethod = orderVM.ShippingMethod,
                Status = "NEW",
                FilePath = "",
                CustomerEmail = orderVM.CustomerEmail,
                CompanyAddress = orderVM.CompanyAddress,
                CompanyEmail = orderVM.CompanyEmail,
                CompanyName = orderVM.CompanyName,
                CompanyPhone = orderVM.CompanyPhone

            };
            try
            {
                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Order added successfully"
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
        [HttpPost("AddHtmlContent")]
        public async  Task<IActionResult> AddHtmlContent()
        {
            var order = _context.Orders.FirstOrDefault(a => a.Status == "NEW");
            var htmlContent = _orderEmailBody.GetHtmlString(order);
            try
            {
                order.HtmlContent = htmlContent;
                order.Status = "INDEXED";
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Record Updated"
                });
            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }

 
        }
        [HttpPost("GenerateNewPdf")]
        public async Task<IActionResult> GenerateNewPdf()
        {
            var order = _context.Orders.FirstOrDefault(a => a.Status == "INDEXED");
            try
            {
                var orderFileName = order.CustomerName + "_" + order.Id+"_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".pdf";
                var orderPdfBytes = _pdfService.GeneratePdf(order.HtmlContent,orderFileName);
                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                string filePath = Path.Combine(wwwRootPath, orderFileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.Write(orderPdfBytes, 0, orderPdfBytes.Length);
                }
                order.Status = "Email";
                order.FilePath = orderFileName;
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Record updated"
                });
            }catch(Exception ex)
            {
                return BadRequest (new
                {
                    message = ex.Message
                });
            }

        }
        [HttpPost("SendOrderEmail")]
        public async Task<IActionResult> SendOrderEmail()
        {
            var order =  _context.Orders.FirstOrDefault(a => a.Status == "Email");
            if (order == null)
            {
                return BadRequest(new
                {
                    message = "Table empty"
                });
            }
            var emailData = _orderMainEmailBody.GetEmailHtml(order.CustomerName, order.Id);

            MailData mailData = new MailData(order.CustomerEmail, "Order Update", emailData);
            //var filePath = "C:/Users/tnyamande/source/repos/EMS/EMS/wwwroot/Images/" + order.FilePath;
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
            string filePath = Path.Combine(wwwRootPath, order.FilePath);
            try
            {
                await _emailService.sendEmailAsync(mailData, new CancellationToken(), filePath);
                order.Status = "Email_Sent";
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Email sent successfully"
                });
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }

        }
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders =  await _context.Orders.Where(a=>a.Status == "EMAIL_SENT").ToListAsync();
            if (orders == null)
            {
                return BadRequest(new
                {
                    message = "No orders in the database"
                });
            }
            return Ok(orders);
        }
        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrder (string Id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == Id);
            if (order == null)
            {
                return BadRequest(new
                {
                    message = "Order not found"
                });
            }
            string wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/");
            string filePath = wwwRoot + order.FilePath;
            var pdfBytes = System.IO.File.ReadAllBytes(filePath);
            var stream = new MemoryStream(pdfBytes);
            var myFile = File(pdfBytes, "application/pdf", order.FilePath);
            OrderViewModel orderVm = new OrderViewModel
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                PlacedOn = order.PlacedOn,
                OrderStatus = order.OrderStatus,
                PickUpLocation = order.PickUpLocation,
                PaymentMethod = order.PaymentMethod,
                ShippingMethod = order.ShippingMethod,
                Status = order.Status,
                PdfFile = myFile

            };
            return Ok(orderVm);
        }

    }
}
