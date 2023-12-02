using Microsoft.EntityFrameworkCore;
using TestApplication.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        //Database.Migrate();
        /*Database.EnsureDeleted();
        Database.EnsureCreated();*/
    }
    
    public DbSet<LinkEntity> Links { get; set; }
}