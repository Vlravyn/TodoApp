using Microsoft.EntityFrameworkCore;
using TodoApp.Core.DataModels;

namespace TodoApp.Core.EntityFramework
{
    public class UserTasksDbContext : DbContext
    {
        public DbSet<UserTask> UserTasks { get; set; }

        public UserTasksDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=userTasks.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>(b =>
            {
                b.HasKey(t => t.Id);
                b.OwnsMany(t => t.Steps, p =>
                {
                    p.Property(t => t.Title)
                     .IsRequired(true);
                    p.Property(t => t.IsCompleted)
                     .HasDefaultValue(false);
                });

                b.Property(t => t.Title)
                 .IsRequired(true);

                b.Property(t => t.IsCompleted)
                 .HasDefaultValue(false);

                b.Property(t => t.IsImportant)
                 .HasDefaultValue(false);

                b.Property(t => t.DueDateUtc)
                 .IsRequired(false)
                 .HasDefaultValue(null);

                b.Property(t => t.RemindUserUtc)
                 .IsRequired(false)
                 .HasDefaultValue(null);

                b.Property(t => t.CreatedAtUtc)
                 .IsRequired(true)
                 .HasDefaultValue(null);

                b.Ignore(t => t.DueDate);
                b.Ignore(t => t.RemindUser);
                b.Ignore(t => t.CreatedAt);
            });
        }

        public override void Dispose()
        {
            SaveChanges();
            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await SaveChangesAsync();
            await base.DisposeAsync();
        }
    }
}