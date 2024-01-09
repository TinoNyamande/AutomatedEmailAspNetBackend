using Microsoft.EntityFrameworkCore;
using EmailAPI.Models;

namespace EmailAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItems> InvoiceItems { get; set; }
        public DbSet<InvoiceModel> InvoiceModel { get; set; }
        public DbSet<InvoiceItemsModel> InvoiceItemsModel { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItems> ReceiptItems { get; set; }



    }
}
