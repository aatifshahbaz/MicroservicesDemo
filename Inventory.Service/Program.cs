using Common.Data;
using Common.MassTransit;
using Common.Repository;
using Inventory.Service;
using Inventory.Service.Data;
using Inventory.Service.Models;
using Inventory.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddSqlite<InventoryDbContext>()
                .AddRepository<InventoryItem>()
                .AddRepository<CatalogItem>()
                .AddMassTransitWithRabbitMQ();

//Cannot generalize services via extensions, because each service use different entity and different repository
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
