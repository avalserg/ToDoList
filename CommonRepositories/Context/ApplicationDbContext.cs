using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence.Context
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Todos> Todos { get; set; }
        public DbSet<ApplicationUserApplicationRole> ApplicationUserApplicationRole { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todos>().HasKey(c => c.Id);
            modelBuilder.Entity<Todos>().Property(b => b.Label).HasMaxLength(1000).IsRequired();

            modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
            modelBuilder.Entity<ApplicationUser>().Property(b => b.Login).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ApplicationUser>().HasIndex(e=>e.Login).IsUnique();
            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.Roles).WithOne(e=>e.ApplicationUser).HasForeignKey(e => e.ApplicatonUserId);
            modelBuilder.Entity<ApplicationUser>().Navigation(e => e.Roles).AutoInclude();

            modelBuilder.Entity<RefreshToken>().HasKey(u => u.Id);
            modelBuilder.Entity<RefreshToken>().Property(u => u.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RefreshToken>().HasOne(e => e.ApplicationUser).WithMany().HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<ApplicationUserApplicationRole>().HasKey(k =>
            new {
                k.ApplicatonUserId,k.ApplicatonUserRoleId
            });
            modelBuilder.Entity<ApplicationUserApplicationRole>().Navigation(e => e.ApplicationUserRole).AutoInclude();

            modelBuilder.Entity<ApplicationUserRole>().HasMany(e => e.Users).WithOne(e=>e.ApplicationUserRole).HasForeignKey(e => e.ApplicatonUserRoleId);

            modelBuilder.Entity<Todos>()
                .HasOne(v => v.User)
                .WithMany(c => c.Todos)
                .HasForeignKey(v => v.OwnerId);

            modelBuilder.Entity<ApplicationUserRole>().HasKey(u => u.Id);
            modelBuilder.Entity<ApplicationUserRole>().Property(b => b.Name).HasMaxLength(50).IsRequired();
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
