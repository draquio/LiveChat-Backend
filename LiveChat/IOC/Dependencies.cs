﻿using System.Text;
using LiveChat.Context;
using LiveChat.Repositories;
using LiveChat.Repositories.Interfaces;
using LiveChat.Services;
using LiveChat.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IPasswordHasher, PasswordHasher>();
            service.AddScoped<IChatRoomService, ChatRoomService>();

            // Repositories
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IMessageRepository, MessageRepository>();
            service.AddScoped<ITokenRepository, TokenRepository>();
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IChatRoomRepository, ChatRoomRepository>();

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

            // Authentication - JWT Context
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accesToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accesToken) && path.StartsWithSegments("/chathub"))
                            {
                                context.Token = accesToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // Authorization
            service.AddAuthorization();
        }
    }
}
