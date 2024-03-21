using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStudyTool.Models;
public class PossibleAnswer
{
    [Key]
    public int PossibleAnswerId { get; set; }

    [ForeignKey("Card")]
    public int CardId { get; set; }

    [Required(ErrorMessage = "Answer is required")]
    [Display(Name = "Answer")]
    public string? Answer { get; set; }

    // Navigation property for the card this possible answer belongs to
    public Card? Card { get; set; }
}
