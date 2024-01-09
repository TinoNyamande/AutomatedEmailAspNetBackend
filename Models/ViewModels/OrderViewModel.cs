using Microsoft.AspNetCore.Mvc;

namespace EmailAPI.Models.ViewModels
{
    public class OrderViewModel
    {
        public string? Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime PlacedOn { get; set; }
        public string? OrderStatus { get; set; }
        public string? PickUpLocation { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingMethod { get; set; }
        public string? Status { get; set; }
        public FileContentResult PdfFile { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
