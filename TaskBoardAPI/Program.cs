using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Middleware;
using System.Collections;
using System.Security.Claims;
using System.Text;
using Yarp.ReverseProxy.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

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

        app.UseMiddleware<JwtBlacklistMiddleware>();

        app.MapControllers();

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        services.AddSingleton<IBlackListService, BlackListService>();

        AddAuthentication(services, configuration);
        AddSwagger(services);

        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }

    private static void AddAuthentication(IServiceCollection services, IConfigurationManager configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskBoard API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Введите 'Bearer' [пробел] для авторизации",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

            options.AddServer(new OpenApiServer { Url = "https://localhost:7000", Description = "API Gateway" });
            options.AddServer(new OpenApiServer { Url = "https://localhost:7001", Description = "Project Service" });
            options.AddServer(new OpenApiServer { Url = "https://localhost:7002", Description = "User Service" });
            options.AddServer(new OpenApiServer { Url = "https://localhost:7003", Description = "Analytics Service" });
        });
    }
}