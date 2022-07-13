using Microsoft.EntityFrameworkCore;
using WEB_API_LAPTOP;
using WEB_API_LAPTOP.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region DatabaseConnect
builder.Services.AddDbContext<BanLaptopEntities>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion  DatabaseConnect

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
