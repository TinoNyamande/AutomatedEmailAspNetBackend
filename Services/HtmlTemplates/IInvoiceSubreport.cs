using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public interface IInvoiceSubreport
    {
        public string GetInvoiceSubreport(List<InvoiceItemsModel> products);
    }
}
