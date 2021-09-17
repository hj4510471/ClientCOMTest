using Prism.Mvvm;
using ProtocolClassLibrary;

namespace ProtocolSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {            
            ProtocolLib lib = new ProtocolLib();
            lib.ConnectLib(ProtocolKind.Socket, "127.0.0.1", 5000);
            Title = lib.GetInfoLib();
        }
    }
}
