using LiveChat.Context;
using LiveChat.Repositories;
using LiveChat.Repositories.Interfaces;
using LiveChat.Services;
using LiveChat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.IOC
{
    public static class Dependencies
    {
        public static void InjectDependencies(this IServiceCollection service, IConfiguration configuration)
        {
            // SQL Server Database Context
            service.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            });

            // SignalR
            service.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10mb
            });

            // Services
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IMessageService, MessageService>();
            service.AddScoped<IFileCleanupService, FileCleanupService>();

            // Repositories
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IMessageRepository, MessageRepository>();

            // Cors policy
            service.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "http://localhost:4200", "http://127.0.0.1:5500")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();

        }
    }
}
