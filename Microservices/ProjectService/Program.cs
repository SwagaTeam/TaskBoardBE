using SharedLibrary.ProjectModels;
using Kafka.Messaging;
using Kafka.Messaging.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiGateway", policy =>
    {
        policy.WithOrigins("https://localhost:7000")  // ����� ��������� ����� ApiGateway
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProducer<TaskModel>(builder.Configuration.GetSection("Kafka:NotificationTask"));
builder.Services.AddConsumer<TaskModel, TaskCreatedMessageHandler>(builder.Configuration.GetSection("Kafka:NotificationTask"));


var app = builder.Build();

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
