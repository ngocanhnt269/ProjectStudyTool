using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStudyTool.Models
{
    public class Pdf
    {
        [Key]
        public int PdfId { get; set; }

        [ForeignKey("CardSet")]
        public int CardSetId { get; set; }

        // Navigation property for the card set associated with this PDF
        public CardSet? CardSet { get; set; }

        // Property to store the slide content
        public string? SlideContent { get; set; }

        // Property to store the flashcards generated from the slide
        public ICollection<Card>? GeneratedFlashcards { get; set; }
    }
}