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

    private static List<Card> GetCards()
    {
        List<Card> cards = new List<Card>();
        cards.Add(new Card { CardId = 1, Question = "What is the capital of France?", Answer = "Paris" });
        cards.Add(new Card { CardId = 2, Question = "What is the capital of Germany?", Answer = "Berlin" });
        return cards;
    }

    private static List<CardSet> GetCardSets()
    {
        List<CardSet> cardSets = new List<CardSet>();
        cardSets.Add(new CardSet { CardSetId = 1, Name = "European Capitals" });
        return cardSets;
    }
}
