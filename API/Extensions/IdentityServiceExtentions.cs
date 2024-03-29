using Core.Entities.Identity;
using Infrastructue.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions;

public static class IdentityServiceEstentions
{
    public static IServiceCollection AddIdentityServcies(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppIdentityDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("IdentityConnection"));
        });

        services.AddIdentityCore<AppUser>(opt =>
        {
            // add identity options here, if necessary
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddSignInManager<SignInManager<AppUser>>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false,
                };
            });

        services.AddAuthorization();

        return services;
    }
}