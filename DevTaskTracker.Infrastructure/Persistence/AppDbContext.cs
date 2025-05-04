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

            // Configure TaskItem
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Status)
                      .HasConversion<string>();

                entity.Property(t => t.Priority)
                      .HasConversion<string>();

                // AssignedBy (AppUser)
                entity.HasOne(t => t.AssignedBy)
                      .WithMany()
                      .HasForeignKey(t => t.AssignedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // LastUpdatedBy (AppUser)
                entity.HasOne(t => t.LastUpdatedBy)
                      .WithMany()
                      .HasForeignKey(t => t.LastUpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Member (assigned user)
                entity.HasOne(t => t.Member)
                      .WithMany(m => m.AssignedTasks)
                      .HasForeignKey(t => t.MemberId)
                      .OnDelete(DeleteBehavior.Restrict);


                // Organization
                entity.HasOne(t => t.Organization)
                      .WithMany(o => o.Tasks)
                      .HasForeignKey(t => t.OrganizationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Optional: configure AppUser explicitly to avoid ambiguity
            //modelBuilder.Entity<AppUser>(entity =>
            //{
            //    entity.HasMany(u => u.AssignedTasks)
            //          .WithOne(t => t.Member)
            //          .HasForeignKey(t => t.MemberId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //});
        }


    }
}
