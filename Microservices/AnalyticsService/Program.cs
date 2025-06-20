using AnalyticsService.Initializers;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Middleware;
using System.Security.Claims;
using System.Text;
using AnalyticsService.BusinessLayer.Abstractions;
using AnalyticsService.BusinessLayer.Implementations;
using AnalyticsService.DataLayer.Abstractions;
using AnalyticsService.DataLayer.Implementations;
using AnalyticsService.BusinessLayer;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Dapper.DapperRepositories;
using AnalyticsService.DataLayer;

namespace AnalyticsService;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
            Env.Load();

        builder.Configuration.AddEnvironmentVariables();

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        using var appDbContext = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();
        await DbContextInitializer.Migrate(appDbContext);

        app.UseCors("AllowApiGateway");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowApiGateway", policy =>
            {
                policy.WithOrigins(Environment.GetEnvironmentVariable("GATEWAY")!)  // ����� ��������� ����� ApiGateway
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        AddAuthentication(services, configuration);

        services.AddHttpContextAccessor();
        services.AddTransient<ForwardAccessTokenHandler>();
        services.AddScoped<ITaskManager, TaskManager>();
        services.AddScoped<ITaskHistoryRepository, TaskHistoryRepository>();
        services
            .AddHttpClient<ITaskManager, TaskManager>(client => client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("PROJECT_SERVICE") + "/item/"))
            .AddHttpMessageHandler<ForwardAccessTokenHandler>();
        services
            .AddHttpClient<IProjectManager, ProjectManager>(client =>client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("PROJECT_SERVICE")!))
            .AddHttpMessageHandler<ForwardAccessTokenHandler>();
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
                    new string[] {}
                }
            });

            var xmlFile = $"{AppDomain.CurrentDomain.FriendlyName}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        var host = Environment.GetEnvironmentVariable("HOST");
        var port = Environment.GetEnvironmentVariable("PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var user = Environment.GetEnvironmentVariable("USERNAME");
        var pass = Environment.GetEnvironmentVariable("PASSWORD");

        var conn = $"Host={host};Port={port};Database={database};Username={user};Password={pass}";

        var user_database = Environment.GetEnvironmentVariable("POSTGRES_USER_DB");
        var user_host = Environment.GetEnvironmentVariable("USER_HOST");

        var userConnection = $"Host={user_host};Port={port};Database={user_database};Username={user};Password={pass}";

        services.AddScoped<IUserRepository>(provider => new UserRepository(userConnection));

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }
}