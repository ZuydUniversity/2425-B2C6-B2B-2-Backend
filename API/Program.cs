using API.Data;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var connectionString = builder.Configuration.GetSection("Database")["ConnectionString"];
            connectionString = connectionString
                .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER") ?? "10.0.2.4")
                .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "BuildingBlocks")
                .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "sa")
                .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "defaultpasswordwhichwillnotwork");

            // Add services to the container.
            builder.Services.AddDbContext<SQLServerDatabaseContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure AppSettings for dependency injection
            builder.Services.Configure<AppSettings>(options =>
            {
                options.ConnectionString = connectionString;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://b2b2buildingblocks.westeurope.cloudapp.azure.com",
                                          "http://10.0.1.4"); // added local ip-adresses just in case
                                  });
            });

            builder.Services.AddControllers();

            // Add Swagger/OpenAPI support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {   
                    Title = "Building Blocks API",
                    Version = "v1",
                    Description = "Proces Mining & Orderbeheer API voor Building Blocks"
                });


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Apply database migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SQLServerDatabaseContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Building Blocks API v1");
                });
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

