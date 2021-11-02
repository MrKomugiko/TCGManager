using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCGManager.Models;
using TCGManager.Stores;

namespace TCGManager.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public FindNewCardViewModel findCardVM { get; set; }
        public CardDetailViewModel cdVM { get; set; }
        public CardCollectionViewModel ccVM { get; set; }
        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModel = this;

            if (CardCollection.MyCardCollection == null)
                CardCollection.MyCardCollection = CardCollection.LoadCards();

            this.cdVM = new CardDetailViewModel(new CardCollectionData(1, new Models.CardModel.Card()));
            this.findCardVM = new FindNewCardViewModel(cdVM);
            this.ccVM = new CardCollectionViewModel(cdVM);
            this.findCardVM.cardCollectionVM = ccVM;
        }

        private ICommand _downloadImagesCommd = null;
        private ICommand _navigateToDeckCreatorCommand = null;

        public ICommand DownloadImagesCommand
        {
            get
            {
                if (_downloadImagesCommd == null) _downloadImagesCommd = new RelayCommand(
                    (object o) => // execute
                    {
                        CardCollection.DownloadAllImages();
                    },
                    (object o) => // canExecute
                    {
                        return true;
                    }
                );

                return _downloadImagesCommd;
            }
        }
        public ICommand NavigateToDeckCreatorCommand
        {
            get
            {
                if (_navigateToDeckCreatorCommand == null) _navigateToDeckCreatorCommand = new RelayCommand(
                    (object o) => // execute
                    {
                        _navigationStore.CurrentViewModel = new DeckCreatorViewModel(_navigationStore,ccVM,cdVM);
                    },
                    (object o) => // canExecute
                    {
                        return true;
                    }
                );

                return _navigateToDeckCreatorCommand;
            }
        }
    }
}
