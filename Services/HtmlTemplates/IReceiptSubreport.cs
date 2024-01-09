using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public interface IReceiptSubreport
    {
        public string GetHtmlString(List<ReceiptItems> items);
    }
}
