using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Yarp.ReverseProxy.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });

            // Добавляем микросервисы
            options.AddServer(new OpenApiServer { Url = "https://localhost:7000", Description = "API Gateway" });
            options.AddServer(new OpenApiServer { Url = "https://localhost:7001", Description = "Project Service" });
            options.AddServer(new OpenApiServer { Url = "https://localhost:7002", Description = "User Service" });
            options.AddServer(new OpenApiServer { Url = "https://localhost:7003", Description = "Analytics Service" });

        });


        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        var app = builder.Build();

        app.UseCors("AllowAll");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("https://localhost:7000/swagger/v1/swagger.json", "API Gateway");
                c.SwaggerEndpoint("https://localhost:7001/swagger/v1/swagger.json", "Project Service");
                c.SwaggerEndpoint("https://localhost:7002/swagger/v1/swagger.json", "User Service");
                c.SwaggerEndpoint("https://localhost:7003/swagger/v1/swagger.json", "Analytics Service");
            });
        }

        app.MapReverseProxy();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        var configuration = builder.Configuration.GetSection("ReverseProxy");

        var proxyConfig = builder.Configuration.GetSection("ReverseProxy").Get<Yarp.ReverseProxy.Configuration.ClusterConfig>();
        if (proxyConfig != null)
        {
            Console.WriteLine("Настроенные маршруты:");
            foreach (var route in builder.Configuration.GetSection("ReverseProxy:Routes").GetChildren())
            {
                Console.WriteLine($"Маршрут: {route.Key} → {route.GetValue<string>("Match:Path")}");
            }

            Console.WriteLine("Настроенные сервисы:");
            foreach (var cluster in builder.Configuration.GetSection("ReverseProxy:Clusters").GetChildren())
            {
                var address = cluster.GetSection("Destinations").GetChildren().FirstOrDefault()?.GetValue<string>("Address");
                Console.WriteLine($"Кластер: {cluster.Key} → {address}");
            }
        }

        app.Run();
    }
}