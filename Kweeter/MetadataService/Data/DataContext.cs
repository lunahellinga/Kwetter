using MetadataService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetadataService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<Metadata> Metadata { get; set; }
}