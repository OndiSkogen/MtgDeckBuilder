using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MtGDeckBuilder.CardObjects;
using MtGDeckBuilder.Models;
using Newtonsoft.Json;

namespace MtGDeckBuilder.Pages.Decks
{
    public class UploadModel : PageModel
    {
        private readonly MtGDeckBuilder.Data.MtGDeckBuilderContext _context;

        public UploadModel(MtGDeckBuilder.Data.MtGDeckBuilderContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Deck Deck { get; set; } = new Deck();
        public IFormFile? Upload { get; set; } 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Upload == null)
            {
                return Page();
            }

            Deck.DeckListFile = Path.Combine("c:", "Decks", Upload.FileName);
            using (var fileStream = new FileStream(Deck.DeckListFile, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            string[] deckList = System.IO.File.ReadAllLines(Deck.DeckListFile);
            bool sideBoard = false;

            int counter = 0;

            foreach (var card in deckList)
            {
                JsonCard cardName = new JsonCard();
                int copies;

                if (card == string.Empty || card == "SIDEBOARD")
                {
                    sideBoard = true;
                    continue;
                }

                if (Int32.TryParse(card[0].ToString(), out copies))
                {
                    copies = Int32.Parse(card[0].ToString());
                    cardName = await GetDetails(card.Substring(2));
                    await Task.Delay(100);
                }

                if (!sideBoard)
                {
                    for (int i = 0; i < copies; i++)
                    {
                        this.Deck.MainDeck[counter] = cardName;
                        counter++;
                        if (counter >= 60)
                        {
                            counter = 0;
                        }
                    }
                }

                if (sideBoard)
                {
                    for (int i = 0; i < copies; i++)
                    {
                        this.Deck.SideBoard[counter] = cardName;
                        counter++;
                    }
                }
            }

            _context.Deck.Add(this.Deck);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<JsonCard> GetDetails(string name)
        {
            string jsonObj = string.Empty;
            JsonCard tempCard = new JsonCard();

            using (var client = new HttpClient())
            {
                var uri = new Uri("https://api.scryfall.com/cards/named?exact=" + name);

                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    jsonObj = await response.Content.ReadAsStringAsync();
                    var temp = JsonConvert.DeserializeObject<JsonCard>(jsonObj);

                    if (temp != null)
                    {
                        tempCard = temp;
                    }
                }
            }

            return tempCard;
        }
    }
}
