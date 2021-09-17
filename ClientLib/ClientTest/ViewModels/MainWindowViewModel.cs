using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region binding
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
        private string _log;
        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
        }
        private string _sendMessage;
        public string SendMessage
        {
            get { return _sendMessage; }
            set { SetProperty(ref _sendMessage, value); }
        }
        private ObservableCollection<string> _protocolList;
        public ObservableCollection<string> ProtocolList
        {
            get { return _protocolList; }
            set { SetProperty(ref _protocolList, value); }
        }

        #endregion
        #region command
        public ICommand ClickedConnectCommand { get; set; }
        public ICommand ClickedDisConnectCommand { get; set; }
        public ICommand ClickedSendMsgCommand { get; set; }
        #endregion
        #region variable
        ClientLib.ClientProtocolLib _client;
        #endregion

        public MainWindowViewModel()
        {
            ClickedConnectCommand = new DelegateCommand(Connect);
            ClickedDisConnectCommand = new DelegateCommand(Disconnect);
            ClickedSendMsgCommand = new DelegateCommand(SendMsg);
            _client = new ClientLib.ClientProtocolLib();
            IpAddr = "127.0.0.1";
            PortNum = 5000;
            ProtocolList = new ObservableCollection<string>();
            ProtocolList.Add("TCP");
            ProtocolList.Add("UDP");
            ProtocolList.Add("SOCKET");
        }

        public void Connect()
        {
            try
            {
                var res = _client.Connect((ClientLib.ProtocolKind)Enum.Parse(typeof(ClientLib.ProtocolKind), Protocol, true), IpAddr, PortNum);

                if (res == false)
                {
                    Log += "Connect Failed \n";
                    Log += _client.GetErrorMsg();
                    return;
                }
                Log += "Connect Success\n";
            }
            catch (Exception e)
            {
                Log += $"Connect Failed : {e.ToString()}\n";
                
            }
        }
        private void Disconnect()
        {
            try
            {
                var res = _client.DisConnect();

                if (res == false)
                {
                    Log += "DisConnect Failed\n";
                    return;
                }

                Log += "DisConnect Success\n";
            }catch(Exception e)
            {
                Log += $"DisConnect Failed : {e.ToString()}\n";
            }
      
        }
        private void SendMsg()
        {
            var res = _client.SendMsg(SendMessage);

            if (res == false)
            {
                Log += "Failed to Send Message\n";
                Log += _client.GetErrorMsg();
                return;
            }
            Log += $"Send : {SendMessage} \n";

            string msg = _client.ReceiveMsg();
            if (string.IsNullOrEmpty(msg))
            {

                return;
            }
            Log += $"receive : {msg} \n";

        }
    }
}
