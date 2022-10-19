using Newtonsoft.Json;


namespace MtGDeckBuilder.CardObjects
{
    public class JsonCard
    {
        public int Copies { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public decimal Cmc { get; set; }
        public string Loyalty { get; set; } = string.Empty;
        public string Oracle_Text { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
        public string Toughness { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public string Set { get; set; } = string.Empty;
        public string[] Color_Identity { get; set; } = new string[0];
        public string[] Color_Indicator { get; set; } = new string[0];
        public string Mana_Cost { get; set; } = string.Empty;
        public Legalities Legalities { get; set; } = new Legalities();
        public string Type_Line { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Flavor_Text { get; set; } = string.Empty;
        public ImageUris Image_Uris { get; set; } = new ImageUris();
        public Prices Prices { get; set; } = new Prices();
        public CardFace[] Card_Faces { get; set; } = new CardFace[0];
    }
}
