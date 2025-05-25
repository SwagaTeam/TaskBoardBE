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

        if(builder.Environment.IsDevelopment())
            Env.Load();

        builder.Configuration.AddEnvironmentVariables();

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        app.UseCors("AllowAll");

        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("GATEWAY") + "/swagger/v1/swagger.json", "API Gateway");
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("PROJECT_API") + "/swagger/v1/swagger.json", "Project Service");
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("ANALYTICS_API") + "/swagger/v1/swagger.json", "Analytics Service");
                c.SwaggerEndpoint(Environment.GetEnvironmentVariable("USER_API") + "/swagger/v1/swagger.json", "User Service");
            });
        //}

        app.MapReverseProxy();

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
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .AddTransforms(builderContext =>
            {
                builderContext.AddRequestTransform(transformContext =>
                {
                    if (transformContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
                    {
                        var authHeader = authHeaderValues.ToString().Trim();

                        if (AuthenticationHeaderValue.TryParse(authHeader, out var parsedHeader))
                        {
                            transformContext.ProxyRequest.Headers.Authorization =
                                new AuthenticationHeaderValue(parsedHeader.Scheme, parsedHeader.Parameter);
                        }
                        else if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            var token = authHeader["Bearer ".Length..].Trim();
                            transformContext.ProxyRequest.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", token);
                        }
                    }

                    return default;
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

            options.AddServer(new OpenApiServer { Url = Environment.GetEnvironmentVariable("GATEWAY"), Description = "API Gateway" });
            options.AddServer(new OpenApiServer { Url = Environment.GetEnvironmentVariable("PROJECT_API"), Description = "Project Service" });
            options.AddServer(new OpenApiServer { Url = Environment.GetEnvironmentVariable("ANALYTICS_API"), Description = "Analytics Service" });
            options.AddServer(new OpenApiServer { Url = Environment.GetEnvironmentVariable("USER_API"), Description = "User Service" });
        });
    }
}
