using Microsoft.EntityFrameworkCore;
using Npgsql;
using Postgres.Data;
using Postgres.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add postgres
if (Environment.GetEnvironmentVariable("DB_TYPE") == "postgres")
{
    builder.Services.AddDbContext<DataContext>(options =>
    {
        // options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
    });
}

if (Environment.GetEnvironmentVariable("DB_TYPE") == "yugabyte")
{
    var urlBuilder = new NpgsqlConnectionStringBuilder();

    urlBuilder.Host = Environment.GetEnvironmentVariable("HOST");
    urlBuilder.Port = 5433;
    urlBuilder.Database = Environment.GetEnvironmentVariable("DATABASE");
    urlBuilder.Username = Environment.GetEnvironmentVariable("USERNAME");
    urlBuilder.Password = Environment.GetEnvironmentVariable("PASSWORD");
    urlBuilder.SslMode = SslMode.VerifyFull;
    urlBuilder.RootCertificate = Environment.GetEnvironmentVariable("ROOT_CERT");

    builder.Services.AddDbContext<DataContext>(options =>
    {
        // options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        options.UseNpgsql(urlBuilder.ConnectionString);
    });
}


builder.Services.AddScoped<KweetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Create postgres model
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    await db.Database.MigrateAsync();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapControllers();

app.Run();