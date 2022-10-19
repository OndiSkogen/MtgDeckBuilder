#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MtGDeckBuilder.CardObjects;
using MtGDeckBuilder.Data;
using MtGDeckBuilder.Models;
using Newtonsoft.Json;

namespace MtGDeckBuilder.Pages.Cards
{
    public class CreateModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public CreateModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Card Card { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string jsonObj = string.Empty;
            JsonCard card = null;

            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.scryfall.com/cards/named?exact=" + Card.Name);

                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    jsonObj = await response.Content.ReadAsStringAsync();

                    card = JsonConvert.DeserializeObject<JsonCard>(jsonObj);
                }
            }

            if (card == null)
            {
                return NotFound();
            }

            Card = Helpers.Helper.TransfomrCard(card, Card);

            _context.Card.Add(Card);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
