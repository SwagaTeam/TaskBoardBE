using SharedLibrary.ProjectModels;

namespace Messaging.Kafka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.MapGet("/", () => "Kafka is working");

            app.UseCors("AllowAll");

            app.Run();
        }
    }
}
