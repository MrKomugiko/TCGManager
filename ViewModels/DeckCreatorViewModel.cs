using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCGManager.Models;
using TCGManager.Models.CardModel;
using TCGManager.Stores;

namespace TCGManager.ViewModels
{
    public class DeckCreatorViewModel : ViewModelBase
    {
        public CardDetailViewModel CardDetailsVM { get; set; }
        public CardCollectionViewModel ccVM { get; set; }
        private readonly NavigationStore navigationStore;
        private ObservableCollection<CardCollectionData> _deckCardsCollection;
        public ObservableCollection<CardCollectionData> PlaneswalkersList => new ObservableCollection<CardCollectionData>(DeckCardsCollection.Where(c => c.cards.type.Contains("Planeswalker")).ToList());
        public ObservableCollection<CardCollectionData> CreaturesList => new ObservableCollection<CardCollectionData>(DeckCardsCollection.Where(c => c.cards.type.Contains("Creature")).ToList());
        public ObservableCollection<CardCollectionData> LandsList => new ObservableCollection<CardCollectionData>(DeckCardsCollection.Where(c => c.cards.type.Contains("Land")).ToList());
        public ObservableCollection<CardCollectionData> SkillsList => new ObservableCollection<CardCollectionData>(DeckCardsCollection.Where(c => c.cards.type.Contains("Creature") == false && c.cards.type.Contains("Land") == false && c.cards.type.Contains("Planeswalker") == false).ToList());

        public ICommand AddCardToDeckCommand { get; set; }
        public ICommand NavigateToMainWindowCommand { get; set; }

        public DeckCreatorViewModel(NavigationStore navigationStore, CardCollectionViewModel ccVM, CardDetailViewModel cardDetailsVM)
        {
            _deckCardsCollection = new ObservableCollection<CardCollectionData>();
            CardDetailsVM = cardDetailsVM;
            this.ccVM = ccVM;
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModel = this;


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

            NavigateToMainWindowCommand = new RelayCommand(
                    (object o) => // execute
                    {
                        _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
                    },
                    (object o) => // canExecute
                    {
                        return true;
                    }
                );
        }

        public ObservableCollection<CardCollectionData> DeckCardsCollection
        {
            get =>_deckCardsCollection;
            set
            {
                _deckCardsCollection = value;
            }
        }
        public int DeckCardsTotal
        {
            get { return CardCollection.MyCardCollection.Where(c => c.cards.type.Contains("Land") == false).Sum(c => c.quantity); }
        }
        public CardCollectionData CurrentSelectedCard
        {
            get => CardDetailsVM.SelectedCard;
            set
            {
                CardDetailsVM.SelectedCard = value;
            }
        }
        public void RefreshDeckListUI()
        {
            DeckCardsCollection = DeckCardsCollection;
            OnPropertyChanged(nameof(DeckCardsCollection));
        }
        private void AddCardToDeck(Card _card)
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

            OnPropertyChanged(nameof(PlaneswalkersList));
            OnPropertyChanged(nameof(CreaturesList));
            OnPropertyChanged(nameof(SkillsList));
            OnPropertyChanged(nameof(CreaturesList));
        }
    }
}
