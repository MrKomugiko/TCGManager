using TCGManager.Models.CardModel;
using TCGManager.Models.CardModel.CardPriceModel;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TCGManager.Models
{
    public class CardCollectionData
    {
        public int quantity { get; set; }
        public CardModel.Card cards { get; set; }
        public Prices priceList { get; set; }

        public MainCategories Category => GetCategory();

        public CardCollectionData(int _quantity, CardModel.Card _card)
        {
            this.quantity = _quantity;

            this.cards = _card;
            this.priceList = new Prices();
        }

        public enum MainCategories
        {
            Default, Land, Planeswalker, Creature, Noncreature, 
        }
        public MainCategories GetCategory()
        {
            if (cards == null) return MainCategories.Default;
            if (cards.type == null) return MainCategories.Default;
            if (cards.type.Contains("Planeswalker")) return MainCategories.Planeswalker;
            if (cards.type.Contains("Land")) return MainCategories.Land;
            if (cards.type.Contains("Creature")) return MainCategories.Creature;

            return MainCategories.Noncreature;
        }
    }
}
