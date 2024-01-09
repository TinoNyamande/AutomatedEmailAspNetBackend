using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public interface IInvoiceBody
    {
        public string GetHtmlTemplate(InvoiceModel invoice);
    }
}
