using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStudyTool.Models;

public class Card
{
    [Key]
    [Display(Name = "Card ID")]
    public int CardId { get; set; }

    [ForeignKey("CardSet")]
    [Display(Name = "Card Set ID")]
    public int CardSetId { get; set; }

    [Display(Name = "Card Number")]
    public int QuestionId { get; set; }

    [Required(ErrorMessage = "Question is required")]
    [Display(Name = "Question")]
    public string? Question { get; set; }

    [Required(ErrorMessage = "Answer is required")]
    [Display(Name = "Answer")]
    public string? Answer { get; set; }

    // Navigation property for possible answers of this card
    public ICollection<PossibleAnswer>? PossibleAnswers { get; set; }

    // Navigation property for the card set this card belongs to
    public CardSet? CardSet { get; set; }
}
