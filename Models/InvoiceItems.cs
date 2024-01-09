namespace EmailAPI.Models
{
    public class InvoiceItems
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double SubTotal { get; set; }
    }
}
