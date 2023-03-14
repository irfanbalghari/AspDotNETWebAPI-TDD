using AspDotNETWebAPI.Core.DataServices;
using AspDotNETWebAPI.Core.Processors;
using AspDotNETWebAPI.Persistance;
using AspDotNETWebAPI.Persistance.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asp DotNET Web API TDD Sample", Version = "v1" });
});

var connString = "DataSource=:memory:";
var conn = new SqliteConnection(connString);
conn.Open();
builder.Services.AddDbContext<RoomBookingAppDbContext>(opt =>opt.UseSqlite(conn));
EnsureDatabaseCreated(conn);

 static void EnsureDatabaseCreated(SqliteConnection conn)
{
	var builder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
	builder.UseSqlite(conn);
	using var context = new RoomBookingAppDbContext(builder.Options);
	context.Database.EnsureCreated();
}

builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
builder.Services.AddScoped<IRoomBookingProcessor, RoomBookingProcessor>();
                           

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
