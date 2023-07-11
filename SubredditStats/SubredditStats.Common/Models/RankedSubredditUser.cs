using Microsoft.EntityFrameworkCore;

namespace SubredditStats.Common.Models
{
    public class RankedSubredditUser
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int totalPosts { get; set; }
    }

    public class RankedSubredditUserDb : DbContext
    {
        public RankedSubredditUserDb(DbContextOptions<RankedSubredditUserDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RankedSubredditUser>();
        }

        public DbSet<RankedSubredditUser> RankedSubredditUsers { get; set; } = null!;
    }
}
