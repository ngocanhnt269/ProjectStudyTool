using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectStudyTool.Data;
using ProjectStudyTool.Models;

namespace ProjectStudyTool.Controllers
{
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Card
        public async Task<IActionResult> Index()
        {
            // var applicationDbContext = _context.Card.Include(c => c.CardSet);
            // return View(await applicationDbContext.ToListAsync());
            var fakeCard = new Card
            {
                CardId = 1,
                CardSetId = 1, // Assuming CardSetId exists in your database
                QuestionId = 1,
                Question = "What is the capital of France?",
                Answer = "Paris",
                CardSet = new CardSet { CardSetId = 1, Name = "Geography" } // Assuming CardSet is related to Card
            };

            // Add the fake card to a list
            var fakeCardList = new List<Card> { fakeCard };

            // Combine the fake card list with real database data
            var cardList = await _context.Card.Include(c => c.CardSet).ToListAsync();
            cardList.AddRange(fakeCardList);

            return View(cardList);
        }

        // GET: Card/Details/5
        public async Task<IActionResult> Details(int? id)
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
}
