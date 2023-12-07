using Microsoft.EntityFrameworkCore;
namespace LocalDevicesSearcher.Models.EFDB
{
    public class DeviceDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<OpenedPort> OpenedPorts { get; set; }
        public DeviceDbContext(DbContextOptions<DeviceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenedPort>()
                .HasKey(op => new { op.Id, op.DeviceId });
            modelBuilder.Entity<OpenedPort>()
                .HasOne(op => op.Device)
                .WithMany(d => d.OpenedPorts)
                .HasForeignKey(op => op.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}