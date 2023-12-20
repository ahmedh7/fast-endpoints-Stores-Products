using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreProdFastEndpoints.Models;

namespace StoreProdFastEndpoints.Context
{
    public class StoreContext : IdentityDbContext<Admin>
    {
        public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
        {

        }
        
        public DbSet<Store>? Stores { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Admin>? Admins { get; set; }
    }
}
