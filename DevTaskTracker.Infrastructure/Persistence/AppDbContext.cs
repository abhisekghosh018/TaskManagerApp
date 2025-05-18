using DevTaskTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevTaskTracker.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Status).HasConversion<string>();
                entity.Property(t => t.Priority).HasConversion<string>();

                entity.HasOne(t => t.AssignedBy)
                      .WithMany()
                      .HasForeignKey(t => t.AssignedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.LastUpdatedBy)
                      .WithMany()
                      .HasForeignKey(t => t.LastUpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Member)
                      .WithMany(m => m.AssignedTasks)
                      .HasForeignKey(t => t.MemberId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Organization)
                      .WithMany(o => o.Tasks)
                      .HasForeignKey(t => t.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasOne(m => m.Organization)
                      .WithMany(o => o.Members)
                      .HasForeignKey(m => m.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.AppUser)
                      .WithMany(a => a.Members)
                      .HasForeignKey(m => m.AppUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasOne(u => u.Organization)
                      .WithMany(o => o.Users)
                      .HasForeignKey(u => u.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
