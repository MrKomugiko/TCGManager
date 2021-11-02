﻿using TCGManager.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading.Tasks;

namespace TCGManager

                // sprawdzenie nazwy ( w przypadku dodatku Midnight hunt, są karty dzień // noc <- niedozwolona nazwa pliku, do podmiany na dzień --- noc)
                card.name = card.name.Replace("//", "---");

                // Console.WriteLine($"Find Card: {card.name} / Type: {card.type} / Rarity: {card.rarity} / Standard-Legal : {card.legalities.Any(l => l.format == "Standard")}");
                if(downloadCover)
                    FetchCardImage(card);

                return card;

        public static void AddCardToCollection(TCGManager.Models.CardModel.Card _card)
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
    }
}