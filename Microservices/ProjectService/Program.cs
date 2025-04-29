using SharedLibrary.ProjectModels;
using Kafka.Messaging;
using Kafka.Messaging.Services.Implementations;
using SharedLibrary.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ProjectService.Initializers;
using ProjectService.DataLayer;
using ProjectService.Services.MailService;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.BusinessLayer.Implementations;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.DataLayer.Repositories.Implementations;
using ProjectService.Models;
using ProjectService.Validator;
using SharedLibrary.Auth;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        using var appDbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        await DbContextInitializer.Migrate(appDbContext);

        app.UseCors("AllowApiGateway");

        // Configure the HTTP request pipeline.
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
        services.Configure<MailSettings>(
           configuration.GetSection(nameof(MailSettings))
        );

        services.AddTransient<IMailService, MailService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IBoardManager, BoardManager>();
        services.AddScoped<IProjectLinkManager, ProjectLinkManager>();
        services.AddScoped<IProjectLinkRepository, ProjectLinkRepository>();
        services.AddScoped<IProjectManager, ProjectManager>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemManager, ItemManager>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IValidateItemManager, ValidateItemManager>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IBoardManager, BoardManager>();
        services.AddScoped<IBoardRepository, BoardRepository>();

        services.AddScoped<IAuth, Auth>();
        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowApiGateway", policy =>
            {
                policy.WithOrigins("https://localhost:7000")  // ����� ��������� ����� ApiGateway
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
        });

        services.AddProducer<ItemModel>(configuration.GetSection("Kafka:NotificationTask"));
        services.AddConsumer<ItemModel, TaskCreatedMessageHandler>(configuration.GetSection("Kafka:NotificationTask"));
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }
}