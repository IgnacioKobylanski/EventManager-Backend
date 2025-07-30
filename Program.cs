using Microsoft.EntityFrameworkCore;
using EventManager.Data; 
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<EventContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
// NOTA: Para el proveedor oficial (MySql.EntityFrameworkCore), se usa 'UseMySQL' con 'S' mayúscula.

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();