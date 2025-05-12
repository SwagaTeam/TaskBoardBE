using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Middleware;
using System.Collections;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        Env.Load();

        var app = builder.Build();

        app.UseCors("AllowAll");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // All swagger endpoints use HTTP
                c.SwaggerEndpoint("http://localhost:7000/swagger/v1/swagger.json", "API Gateway");
                c.SwaggerEndpoint("http://localhost:7001/swagger/v1/swagger.json", "Project Service");
                c.SwaggerEndpoint("http://localhost:7002/swagger/v1/swagger.json", "User Service");
                c.SwaggerEndpoint("http://localhost:7003/swagger/v1/swagger.json", "Analytics Service");
                c.SwaggerEndpoint("http://localhost:7004/swagger/v1/swagger.json", "Contributors Service");
            });
        }

        // Remove HTTPS redirection to use HTTP only
        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<JwtBlacklistMiddleware>();

        app.MapControllers();
        app.MapReverseProxy();

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
                 .LoadFromConfig(configuration.GetSection("ReverseProxy"))
                 .AddTransforms(builderContext =>
                 {
                     builderContext.AddRequestTransform(async transformContext =>
                     {
                         if (transformContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                         {
                             transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue(authHeader);
                         }
                     });
                 });
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
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

            // Swagger servers use HTTP only
            options.AddServer(new OpenApiServer { Url = "http://localhost:7000", Description = "API Gateway" });
            options.AddServer(new OpenApiServer { Url = "http://localhost:7001", Description = "Project Service" });
            options.AddServer(new OpenApiServer { Url = "http://localhost:7002", Description = "User Service" });
            options.AddServer(new OpenApiServer { Url = "http://localhost:7003", Description = "Analytics Service" });
            options.AddServer(new OpenApiServer { Url = "http://localhost:7004", Description = "Contributors Service" });
        });
    }
}