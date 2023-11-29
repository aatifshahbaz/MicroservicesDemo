using Catalog.Service.Data;
using Catalog.Service.Models;
using Catalog.Service.Repositories;
using Catalog.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSqlite<ApplicationDbContext>();

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.AddTransient<IRepository<Item>, SqlRepository<Item>>();
            builder.Services.AddScoped<IItemService, ItemService>();

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
        }
    }
}