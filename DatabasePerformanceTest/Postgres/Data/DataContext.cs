using Microsoft.EntityFrameworkCore;
using Postgres.Models;

namespace Postgres.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
    }
    
    public DbSet<Kweet> Kweets { get; set; }
}