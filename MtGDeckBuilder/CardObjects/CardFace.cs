namespace MtGDeckBuilder.CardObjects
{
    public class CardFace
    {
        public string Artist { get; set; } = string.Empty;
        public decimal Cmc { get; set; }
        public string[] Colors { get; set; } = new string[0];
        public string Flavor_Text { get; set; } = string.Empty;
        public ImageUris ImageUris { get; set; } = new ImageUris();
        public string Loyalty { get; set; } = string.Empty;
        public string Mana_Cost { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
        public string Toughness { get; set; } = string.Empty;
        public string Type_Line { get; set; } = string.Empty;
        public string Printed_Name { get; set; } = string.Empty;
        public string Printed_Text { get; set; } = string.Empty;
        public string Printed_Type_Line { get; set; } = string.Empty;
    }
}
