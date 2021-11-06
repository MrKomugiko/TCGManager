using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TCGManager.Models;

namespace TCGManager.ViewModels
{
    public class CollectionFilteringViewModel : ViewModelBase
    {
        public ObservableCollection<CardCollectionData> GetModel()
        {
            var model = orginalmodelbackup.ToList();

            if (String.IsNullOrWhiteSpace(SelectedSet) == false)
                model = model.Where(c => c.cards.setName == SelectedSet).ToList();
            if (NoDuplicatesIsEnabled)
                model = model.GroupBy(c => c.cards.name).Select(grp => grp.FirstOrDefault()).ToList();


            return new ObservableCollection<CardCollectionData>(model);
        }
        public List<string> ListOfSetsInCollection { get; set; }
        public CollectionFilteringViewModel(CardCollectionViewModel _ccVM)
        {
            ListOfSetsInCollection = CardCollection.SetsInCollection;
            OnPropertyChanged(nameof(ListOfSetsInCollection));
            ccVM = _ccVM;
            orginalmodelbackup = _ccVM.model;
            orginalmodelbackup_uniquenames = new ObservableCollection<CardCollectionData>(orginalmodelbackup.GroupBy(c => c.cards.name).Select(grp => grp.FirstOrDefault()));

            FilteredModel = NoDuplicatesIsEnabled ? orginalmodelbackup_uniquenames : orginalmodelbackup;
            SetFilterPropertyCommand = new RelayCommand(
                (object o) => // execute
                {
                    if (o is Array)
                    {
                        ModifyFilter(new Multiparametr((object[])o));
                    }
                    else
                    {
                        ModifyFilter((string)o);
                    }
                },
                (object o) => // canExecute
                {
                    return true;
                }
            );
        }

        public string SelectedSet {
            get => _selectedSet;
            set
            {
                _selectedSet = value;
                if(String.IsNullOrWhiteSpace(value))
                {
                    // nie wybrano sortowania po secie
                    ApplyFilters(GetModel());
                    return;
                }
                
                ApplyFilters(GetModel());
            } 
        }

      

        public ObservableCollection<CardCollectionData> FilteredModel { get; set; }
        public ICommand SetFilterPropertyCommand { get; set; }
        public bool NoDuplicatesIsEnabled
        {
            get => noDuplicatesIsEnabled;
            set
            {
                noDuplicatesIsEnabled = value;
                ApplyFilters(GetModel());

                OnPropertyChanged(nameof(noDuplicatesIsEnabled));
            }
        }

        private string _nameFilter;
        private bool noDuplicatesIsEnabled;
        private List<string> _colorFilters = new List<string>();
        private List<string> _costFilters = new List<string>();
        private List<string> _rarityFilters = new List<string>();
        private List<string> _typeFilters = new List<string>();
        private string _selectedSet;
        private readonly CardCollectionViewModel ccVM;
        private readonly ObservableCollection<CardCollectionData> orginalmodelbackup;
        private readonly ObservableCollection<CardCollectionData> orginalmodelbackup_uniquenames;


        private void ApplyFilters(ObservableCollection<CardCollectionData> model)
        {
            var filteredmodel = new ObservableCollection<CardCollectionData>();
            foreach (var card in model.ToList())
            {
                var filteredcard = card;
                bool filtrationResult = true;
                if (string.IsNullOrEmpty(_nameFilter) == false)
                {
                    if (card.cards.name.Contains(_nameFilter) == false) continue;
                }

                if (_typeFilters.Count > 0)
                {
                    filtrationResult = true;
                    var spilittType = card.cards.type.Split(" ");

                    // przypadek typu karty Legendary Planeswalker ( dwu członowe )

                    if (spilittType.Length > 1)
                    {
                        if (card.cards.type.Split(" ")[1] == "Planeswalker") // Legendary Planeswalker
                        {
                            spilittType[0] = "Planeswalker";
                        }

                        if (card.cards.type.Split(" ")[1] == "Land") // Land } Basic Land
                        {
                            spilittType[0] = "Land";
                        }
                    }
                    if (_typeFilters.Contains(spilittType[0]) == false)
                    {
                        filtrationResult = false;
                    }

                    if (filtrationResult == false) continue;
                }

                if (_colorFilters.Count > 0)
                {
                    filtrationResult = true;
                    if (card.cards.colors == null)
                    {
                        card.cards.colors = new List<string>() { "null" };
                    }

                    if (card.cards.name.Contains("Jack"))
                    {
                        var xx = 1;
                    }


                    if (_colorFilters.Any(c => card.cards.colors.Contains(c)) == false) continue;

                    foreach (string color in _colorFilters)
                    {
                        if (card.cards.colors.Contains(color) == false)
                            filtrationResult = false;
                    }
                    if (filtrationResult == false) continue;
                }

                if (_costFilters.Count > 0)
                {
                    filtrationResult = true;

                    if (_costFilters.Contains("7plus"))
                    {
                        if ((int)Double.Parse(card.cards.cmc.Replace(".", ",")) >= 7)
                        {
                            // jeżeli koszt to 7+ przechodzi dalej
                            filtrationResult = true;
                        }
                        else if (_costFilters.Contains(((int)Double.Parse(card.cards.cmc.Replace(".", ","))).ToString()) == false)
                        {
                            // jezeli jego koszt wynosi mniej niz 7, sprawdz czy jest uwzgledniony w pozostalych opcjach 0 - 6
                            filtrationResult = false;
                        }
                    }
                    else
                    {
                        if (_costFilters.Contains(((int)Double.Parse(card.cards.cmc.Replace(".", ","))).ToString()) == false)
                        {
                            filtrationResult = false;
                        }
                    }
                    if (filtrationResult == false) continue;
                }

                if (_rarityFilters.Count > 0)
                {
                    filtrationResult = true;

                    if (_rarityFilters.Contains(card.cards.rarity) == false) continue;
                }
                filteredmodel.Add(filteredcard);
            }
            FilteredModel = filteredmodel;
            ccVM.RefreshListUI(FilteredModel);
        }
        private void ModifyFilter(string namesearchvalue)
        {
            _nameFilter = namesearchvalue;
            ApplyFilters(NoDuplicatesIsEnabled ? orginalmodelbackup_uniquenames : orginalmodelbackup);
        }
        private void ModifyFilter(Multiparametr parametrs)
        {

            if (parametrs == null) return;
            //-----------------------------------------------------------
            //  TOGGLE BUTTONS


            UpdateFilter(parametrs, CardCollection.CardTypesList, ref _typeFilters);
            UpdateFilter(parametrs, CardCollection.CardColorsList, ref _colorFilters);
            UpdateFilter(parametrs, CardCollection.CardRairtyList, ref _rarityFilters);
            UpdateFilter(parametrs, CardCollection.CardCostList, ref _costFilters);

            ApplyFilters(GetModel());
        }
        private void UpdateFilter(Multiparametr parameters, List<string> filterData, ref List<string> currentfiltervalues)
        {
            foreach (string _type in filterData)
            {
                if (parameters.ParametrName.Contains(_type))
                {
                    if (currentfiltervalues.Contains(_type))
                        currentfiltervalues.Remove(_type);
                    else
                        currentfiltervalues.Add(_type);
                }
            }

        }
        public enum Colors
        {
            Black,
            Blue,
            White,
            Red,
            Green
        }
    }

    internal class Multiparametr
    {
        public string ParametrName;
        public string ParametrValue;
  
        public Multiparametr(object[] objects)
        {
            ParametrName = (string)objects[0];
            if(objects.Length>1)
                ParametrValue = (string)objects[1].ToString();
        }
    }
}
