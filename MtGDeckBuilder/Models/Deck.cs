using MtGDeckBuilder.CardObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtGDeckBuilder.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ColorAffiliation { get; set; } = string.Empty;
        public string DeckListFile { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        [NotMapped]
        public JsonCard[] MainDeck { get; set; } = new JsonCard[60];
        [NotMapped]
        public JsonCard[] SideBoard { get; set; } = new JsonCard[15];
    }
}
