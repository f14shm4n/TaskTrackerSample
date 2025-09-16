using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TaskTracker.API.Application.Jwt;
using TaskTracker.API.Application.Services;
using TaskTracker.API.Middlewares;
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

            builder.Services
                .AddProblemDetails()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                    };
                });

            builder.Services
                .AddAuthorization()
                .AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task track API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            UseMiddlewares(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            SetupDatabase(app);

            app.Run();
        }

        #region Utils

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services
                .Configure<AppJwtOptions>(builder.Configuration.GetSection("Jwt"))
                .AddDbContextPool<TaskTrackerDbContext>(o =>
                {
                    o.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    x =>
                    {
                        x.MigrationsAssembly(typeof(Program).Assembly);
                    });
                })
                .AddMediatR(c =>
                {
                    c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                })
                .AddScoped<IWorkAssignmentRepository, WorkAssignmentRepository>()
                .AddScoped<IDataSeeder, DataSeeder>()
                .AddTransient<ApiResponseProblemDetailMiddleware>();
        }

        private static void UseMiddlewares(WebApplication app)
        {
            app.UseMiddleware<ApiResponseProblemDetailMiddleware>();
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
#pragma warning disable CA1848 // Использовать делегаты LoggerMessage
                    logger.LogError(ex, "Failed to create database or apply migrations.");
#pragma warning restore CA1848 // Использовать делегаты LoggerMessage
                }
            }
        }

        #endregion
    }
}
