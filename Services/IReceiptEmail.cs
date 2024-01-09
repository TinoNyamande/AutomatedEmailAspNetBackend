namespace EmailAPI.Services
{
    public interface IReceiptEmail
    {
        public string GetEmailHtml(string name, string receiptID);
    }
}
