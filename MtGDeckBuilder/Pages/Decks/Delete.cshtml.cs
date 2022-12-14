#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtGDeckBuilder.Data;
using MtGDeckBuilder.Models;

namespace MtGDeckBuilder.Pages.Decks
{
    public class DeleteModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public DeleteModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Deck Deck { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Deck = await _context.Deck.FirstOrDefaultAsync(m => m.Id == id);

            if (Deck == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Deck = await _context.Deck.FindAsync(id);

            if (Deck != null)
            {
                _context.Deck.Remove(Deck);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
