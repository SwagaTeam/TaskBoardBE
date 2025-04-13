using UserService.DataLayer;
using UserService.Initializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiGateway", policy =>
    {
        policy.WithOrigins("https://localhost:7000")  // Здесь указываем адрес ApiGateway
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

DbContextInitializer.Initialize(builder.Services, configuration["ConnectionStrings:DefaultConnection"]);

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var appDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

DbContextInitializer.Migrate(appDbContext);

app.UseCors("AllowApiGateway");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
