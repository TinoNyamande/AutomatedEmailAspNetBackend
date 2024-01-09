namespace EmailAPI.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public List<InvoiceItems> Products {get;set;}
    }
}
