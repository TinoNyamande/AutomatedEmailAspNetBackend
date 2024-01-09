using Microsoft.AspNetCore.Mvc;

namespace EmailAPI.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public string? Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public List<InvoiceItemsModel>? Items { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public double? Total { get; set; }

        public FileContentResult PdfFile { get; set; }

    }
}
