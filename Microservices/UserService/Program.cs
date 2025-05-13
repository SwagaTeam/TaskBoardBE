using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Auth;
using SharedLibrary.Dapper;
using SharedLibrary.Middleware;
using System.Security.Claims;
using System.Text;
using UserService.BusinessLayer.Manager;
using UserService.DataLayer;
using UserService.DataLayer.Repositories.Abstractions;
using UserService.DataLayer.Repositories.Implementations;
using UserService.Initializers;


internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
            Env.Load();

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        await using var appDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        await DbContextInitializer.Migrate(appDbContext);

        app.UseCors("AllowApiGateway");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<JwtBlacklistMiddleware>();


        app.UseAuthorization();

        var avatarPath = Environment.GetEnvironmentVariable("AVATAR_STORAGE_PATH");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(avatarPath),
            RequestPath = "/avatars"
        });

        app.MapControllers();

        await app.RunAsync();
    }

    public static void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowApiGateway", policy =>
            {
                policy.WithOrigins(Environment.GetEnvironmentVariable("GATEWAY")!) // ����� ��������� ����� ApiGateway
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "������� 'Bearer' [������] ��� �����������",
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
                    new string[] { }
                }
            });
        });

        AddAuthentication(services, configuration);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserManager, UserManager>();

        services.AddScoped<IAuth, Auth>();
        services.AddScoped<Auth>();
        services.AddScoped<IEncrypt, Encrypt>();

        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var host = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var port = Environment.GetEnvironmentVariable("PORT");
        var database = Environment.GetEnvironmentVariable("DATABASE");
        var user = Environment.GetEnvironmentVariable("USERNAME");
        var pass = Environment.GetEnvironmentVariable("PASSWORD");

        var conn = $"Host={host};Port={port};Database={database};Username={user};Password={pass}";

        DbContextInitializer.Initialize(services, conn);
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
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }
}