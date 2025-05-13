using System.Security.Claims;
using System.Text;
using Contributors.BusinessLayer.Abstractions;
using Contributors.BusinessLayer.Implementations;
using Contributors.DataLayer.Repositories.Abstractions;
using Contributors.DataLayer.Repositories.Implementations;
using Contributors.Initilizers;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Auth;
using SharedLibrary.Middleware;

class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        Env.Load();

        var app = builder.Build();
        
        app.UseCors("AllowApiGateway");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<JwtBlacklistMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    
    private static void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddScoped<IAuth, Auth>();
        services.AddScoped<IContributorsManager, ContributorsManager>();
        services.AddScoped<IContributorsRepository, ContributorsRepository>();

        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowApiGateway", policy =>
            {
                policy.WithOrigins("http://localhost:5000")
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        AddAuthentication(services, configuration);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {

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
        });
        
        DbContextInitializer.Initialize(services, configuration["ConnectionStrings:DefaultConnection"]!);
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
}
