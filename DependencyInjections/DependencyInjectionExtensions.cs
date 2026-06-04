using System.Text;
using Application.Abstractions;
using Application.Services;
using Application.Settings;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Events;
using Redis;
using JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistance;
using Persistance.Repositories;
using StackExchange.Redis;
using LoginNotification;

namespace DependencyInjections;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddScoped<IEventHandler<MessageSentDomainEvent>, SignalRMessageSentHandler>();
        services.AddScoped<IEventHandler<UserRegisteredDomainEvent>, NotifyAllUsersOnUserRegisteredHandler>();
        services.AddScoped<IEventHandler<UserRegisteredDomainEvent>, SaveUserNotificationHandler>();
        services.AddScoped<IEventHandler<PostCreatedDomainEvent>, NotifyAllUsersOnPostCreatedHandler>();
        services.AddScoped<IEventHandler<PostCreatedDomainEvent>, SavePostNotificationHandler>();
        services.AddScoped<IEventHandler<PostCreatedDomainEvent>, IncrementLeaderboardOnPostCreatedHandler>();

        services.AddScoped<IRegisterUserService, RegisterUserService>();
        services.AddScoped<IGetUserService, GetUserService>();
        services.AddScoped<ILoginUserService, LoginUserService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ICreatePostService, CreatePostService>();
        services.AddScoped<IGetNotificationsServer, GetNotificationsService>();
        services.AddScoped<ISendMessage, SendMessage>();
        services.AddScoped<ICreateConversationService, CreateConversationService>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.Section));
        var settings = configuration.GetSection(RedisSettings.Section).Get<RedisSettings>();

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(settings.ConnectionString));
        services.AddScoped<ILeaderboardService, RedisLeaderboardService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IRefreshTokenRepostory, RefreshTokenRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<INotificationBroadcaster, LoggingNotificationBroadcaster>();


        return services;
    }

    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
        var jwtSettings = configuration.GetSection(JwtSettings.Section).Get<JwtSettings>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        if (jwtSettings == null)
        {
            throw new InvalidOperationException("JWT settings not found in configuration");
        }
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["just_cookie"];
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}