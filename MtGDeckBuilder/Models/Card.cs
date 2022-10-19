using MtGDeckBuilder.CardObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtGDeckBuilder.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int Copies { get; set; } = 1;
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cmc { get; set; }
        public string Loyalty { get; set; } = string.Empty;
        public string Oracle_Text { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
        public string Toughness { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Set { get; set; } = string.Empty;
        [NotMapped]
        public string[] Color_Identity { get; set; } = new string[0];
        [NotMapped]
        public string[] Color_Indicator { get; set; } = new string[0];
        public string Mana_Cost { get; set; } = string.Empty;
        [NotMapped]
        public Legalities Legalities { get; set; } = new Legalities();
        public string Type_Line { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Flavor_Text { get; set; } = string.Empty;
        [NotMapped]
        public ImageUris Image_Uris { get; set; } = new ImageUris();
        [NotMapped]
        public Prices Prices { get; set; } = new Prices();
        [NotMapped]
        public CardFace[] Card_Faces { get; set; } = new CardFace[0];
    }
}
