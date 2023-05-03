using RKeeperWaiter.Models;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class ModifierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Modifier _modifier;
        private int _count;
        private bool _selected;

        public string Name => _modifier.Name;
        public ICommand IncreaseCount { get; private set; }
        public ICommand DecreaseCount { get; private set; }
        public ICommand Select { get; private set; }
        public ICommand UnSelect { get; private set; }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }
        public bool Selected 
        {
            get => _selected;
            set 
            {
                _selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }

        public ModifierViewModel(Modifier modifier)
        {
            IncreaseCount = new Command(OnIncreaseCount);
            DecreaseCount = new Command(OnDecreaseCount);

            _modifier = modifier;
            _count = modifier.Count;
            _selected = modifier.Selected;
        }

        private void OnIncreaseCount()
        {
            if (_modifier.Count == 0)
            {
                _modifier.Select();
            }

            _modifier.IncreaseCount();
            Count = _modifier.Count;
        }

        private void OnDecreaseCount()
        {
            _modifier.DecreaseCount();
            Count = _modifier.Count;

            if (_modifier.Count == 0)
            {
                _modifier.UnSelect();
            }
        }
    }
}
