using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientArgsTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public enum ArgsOrder
        {
            START = 0,
            ID = 1,
            RETURN = 2,
            OBJECT = 3,
            METHOD = 4,
            ARGS1 = 5,
            ARGS2 = 6,
            ARGS3 = 7,
            ARGS4 = 8,

        }

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


        private string _sendArgs;
        public string SendArgs
        {
            get { return _sendArgs; }
            set { SetProperty(ref _sendArgs, value); }
        }

        private string _receiveArgs;
        public string ReceiveArgs
        {
            get { return _receiveArgs; }
            set { SetProperty(ref _receiveArgs, value); }
        }
        #endregion

        #region Command
        public ICommand ClickedConnectCommand { get; set; }
        public ICommand ClickedDisConnectCommand { get; set; }
        public ICommand ClickedSendMsgCommand { get; set; }
        public ICommand SendArgsCommand { get; set; }
        #endregion

        #region Variable
        private readonly char SEPARATORARGS = '^';
        private readonly char SPECIAL = '~';
        private readonly char STARTARGS = '$';
        private readonly char ENDARGS = '#';
        private readonly string RETURN_SUCCESS = "1";
        private readonly string RETURN_FAILED = "0";
        private readonly string STATUS_SUCCESS = "0";

        ClientLib.ClientProtocolLib _client;
        #endregion
        public MainWindowViewModel()
        {
            _client = new ClientLib.ClientProtocolLib();

            SendArgsCommand = new DelegateCommand(SendArgsCmd);
            ClickedConnectCommand = new DelegateCommand(ConnectCmd);
            ClickedDisConnectCommand = new DelegateCommand(DisconnectCmd);
            ClickedSendMsgCommand = new DelegateCommand(SendMsgCmd);

            IpAddr = "127.0.0.1";
            PortNum = 5000;
            ProtocolList = new ObservableCollection<string>();
            ProtocolList.Add("TCP");
            ProtocolList.Add("UDP");
            ProtocolList.Add("SOCKET");

        }

        #region Cmd
        public void ConnectCmd()
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
        private void DisconnectCmd()
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
            }
            catch (Exception e)
            {
                Log += $"DisConnect Failed : {e.ToString()}\n";
            }

        }
        private void SendMsgCmd()
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

        private void SendArgsCmd()
        {
            if (string.IsNullOrEmpty(SendArgs))
            {
                Log += $"Error. Argument to Send is Empty";
                return;
            }
            bool res = _client.SendArgs(SendArgs);
            if (res == false)
            {
                Log += $"Failed to Send Args";
                Log += $"Error : {_client.GetErrorMsg()}";
                return;
            }

            string recvArgs = _client.ReceiveArgs();

            ParseReceiveArgs(recvArgs);
        }
        #endregion

        private string[] SplitSpecificWord(string str)
        {

            // ‘$’, ‘^’, ‘#’, '~' 앞에 '~' 가 있는 지 확인하기
            Collection<string> list = new Collection<string>();
            int cur = 0;
            for (int i=0; i<str.Length-1; ++i)
            {
                if (str[i].Equals('~'))
                {
                    if (str[i + 1].Equals('~'))
                    {
                        str = str.Remove(i, 1);
                     
                    }
                    else if (str[i + 1].Equals(STARTARGS))
                    {
                        str = str.Remove(i, 1);
                       
                    }
                    else if (str[i + 1].Equals(ENDARGS))
                    {
                        str = str.Remove(i, 1);
                       
                    }else if (str[i + 1].Equals(SEPARATORARGS))
                    {
                        str = str.Remove(i, 1);
                       
                    }
                }else if (str[i].Equals(SEPARATORARGS))
                {
                    list.Add(str.Substring(cur, i - cur));
                    cur = i+1;

                    if (str.LastIndexOf(SEPARATORARGS.ToString()) < cur)
                    {
                        list.Add(str.Substring(cur));
                        break;
                    }
                }
            }
            //Log += $"Result : {str}\n";
            string[] res = new string[list.Count];
            var pos = list.GetEnumerator();
            for(int i=0; pos.MoveNext(); ++i)
            {
                res[i] = pos.Current;
                //Log += $"{res[i]}\n";
            }
            return res;
        }

        private void ParseReceiveArgs(string recvArgs)
        {
            if (string.IsNullOrEmpty(recvArgs))
            {
                ReceiveArgs = "Failed : SendArgs is Empty";
                return;
            }
      
            string tmpArgs = recvArgs;
            // ‘$’, ‘^’, ‘#’, '~' 앞에 '~' 가 있는 지 확인하기
            var splitRes = SplitSpecificWord(tmpArgs);

            if (splitRes[(int)ArgsOrder.START].Equals(STARTARGS.ToString()) == false)
            {
                ReceiveArgs = "Failed : not start with $";
                return;
            }
            if (splitRes[splitRes.Length - 1].Contains(ENDARGS.ToString()) == false)
            {
                ReceiveArgs = "Failed : not end with #";
                return;
            }
            ReceiveArgs = recvArgs;
            Log += $"ID : {splitRes[(int)ArgsOrder.ID]}\n";
            // {(string.IsNullOrEmpty(splitRes[(int)ArgsOrder.RETURN]) ? ": splitRes[(int)ArgsOrder.RETURN])}";
            Log += $"Obj : {splitRes[(int)ArgsOrder.OBJECT]}\n";
            Log += $"Method : {splitRes[(int)ArgsOrder.METHOD]}\n";

            // check return code
            if (string.IsNullOrEmpty(splitRes[(int)ArgsOrder.RETURN]))
            {
                Log += $"Return : Return is Empty\n";
            }
            else if(splitRes[(int)ArgsOrder.RETURN].Equals(RETURN_SUCCESS))
            {
                Log += $"Return : Success\n";
            }
            else if (splitRes[(int)ArgsOrder.RETURN].Equals(RETURN_FAILED))
            {
                Log += $"Return : Failed\n";
            }
            else
            {
                Log += $"Return : Error\n";
                return;
            }

            ParseStatusArgument(splitRes);
           
        }

        private void ParseStatusArgument(string[] argArr)
        {

            switch (argArr[(int)ArgsOrder.METHOD].ToLower())
            {
                case "status":
                    Log += $"Status Code : {argArr[(int)ArgsOrder.ARGS1]}\n";
                    Log += $"Project : {argArr[(int)ArgsOrder.ARGS2]}\n";
                    int pageNum = int.Parse(argArr[(int)ArgsOrder.ARGS3]);
                    if (pageNum < 0)
                    {
                        Log += $"Page Num : Not Found\n";
                    }
                    else
                    {
                        Log += $"Page Num : {pageNum}\n";
                    }

                    int onAir = int.Parse(argArr[(int)ArgsOrder.ARGS4]);
                    if(onAir == 0)  // 0 : Not onair, 1 : OnAir
                    {
                        Log += $"Existing On Air Page : None\n";
                    }
                    else
                    {
                        Log += $"Existing On Air Page : {onAir}\n";
                    }
                   
                    break;
                default:
                    if (argArr[(int)ArgsOrder.ARGS1].Equals(STATUS_SUCCESS))
                    {
                        Log += $"Status : Success. Message : {argArr[(int)ArgsOrder.ARGS2]}\n";
                    }
                    else
                    {
                        Log += $"Status : Failed.\n";
                    }
                    break;
            }
          
        }
    }
}
