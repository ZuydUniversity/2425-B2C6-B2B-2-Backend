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

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void ConfigureHttps(WebApplicationBuilder builder)
        {
            // Check if SSL certificate and password are provided via environment variables
            var sslCert = Environment.GetEnvironmentVariable("SSL_CERTIFICATE");
            var sslPassword = Environment.GetEnvironmentVariable("SSL_PASSWORD");

            if (!string.IsNullOrEmpty(sslCert))
            {
                try
                {
                    Console.WriteLine("Attempting to load SSL certificate...");

                    // Validate that the certificate appears to be Base64 encoded
                    if (!IsValidBase64String(sslCert))
                    {
                        throw new FormatException("The SSL_CERTIFICATE does not appear to be a valid Base64 string");
                    }

                    // Convert Base64 string to certificate bytes
                    var certBytes = Convert.FromBase64String(sslCert);
                    Console.WriteLine($"Successfully decoded certificate from Base64. Certificate size: {certBytes.Length} bytes");

                    X509Certificate2 certificate;
                    X509Certificate2Collection certCollection = new X509Certificate2Collection();

                    // Load the certificate into the collection first
                    certCollection.Import(
                        certBytes,
                        sslPassword,
                        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                    // Get the certificate with private key from the collection
                    certificate = certCollection[0];

                    // Log certificate details for verification
                    Console.WriteLine($"Certificate Subject: {certificate.Subject}");
                    Console.WriteLine($"Certificate Issuer: {certificate.Issuer}");
                    Console.WriteLine($"Certificate Valid From: {certificate.NotBefore}");
                    Console.WriteLine($"Certificate Valid To: {certificate.NotAfter}");
                    Console.WriteLine($"Certificate Has Private Key: {certificate.HasPrivateKey}");
                    Console.WriteLine($"Certificates in chain: {certCollection.Count}");

                    // Configure Kestrel to use HTTPS with the certificate collection
                    builder.WebHost.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(8080); // HTTP
                        serverOptions.ListenAnyIP(8081, listenOptions =>
                        {
                            // Use the certificate collection for HTTPS
                            listenOptions.UseHttps(httpsOptions =>
                            {
                                httpsOptions.ServerCertificateSelector = (_, _) => certificate;
                                httpsOptions.ServerCertificate = certificate;
                                httpsOptions.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
                                httpsOptions.CheckCertificateRevocation = false;
                            });
                        }); // HTTPS
                    });

                    Console.WriteLine("HTTPS configured successfully with certificate from environment variables");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to configure HTTPS: {ex.Message}");
                    Console.WriteLine($"Exception type: {ex.GetType().Name}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }

                    Console.WriteLine("Running without HTTPS. Please check your certificate and password.");
                }
            }
            else
            {
                Console.WriteLine("SSL_CERTIFICATE not provided. Running without HTTPS.");
            }
        }

        // Helper method to validate if a string is Base64 encoded
        private static bool IsValidBase64String(string base64)
        {
            // Quick validation - ensure string length is multiple of 4
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0)
            {
                return false;
            }

            // Check if the string contains only valid Base64 characters
            foreach (char c in base64)
            {
                if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') ||
                      (c >= '0' && c <= '9') || c == '+' || c == '/' || c == '='))
                {
                    return false;
                }
            }

            // Ensure padding is valid
            int paddingCount = 0;
            for (int i = base64.Length - 1; i >= 0; i--)
            {
                if (base64[i] == '=')
                    paddingCount++;
                else
                    break;
            }

            return paddingCount <= 2;  // Valid Base64 can have at most 2 padding characters
        }
    }
}

