

namespace ProjectStudyTool.Data;
public static class SeedData
{
    public static void Seed(this ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>().HasData(
            GetUsers()
        );
    }

    private static List<User> GetUsers()
    {
        List<User> users = new List<User>();
        users.Add(new User { UserId = 1, Username = "aa@aa.aa", Password = "password" });
        users.Add(new User { UserId = 2, Username = "bb@bb.bb", Password = "password" });
        return users;
    }
}
