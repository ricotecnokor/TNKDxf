using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TNKDxf.ViewModel;

namespace TNKDxf
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ObservableCollection<AbaModel> Tabs { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
