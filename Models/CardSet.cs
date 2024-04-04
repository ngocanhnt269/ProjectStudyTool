namespace ProjectStudyTool.Models;
public class CardSet
{
    [Key]
    [Display(Name = "Card Set ID")]
    public int CardSetId { get; set; }

    [ForeignKey("UserId")]
    public string? UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Name")]
    public string? Name { get; set; }

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Modified Date")]
    public DateTime ModifiedDate { get; set; }

    // Navigation property for cards in this set
    public ICollection<Card>? Cards { get; set; }

    // Property to store the path or URL of the PDF file
    [Display(Name = "PDF File")]
    public string? PdfFileUrl { get; set; }

    // Navigation property for the user who owns this card set
    public User? User { get; set; }
}
