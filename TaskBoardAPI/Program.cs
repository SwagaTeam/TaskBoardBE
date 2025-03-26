using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });

    // Добавляем микросервисы
    options.AddServer(new OpenApiServer { Url = "https://localhost:7001", Description = "Project Service" });
    options.AddServer(new OpenApiServer { Url = "https://localhost:7002", Description = "User Service" });
    options.AddServer(new OpenApiServer { Url = "https://localhost:7003", Description = "Analytics Service" });

});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("https://localhost:7001/swagger/v1/swagger.json", "Project Service");
        c.SwaggerEndpoint("https://localhost:7002/swagger/v1/swagger.json", "User Service");
        c.SwaggerEndpoint("https://localhost:7003/swagger/v1/swagger.json", "Analytics Service");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
