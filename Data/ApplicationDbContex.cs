using BTLNHOM14.Models;
using Microsoft.EntityFrameworkCore;

namespace BTLNhom14.Data
{
    public class ApplicationDbContex : DbContext
    {
        public ApplicationDbContex (DbContextOptions<ApplicationDbContex>options) : base(options)

        {

        }
        public DbSet<Product>Product {get; set;} = default!;
        public DbSet<Category>Category {get; set;} = default!;
        public DbSet<User>User {get; set;} =default!;
    }
}