using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SweetSavory.Models
{
  public class SweetSavoryContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Treat> Treatss { get; set; }
    public DbSet<Flavor> Flavors { get; set; }
    
    public DbSet<FlavorTreat> FlavorTreat { get; set; }

    public SweetSavoryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}