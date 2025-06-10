using API.Data;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Apply database migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SQLServerDatabaseContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
