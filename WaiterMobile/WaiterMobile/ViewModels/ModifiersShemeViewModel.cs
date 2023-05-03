using RKeeperWaiter.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace WaiterMobile.ViewModels
{
    public class ModifiersShemeViewModel : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private ModifiersSheme _modifiersSheme;

        public ObservableCollection<ModifiersGroupViewModel> ModifiersGroups { get; private set; }

        public ModifiersShemeViewModel(ModifiersSheme modifiersSheme)
        {
            _modifiersSheme = modifiersSheme;
            ModifiersGroups = MakeGroupViewModels();
        }

        public ObservableCollection<ModifiersGroupViewModel> MakeGroupViewModels()
        {
            ObservableCollection<ModifiersGroupViewModel> modifiersGroupViewModels = new ObservableCollection<ModifiersGroupViewModel>();

            foreach (ModifiersGroup modifiersGroup in _modifiersSheme.ModifiersGroups)
            {
                modifiersGroupViewModels.Add(new ModifiersGroupViewModel(modifiersGroup));
            }

            return modifiersGroupViewModels;
        }
    }
}
