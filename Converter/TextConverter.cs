using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProjectStudyTool.Converter;

public class TextConverter
{
    public static Card[] convertTextToCardArray(string input)
    {
        // Split the input into two parts: questions and answers
        string[] parts = Regex.Split(input, @"(?=ANSWER)");
        
        // Split the questions and answers into separate strings
        string[] questions = Regex.Split(parts[0], @"QUESTION \d+:")
            .Where(q => !string.IsNullOrWhiteSpace(q))
            .Select(q => q.Trim()).ToArray();
        string[] answers = parts.Skip(1)
            .Select(a => Regex.Replace(a, @"^ANSWER \d+:\s*", string.Empty).Trim())
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .ToArray();
        
        // Check if the number of questions and answers match        
        if (questions.Length != answers.Length)
        {
            Console.WriteLine("ERROR: TextConverter: Number of questions and answers do not match");
            Console.WriteLine("Questions: " + questions.Length);
            Console.WriteLine("Answers: " + answers.Length);
            for (int i = 0; i < questions.Length; i++)
            {
                Console.WriteLine("Question: " + questions[i]);
            }
            for (int i = 0; i < answers.Length; i++)
            {
                Console.WriteLine("Answer: " + answers[i]);
            }
            throw new ArgumentException("Number of questions and answers do not match");
        }

        // Create an array of cards
        var cards = new Card[Math.Min(questions.Length, answers.Length)];
        for (int i = 0; i < cards.Length; i++)
        {
            var card = new Card
            {
                Question = questions[i],
                Answer = answers[i]
            };
            cards[i] = card;
        } 
        
        return cards;
    }
}
