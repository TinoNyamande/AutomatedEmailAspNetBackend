using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public class ReceiptSubreport:IReceiptSubreport
    {

        public string GetHtmlString(List<ReceiptItems> items)
        {
            string body = "";
            foreach (var item in items)
            {
                body += $@"<tr>
                    <td>{item.ProductName}</td>
                    <td>{item.Quantity}</td>
                    <td>{item.Price}</td>
                    <td>{item.Total}</td>

                </tr>";
            }
            return body;
        }
    }
}
