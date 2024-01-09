using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public class InvoiceBody : IInvoiceBody
    {
		private readonly IInvoiceSubreport _subreport;
        public InvoiceBody(IInvoiceSubreport subreport)
        {

            _subreport = subreport;

        }
        public string GetHtmlTemplate(InvoiceModel invoice)
        {
            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Invoice Template</title>
  <style>
    body {{
      font-family: 'Arial', sans-serif;
      margin: 0;
      padding: 20px;
    }}

    .invoice {{
      width: 80%;
      margin: 0 auto;
      border: 1px solid #ccc;
      padding: 5%;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
      background-color:lightblue
    }}

    .invoice-header {{
      text-align: center;
    }}

    .invoice-details {{
      display: flex;
      justify-content: space-between;
    }}

    .invoice-items {{
      margin-top: 20px;
    }}

    table {{
      width: 100%;
      border-collapse: collapse;
      margin-top: 10px;
    }}

    th, td {{
      border: 1px solid #ddd;
      padding: 8px;
      text-align: left;
    }}

    th {{
      background-color:blue;
    }}

    .invoice-total {{
      margin-top: 20px;
      text-align: right;
    }}
    invoice-container {{
      width: 100%;
      height: 100%;
      background-color: lightskyblue;
    }}
  </style>
</head>
<body>

  <div class=""invoice"">
    <div class=""invoice-header"">
      <h1>Invoice</h1>
    </div>

    <div class=""invoice-details"">
      <div>
        <p><strong>From:</strong> {invoice.CompanyName}</p>
        <p>{invoice.CompanyAddress}</p>
        <p>{invoice.CompanyEmail}</p>
      </div>
      <div>
        <p><strong>To:</strong>{invoice.CustomerName}</p>
        <p>{invoice.CustomerAddress}</p>
        <p>{invoice.CompanyEmail}</p>
      </div>
    </div>

    <div class=""invoice-items"">
      <table>
        <thead>
          <tr>
            <th>Description</th>
            <th>Quantity</th>
            <th>Unit Price</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
";
            var subreport = _subreport.GetInvoiceSubreport(invoice.Items);
            var footer = $@"    </tbody>
        </table>
        <div class=""total"">
            <h3>Total: ${invoice.Total}</h3>
        </div>
        <div class=""footer"">
            <!-- Add additional information or notes here -->
            <p>Thank you for your business!</p>
        </div>
    </div>
</body>
</html>";
            return body + subreport + footer;
           

        }
    }
}
