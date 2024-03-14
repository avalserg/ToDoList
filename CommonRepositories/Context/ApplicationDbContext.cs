using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories.Context
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todos> Todos { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todos>().HasKey(c => c.Id);
            modelBuilder.Entity<Todos>().Property(b => b.Label).HasMaxLength(1000).IsRequired();
            

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(b => b.Login).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().HasIndex(e=>e.Login).IsUnique();
            modelBuilder.Entity<User>().HasOne(e => e.UserRole).WithMany(u=>u.Users).HasForeignKey(e => e.UserRoleId);

            modelBuilder.Entity<Todos>()
                .HasOne(v => v.User)
                .WithMany(c => c.Todos)
                .HasForeignKey(v => v.OwnerId);

            modelBuilder.Entity<UserRole>().HasKey(u => u.Id);
            modelBuilder.Entity<UserRole>().Property(b => b.Name).HasMaxLength(50).IsRequired();
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
