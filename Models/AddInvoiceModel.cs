namespace EmailAPI.Models
{
    public class AddInvoiceModel
    {
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
    }
}
