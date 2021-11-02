using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TCGManager.Models;
using TCGManager.Models.CardModel;

namespace TCGManager.ViewModels
{
    public class CardCollectionViewModel : ViewModelBase
    {
        private CardDetailViewModel CardDetailsVM;

        public CardCollectionViewModel(CardDetailViewModel cardDetailsVM)
        {
            CardDetailsVM = cardDetailsVM;
            _model = new ObservableCollection<CardCollectionData>(CardCollection.MyCardCollection);
        }

        private ObservableCollection<CardCollectionData> _model;
        public ObservableCollection<CardCollectionData> model 
        {
            get => _model;
            set
            {
                _model = value;
            }
        }

        public int CollectionSize_CountUnique
        {
            get { return CardCollection.MyCardCollection.Where(c => c.cards.type.Contains("Land") == false).Count(); }
        }

        public int CollectionSize_SumAll
        {
            get { return CardCollection.MyCardCollection.Where(c => c.cards.type.Contains("Land") == false).Sum(c=>c.quantity); }
        }

        public CardCollectionData CurrentSelectedCard 
        {
            get => CardDetailsVM.SelectedCard;
            set
            {
                if (value == null) return;
                CardDetailsVM.SelectedCard = value;
            }
        }

        internal void RefreshListUI(ObservableCollection<CardCollectionData> updatedData)
        {
            _model = updatedData;
            OnPropertyChanged(nameof(model));
        }
    }
}
