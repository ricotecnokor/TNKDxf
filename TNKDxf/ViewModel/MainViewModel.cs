using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace TNKDxf.ViewModel
{ 
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<TabItem> Tabs { get; } = new ObservableCollection<TabItem>();

        public ICommand AddTabsCommand { get; }

        public MainViewModel()
        {
            AddTabsCommand = new RelayCommand(AddTabs);

            // Adiciona a primeira aba
            Tabs.Add(new TabItem
            {
                Header = "Primeira Aba",
                Content = new TextBlock { Text = "Conteúdo inicial" }
            });
        }

        private void AddTabs()
        {
            List<string> tabNames = new List<string>
            {
                "Segunda Aba",
                "Terceira Aba",
                "Aba Personalizada",
                "Última Aba"
            };

            foreach (string name in tabNames)
            {
                Tabs.Add(new TabItem
                {
                    Header = name,
                    Content = new TextBlock { Text = $"Conteúdo da {name}" }
                });
            }
        }
    }
}
