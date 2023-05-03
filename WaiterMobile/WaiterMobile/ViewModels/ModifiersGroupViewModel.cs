using RKeeperWaiter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WaiterMobile.ViewModels
{
    public class ModifiersGroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ModifiersGroup _modifiersGroup;

        public string Name => _modifiersGroup.Name;
        public ObservableCollection<ModifierViewModel> Modifiers { get; private set; }
        public bool IsInDownLimit => _modifiersGroup.IsInDownLimit();
        public bool IsInUpLimit => _modifiersGroup.IsInUpLimit();

        public ModifiersGroupViewModel(ModifiersGroup modifiersGroup)
        {
            _modifiersGroup = modifiersGroup;
            Modifiers = MakeModifierViewModels();
        }

        private ObservableCollection<ModifierViewModel> MakeModifierViewModels()
        {
            ObservableCollection<ModifierViewModel> modifiersViewModels = new ObservableCollection<ModifierViewModel>();

            foreach (Modifier modifier in _modifiersGroup.Modifiers)
            {
                modifiersViewModels.Add(new ModifierViewModel(modifier));
            }

            return modifiersViewModels;
        }
    }
}
