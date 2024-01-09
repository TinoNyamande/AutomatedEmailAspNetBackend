namespace EmailAPI.Services
{
    public class InvoiceEmail:IInvoiceEmail
    {
        public string GetEmailHtml(string name, string orderId)
        {
            var body = $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        .header {{
             text-align: center;
             color: grey;
        }}
        .body {{
            background-color: aqua;
        }}
        .main {{
            text-align: center;
        }}
    </style>
</head>
<body class='body'>

<div>
    <div class='header'>
        <h3>Order details</h3>
    </div>
    <hr>
    <div class='main'>
        <p>Dear {name}</p><br>
        <p>Attached below is a pdf file with the invoice details</p>
    </div>
    
    
</div>
 
</body>
</html>";
            return body;
        }
    }
}
