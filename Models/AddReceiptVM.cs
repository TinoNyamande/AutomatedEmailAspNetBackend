namespace EmailAPI.Models
{
    public class AddReceiptVM
    {
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public List<ReceiptItems>? Items { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
