#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MtGDeckBuilder.CardObjects;
using MtGDeckBuilder.Data;
using MtGDeckBuilder.Models;
using Newtonsoft.Json;

namespace MtGDeckBuilder.Pages.Cards
{
    public class IndexModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public IndexModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        public IList<Card> Card { get;set; }

        public async Task OnGetAsync()
        {
            Card = await _context.Card.ToListAsync();
            string jsonObj = string.Empty;

            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.scryfall.com/cards/named?exact=" + "jace, vryn's prodigy");

                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    jsonObj = await response.Content.ReadAsStringAsync();

                    JsonCard card = JsonConvert.DeserializeObject<JsonCard>(jsonObj);
                }
            }
        }
    }
}
