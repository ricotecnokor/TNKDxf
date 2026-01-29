using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TNKDxf.Dominio.Listas
{
    public class ArquivoItem : INotifyPropertyChanged
    {
        private string _nome;
        private bool _selecionado;
        private bool _errado;
        private bool _aberto;
        private bool _enviado;
        private bool _podeConverter = true;
        private bool _podeBaixar = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Nome { get => _nome; set { _nome = value; OnPropertyChanged(); } }
        public bool Selecionado { get => _selecionado; set { _selecionado = value; OnPropertyChanged(); } }
        public bool Errado { get => _errado; set { _errado = value; OnPropertyChanged(); } }
        public bool Aberto { get => _aberto; set { _aberto = value; OnPropertyChanged(); } }
        public bool Enviado { get => _enviado; set { _enviado = value; OnPropertyChanged(); } }

        // controla se o botão Converter está habilitado para esta linha
        public bool PodeConverter { get => _podeConverter; set { _podeConverter = value; OnPropertyChanged(); } }
        public bool PodeBaixar { get => _podeBaixar; set { _podeBaixar = value; OnPropertyChanged(); } }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
