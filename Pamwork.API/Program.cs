using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pamwork.API.Helpers;
using Pamwork.BAL.Abstract;
using Pamwork.BAL.Concrete;
using Pamwork.DAL.Abstract;
using Pamwork.DAL.Concrete;

namespace Pamwork.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register AutoMapper and scan the current AppDomain for profiles
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register PamworkDbContext with SQL Server and migrations assembly configuration
            builder.Services.AddDbContext<PamworkDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("Pamwork.API")
                )
            );

            // Swagger/OpenAPI setup
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Pamwork API",
                    Version = "v1"
                });
            });

            // Register services and repositories
            builder.Services.AddScoped<INoteService, NoteManager>();
            builder.Services.AddScoped<INoteDal, EfNoteDal>();

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // For detailed error info in development
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pamwork API V1");
                    c.RoutePrefix = string.Empty; // Swagger UI at root
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Enable CORS
            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();

            // Map controller routes
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
