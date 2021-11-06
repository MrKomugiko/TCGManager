using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TCGManager.Models;
using TCGManager.Models.CardModel;

namespace TCGManager.ViewModels
{
    public class DeckCollectionViewModel : ViewModelBase
    {
        private CardDetailViewModel CardDetailsVM;
        public ICommand AddCardToDeckCommand { get; set; }
        public DeckCollectionViewModel(CardDetailViewModel cardDetailsVM)
        {
            CardDetailsVM = cardDetailsVM;
            _deckCardsCollection = new ObservableCollection<CardCollectionData>();

            // Navigation Commands:
            AddCardToDeckCommand = new RelayCommand(
                (object parametr) => // execute
                {
                    AddCardToDeck(CurrentSelectedCard.cards);
                    RefreshDeckListUI();
                },
                (object parametr) => // canExecute
                {
                    return true;
                }
            );
        }

        public CardCollectionData CurrentSelectedCard
        {
            get => CardDetailsVM.SelectedCard;
            set
            {
                if (value == null) return;
                CardDetailsVM.SelectedCard = value;
                OnPropertyChanged(nameof(CurrentSelectedCard));
            }
        }
        public ObservableCollection<CardCollectionData> DeckCardsCollection
        {
            get => _deckCardsCollection;
            set
            {
                _deckCardsCollection = value;
            }
        }
        private ObservableCollection<CardCollectionData> _deckCardsCollection;
        public void AddCardToDeck(Card _card)
        {
            if (_card == null) return;

            var matchCard = DeckCardsCollection.FirstOrDefault(c => c.cards.id == _card.id);
            if (matchCard != null)
            {
                if (matchCard.quantity + 1 > 4 && matchCard.cards.type.Contains("Land") == false) return;

                DeckCardsCollection.Remove(matchCard);
                matchCard.quantity++;
                DeckCardsCollection.Add(matchCard);
            }
            else
            {
                DeckCardsCollection.Add(new CardCollectionData(1, _card));
            }
        }
        public void RefreshDeckListUI()
        {
            DeckCardsCollection = DeckCardsCollection;
            OnPropertyChanged(nameof(DeckCardsCollection));
        }
    }
}

