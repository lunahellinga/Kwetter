using ContentService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContentService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<Content> Content { get; set; }
}