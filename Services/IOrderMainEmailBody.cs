namespace EmailAPI.Services
{
    public interface IOrderMainEmailBody
    {
        public string GetEmailHtml(string name , string orderID);
    }
}
