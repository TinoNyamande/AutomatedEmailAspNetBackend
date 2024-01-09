using EmailAPI.Models;

namespace EmailAPI.Services.HtmlTemplates
{
    public class ReceiptBody:IReceiptBody
    {
        private readonly IReceiptSubreport _receiptSubreport;
        public ReceiptBody(IReceiptSubreport receiptSubreport)
        {
            _receiptSubreport = receiptSubreport;
        }
        public string GetHtmlString(Receipt receipt)
        {
            var body = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Receipt</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }}

        .receipt {{
            width: 100%;
            max-width: 80%;
            margin: 5% auto;
            padding: 2%;
            border: 1px solid #ccc;
        }}

        .header {{
            text-align: center;
            margin-bottom: 20px;
        }}

        .details {{
            margin-bottom: 20px;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }}

        table, th, td {{
            border: 1px solid #ccc;
        }}

        th, td {{
            padding: 10px;
            text-align: left;
        }}

        .total {{
            text-align: right;
            font-weight: bold;
        }}

        .footer {{
            margin-top: 20px;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class=""receipt"">
        <div class=""header"">
            <h1>Receipt</h1>
            <hr>
            <br>
        </div>
        <div class=""details"">
            <p><strong>Date:</strong> {receipt.ReceiptDate}</p>
            <p><strong>Receipt Id:</strong> {receipt.Id}</p>
        </div>
        <table>
            <thead>
                <tr>
                    <th>Item</th>
                    <th>Quantity</th>
                     <th>Price</th>
                    <th>Subtotal</th>
                </tr>
            </thead>
            <tbody>";

            string table = _receiptSubreport.GetHtmlString(receipt.Items);
            string footer = $@"           </tbody>
        </table>
        <div class=""total"">
            <p>Total: $ {receipt.Total}</p>
        </div>
        <div class=""footer"">
            <p>Thank you for your purchase!</p>
        </div>
    </div>
</body>
</html>";

            return body + table + footer;
        }

        
    }
}
