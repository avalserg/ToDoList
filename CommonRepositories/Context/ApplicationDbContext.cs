using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories.Context
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todos> Todos { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todos>().HasKey(c => c.Id);
            modelBuilder.Entity<Todos>().Property(b => b.Label).HasMaxLength(1000).IsRequired();
            

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(b => b.Name).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<Todos>()
                .HasOne(v => v.User)
                .WithMany(c => c.Todos)
                .HasForeignKey(v => v.OwnerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
