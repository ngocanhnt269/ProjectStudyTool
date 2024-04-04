using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectStudyTool.Controllers;

public class CardController : Controller
{
    private readonly ApplicationDbContext _context;

    public CardController(ApplicationDbContext context)
    {
        _context = context;
    }


    // GET: Card
    public IActionResult Index()
    {
        var cardList = new List<Card>();
        // If user is not logged in, display temporary cards
        if (!User.Identity!.IsAuthenticated)
        {
            var cardListJson = TempData["AllTemporaryCardsJSON"];
            cardList = JsonSerializer.Deserialize<List<Card>>(cardListJson!.ToString()!);
        }
        // If user is logged in, display user's all cards
        else
        {
            var currentUserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            cardList = _context.Card.Where(c => c.CardSet!.UserId == currentUserId).ToList(); //only show cards that belong to the user
            if (cardList.Count == 0)
            {
                Console.WriteLine("No cards found for user");
            }
        }
        return View(cardList);
    }

    // GET: Card/Create
    public IActionResult Create()
    {
        ViewData["CardSetId"] = new SelectList(_context.Set<CardSet>(), "CardSetId", "Name");
        return View();
    }

    // POST: Card/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CardId,CardSetId,QuestionId,Question,Answer")] Card card)
    {
        if (ModelState.IsValid)
        {
            _context.Add(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CardSetId"] = new SelectList(_context.Set<CardSet>(), "CardSetId", "Name", card.CardSetId);
        return View(card);
    }

    // GET: Card/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var card = await _context.Card.FindAsync(id);
        if (card == null)
        {
            return NotFound();
        }
        ViewData["CardSetId"] = new SelectList(_context.Set<CardSet>(), "CardSetId", "Name", card.CardSetId);
        return View(card);
    }

    // POST: Card/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CardId,CardSetId,QuestionId,Question,Answer")] Card card)
    {
        if (id != card.CardId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(card);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(card.CardId))
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
        ViewData["CardSetId"] = new SelectList(_context.Set<CardSet>(), "CardSetId", "Name", card.CardSetId);
        return View(card);
    }

    // GET: Card/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var card = await _context.Card
            .Include(c => c.CardSet)
            .FirstOrDefaultAsync(m => m.CardId == id);
        if (card == null)
        {
            return NotFound();
        }

        return View(card);
    }

    // POST: Card/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var card = await _context.Card.FindAsync(id);
        if (card != null)
        {
            _context.Card.Remove(card);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CardExists(int id)
    {
        return _context.Card.Any(e => e.CardId == id);
    }

}
