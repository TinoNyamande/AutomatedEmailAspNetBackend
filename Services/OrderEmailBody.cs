using EmailAPI.Models;
namespace EmailAPI.Services
{
    public class OrderEmailBody : IOrderEmailBody
    {
        public string GetHtmlString(Order order)
        {
            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <style>
      body {{
        background-color: lightblue;
      }}
      .header {{
        text-align: center;
      }}
      .mytable {{
        width: 80%;
        margin-left: 10%;
        margin-right: 10%;
        background-color: gainsboro;
        border-collapse: separate;
        border-spacing: 20px;
        padding-top: 3%;
        padding-left: 3%;
      }}

      th {{
        width: 50%;
        text-align: left;
        border: 1px solid grey;
      }}

      tr {{
        border: 1px solid grey;
      }}

      .logo {{
        width: 100%;
        height: 15vh;
      }}

      .logo-image {{
        height: 8vh;
        width: 100%;
      }}
      .footer {{
        margin-top: 5vh;
        margin-left: 10%;
      }}
      .signature {{
        width: 15%;
        height: 5vh;
      }}
      .table-container {{
        text-align: center;
      }}
    </style>
  </head>
  <body>
    <div class=""logo"">
      <img
        class=""logo-image""
        src=""C:\Users\hp i5\Documents\Your paragraph text.png""
      />
    </div>
    <div class=""header"">
      <h1>{order.CompanyName}</h1>
      <span>{order.CompanyAddress}</span>, 
      <span>{order.CompanyEmail}</span>,
      <span>{order.CompanyPhone}</span><br>
          
      <hr/>
      <p>Dear {order.CustomerName} </p>
      <p>Your order was received successfully. Below are the details of your order</p>
    </div>
    <div class=""table-container"">
      <table class=""mytable"">
        <thead>
          <tr>
            <th>Order Id</th>
            <th>{order.Id}</th>
          </tr>
          <tr>
            <th>Placed On</th>
            <th>{order.PlacedOn}</th>
          </tr>
          <tr>
            <th>Order Status</th>
            <th>{order.OrderStatus}</th>
          </tr>
          <tr>
            <th>Pick up Location</th>
            <th>{order.PickUpLocation}</th>
          </tr>
          <tr>
            <th>Payment Method</th>
            <th>{order.PaymentMethod}</th>
          </tr>
          <tr>
            <th>Shipping Method</th>
            <th>{order.ShippingMethod}</th>
          </tr>
        </thead>
      </table>

    </div>
    <div class=""footer"">
      <div>
        <p>Yours Sincerely</p>
        <p> Tino N </p>
      </div>
      <img
        class=""signature""
        src=""C:\Users\hp i5\Documents\HCS\Attachment\EMS\AutomatedEmail\automatedemailfrontend\public\th (2).jpg""
      />
    </div>
    <hr/>
    <div>
      <p>{order.CompanyName}-2024</p>
    </div>
  </body>
</html>

          ";
            return body;
        }
    }
}
