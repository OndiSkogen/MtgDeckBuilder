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

namespace MtGDeckBuilder.Pages.Decks
{
    public class DetailsModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public DetailsModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        public Deck Deck { get; set; }
        public List<JsonCard> Creatures { get; set; } = new List<JsonCard>();
        public List<JsonCard> Spells { get; set; } = new List<JsonCard>();
        public List<JsonCard> OtherSpells { get; set; } = new List<JsonCard>();
        public List<JsonCard> Lands { get; set; } = new List<JsonCard>();
        public List<JsonCard> MainDeck { get; set; } = new List<JsonCard>();
        public List<JsonCard> Sideboard { get; set; } = new List<JsonCard>();
        public int CreatureCount { get; set; } = 0;
        public int SpellCount { get; set; } = 0;
        public int OtherSpellCount { get; set; } = 0;
        public int LandCount { get; set; } = 0;

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
            }

            foreach (JsonCard c in Deck.MainDeck)
            {
                char separator = ' ';
                string[] temp = c.Type_Line.Split(separator);

                if (temp.Contains("Creature"))
                {
                    if (!Creatures.Contains(c))
                    {
                        CreatureCount = CreatureCount + c.Copies;
                        Creatures.Add(c);
                    }
                    continue;
                }

                switch (temp[0])
                {
                    case "Land":
                    case "Basic":
                        if (!Lands.Contains(c))
                        {
                            LandCount = LandCount + c.Copies;
                            Lands.Add(c);
                        }
                        break;
                    case "Instant":
                    case "Sorcery":
                        if (!Spells.Contains(c))
                        {
                            SpellCount = SpellCount + c.Copies;
                            Spells.Add(c);
                        }
                        break;
                    case "Creature":
                        if (!Creatures.Contains(c))
                        {
                            CreatureCount = CreatureCount + c.Copies;
                            Creatures.Add(c);
                        }
                        break;
                    case "Legendary":
                        switch (temp[1])
                        {
                            case "Land":
                            case "Basic":
                                if (!Lands.Contains(c))
                                {
                                    LandCount = LandCount + c.Copies;
                                    Lands.Add(c);
                                }
                                break;
                            case "Instant":
                            case "Sorcery":
                                if (!Spells.Contains(c))
                                {
                                    SpellCount = SpellCount + c.Copies;
                                    Spells.Add(c);
                                }
                                break;
                            case "Creature":
                                if (!Creatures.Contains(c))
                                {
                                    CreatureCount = CreatureCount + c.Copies;
                                    Creatures.Add(c);
                                }
                                break;
                            default:
                                if (!OtherSpells.Contains(c))
                                {
                                    OtherSpellCount = OtherSpellCount + c.Copies;
                                    OtherSpells.Add(c);
                                }
                                break;
                        }
                        break;
                    default:
                        if (!OtherSpells.Contains(c))
                        {
                            OtherSpellCount = OtherSpellCount + c.Copies;
                            OtherSpells.Add(c);
                        }
                        break;

                }
            }

            return Page();
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
    }
}
