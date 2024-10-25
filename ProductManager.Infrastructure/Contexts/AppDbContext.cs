using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
       : base(options)
        {
            _configuration = configuration;
        }


        public DbSet<Product> Products { get; set; }
           

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

     
           protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
         
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired() 
                    .HasMaxLength(100); 

                entity.Property(e => e.Description)
                    .HasMaxLength(500); 

                
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)") 
                    .IsRequired(); 

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.DateUpdate);

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
