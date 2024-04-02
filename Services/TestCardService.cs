using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStudyTool.Services;
public class TestCardService
{
    private readonly CardService _cardService;
    public TestCardService(CardService cardService)
    {
        _cardService = cardService;
    } 

    public void testGetCardsByCardSetId(int cardSetId)
    {
        Console.WriteLine("-- Starting Test --");

        var cards = _cardService?.GetCardsByCardSetId(cardSetId);
        foreach (var card in cards!)
        {
            Console.WriteLine("CardID: " + card.CardId);
            Console.WriteLine("Question: " + card.Question);
            Console.WriteLine("Answer: " + card.Answer);
            Console.WriteLine("CardSetId: " + card.CardSetId);
            Console.WriteLine();
        }
        Console.WriteLine("-- Ending Test --");
    }
    
    public void testCreateCardsFromText() {
        Console.WriteLine("-- Starting Test --");
        _cardService?.CreateCardsFromText(getTestString(), 1);
        Console.WriteLine("-- Ending Test --");
    }

    public void testGetAllCards()
    {
        Console.WriteLine("-- Starting Test --");

        var cards = _cardService?.GetAllCards();
        foreach (var card in cards!)
        {
            Console.WriteLine("CardID: " + card.CardId);
            Console.WriteLine("Question: " + card.Question);
            Console.WriteLine("Answer: " + card.Answer);
            Console.WriteLine("CardSetId: " + card.CardSetId);
            Console.WriteLine();
        }
        Console.WriteLine("-- Ending Test --");
    }
    public void testCreateCardSetFromText(string text, string name = "Linux 1", int userId = 1) {

        Console.WriteLine("-- Starting Test --");
        _cardService?.CreateCardSetFromText(text, name, userId );

        Console.WriteLine(" Cards created: ");
        
        var cards = _cardService?.GetAllCards();
        foreach (var card in cards!)
        {
            Console.WriteLine(card.Question);
            Console.WriteLine(card.Answer);
            Console.WriteLine();
        }

        // Get the latest card set and print its details
        // var createdCardSet = _cardService?.GetLatestCardSet();
        // Console.WriteLine("CardSetId: " + createdCardSet!.CardSetId);
        // Console.WriteLine("Name: " + createdCardSet.Name);
        // Console.WriteLine("UserId: " + createdCardSet.UserId);
        // Console.WriteLine("CreatedDate: " + createdCardSet.CreatedDate);
        // Console.WriteLine("ModifiedDate: " + createdCardSet.ModifiedDate);
        // Console.WriteLine("PDF File URL: " + createdCardSet.PdfFileUrl);
        // Console.WriteLine();
        
        Console.WriteLine("-- Ending Test --");
    }
    public string getTestString()
    {
        return "QUESTION 1: Who announced the GNU Project in 1983? QUESTION 2: What does GNU stand for in the context of the GNU Project? QUESTION 3: What is the purpose of the GNU Project? QUESTION 4: What did Richard Stallman present alongside the announcement of the GNU Project? QUESTION 5: How did Stallman clarify a potential confusion regarding the GNU Project's distribution? QUESTION 6: Where can you find the GNU Manifesto online? QUESTION 7: What is the intention behind the GNU system according to the manifesto? QUESTION 8: How does Stallman explain the confusion around the distribution of GNU copies? QUESTION 9: What service does the manifesto mention companies providing for profit? QUESTION 10: What is the URL for Richard Stallman's personal website? QUESTION 11: How does the GNU system aim to be distributed according to Stallman? QUESTION 12: Which year did Stallman announce the GNU Project? QUESTION 13: What type of software system is the GNU Project creating? QUESTION 14: Why did Stallman add a footnote to a sentence in the Manifesto? QUESTION 15: What does GNU's Not UNIX imply about the project? QUESTION 16: How is the GNU system supposed to be given away according to the manifesto? QUESTION 17: What was never the intent regarding the distribution of GNU according to Stallman? QUESTION 18: Where can you find the initial announcement of the GNU Project online? QUESTION 19: What did Stallman intend in terms of payment for using the GNU system? QUESTION 20: What did Stallman imply about companies in the GNU Manifesto? ANSWER 1: Richard Stallman. ANSWER 2: GNU stands for Gnu’s Not UNIX. ANSWER 3: To create an operating system, kernel, and system programs to give away for free. ANSWER 4: The GNU Manifesto. ANSWER 5: Stallman clarified that nobody would have to pay for permission to use the GNU system; however, the distribution may involve a charge. ANSWER 6: www.gnu.org/gnu/manifesto.html ANSWER 7: To be given away for free to everyone who can use it. ANSWER 8: Stallman explained that the words in the manifesto were not clear and could be misinterpreted as always being distributed at little or no charge. ANSWER 9: The manifesto mentions the possibility of companies providing the service of distribution for a profit. ANSWER 10: www.stallman.org ANSWER 11: The GNU system is supposed to be given away free to everyone. ANSWER 12: In 1983. ANSWER 13: A UNIX-compatible software system. ANSWER 14: Stallman added a footnote to clarify confusion that the wording was careless and could be misinterpreted. ANSWER 15: It implies that the project is not Unix but an alternative to it. ANSWER 16: The GNU system is supposed to be given away so that everyone can use it. ANSWER 17: That copies of GNU should always be distributed at little or no charge. ANSWER 18: www.gnu.org/gnu/initial-announcement.html ANSWER 19: That nobody would have to pay for permission to use the GNU system. ANSWER 20: The manifesto mentions the possibility of companies providing the service of distribution for a profit.";
        
    }
}