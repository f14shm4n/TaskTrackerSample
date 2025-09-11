
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using TaskTracker.Domain.Aggregates.WorkAssignment;
using TaskTracker.Infrastructure;
using TaskTracker.Infrastructure.Repositories;

namespace TaskTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddServices(builder);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            SetupDatabase(app);

            app.Run();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextPool<TaskTrackerDbContext>(o =>
            {
                o.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    x =>
                    {
                        x.MigrationsAssembly(typeof(Program).Assembly);
                    });
            });

            builder.Services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddScoped<IWorkAssignmentRepository, WorkAssignmentRepository>();
        }

        private static void SetupDatabase(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            using (var ctx = app.Services.CreateScope())
            {
                var db = ctx.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();
                try
                {
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to create database or apply migrations.");
                }
            }
        }
    }
}
