using Catalog.Service.Data;
using Catalog.Service.Models;
using Catalog.Service.Services;
using Common.Data;
using Common.MassTransit;
using Common.Repository;


namespace Catalog.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.AddSqlite<CatalogDbContext>()
                            .AddRepository<Item>()
                            .AddMassTransitWithRabbitMQ();

            //Cannot generalize services via extensions, because each service use different entity and different repository
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