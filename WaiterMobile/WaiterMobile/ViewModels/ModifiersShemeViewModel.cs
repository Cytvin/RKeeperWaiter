using RKeeperWaiter.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace WaiterMobile.ViewModels
{
    public class ModifiersShemeViewModel : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private ModifiersSheme _modifiersSheme;

        public ObservableCollection<ModifiersGroupViewModel> RequiredModifiersGroups { get; private set; }
        public ObservableCollection<ModifiersGroupViewModel> OptionalModifiersGroups { get; private set; }
        public bool HasRequiredModifiersGroups => RequiredModifiersGroups.Count > 0;
        public bool HasOptionalModifiersGroups => OptionalModifiersGroups.Count > 0;
        public ModifiersShemeViewModel(ModifiersSheme modifiersSheme)
        {
            _modifiersSheme = modifiersSheme;
            RequiredModifiersGroups = MakeGroupViewModels(_modifiersSheme.RequiredModifiersGroups);
            OptionalModifiersGroups = MakeGroupViewModels(_modifiersSheme.OptionalModifiersGroups);
        }

        public ObservableCollection<ModifiersGroupViewModel> MakeGroupViewModels(IEnumerable<ModifiersGroup> modifiersGroups)
        {
            ObservableCollection<ModifiersGroupViewModel> modifiersGroupViewModels = new ObservableCollection<ModifiersGroupViewModel>();

            foreach (ModifiersGroup modifiersGroup in modifiersGroups)
            {
                modifiersGroupViewModels.Add(new ModifiersGroupViewModel(modifiersGroup));
            }

            return modifiersGroupViewModels;
        }
    }
}
