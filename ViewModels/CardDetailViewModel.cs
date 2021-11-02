using System;
using System.IO;
using TCGManager.Models;
using TCGManager.Models.CardModel;

namespace TCGManager.ViewModels
{
    public class CardDetailViewModel : ViewModelBase
    {
        private CardCollectionData _selectedCard = new CardCollectionData(1, new Card());

        public string Name => _selectedCard.cards.name == null ? "nie dotyczy" : _selectedCard.cards.name;
        public string Rarity => _selectedCard.cards.rarity == null ? "nie dotyczy" : _selectedCard.cards.rarity;
        public string Description => _selectedCard.cards.text == null ? "nie dotyczy" : _selectedCard.cards.text;
        public string Price => _selectedCard.priceList.usd == null ? "brak danych" : _selectedCard.priceList.usd;
        public string Type  => _selectedCard.cards.type == null ? "nie dotyczy" : _selectedCard.cards.type;
        public string Power => _selectedCard.cards.power == null ? "nie dotyczy" : _selectedCard.cards.power;
        public string Flavor  => _selectedCard.cards.flavor == null ? "nie dotyczy" : _selectedCard.cards.flavor;
        public string SetName => _selectedCard.cards.setName == null ? "nie dotyczy" : _selectedCard.cards.setName;
        public string ManaCost => _selectedCard.cards.manaCost == null ? "nie dotyczy" : _selectedCard.cards.manaCost;
        public string Toughness => _selectedCard.cards.toughness == null?"nie dotyczy": _selectedCard.cards.toughness;
        

        public  CardCollectionData SelectedCard
        {
            get => _selectedCard;
            set
            {
                _selectedCard = value;
               
                    if (_selectedCard.cards != null)
                    {
                        OnPropertyChanged(
                            nameof(Toughness), nameof(ManaCost), nameof(SetName),
                            nameof(Flavor), nameof(Power), nameof(Type), nameof(Price),
                            nameof(Description), nameof(Rarity), nameof(Name)

                        );


                        if (_selectedCard.cards.name != null)
                        {
                            if (File.Exists(_selectedCard.cards.GetFormattedImageURL) == true)
                            {
                                // załąduj z pamieci
                                SelectedCardImageURL = _selectedCard.cards.GetFormattedImageURL;
                            }
                            else
                            {
                                // sciagnij z neta
                                SelectedCardImageURL = value.cards.imageUrl;
                            }
                            IsCardSelected = true;
                        }
                        else
                        {
                            SelectedCardImageURL = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\emptyCardImage.png";
                            IsCardSelected = false;
                        }
                    }
                else
                    IsCardSelected = false;
                  
            }
        }

        private string _selectedCardImageURL = AppDomain.CurrentDomain.BaseDirectory+ "\\Resources\\emptyCardImage.png";

        public string SelectedCardImageURL
        {
            get { return _selectedCardImageURL; }
            set { 
                _selectedCardImageURL = value;
                OnPropertyChanged(nameof(SelectedCardImageURL));
            }
        }

        private CardCollectionData _empty = new CardCollectionData(1, new Card());

        public CardDetailViewModel(CardCollectionData cardData)
        {
            _selectedCard = cardData;
        }

        private bool _isCardSelected;
        public bool IsCardSelected
        {
            get
            {
               return _isCardSelected;
            }
            set
            {
                _isCardSelected = value;
                OnPropertyChanged(nameof(IsCardSelected));
            }
        }

    }
}
