using Microsoft.Extensions.Options;
using PusherServer;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure CustomPusherOptions from appsettings.json
            builder.Services.Configure<CustomPusherOptions>(builder.Configuration.GetSection("Pusher"));

            // Add Pusher as a singleton service
            builder.Services.AddSingleton<Pusher>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<CustomPusherOptions>>().Value;
                return new Pusher(
                    options.AppId,
                    options.Key,
                    options.Secret,
                    new PusherOptions
                    {
                        Cluster = options.Cluster,
                        Encrypted = true
                    });
            });

            // Add services to the container.
            builder.Services.AddControllers();

            
            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500") // Specific origin
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins"); // CORS should be before UseAuthorization
            app.UseAuthorization();
            app.MapControllers();


            app.Run();
        }
    }
}
