using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public class InvoiceSubreport : IInvoiceSubreport
    {
        public string GetInvoiceSubreport(List<InvoiceItemsModel> products)
        {

			string body = "";
			foreach(var product in products)
			{
				body += $@"      <tr>
                    <td>{product.ProductName}</td>
                    <td>{product.Quantity}</td>
                    <td>{product.Price}</td>
                    <td>{product.Total}</td>
                </tr>";
			}
	

			return body ;


		}
    }
}
