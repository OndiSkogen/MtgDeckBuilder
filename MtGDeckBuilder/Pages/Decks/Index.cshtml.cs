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
    public class IndexModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public IndexModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        public IList<Deck> Deck { get;set; }

        public async Task OnGetAsync()
        {
            Deck = await _context.Deck.ToListAsync();
        }
    }
}
