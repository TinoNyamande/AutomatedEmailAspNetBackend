namespace EmailAPI.Models
{
    public class Order
    {
        public string? Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime PlacedOn { get; set; }
        public string? OrderStatus { get; set; }
        public string? PickUpLocation { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingMethod { get; set; }
        public string? Status { get; set; }
        public string? FilePath { get; set; }
        public string? HtmlContent { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyName { get;set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyPhone { get; set; }

    }
}
