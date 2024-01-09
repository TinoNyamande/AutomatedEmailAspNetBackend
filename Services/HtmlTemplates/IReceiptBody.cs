using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public interface IReceiptBody
    {
        public string GetHtmlString(Receipt receipt);
    }
}
