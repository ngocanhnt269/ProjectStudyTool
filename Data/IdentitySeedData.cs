using Microsoft.AspNetCore.Identity;

namespace ProjectStudyTool.Data;
public static class IdentitySeedData
{
    public static async Task Initialize(ApplicationDbContext context,
        UserManager<IdentityUser> userManager) {
        context.Database.EnsureCreated(); // create the database if it doesn't exist

        string password4all = "P@$$w0rd";


        if (await userManager.FindByNameAsync("aa@aa.aa") == null){
            var user = new IdentityUser {
                UserName = "aa@aa.aa",
                Email = "aa@aa.aa",
                PhoneNumber = "6902341234",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
            }
        }

        if (await userManager.FindByNameAsync("bb@bb.bb") == null) {
            var user = new IdentityUser {
                UserName = "bb@bb.bb",
                Email = "bb@bb.bb",
                PhoneNumber = "7788951456",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
            }
        }

        if (await userManager.FindByNameAsync("cc@cc.cc") == null) {
            var user = new IdentityUser {
                UserName = "cc@cc.cc",
                Email = "cc@cc.cc",
                PhoneNumber = "6572136821",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
            }
        }

        if (await userManager.FindByNameAsync("dd@dd.dd") == null) {
            var user = new IdentityUser {
                UserName = "dd@dd.dd",
                Email = "dd@dd.dd",
                PhoneNumber = "6041234567",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
            }
        }
    }
}
