using API.Data;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            ConfigureHttps(builder);
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
                                      policy.AllowAnyOrigin() // Allow any origin to temporary bypass CORS errors
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
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
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void ConfigureHttps(WebApplicationBuilder builder)
        {
            // Check if SSL certificate, key, and password are provided via environment variables
            var sslCert = Environment.GetEnvironmentVariable("SSL_CERTIFICATE");
            var sslKey = Environment.GetEnvironmentVariable("SSL_PRIVATE_KEY");
            var sslPassword = Environment.GetEnvironmentVariable("SSL_PASSWORD");

            if (!string.IsNullOrEmpty(sslCert) && !string.IsNullOrEmpty(sslKey))
            {
                try
                {
                    // Convert Base64 strings to certificate and key bytes
                    var certBytes = Convert.FromBase64String(sslCert);
                    var keyBytes = Convert.FromBase64String(sslKey);

                    // Create X509Certificate2 from the certificate and key files
                    var certificate = X509Certificate2.CreateFromPem(
                        System.Text.Encoding.UTF8.GetString(certBytes),
                        System.Text.Encoding.UTF8.GetString(keyBytes));

                    // If a password is provided, create a new certificate with the password
                    if (!string.IsNullOrEmpty(sslPassword))
                    {
                        // Export the certificate to PFX format
                        byte[] pfxData = certificate.Export(X509ContentType.Pfx);

                        // Create a new certificate with the password and ephemeral key set
                        certificate = new X509Certificate2(
                            pfxData,
                            sslPassword,  // Use the password directly (not Base64 encoded)
                            X509KeyStorageFlags.EphemeralKeySet);

                        Console.WriteLine("Certificate loaded with password protection.");
                    }

                    // Configure Kestrel to use HTTPS with the certificate
                    builder.WebHost.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(8080); // HTTP
                        serverOptions.ListenAnyIP(8081, listenOptions =>
                        {
                            listenOptions.UseHttps(certificate);
                        }); // HTTPS
                    });

                    Console.WriteLine("HTTPS configured successfully with certificate from environment variables.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to configure HTTPS: {ex.Message}");
                    Console.WriteLine($"Exception details: {ex}");
                }
            }
            else
            {
                Console.WriteLine("SSL_CERTIFICATE or SSL_PRIVATE_KEY not provided. Running without HTTPS.");
            }
        }
    }
}

