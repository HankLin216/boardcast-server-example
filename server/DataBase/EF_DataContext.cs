using Microsoft.EntityFrameworkCore;

namespace boardcast_server_example.DataBase;
public class EF_DataContext : DbContext
{
    public EF_DataContext(DbContextOptions<EF_DataContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(e =>
        {
            e.Property(e => e.Status).HasDefaultValue("Unknown");
            e.Property(e => e.Create_at).HasColumnType("timestamp").HasDefaultValueSql("Now()");
            e.Property(e => e.Update_at).HasColumnType("timestamp").HasDefaultValueSql("Now()");
        }).UseSerialColumns();
    }

    public required DbSet<Device> Devices { get; set; }
}