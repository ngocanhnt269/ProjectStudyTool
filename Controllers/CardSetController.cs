using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectStudyTool.Controllers;

public class CardSetController : Controller
{
    private readonly ApplicationDbContext _context;

    public CardSetController(ApplicationDbContext context)
    {
        _context = context;
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
