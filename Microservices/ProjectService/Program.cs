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
using SharedLibrary.Auth;
using DotNetEnv;
using Microsoft.Extensions.FileProviders;

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
        using var appDbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        await DbContextInitializer.Migrate(appDbContext);

        app.UseCors("AllowApiGateway");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        var avatarPath = Environment.GetEnvironmentVariable("DOCUMENT_STORAGE_PATH");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(avatarPath),
            RequestPath = "/documents"
        });

        app.UseMiddleware<JwtBlacklistMiddleware>();
        
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
        services.AddScoped<IUserProjectManager, UserProjectManager>();
        services.AddScoped<IUserProjectRepository, UserProjectRepository>();
        services.AddScoped<IItemTypeManager, ItemTypeManager>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
        services.AddScoped <IStatusRepository, StatusRepository>();
        services.AddScoped<IStatusManager, StatusManager>();
        services.AddScoped<IItemBoardsRepository, ItemBoardsRepository>();
        services.AddScoped<ISprintManager, SprintManager>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<IDocumentManager,  DocumentManager>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();

        services.AddScoped<IAuth, Auth>();
        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

        services.AddProducer<ItemModel>(configuration.GetSection("Kafka:NotificationTask"));
        services.AddConsumer<ItemModel, TaskCreatedMessageHandler>(configuration.GetSection("Kafka:NotificationTask"));

        var host = Environment.GetEnvironmentVariable("HOST");
        var port = Environment.GetEnvironmentVariable("PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }
}