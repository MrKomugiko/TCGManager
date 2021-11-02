using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TCGManager.Models.CardModel
{
    public class Card
    {
        public string name { get; set; }
        public string manaCost { get; set; }
        public string cmc { get; set; }
        public List<string> colors { get; set; }
        public List<string> colorIdentity { get; set; }

        [JsonIgnore] public string GetColorString {
            get
            {
                if (colors == null) return "";

                return string.Join(",", colors);
            }
        }
        [JsonIgnore]
        public string GetFormattedImageURL
        {
            get
            {
              return AppDomain.CurrentDomain.BaseDirectory + $"\\DATA\\IMAGES\\{set.ToUpper()}_{number}_{name}.png";
            }
        }
        public string type { get; set; }
        public List<string> subtypes { get; set; }
        public string rarity { get; set; }
        public string set { get; set; }
        public string setName { get; set; }
        public string text { get; set; }
        public string flavor { get; set; }
        public string artist { get; set; }
        public string number { get; set;}
        public string power { get; set; }
        public string toughness { get; set; }
        public string layout { get; set; }
        public string multiverseid { get; set; }
        public string imageUrl { get; set; }
        public List<object> foreignNames { get; set; }
        public List<string> printings { get; set; }
        public string originalText { get; set; }
        public string originalType { get; set; }
        public List<Legality> legalities { get; set; }
        public string id { get; set; }
    }

}
