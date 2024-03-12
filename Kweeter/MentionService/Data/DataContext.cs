using MentionService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MentionService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<Mention> Mentions { get; set; }
}