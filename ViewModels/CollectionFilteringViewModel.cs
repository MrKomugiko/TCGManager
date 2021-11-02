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

        public CollectionFilteringViewModel(CardCollectionViewModel _ccVM)
        {
            ccVM = _ccVM;
            orginalmodelbackup = _ccVM.model;

            FilteredModel = orginalmodelbackup;
            SetFilterPropertyCommand = new RelayCommand(
                (object o) => // execute
                {
                    ModifyFilter((string)o);
                },
                (object o) => // canExecute
                {
                    return true;
                }
            );
        }

        private readonly ObservableCollection<CardCollectionData> orginalmodelbackup;
        public ObservableCollection<CardCollectionData> FilteredModel { get; set; }

        private void ApplyFilters(ObservableCollection<CardCollectionData> model)
        {
            var filteredmodel = new ObservableCollection<CardCollectionData>();
            foreach (var card in model.ToList())
            {
                var filteredcard = card;
                if (card.cards.name.Contains(_nameFilter) == false) continue;

                if (filteredcard.cards.colors != null)
                {
                    if (_colorFilters.Any(s => card.cards.colors.Contains(s)) == false) continue;
                }

/*                if (filteredcard.cards.power != null)
                {
                    if (_powerFilters.Contains(card.cards.power) == false) continue;
                }

                if (filteredcard.cards.cmc != null)
                {
                    if (_powerFilters.Contains(card.cards.cmc) == false) continue;
                }

                if (filteredcard.cards.rarity != null)
                {
                    if (_rarityFilters.Contains(card.cards.rarity) == false) continue;
                }

                if (filteredcard.cards.type != null)
                {
                    if (_typeFilters.Contains(card.cards.type) == false) continue;
                }
*/
                filteredmodel.Add(filteredcard);
            }
            FilteredModel = filteredmodel;
        }

        private string _nameFilter;
        private List<string> _colorFilters = new List<string>();
        private readonly CardCollectionViewModel ccVM;

        /*        private List<string> _powerFilters = new List<string>();
private List<string> _costFilters = new List<string>();
private List<string> _rarityFilters = new List<string>();
private List<string> _typeFilters = new List<string>();
*/

        public ICommand SetFilterPropertyCommand { get; set; }

        private void ModifyFilter(string filtername)
        {
            if (filtername == null) return;

            _nameFilter = filtername;

            if(Enum.IsDefined(typeof(Colors), filtername))
            {
                // dodanie nowego sortowania po kolorze
                if(_colorFilters.Contains(filtername) == false)
                {
                    _colorFilters.Add(filtername);
                }
            }

            ApplyFilters(orginalmodelbackup);
            ccVM.RefreshListUI(FilteredModel);
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
}
