using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MultiClientText_cs.ViewModels
{
    public class ClientData
    {
        public ClientLib.ProtocolKind protocol;
        public string ipAddr { get; set; }
        public int portNum { get; set; }
        public int id { get; set; }
        public string strProtocol { get; set; }
    }
    public class MainWindowViewModel : BindableBase
    {

        #region Binding
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        private string _ipAddr;
        public string IpAddr
        {
            get { return _ipAddr; }
            set { SetProperty(ref _ipAddr, value); }
        }
        private int _portNum;
        public int PortNum
        {
            get { return _portNum; }
            set { SetProperty(ref _portNum, value); }
        }
        private string _protocol;
        public string Protocol
        {
            get { return _protocol; }
            set { SetProperty(ref _protocol, value); }
        }
        private ObservableCollection<string> _protocolList;
        public ObservableCollection<string> ProtocolList
        {
            get { return _protocolList; }
            set { SetProperty(ref _protocolList, value); }
        }

        private ObservableCollection<ClientData> _list;
        public ObservableCollection<ClientData> List {
            get { return _list; }
            set { SetProperty(ref _list, value); }
        }

        private ClientData _selectClient;
        public ClientData SelectClient
        {
            get { return _selectClient; }
            set { SetProperty(ref _selectClient, value); }
        }
        private string _selectClientID;
        public string SelectClientID
        {
            get { return _selectClientID; }
            set { SetProperty(ref _selectClientID, value); }
        }
        #endregion

        #region Command
        public ICommand ClickedConnectCommand { get; set; }
        public ICommand ClickedDisConnectCommand { get; set; }
        public ICommand ChangeClientSelectedCommand { get; set; }
        public ICommand ClickedSendMsgCommand { get; set; }

        #endregion

        #region Variable
        ClientLib.MultiClientLib _clientManager;
        #endregion


        public MainWindowViewModel()
        {
            List = new ObservableCollection<ClientData>();
            ProtocolList = new ObservableCollection<string>();
            ProtocolList.Add("TCP");
            ProtocolList.Add("UDP");
            ProtocolList.Add("SOCKET");
            IpAddr = "127.0.0.1";
            PortNum = 5000;
            _clientManager = new ClientLib.MultiClientLib();


            ClickedConnectCommand = new DelegateCommand(ClickedConnectCmd);
            ClickedDisConnectCommand = new DelegateCommand(ClickDisconnectCmd);
            ChangeClientSelectedCommand = new DelegateCommand<object>(ChangeSelectedClientCmd);
            ClickedSendMsgCommand = new DelegateCommand<object>(ClickedSendMsgCmd);
        }

        private void ClickedSendMsgCmd(object obj)
        {
            SelectClient = (ClientData)obj;
            SelectClientID = SelectClient.id.ToString();
        }

        private void ChangeSelectedClientCmd(object obj)
        {
            SelectClient = (ClientData)obj;
            SelectClientID = SelectClient.id.ToString();
        }

        private void ClickedConnectCmd()
        {
            var tmp = new ClientData()
            {
                ipAddr = IpAddr,
                portNum = PortNum,
                protocol = (ClientLib.ProtocolKind)Enum.Parse(typeof(ClientLib.ProtocolKind), Protocol, true),
                strProtocol = Protocol
            };
            int id;
            bool res = _clientManager.Connect(tmp.protocol, tmp.ipAddr, tmp.portNum, out id);
            if (res == false)
            {
                return;
            }
            tmp.id = id;
            List.Add(tmp);
        }
        private void ClickDisconnectCmd()
        {
            if(SelectClient == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SelectClientID))
            {
                return;
            }
            int id = int.Parse(SelectClientID);
            try
            {
                _clientManager.DisConnect(id);

            }
            catch
            {
                // log
            }
            finally
            {
                List.RemoveAt(id);
            }
        }
    }
}
