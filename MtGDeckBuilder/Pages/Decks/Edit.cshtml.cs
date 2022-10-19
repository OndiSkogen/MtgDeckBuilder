#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MtGDeckBuilder.CardObjects;
using MtGDeckBuilder.Data;
using MtGDeckBuilder.Models;
using Newtonsoft.Json;

namespace MtGDeckBuilder.Pages.Decks
{
    public class EditModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public EditModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Deck Deck { get; set; }
        public List<JsonCard> MainDeck { get; set; } = new List<JsonCard>();
        public List<JsonCard> Sideboard { get; set; } = new List<JsonCard>();
        public string SaveMainDeck { get; set; } = string.Empty;
        public string SaveSideboard { get; set; } = string.Empty;
        public IFormFile SaveFile { get; set; }


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

            string[] deckList = System.IO.File.ReadAllLines(Deck.DeckListFile);

            bool sideBoard = false;
            int counter = 0;

            foreach (var card in deckList)
            {
                JsonCard jCard = new JsonCard();
                int copies;

                if (card == string.Empty || card.ToUpper() == "SIDEBOARD")
                {
                    sideBoard = true;
                    continue;
                }

                if (Int32.TryParse(card[0].ToString(), out copies))
                {
                    copies = Int32.Parse(card[0].ToString());
                    jCard = await GetDetails(card.Substring(2));
                    await Task.Delay(100);
                    jCard.Copies = copies;
                }

                if (!sideBoard)
                {
                    if (!MainDeck.Contains(jCard))
                    {
                        MainDeck.Add(jCard);
                    }

                    for (int i = 0; i < copies; i++)
                    {
                        Deck.MainDeck[counter] = jCard;
                        counter++;
                        if (counter >= 60)
                        {
                            counter = 0;
                        }
                    }
                }

                if (sideBoard)
                {
                    if (!Sideboard.Contains(jCard))
                    {
                        Sideboard.Add(jCard);
                    }

                    for (int i = 0; i < copies; i++)
                    {
                        Deck.SideBoard[counter] = jCard;
                        counter++;
                    }
                }

                SaveMainDeck = string.Empty;
                foreach (JsonCard mainCard in MainDeck)
                {
                    SaveMainDeck += mainCard.Copies.ToString().Trim() + " " + mainCard.Name.Trim() + "\n";
                }
                SaveSideboard = string.Empty;
                foreach (JsonCard sideCard in Sideboard)
                {
                    SaveSideboard += sideCard.Copies.ToString().Trim() + " " + sideCard.Name.Trim() + "\n";
                }
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Deck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeckExists(Deck.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostSave(int? id,string saveMainDeck, string saveSideboard)
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

            string saveString = saveMainDeck + "\nSIDEBOARD\n" + saveSideboard;

            using (var file = new StreamWriter(Deck.DeckListFile))
            {
                await file.WriteLineAsync(saveString);
            }

            return await OnGetAsync(Deck.Id);
        }

        public async Task<JsonCard> GetDetails(string name)
        {
            string jsonObj = string.Empty;
            JsonCard tempCard = null;

            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.scryfall.com/cards/named?exact=" + name);

                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    jsonObj = await response.Content.ReadAsStringAsync();
                    tempCard = JsonConvert.DeserializeObject<JsonCard>(jsonObj);
                }
            }

            return tempCard;
        }

        private bool DeckExists(int id)
        {
            return _context.Deck.Any(e => e.Id == id);
        }
    }
}
