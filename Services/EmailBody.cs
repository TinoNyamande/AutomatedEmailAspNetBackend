namespace EmailAPI.Services
{
    public class EmailBody :IEmailBody
    {
        public string GetBody(string name , double bal)
        {
            var body = $@"<html>
                          <head>
                                <style>
                                    body {{
                                          background-color:blue
                                      }}
                                    .header {{
                                         color:grey
                                      }}
                                    .main {{
                                        background-color:green
                                    }}
                                </style>
                           </head>
                             <body class='main'>
                                 <h4 class = 'header'>Hello {name}</h4>
                                 <br><br><p>You have {bal} remaining in your account</p>

                            </body>
                </html>";
            return body;
        }
    }
}
