namespace EmailAPI.Services
{
    public interface IInvoiceEmail
    {
        public string GetEmailHtml(string name, string invoiceID);
    }
}
