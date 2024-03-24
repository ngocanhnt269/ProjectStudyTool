using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectStudyTool.Models;
using ProjectStudyTool.Data;
using ProjectStudyTool.Converter;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ProjectStudyTool.Services;
public class CardService
{
    private readonly ApplicationDbContext _context;

    public CardService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /**
     * Create a new card set from text and add a CardSet and Cards to the database. 
     * @param text, the inputed text, from gpt.
     * @param name, Requires a name for the card set.
     * @param userId, the user id, default is 1.
     * @return cardSet.CardSetId, the id of the created card set.
     */
    // public CardSet? CreateCardSetFromText(string text, string name, int userId = 1)
    // {
    //     // Convert the text to an array of cards
    //     var cards = TextConverter.convertTextToCardArray(text);

    //     // TODO: remove
    //     Console.WriteLine("Cards: " + cards.Length);
    //     for (int i = 0; i < cards.Length; i++)
    //     {
    //         Console.WriteLine("Question: " + cards[i].Question);
    //         Console.WriteLine("Answer: " + cards[i].Answer);
    //     }

    //     if (cards.Length == 0)
    //     {
    //         Console.WriteLine("No cards found in text");
    //         return null;
    //     }

    //     // Create a new card set
    //     var cardSet = new CardSet
    //     {
    //         Name = name,
    //         UserId = userId,
    //         CreatedDate = DateTime.Now,
    //         ModifiedDate = DateTime.Now,
    //     };


    //     //TODO: remove
    //     Console.WriteLine("CardSet: " + cardSet.Name);
    //     Console.WriteLine("CardSet: " + cardSet.UserId);
    //     Console.WriteLine("CardSet: " + cardSet.CreatedDate);
    //     Console.WriteLine("CardSet: " + cardSet.ModifiedDate);
    //     Console.WriteLine("CardSet: " + cardSet.Cards);
    
    //     if (cardSet.UserId == 0)
    //     {
    //         Console.WriteLine("User ID is required");
    //         return null;
    //     }

    //     // Add the card set to the database
    //     CreateCardSet(cardSet);

    //     // Add the cards to the database
    //     // for (int i = 0; i < cards.Length; i++)
    //     // {
    //     //     cards[i].CardSetId = cardSet.CardSetId;
    //     //     CreateCard(cards[i]);
    //     // }
    //     for (Card card in cards)
    //     {
    //         // print card
    //         Console.WriteLine("Card: " + card.Question);
    //         Console.WriteLine("Card: " + card.Answer);
    //         card.CardSetId = cardSet.CardSetId;

    //     }
    //     return cardSet;
    // }
public CardSet? CreateCardSetFromText(string text, string name, int userId = 1)
{
    // Convert the text to an array of cards
    var cards = TextConverter.convertTextToCardArray(text);

    if (cards.Length == 0)
    {
        Console.WriteLine("No cards found in text");
        return null;
    }

    // Create a new card set
    var cardSet = new CardSet
    {
        Name = name,
        UserId = userId,
        CreatedDate = DateTime.Now,
        ModifiedDate = DateTime.Now,
        Cards = new List<Card>()  // Initialize the Cards collection
    };

    // Add the card set to the database
    CreateCardSet(cardSet);

    // Assign the CardSetId to each card and add them to the card set
    foreach (var card in cards)
    {
        card.CardSetId = cardSet.CardSetId;
        cardSet.Cards.Add(card);  // Add card to the card set's collection
    }

    // Save changes to the database
    _context.SaveChanges();

    return cardSet;
}

    public List<Card> CreateCardsByCardSetId(int cardSetId)
    {
        var cards = GetCardsByCardSetId(cardSetId);
        foreach (var card in cards)
        {
            _context.Cards!.Add(card);
        }
        _context.SaveChanges();
        return cards;
    }

    // Create a new card set from text
    public void CreateCardsFromText(string text, int cardSetId)
    {
        // Convert the text to an array of cards
        var cards = TextConverter.convertTextToCardArray(text);

        if (cards.Length == 0)
        {
            Console.WriteLine("No cards found in text");
            return;
        }

        // Add the cards to the database
        foreach (var card in cards)
        {
            card.CardSetId = cardSetId;
            CreateCard(card);
        }
    }
    
    // Get all cards in a card set
    public List<Card> GetCardsByCardSetId(int cardSetId)
    {
        return _context.Cards!.Where(c => c.CardSetId == cardSetId).ToList();
    }

    /**
        Card
    */
    // Create a new card
    public void CreateCard(Card card)
    {
        _context.Cards!.Add(card);
        _context.SaveChanges();
    }

    // Get all cards
    public List<Card> GetAllCards()
    {
        return _context.Cards!.Include(c => c.PossibleAnswers).ToList();
    }

    // Get card by ID
    public Card? GetCardById(int id)
    {
        return _context.Cards!.Include(c => c.PossibleAnswers).FirstOrDefault(c => c.CardId == id);
    }

    // Update a card
    public void UpdateCard(Card card)
    {
        _context.Cards!.Update(card);
        _context.SaveChanges();
    }

    // Delete a card
    public void DeleteCard(int id)
    {
        var card = _context.Cards!.Find(id);
        if (card != null)
        {
            _context.Cards!.Remove(card);
            _context.SaveChanges();
        }
    }

    /**
        CardSet
    */
    // Get all card sets for a user
    public List<CardSet> GetCardSetsByUserId(int userId)
    {
        return _context.CardSets!.Include(cs => cs.Cards).Where(cs => cs.UserId == userId).ToList();
    }

    // Get the most recent card set ID
    public int GetLatestCardSetId()
    {
        var firstCard = _context.CardSets!.OrderByDescending(cs => cs.CardSetId).FirstOrDefault();
        if (firstCard == null)
        {
            return 0;
        }
        return firstCard.CardSetId;
    }

    // Get the most recent card set
    public CardSet? GetLatestCardSet()
    {
        return _context.CardSets!.Include(cs => cs.Cards).OrderByDescending(cs => cs.CardSetId).FirstOrDefault();
    }

    // Create a new card set
    public CardSet CreateCardSet(CardSet cardSet)
    {
        _context.CardSets!.Add(cardSet);
        _context.SaveChanges();
        return cardSet;
    }

    // Get all card sets
    public List<CardSet> GetAllCardSets()
    {
        return _context.CardSets!.Include(cs => cs.Cards).ToList();
    }

    // Get card set by ID
    public CardSet? GetCardSetById(int id)
    {
        return _context.CardSets!.Include(cs => cs.Cards).FirstOrDefault(cs => cs.CardSetId == id);
    }

    // Update a card set
    public void UpdateCardSet(CardSet cardSet)
    {
        _context.CardSets!.Update(cardSet);
        _context.SaveChanges();
    }

    // Delete a card set
    public void DeleteCardSet(int id)
    {
        var cardSet = _context.CardSets!.Find(id);
        if (cardSet != null)
        {
            _context.CardSets!.Remove(cardSet);
            _context.SaveChanges();
        }
    }

}
