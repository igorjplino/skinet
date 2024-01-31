using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructue.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new AppUser
            {
                DisplayName = "Bob",
                Email = "bob@test.com",
                UserName = "bob@test.com",
                Address = new()
                {
                    FirstName = "bob",
                    LastName = "Bobbity",
                    Street = "10 The Street",
                    City = "New York",
                    State = "NY",
                    ZioCode = "90210"
                }
            };

            await userManager.CreateAsync(user, "Pa$$w0rD!");
        }
    }
}