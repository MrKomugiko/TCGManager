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
    public class FindNewCardViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get;set; }
        private CardDetailViewModel CardDetailsVM { get; set; }

        private string _cardName;
        private string _cardNumber;
        private string _cardSetCode;
        internal CardCollectionViewModel cardCollectionVM;

        public string CardName
        {
            get => _cardName;
            set
            {
                _cardName = value;
                OnPropertyChanged(nameof(CardName));
            }
        }
        public string CardSetCode
        {
            get => _cardSetCode;
            set
            {
                _cardSetCode = value;
                OnPropertyChanged(nameof(CardSetCode));
            }
        }
        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                _cardNumber = value;
                OnPropertyChanged(nameof(CardNumber));
            }
        }

        public void ClearInputFields()
        {
            CardName = "";
            CardNumber = "";
            CardSetCode = "";

            OnPropertyChanged(nameof(CardName));
            OnPropertyChanged(nameof(CardNumber));
            OnPropertyChanged(nameof(CardSetCode));
        }

        public CardCollectionData SearchCardDataFromCollectionByNumberAndSet()
        {
            return CardCollection.MyCardCollection.Where(c => c.cards.number == CardNumber && c.cards.set == CardSetCode).FirstOrDefault();
        }

        public FindNewCardViewModel(CardDetailViewModel cardDetailsVM)
        {
            this.CardDetailsVM = cardDetailsVM;
            CurrentViewModel = this;
        }
        private bool _isCoverDownloadingEnabled;
        public bool IsCoverDownloadingEnabled
        {
            get => _isCoverDownloadingEnabled;
            set
            {
                _isCoverDownloadingEnabled = value;
                OnPropertyChanged(nameof(IsCoverDownloadingEnabled));
            }
        }
        

        public bool IsGetDataFromInternetEnabled 
        {
            get => _isGetDataFromInternetEnabled;
            set
            {
                _isGetDataFromInternetEnabled = value;
                OnPropertyChanged(nameof(IsGetDataFromInternetEnabled));
            } 
        }
        private ICommand _searchCommand = null;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null) _searchCommand = new RelayCommand(
                    (object o) => // execute
                    {
                        CardCollectionData foundcard;
                        if (IsGetDataFromInternetEnabled)
                        {
                            if(CardNumber == null || CardSetCode == null || CardName == null)
                            {
                                MessageBox.Show("All fields required.");
                                return;
                            }
                            // jezeli nie znaleziono karty w lokalnym spisie poszukaj jej w necie
                            foundcard = new CardCollectionData(1, CardCollection.FetchCardData(CardName, Int32.Parse(CardNumber), CardSetCode, downloadCover:IsCoverDownloadingEnabled));
                        }
                        else
                            foundcard = SearchCardDataFromCollectionByNumberAndSet();
                        if (foundcard == null)
                        {
                            if (IsGetDataFromInternetEnabled)
                                MessageBox.Show("not found this card in your collection \nnether in internet resources");
                            else
                                MessageBox.Show("not found this card in your collection");
                        }
                        else
                        {
                            if (foundcard == null) return;
                            CardDetailsVM.SelectedCard = foundcard;
                            ClearInputFields();
                        }
                    },
                    (object o) => // canExecute
                    {
                        // nie przesyłaj formularza gdy sa niepełne dane
                        if(IsGetDataFromInternetEnabled)
                        {
                            if (CardName != null && CardSetCode != null && CardNumber != null) return true;
                        }
                        else
                        {
                            if (CardSetCode != null && CardNumber != null) return true;
                        }

                        return true;
                    }
                );

                return _searchCommand;
            }
        }

        public void AddCardToCollection(Card newCard)
        {
            if (newCard == null) return;
            if (newCard.name == null) return;

            CardCollection.MyCardCollection.Reverse();
            CardCollection.AddCardToCollection(newCard);
            CardCollection.MyCardCollection.Reverse();
            cardCollectionVM.RefreshListUI(new ObservableCollection<CardCollectionData>(CardCollection.MyCardCollection));
        }

        private bool _isGetDataFromInternetEnabled;
        private ICommand _addCardToCollectionCommand = null;
        public ICommand AddCardToCollectionCommand
        {
            get
            {
                if (_addCardToCollectionCommand == null) _addCardToCollectionCommand = new RelayCommand(
                    (object parametr) => // execute
                    {
                        AddCardToCollection(CardDetailsVM.SelectedCard.cards);
                    },
                    (object parametr) => // canExecute
                    {
                        return true;
                    }
                );

                return _addCardToCollectionCommand;
            }
        }

    }
}
