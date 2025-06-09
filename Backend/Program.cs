using Backend.Services;
using Backend.Simulator;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient<CustomerService>(c =>
        {
            c.BaseAddress = new Uri("http://b2b2buildingblocks.westeurope.cloudapp.azure.com:8080/");
        });

        services.AddHttpClient<OrderService>(c =>
        {
            c.BaseAddress = new Uri("http://b2b2buildingblocks.westeurope.cloudapp.azure.com:8080/");
        });

        services.AddTransient<CustomerSimulator>();
    });

var app = builder.Build();

var simulator = app.Services.GetRequiredService<CustomerSimulator>();
await simulator.StartAsync();
