using TCGManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TCGManager.Models.CardModel;
using TCGManager.Models.CardModel.CardPriceModel;
using System.Linq;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading.Tasks;

namespace TCGManager
{
    public static class CardCollection
    {
        public static List<CardCollectionData> MyCardCollection;
        public static List<string> SetsInCollection
        {
            get
            {
               var list = MyCardCollection.Select(item => item.cards.setName).Distinct().ToList();
                list.Insert(0,"");
                return list;
            } 
        }
        static WebClient wc = new WebClient();
        
        static CardCollection()
        {
            MyCardCollection = LoadCards();
        }
        
        public static List<CardCollectionData>? LoadCards()
        {
            Console.WriteLine("load cards");
            var restoredCollection = new List<CardCollectionData>();
            Directory.CreateDirectory("DATA");
            string[] cardFiles = Directory.GetFiles("DATA/");
            foreach (var cardFile in cardFiles)
            {
                var cardData = File.ReadAllText(cardFile);
                CardCollectionData _card = JsonConvert.DeserializeObject<CardCollectionData>(cardData);
                restoredCollection.Add(_card);
                
            }
            return restoredCollection;
        }
        public static TCGManager.Models.CardModel.Card FetchCardData(string Name, int Number, string Set, bool downloadCover = false)
        {

                var path = $"https://api.magicthegathering.io/v1/cards?name={Name}&number={Number}&set={Set}";

                string jsonResult = wc.DownloadString(path);

                Root _root = JsonConvert.DeserializeObject<Root>(jsonResult);
                
                if (_root.cards.Count == 0) return null;
                TCGManager.Models.CardModel.Card card = _root.cards[0];

                // sprawdzenie nazwy ( w przypadku dodatku Midnight hunt, są karty dzień // noc <- niedozwolona nazwa pliku, do podmiany na dzień --- noc)
                card.name = card.name.Replace("//", "---");

                // Console.WriteLine($"Find Card: {card.name} / Type: {card.type} / Rarity: {card.rarity} / Standard-Legal : {card.legalities.Any(l => l.format == "Standard")}");
                if(downloadCover)
                    FetchCardImage(card);

                return card;

        }
        static Prices FetchCardPrices(TCGManager.Models.CardModel.Card _card)
        {
            if (_card == null) return new Prices();

            var path = $"https://api.scryfall.com/cards/{_card.set.ToLower()}/{_card.number}";
            string jsonResult = wc.DownloadString(path);
            TCGManager.Models.CardModel.CardPriceModel.Card cardPricesData = JsonConvert.DeserializeObject<TCGManager.Models.CardModel.CardPriceModel.Card>(jsonResult);

            return cardPricesData.prices;
        }
        public static void AddCardToCollection(TCGManager.Models.CardModel.Card _card)
        {
            if (_card == null) return;

            var matchCard = MyCardCollection.FirstOrDefault(c => c.cards.id == _card.id);
            if (matchCard != null)
            {
                matchCard.quantity++;
                Console.WriteLine($"[Total copies:{matchCard.quantity}]");
            }
            else
            {
                Console.WriteLine("[New!]");
                MyCardCollection.Add(new CardCollectionData(1, _card));
            }

            matchCard = new CardCollectionData(1, _card);
            matchCard.priceList = FetchCardPrices(_card);

            string cardDataString = JsonConvert.SerializeObject(matchCard);
            File.WriteAllText($"DATA/{matchCard.cards.set}_{matchCard.cards.number}_{matchCard.cards.name}.json", cardDataString);
        }
        public static void FetchCardImage(Models.CardModel.Card _card)
        {
            if (Directory.Exists("DATA\\IMAGES") == false)
            {
                Directory.CreateDirectory("DATA\\IMAGES");
            }
            if (_card.imageUrl == null) return;
            byte[] data = wc.DownloadData(_card.imageUrl);

            using (MemoryStream mem = new MemoryStream(data))
            {
                using (var yourImage = Image.FromStream(mem))
                {
                    // If you want it as Png
                    yourImage.Save($"DATA\\IMAGES\\{_card.set.ToUpper()}_{_card.number}_{_card.name}.png", ImageFormat.Png);
                }
            }
        }
        public static void DownloadAllImages()
        {
            foreach (var card in MyCardCollection)
            {
                if (File.Exists($"DATA\\IMAGES\\{card.cards.set.ToUpper()}_{card.cards.number}_{card.cards.name}.png") == false)
                {
                    FetchCardImage(card.cards);
                }
            }
        }

        public readonly static List<string> CardTypesList = new List<string>()
        {
            "Land","Creature","Sorcery","Instant","Enchantment","Artifact","Planeswalker"
        };
        public readonly static List<string> CardColorsList = new List<string>()
        {
            "Black","Blue","White","Green","Red","null"
        };
        public readonly static List<string> CardCostList = new List<string>()
        {
            "0","1","2","3","4","5","6","7plus"
        };
        public readonly static List<string> CardRairtyList = new List<string>()
        {
            "Common","Uncommon","Rare","Mythic"
        };

        public static List<string> ListOfSetsInCollection { get; set; }
    }
}
