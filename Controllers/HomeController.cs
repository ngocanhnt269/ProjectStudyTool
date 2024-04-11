using Radzen.Blazor.Rendering;

namespace ProjectStudyTool.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // TODO: remove this later
    private readonly CardService _cardService;
    private readonly UserManager<IdentityUser> _userManager;
    public HomeController(ILogger<HomeController> logger, CardService cardService, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _cardService = cardService;
        _userManager = userManager;
    }

    [HttpGet]
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Get user's study contents on Home page
    [HttpPost]
    public async Task<IActionResult> IndexAsync(string userContent)
    {
        // Console.WriteLine("************User content:\n" + userContent);
        if (!ValidateContent(userContent))
        {
            Console.WriteLine("Empty user content");
            return Redirect("/Home");
        }
        // Console.WriteLine(userContent);
        var openAiService = new OpenAiService();
        var response = await openAiService.UseOpenAiService(userContent);
        ViewBag.ResponseContent = response[^1].Content;

        // if response content is empty, return to Home page
        if (response[^1].Content == null)
        {
            Console.WriteLine("Empty response content");
            return Redirect("/Home");
        }

        // if current user is guest user, create temporary cards and store them in TempData
        if (!User.Identity!.IsAuthenticated)
        {
            var allTemporaryCards = _cardService.CreateCardsFromTextForNonLoggedInUser(response[^1].Content!);
            TempData["AllTemporaryCardsJSON"] = JsonSerializer.Serialize(allTemporaryCards);
            return RedirectToAction("Index", "Card");
        }

        // if current user is logged in, create card set and store it in database
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || _userManager.FindByIdAsync(currentUserId) == null)
        {
            Console.WriteLine("Current user ID is null");
            return Redirect("/Home");
        }
        var cardSet = _cardService.CreateCardSetFromText(response[^1].Content!, "My Study Set", currentUserId!);
        
        if (cardSet == null)
        {
            Console.WriteLine("CardSet is null");
            return Redirect("/Home");
        }

        var cardSetId = cardSet!.CardSetId;   
        
        // go to Edit page of CardSet that was just created
        return RedirectToAction("Edit", "CardSet", new { id = cardSetId });
    }

    // Validate user's study contents
    public bool ValidateContent(string userContent)
    {
        if (string.IsNullOrEmpty(userContent))
        {
            return false;
        }
        return true;
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 10485760)] // Limit file size to 10MB
    [Route("Index/FileUpload")]
    public async Task<IActionResult> IndexGetFileAsync(IFormFile file) {
        if (file != null && file.Length > 0)
        {
            string content = "";
            if (file.ContentType == "text/plain") {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    content = await reader.ReadToEndAsync();
                }
            } else if (file.ContentType == "application/pdf") {
                using (var ms = new MemoryStream()) {
                    await file.CopyToAsync(ms);
                    ms.Position = 0;
                    using (var pdf = PdfDocument.Open(ms)) {
                        foreach (var page in pdf.GetPages()) {
                            content += page.Text;
                        }
                    }
                }
            }
            // trim content to 2000 characters, remove spaces between lines
            content = content.Substring(0, Math.Min(content.Length, 2000)).Replace("\n", "").Replace("\r", "").Replace("\t", "");
            return await IndexAsync(content);
        }
        Console.WriteLine("File is null or empty");
        return Redirect("/Home");
    }

    // TODO: To test things - to remove later
    public IActionResult Test()
    {
        // TestCardService testCardService = new TestCardService(_cardService);
        // var testString = testCardService.getTestString();

        // testCardService.testCreateCardSetFromText(testString, "Linux 1", 1);
        // testCardService.testGetAllCards();

        // _cardService.CreateCardSetFromText(testString, "Linux 1", 1);
        return View("Test");

    }

    public IActionResult PrintAllCards()
    {
        var cards = _cardService.GetAllCards();
        foreach (var card in cards)
        {
            Console.WriteLine("CardID: " + card.CardId);
            Console.WriteLine("Question: " + card.Question);
            Console.WriteLine("Answer: " + card.Answer);
            Console.WriteLine("CardSetId: " + card.CardSetId);
            Console.WriteLine();
        }
        return View("Test");
    }
    
    /**
    * Create a default card set for testing and loading new CardSet. UserId 1 is used by default.
    */
    [HttpPost]
    public IActionResult CreateDefaultCardSet()
    {
        TestCardService testCardService = new TestCardService(_cardService);
        string testString = testCardService.getTestString();

        testCardService.testCreateCardSetFromText(testString, "Linux 1", "1");
        testCardService.testGetAllCards();

        return View("Test");
    }

    [HttpPost]
    public IActionResult CreateCardSetFromText(string text, string name, string userId)
    {
        // Create a new card set from text
        var createdCardSet = _cardService.CreateCardSetFromText(text, name, userId);

        // Get the latest card set and print its details
        var cards = _cardService.GetCardsByCardSetId(createdCardSet!.CardSetId);

        foreach (var card in cards)
        {
            Console.WriteLine("CardID: " + card.CardId);
            Console.WriteLine("Question: " + card.Question);
            Console.WriteLine("Answer: " + card.Answer);
            Console.WriteLine("CardSetId: " + card.CardSetId);
            Console.WriteLine();
        }

        return View("Test");
    }

    [HttpPost]
    public IActionResult GetCardsByCardSetId(int cardSetId)
    {
        var cards = _cardService.GetCardsByCardSetId(cardSetId);
        foreach (var card in cards)
        {
            Console.WriteLine("CardID: " + card.CardId);
            Console.WriteLine("Question: " + card.Question);
            Console.WriteLine("Answer: " + card.Answer);
            Console.WriteLine("CardSetId: " + card.CardSetId);
            Console.WriteLine();
        }
        return View("Test");
    }

    [HttpPost]
    public IActionResult GetCardSetsByUserId(string userId1)
    {
        var cardSets = _cardService.GetCardSetsByUserId(userId1);
        foreach (var cardSet in cardSets)
        {
            Console.WriteLine("CardSetId: " + cardSet.CardSetId);
            Console.WriteLine("Name: " + cardSet.Name);
            Console.WriteLine("UserId: " + cardSet.UserId);
            Console.WriteLine("CreatedDate: " + cardSet.CreatedDate);
            Console.WriteLine("ModifiedDate: " + cardSet.ModifiedDate);
            Console.WriteLine();
        }
        return View("Test");
    }
}
