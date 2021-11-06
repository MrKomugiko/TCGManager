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
        public DeckCollectionViewModel DeckCollectionVM { get;set;}
        public CardDetailViewModel CardDetailsVM { get; set; }
        public CardCollectionViewModel ccVM { get; set; }
        private ObservableCollection<CardCollectionData> _deckCardsCollection;
        public CollectionFilteringViewModel fcVM { get; set; }
        public ICommand NavigateToMainWindowCommand { get; set; }
        public ICommand ClearDeckCommand { get;set; }
        public DeckCreatorViewModel(NavigationStore navigationStore, CardCollectionViewModel ccVM, CardDetailViewModel cardDetailsVM)
        {
            _deckCardsCollection = new ObservableCollection<CardCollectionData>();
            CardDetailsVM = cardDetailsVM;
            this.ccVM = ccVM;
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModel = this;
            this.fcVM = new CollectionFilteringViewModel(ccVM);  

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

            ClearDeckCommand = new RelayCommand(
                    (object o) => // execute
                    {
                        DeckCollectionVM.DeckCardsCollection.Clear();
                        DeckCollectionVM.RefreshDeckListUI();
                    },
                    (object o) => // canExecute
                    {
                       return true;
                    }
               );

        DeckCollectionVM = new DeckCollectionViewModel(cardDetailsVM);
                 }
    }
}
