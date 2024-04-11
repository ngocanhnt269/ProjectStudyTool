using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Extensions.Msal;
using Radzen;

namespace ProjectStudyTool.Controllers;

[Authorize]
public class CardSetController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly CardService _cardService;

    public CardSetController(ApplicationDbContext context, CardService cardService)
    {
        _context = context;
        _cardService = cardService; 
    }

    // GET: CardSet/DownloadPdf/5
    public async Task<IActionResult> DownloadPdf(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cardSet = await _context.CardSet
            .Include(c => c.Cards)
            .FirstOrDefaultAsync(m => m.CardSetId == id);
        var cards = await _cardService.GetCardsByCardSetIdAsync(id.Value); 

        if (cardSet == null || cards == null || cards.Count == 0)
        {
            return NotFound();
        }

        MemoryStream memoryStream = new MemoryStream();
        var pdfWriter = new PdfWriter(memoryStream);
        var pdfDocument = new PdfDocument(pdfWriter);
        var document = new Document(pdfDocument);
        pdfWriter.SetCloseStream(false);
        
        // create table with 2 columns and 1 row for each card with a header
        Table table = new Table(3, false);

        // title
        Div titleDiv = new Div();

        // string imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.ico");
        // try {
        //     ImageData imageData = ImageDataFactory.Create(imagePath);
        //     Image image = new Image(imageData);
        //     div.Add(image);
        // } catch (Exception ex) {
        //     Console.WriteLine(ex.Message);
        // }

        // add title to div
        Paragraph logo = new Paragraph("Project Study Tool")
            // .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(24);
        Paragraph title = new Paragraph(cardSet.Name)
            // .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(24);
        titleDiv.Add(logo);
        titleDiv.Add(title);
        document.Add(titleDiv);


        // Headings
        Cell cellQuestionId = new Cell(1,1)
        .SetTextAlignment(TextAlignment.CENTER)
        .Add(new Paragraph("Question ID"));
        Cell cellQuestion = new Cell(1,1)
        // .SetTextAlignment(TextAlignment.CENTER)
        .Add(new Paragraph("Question"));
        Cell cellAnswer = new Cell(1,1)
        // .SetTextAlignment(TextAlignment.CENTER)
        .Add(new Paragraph("Answer"));

        table.AddCell(cellQuestionId);
        table.AddCell(cellQuestion);
        table.AddCell(cellAnswer);

        // Iterate through each card in the card set
        // and add the question ID, question, and answer to the table
        for (int i = 0; i < cards.Count; i++)
        {
            Cell cellQuestionIdValue = new Cell(1,1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(cards[i].QuestionId.ToString()));
            Cell cellQuestionValue = new Cell(1,1)
                // .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(cards[i].Question));
            Cell cellAnswerValue = new Cell(1,1)
                // .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(cards[i].Answer));

            table.AddCell(cellQuestionIdValue);
            table.AddCell(cellQuestionValue);
            table.AddCell(cellAnswerValue);
        }

        // Add the table to the document
        document.Add(table);

        document.Close();
        byte[] byteInfo = memoryStream.ToArray();
        memoryStream.Write(byteInfo, 0, byteInfo.Length);
        memoryStream.Position = 0;

        FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/pdf")
        {
        FileDownloadName = cardSet.Name + ".pdf"
        };

        return fileStreamResult;
    }

    // GET: CardSet
    public IActionResult Index()
    {
        var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id;
        var cardSetList = _context.CardSet.Where(c => c.UserId == userId).ToList();//only show card sets that belong to the user
        return View(cardSetList);
    }

    // GET: CardSet/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cardSet = await _context.CardSet
            .Include(c => c.User)
            .FirstOrDefaultAsync(m => m.CardSetId == id);
        if (cardSet == null)
        {
            return NotFound();
        }

        return View(cardSet);
    }

    // GET: CardSet/Create
    public IActionResult Create()
    {
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "Password");
        return View();
    }

    // POST: CardSet/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CardSet cardSet)
    {
        cardSet.UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name)?.Id;
        if (ModelState.IsValid)
        {
            try
            {
                cardSet.CreatedDate = DateTime.Now;
                _context.Add(cardSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "An error occurred while saving the cardset.");
                return View(cardSet); // Redisplay the form with an error message
            }
        }
        return View(cardSet); // Redisplay the form with validation errors
    }


    // GET: CardSet/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cardSet = await _context.CardSet.FindAsync(id);
        if (cardSet == null)
        {
            return NotFound();
        }
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "Password", cardSet.UserId);
        return View(cardSet);
    }

    // POST: CardSet/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CardSetId,UserId,Name,CreatedDate,ModifiedDate,PdfFileUrl")] CardSet cardSet)
    {
        if (id != cardSet.CardSetId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cardSet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardSetExists(cardSet.CardSetId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "Password", cardSet.UserId);
        return View(cardSet);
    }

    // GET: CardSet/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cardSet = await _context.CardSet
            .Include(c => c.User)
            .FirstOrDefaultAsync(m => m.CardSetId == id);
        if (cardSet == null)
        {
            return NotFound();
        }

        return View(cardSet);
    }

    // POST: CardSet/Delete/5
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cardSet = await _context.CardSet.FindAsync(id);
        if (cardSet != null)
        {
            _context.CardSet.Remove(cardSet);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    private bool CardSetExists(int id)
    {
        return _context.CardSet.Any(e => e.CardSetId == id);
    }
}
