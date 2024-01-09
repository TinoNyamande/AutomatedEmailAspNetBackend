using DinkToPdf;
using DinkToPdf.Contracts;
using EmailAPI.Data;
using EmailAPI.Models;
using EmailAPI.Services;
using EmailAPI.Services.HtmlTemplates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr") ?? 
    throw new InvalidOperationException("Connection string  not found.")));

builder.Services.AddCors(options =>
{
      options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();

                       });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection(nameof(MailSettings)));


builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<PDFService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailBody, EmailBody>();
builder.Services.AddScoped<IOrderEmailBody, OrderEmailBody>();
builder.Services.AddScoped<IOrderMainEmailBody, OrderMainEmailBody>();
builder.Services.AddScoped<IInvoiceBody, InvoiceBody>();
builder.Services.AddScoped<IInvoiceSubreport, InvoiceSubreport>();
builder.Services.AddScoped<IReceiptBody, ReceiptBody>();
builder.Services.AddScoped<IReceiptSubreport, ReceiptSubreport>();
builder.Services.AddScoped<IInvoiceEmail, InvoiceEmail>();
builder.Services.AddScoped<IReceiptEmail, ReceiptEmail>();

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
