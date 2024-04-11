using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Extensions.Msal;
using Radzen;

namespace ProjectStudyTool.Controllers;

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
    // TODO: position the question and answer centered on the page
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


        // Iterate through each card in the card set
        for (int i = 0; i < cards.Count; i++)
        {
            // for each card create a new page for the question

            // TODO: create a div to center the text on the page
            // Div div = new Div()
            // .SetWidth(UnitValue.CreatePercentValue(100))
            // .SetHeight(UnitValue.CreatePercentValue(100));

            document.Add(
                new Paragraph(cards[i].Question)
                    .SetFontSize(24)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetTextAlignment(TextAlignment.CENTER)
                );
            document.Add(new AreaBreak());

            // for each card create a new page for the answer
            document.Add(
                new Paragraph(cards[i].Answer)
                    .SetFontSize(24)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetTextAlignment(TextAlignment.CENTER)
                ); 

            // Add AreaBreak if not the last card and not on the last iteration
            if (i < cards.Count - 1)
            {
                document.Add(new AreaBreak());
            }
        }

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
    // public async Task<IActionResult> DownloadPdf(int? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }

    //     var cardSet = await _context.CardSet
    //         .Include(c => c.Cards)
    //         .FirstOrDefaultAsync(m => m.CardSetId == id);
    //     var cards = await _cardService.GetCardsByCardSetIdAsync(id.Value); 

    //     if (cardSet == null)
    //     {
    //         return NotFound();
    //     }

    //     MemoryStream memoryStream = new MemoryStream();

    //     var pdfWriter = new PdfWriter(cardSet.Name + ".pdf");
    //     var pdfDocument = new PdfDocument(pdfWriter);
    //     var document = new Document(pdfDocument);
    //     pdfWriter.SetCloseStream(false);
        
    //     // Iterate through each card in the card set
    //     foreach (var card in cards)
    //     {
    //         // for each card create a new page for the question
    //         document.Add(new Paragraph(card.Question));
    //         document.Add(new AreaBreak());
    //         // for each card create a new page for the answer
    //         document.Add(new Paragraph(card.Answer));
    //         document.Add(new AreaBreak());
    //     }

    //     document.Close();
    //     byte[] byteInfo = memoryStream.ToArray();
    //     memoryStream.Write(byteInfo, 0, byteInfo.Length);
    //     memoryStream.Position = 0;

    //     FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/pdf"); 

    //     return fileStreamResult;
    // }



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
