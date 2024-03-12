using Mongo.Controllers;
using Mongo.Data;
using Mongo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo config
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));


// Mongo service
builder.Services.AddSingleton<KweetService>();
// Controller
builder.Services.AddScoped<KweetController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapControllers();

app.Run();
