using Chat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ForSqlServerUseIdentityColumns();

            modelBuilder.Entity<User>(x =>
            {
                x.ToTable("Users");
                x.HasKey(a => a.Id);
                x.Property(a => a.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Message>(x =>
            {
                x.ToTable("Messages");
                x.HasKey(a => a.Id);
                x.Property(a => a.Id).ValueGeneratedOnAdd();
                x.HasOne(a => a.User).WithOne().OnDelete(DeleteBehavior.Restrict);
                x.HasOne(a => a.ToUser).WithOne().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefreshToken>(x =>
            {
                x.ToTable("RefreshTokens");
                x.HasKey(a => a.Id);
                x.Property(a => a.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
