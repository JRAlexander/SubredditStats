using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SubredditStats.Common.Models
{
    // Class for Ranked Posts 
    [SwaggerSchema(Required = new[] { "Description" })]
    public class RankedPost
    {
        [SwaggerSchema("Reddit post permalink", ReadOnly = true)]
        public int Id { get; set; }
        [SwaggerSchema("Reddit post identifier", ReadOnly = true)]
        public string? PostId { get; set; }
        [SwaggerSchema("The date it was created", Format = "date")]
        public DateTime Created { get; set; }
        [SwaggerSchema("Reddit post title", ReadOnly = true)]
        public string? Title { get; set; }
        public string? Name { get; set; }
        [SwaggerSchema("Reddit post author", ReadOnly = true)]
        public string? Author { get; set; }
        [SwaggerSchema("Reddit post permalink", ReadOnly = true)]
        public string? Permalink { get; set; }
        [SwaggerSchema("Reddit post upvotes", ReadOnly = true)]
        public int UpVotes { get; set; }
        [SwaggerSchema("Reddit post score", ReadOnly = true)]
        public int Score { get; set; }
        [SwaggerSchema("Reddit post upvore ratio", ReadOnly = true)]
        public double UpvoteRatio { get; set; }
    }

    public class RankedPostDb : DbContext
    {
        public RankedPostDb(DbContextOptions<RankedPostDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RankedPost>();
        }

        public DbSet<RankedPost> RankedPosts { get; set; } = null!;
    }
}
