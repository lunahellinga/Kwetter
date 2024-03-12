using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TagService.Models;

namespace TagService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<TagUse> TagUses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TagUse>()
            .HasKey(t => new { t.TagId, t.KweetId });
    }
}