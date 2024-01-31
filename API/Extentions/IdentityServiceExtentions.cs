using Core.Entities.Identity;
using Infrastructue.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions;

public static class IdentityServiceEstentions
{
    public static IServiceCollection AddIdentityServcies(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppIdentityDbContext>( opt =>
        {
            opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
        });

        services.AddIdentityCore<AppUser>(opt => 
        {
            // add identity options here, if necessary
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager<SignInManager<AppUser>>();

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}