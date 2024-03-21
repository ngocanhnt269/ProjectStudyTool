
namespace ProjectStudyTool.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }

    // uncomment the following line when we have the CardSet model
    public List<CardSet>? CardSets { get; set; }
}
