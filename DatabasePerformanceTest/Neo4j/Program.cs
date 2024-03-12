using Neo4j.Data;
using Neo4j.Driver;
using Neo4j.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Neo4j
var neo4jSettings = builder.Configuration.GetSection("Neo4j").Get<Neo4jSettings>();
if (neo4jSettings is null) throw new Exception("No neo4j config");
builder.Services.AddSingleton<IDriver>(options =>
    GraphDatabase.Driver(neo4jSettings.Uri, AuthTokens.Basic(neo4jSettings.Username, neo4jSettings.Password)));

builder.Services.AddScoped<KweetService>();

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