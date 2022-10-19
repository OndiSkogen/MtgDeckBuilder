using MtGDeckBuilder.CardObjects;
using MtGDeckBuilder.Models;

namespace MtGDeckBuilder.Helpers
{
    public static class Helper
    {
        public static Card TransfomrCard(JsonCard jCard, Card? card)
        {
            if (card == null) card = new Card();

            card.Name = jCard.Name;
            card.Artist = jCard.Artist;
            card.Cmc = jCard.Cmc;
            card.Oracle_Text = jCard.Oracle_Text;
            card.Power = jCard.Power;
            card.Card_Faces = jCard.Card_Faces;
            card.Color_Identity = jCard.Color_Identity;
            card.Color_Indicator = jCard.Color_Indicator;
            card.Flavor_Text = jCard.Flavor_Text;
            card.Image_Uris = jCard.Image_Uris;
            card.Legalities = jCard.Legalities;
            card.Loyalty = jCard.Loyalty;
            card.Mana_Cost = jCard.Mana_Cost;
            card.Prices = jCard.Prices;
            card.Rarity = jCard.Rarity;
            card.Set = jCard.Set;
            card.Toughness = jCard.Toughness;
            card.Type_Line = jCard.Type_Line;

            return card;
        }
    }
}
