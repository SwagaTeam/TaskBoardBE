using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Kafka.Messaging.Services.Abstractions;
using Kafka.Messaging.Services.Implementations;
using Kafka.Messaging.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectService.BusinessLayer;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.BusinessLayer.Implementations;
using ProjectService.DataLayer;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.DataLayer.Repositories.Implementations;
using ProjectService.Initializers;
using SharedLibrary.Auth;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Middleware;
using SharedLibrary.Models.KafkaModel;

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

        var documentPath = Environment.GetEnvironmentVariable("DOCUMENT_STORAGE_PATH");
        var attachmentPath = Environment.GetEnvironmentVariable("ATTACHMENT_STORAGE_PATH");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(documentPath),
            RequestPath = "/documents"
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(attachmentPath),
            RequestPath = "/attachments"
        });

        app.UseMiddleware<JwtBlacklistMiddleware>();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }


    private static void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka:NotificationTask"));
        services.AddTransient<ForwardAccessTokenHandler>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IBoardManager, BoardManager>();
        services.AddScoped<IProjectLinkManager, ProjectLinkManager>();
        services.AddHttpClient<IItemManager, ItemManager>
            (client => client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ANALYTICS_SERVICE") + "/analytics/"))
            .AddHttpMessageHandler<ForwardAccessTokenHandler>();
        services.AddScoped<IValidateBoardManager, ValidateBoardManager>();
        services.AddScoped<IValidateItemManager, ValidateItemManager>();
        services.AddScoped<IValidateDocumentManager, ValidateDocumentManager>();
        services.AddScoped<IValidateSprintManager, ValidateSprintManager>();
        services.AddScoped<IValidateStatusManager, ValidateStatusManager>();
        services.AddScoped<IProjectLinkRepository, ProjectLinkRepository>();
        services.AddScoped<IProjectManager, ProjectManager>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IContributorsRepository, ContributorsRepository>();
        services.AddScoped<IContributorsManager, ContributorsManager>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IBoardManager, BoardManager>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IUserProjectManager, UserProjectManager>();
        services.AddScoped<IUserProjectRepository, UserProjectRepository>();
        services.AddScoped<IItemTypeManager, ItemTypeManager>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IStatusManager, StatusManager>();
        services.AddScoped<IItemBoardsRepository, ItemBoardsRepository>();
        services.AddScoped<ISprintManager, SprintManager>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<IDocumentManager,  DocumentManager>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddSingleton<IHostedService, KafkaConsumer<TaskEventMessage>>();
        services.AddScoped<IAuth, Auth>();
        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IMessageHandler<TaskEventMessage>, TaskEventMessageHandler>();

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

        services.AddSingleton<IKafkaProducer<TaskEventMessage>, KafkaProducer<TaskEventMessage>>();


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