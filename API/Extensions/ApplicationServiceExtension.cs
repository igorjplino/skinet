﻿using API.Errors;
using Core.Interfaces;
using Infrastructue.Data;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
            return ConnectionMultiplexer.Connect(options);
        });

        services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
            });
        });

        return services;
    }
}
