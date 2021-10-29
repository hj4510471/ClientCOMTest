using ClientArgsTest.Views;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
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

        public enum STATUS_ORDER
        {
            ONAIR = 0,
            LOAD = 1,
            REPLACE = 2,
            STAMP1 = 3,
            STAMP2 = 4,
            STAMP3 = 5,
        }

        public enum COMMAND_KIND
        {
            LOAD,
            FIND,
            REPLACE,
            PLAY,
            STOP,
            SETSTAMP,
            PLAYSTAMP,
            STOPSTAMP,
            
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
        private bool _isCheckStatusSuccess;
        public bool IsCheckStatusSuccess
        {
            get { return _isCheckStatusSuccess; }
            set { SetProperty(ref _isCheckStatusSuccess, value); }
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
        private string _loadTemplatePath;
        public string LoadTemplatePath
        {
            get { return _loadTemplatePath; }
            set { SetProperty(ref _loadTemplatePath, value); }
        }
        private string _findPageNum;
        public string FindPageNum
        {
            get { return _findPageNum; }
            set { SetProperty(ref _findPageNum, value); }
        }

        private string _loadArgs;
        public string LoadArgs
        {
            get { return _loadArgs; }
            set { SetProperty(ref _loadArgs, value); }
        }
        private string _findArgs;
        public string FindArgs
        {
            get { return _findArgs; }
            set { SetProperty(ref _findArgs, value); }
        }
        private string _replaceArgs;
        public string ReplaceArgs
        {
            get { return _replaceArgs; }
            set { SetProperty(ref _replaceArgs, value); }
        }
        private string _playStopArgs;
        public string PlayStopArgs
        {
            get { return _playStopArgs; }
            set { SetProperty(ref _playStopArgs, value); }
        }
        #endregion

        #region Command
        public ICommand ClickedConnectCommand { get; set; }
        public ICommand ClickedDisConnectCommand { get; set; }
        public ICommand ClickedSendMsgCommand { get; set; }
        public ICommand SendArgsCommand { get; set; }

        //public ICommand MakeCommand { get; set; }

        public ICommand CheckStatusCmd { get; set; }
        public ICommand LoadTemplateCmd { get; set; }
        public ICommand FindPageNumCmd { get; set; }
        public ICommand PlayCmd { get; set; }
        public ICommand StopCmd { get; set; }
        

        public ICommand BtnCueClicked { get; set; }
        public ICommand BtnInClicked { get; set; }
        public ICommand BtnOutClicked { get; set; }


        #endregion

        #region Variable
        private readonly char SEPARATORARGS = '^';
        private readonly char SPECIAL = '~';
        private readonly char STARTARGS = '$';
        private readonly char ENDARGS = '#';
        private readonly string RETURN_SUCCESS = "1";
        private readonly string RETURN_FAILED = "0";
        private readonly string STATUS_SUCCESS = "0";

        private readonly string STATUS_INIT = "00";
        private readonly string STATUS_RUN = "10";
        private readonly string STATUS_COMPLETE = "01";


        ClientLib.ClientProtocolLib _client;

        string curCommand = "";
        int commandResult = 0; // -1 : init or Failed, 0 : Running  1 : Success
        //SetCommandWindow childWindow;
        #endregion
        public MainWindowViewModel()
        {
            _client = new ClientLib.ClientProtocolLib();

            SendArgsCommand = new DelegateCommand(SendArgsCmd);
            ClickedConnectCommand = new DelegateCommand(ConnectCmd);
            ClickedDisConnectCommand = new DelegateCommand(DisconnectCmd);
            ClickedSendMsgCommand = new DelegateCommand(SendMsgCmd);

            //MakeCommand = new DelegateCommand(ShowMakeCommand);

            CheckStatusCmd = new DelegateCommand(SendStatudCmd);
            LoadTemplateCmd = new DelegateCommand(SendLoadCmd);
            FindPageNumCmd = new DelegateCommand(SendFindCmd);
            PlayCmd = new DelegateCommand(SendPlayCmd);
            StopCmd = new DelegateCommand(SendStopCmd);

            BtnCueClicked = new DelegateCommand(BtnCueClickedCmd);
            BtnInClicked = new DelegateCommand(BtnInClickedCmd);
            BtnOutClicked = new DelegateCommand(BtnOutClickedCmd);

            IpAddr = "127.0.0.1";
            PortNum = 6565;
            ProtocolList = new ObservableCollection<string>();
            ProtocolList.Add("TCP");
            //ProtocolList.Add("UDP");
            //ProtocolList.Add("SOCKET");

            LoadArgs = "$^0^^apc^load^filepath^#";
            FindArgs = "$^0^^apc^find^pagenum^#";
            PlayStopArgs = "$^0^^apc^play^#";
            ReplaceArgs = "$^0^^apc^Replace^alias^value^alias1^value1^#";
        }

   

        #region Client Connect,Send,Receive
        public void ConnectCmd()
        {
            try
            {
                var res = _client.Connect((ClientLib.ProtocolKind)Enum.Parse(typeof(ClientLib.ProtocolKind), Protocol, true), IpAddr, PortNum);

                if (res == false)
                {
                    WriteLog("Connect Failed");
                    WriteLog(_client.GetErrorMsg());
                    
                    return;
                }else
                    WriteLog("Connect Success");
            }
            catch (Exception e)
            {
                WriteLog($"Connect Failed : {e.ToString()}");

            }
        }
        private void DisconnectCmd()
        {
            try
            {
                var res = _client.DisConnect();

                if (res == false)
                {
                    WriteLog("DisConnect Failed");
                    return;
                }

                WriteLog("DisConnect Success");
            }
            catch (Exception e)
            {
                WriteLog($"DisConnect Failed : {e.ToString()}");
            }

        }
        private void SendMsgCmd()
        {
            var res = _client.SendMsg(SendMessage);

            if (res == false)
            {
                WriteLog("Failed to Send Message");
                WriteLog(_client.GetErrorMsg());
                return;
            }
            WriteLog($"Send : {SendMessage}");

            string msg = _client.ReceiveMsg();
            if (string.IsNullOrEmpty(msg))
            {

                return;
            }

            WriteLog($"receive : {msg}");
        }

        private void SendArgsCmd()
        {
            if (string.IsNullOrEmpty(SendArgs))
            {
                WriteLog($"Error. Argument to Send is Empty");
                return;
            }

            bool res = _client.SendArgs(SendArgs);
            if (res == false)
            {
                WriteLog($"Failed to Send Args");
                WriteLog($"Error : {_client.GetErrorMsg()}");
                return;
            }
            
            WriteLog($"Send Args : {SendArgs}");
            string recvArgs = _client.ReceiveArgs();
            WriteLog($"Receive Args : {recvArgs}");
            ReceiveArgs = recvArgs;
            WriteLog(ParseReceiveArgs(recvArgs));
        }
        #endregion

        #region Parse
        private string[] SplitSpecificWord(string str)
        {

            // ‘$’, ‘^’, ‘#’, '~' 앞에 '~' 가 있는 지 확인하기
            Collection<string> list = new Collection<string>();
            int cur = 0;
            for (int i = 0; i < str.Length - 1; ++i)
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

                    }
                    else if (str[i + 1].Equals(SEPARATORARGS))
                    {
                        str = str.Remove(i, 1);

                    }
                }
                else if (str[i].Equals(SEPARATORARGS))
                {
                    list.Add(str.Substring(cur, i - cur));
                    cur = i + 1;

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
            for (int i = 0; pos.MoveNext(); ++i)
            {
                res[i] = pos.Current;
                //Log += $"{res[i]}\n";
            }
            return res;
        }

        private string ParseReceiveArgs(string recvArgs)
        {
            //“$^client_id^return^object^method^argument^argument^argument^...^#"
            /*
                client_id	: 0 ( 임의 번호 사용 )
                return		: MIR에서 응답시 사용 ( 0:실패, 1:성공 )
                object		: apc
                method	: 멤버함수 (명령어)
                argument	: 인자
             */

            if (string.IsNullOrEmpty(recvArgs))
            {
                
                return "Failed : recvArgs is Empty";
            }

            string tmpParse = string.Empty;
            // ‘$’, ‘^’, ‘#’, '~' 앞에 '~' 가 있는 지 확인하기
            var splitRes = SplitSpecificWord(recvArgs);

            if (splitRes[(int)ArgsOrder.START].Equals(STARTARGS.ToString()) == false)
            {           
                return "Failed : not start with $";
            }
            if (splitRes[splitRes.Length - 1].Contains(ENDARGS.ToString()) == false)
            {              
                return "Failed : not end with #";
            }
            //ReceiveArgs = recvArgs;
            tmpParse += $"ID : {splitRes[(int)ArgsOrder.ID]}\n";
            // {(string.IsNullOrEmpty(splitRes[(int)ArgsOrder.RETURN]) ? ": splitRes[(int)ArgsOrder.RETURN])}";
            tmpParse += $"Obj : {splitRes[(int)ArgsOrder.OBJECT]}\n";
            tmpParse += $"Method : {splitRes[(int)ArgsOrder.METHOD]}\n";

            // check return code
            if (string.IsNullOrEmpty(splitRes[(int)ArgsOrder.RETURN]))
            {
                tmpParse += $"Return : Return is Empty\n";
            }
            else if (splitRes[(int)ArgsOrder.RETURN].Equals(RETURN_SUCCESS))
            {
                tmpParse += $"Return : Success\n";
            }
            else if (splitRes[(int)ArgsOrder.RETURN].Equals(RETURN_FAILED))
            {
                tmpParse += $"Return : Failed\n";
            }
            else
            {
                tmpParse += $"Return : Error\n";
                return tmpParse;
            }

            tmpParse += ParseStatusArgument(splitRes);

            return tmpParse;
        }

        private string ParseStatusArgument(string[] argArr)
        {
            string tmpParse = string.Empty;
            switch (argArr[(int)ArgsOrder.METHOD].ToLower())
            {
                case "status":
                    tmpParse += $"Status Code : {argArr[(int)ArgsOrder.ARGS1]}\n";
                    //ParseStatusCode(argArr[(int)ArgsOrder.ARGS1]);
                    tmpParse += $"Project : {argArr[(int)ArgsOrder.ARGS2]}\n";
                    int pageNum = int.Parse(argArr[(int)ArgsOrder.ARGS3]);
                    if (pageNum < 0)    // -1 : page Not Found
                    {
                        tmpParse += $"Page Num : Not Found\n";
                    }
                    else
                    {
                        tmpParse += $"Page Num : {pageNum}\n";
                    }

                    int onAir = int.Parse(argArr[(int)ArgsOrder.ARGS4]);
                    if (onAir == 0)  // 0 : Not onair, 1 : OnAir
                    {
                        tmpParse += $"Existing On Air Page : None\n";
                    }
                    else
                    {
                        tmpParse += $"Existing On Air Page : {onAir}\n";
                    }

                    break;
                default:
                    if (argArr[(int)ArgsOrder.ARGS1].Equals(STATUS_SUCCESS))
                    {
                        tmpParse += $"Status : Success. Message : {argArr[(int)ArgsOrder.ARGS2]}\n";
                    }
                    else
                    {
                        tmpParse += $"Status : Failed. Message : {argArr[(int)ArgsOrder.ARGS2]}\n";
                    }
                    break;
            }
            return tmpParse;
        }

        private bool ParseStatusCode(string code)
        {
            // 6자리
            // 일반 페이지 : 앞 3바이트 사용
            //   1번째 : 1 : 송출중 , 0 : 송출중이 아님(or 완료로 초기화)
            //   2번째 : ‘8’ : load 중, ‘4’ : load완료, ‘6’ : load완료& find중, ‘5’ : load완료 & find 완료
            //   3번째 : ‘8’ : replace중, ‘4’ : replace완료, ‘6’ : replace완료&(play/stop진행중), ‘5’ : replace완료&play완료
            bool isOnAir = code[0].ToString().Equals("1");
            if (code[0].ToString().Equals("0"))
            {
                //  off-Air 또는 Done
                Log += "On Air : N\n";
            }else
            {
                // On-Air
                Log += "On Air : Y\n";
            }
            // 2번째
            // 0011 00 00
            //  0011 : 해당 ASCII 로 변환하기 위한 값
            //  00 : LOAD 상태 ( 00/10/01 ) 초기(실패)/작업중/완료
            //  00 : FIND 상태 ( 00/10/01 ) 초기(실패)/작업중/완료
            //      ‘8’ : load 중, ‘4’ : load완료, ‘6’ : load완료& find중, ‘5’ : load완료 & find 완료
            //      1000                0100                0110                    0101
            string second = MakeBinaryStr(code[1].ToString());//Convert.ToString(code[(int)STATUS_ORDER.LOAD], 2);
            string LoadState = second.Substring(4, 2);
            string FindState = second.Substring(6);

            // 3번째
            // 0011 00 00
            //  0011 : 해당 ASCII 로 변환하기 위한 값
            //  00 : REPLACE 상태       ( 00/10/01 ) 초기(실패)/작업중/완료  
            //  00 : ( PLAY/STOP 상태 ) ( 00/10/01 ) 초기(실패)/작업중/완료
            //  ‘8’ : replace중, ‘4’ : replace완료, ‘6’ : replace완료&(play/stop진행중), ‘5’ : replace완료&play완료
            //      '0': stop 완료후 ( replace, play/stop 상태가  초기화 된다, 동시에 2번재 바이트도 ‘4’ 즉 로드  완료로 변경된다 )
            //
            string third = MakeBinaryStr(code[2].ToString());//Convert.ToString(code[(int)STATUS_ORDER.REPLACE], 2);
            string ReplaceState = third.Substring(4, 2);
            string PlayState = third.Substring(6);

            Log += $"Load : {GetEachStatusParse(LoadState)}\tFind : {GetEachStatusParse(FindState)}\tReplace : {GetEachStatusParse(ReplaceState)}\tPlay : {GetEachStatusParse(PlayState)}\n";
            // STAMP : 뒤 3바이트 사용
            //   하나의 Stamp당 한자리
            //  0000 00 00
            // 00 :  Set Stamp
            //  '2' : 작업중,	'0' : Disable_설정된 파일이 없음, 	'1' : Enable_설정된 파일이 있음
            //      10              00                                  01
            // 00 :  Play / Stop Stamp
            //  '2' : 작업중,	'0' : Disable_Stop Stamp,         '1' : Enable_Play Stamp

            if (curCommand.Equals("stop"))
            {
                if(isOnAir==false && LoadState.Equals(STATUS_COMPLETE)&&PlayState.Equals(STATUS_INIT))
                {
                    return true;
                }
            }else if (curCommand.Equals("load"))
            {
                if (LoadState.Equals(STATUS_COMPLETE))
                    return true;
            }
            else if (curCommand.Equals("find"))
            {
                if (FindState.Equals(STATUS_COMPLETE))
                    return true;
            }
            else if (curCommand.Equals("replace"))
            {
                if (ReplaceState.Equals(STATUS_COMPLETE))
                    return true;
            }
            else if (curCommand.Equals("play"))
            {
                if (PlayState.Equals(STATUS_COMPLETE)&&isOnAir)
                    return true;
            }
            return false;
        }

        private string MakeBinaryStr(string src )
        {
            string tmp = string.Empty;
            int num;
            bool res = int.TryParse(src, out num);
            if (res == false)
                return "";
            char[] buff = new char[8];

            for (int i = 7; i >= 0; i--)
            {
                int mask = 1 << i;
                buff[7 - i] = (num & mask) != 0 ? '1' : '0';
            }

            return new string(buff);
        }
        private string GetEachStatusParse(string status)
        {
            if (status.Equals(STATUS_INIT))
            {
                return "Init or Fail";
            }else if (status.Equals(STATUS_COMPLETE))
            {
                return "Success";
            }
            else if(status.Equals(STATUS_RUN))
            {
                return "Running";
            }
            else
            {
                return "Status Parse Failed";
            }
        }
        #endregion

        #region Cmd
        private void SendStatudCmd()
        {
            MakeSendArgument("status", null);
            SendArgsCmd();
        }
        private void SendLoadCmd()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }
            string filename = openFileDialog.FileName;
            MakeSendArgument("load", filename);
            LoadTemplatePath = filename;
            SendArgsCmd();
            LoadArgs = SendArgs;
            if (IsCheckStatusSuccess)
            {
                Thread tmp = new Thread(CheckStatusThread);
                tmp.IsBackground = true;
                tmp.Start();
            }
        }

        private void SendFindCmd()
        {
            int num;
            if(int.TryParse(FindPageNum,out num) == false)
            {
                return;
            }
            MakeSendArgument("find", num.ToString());
            SendArgsCmd();
            FindArgs = SendArgs;
            if (IsCheckStatusSuccess)
            {
                Thread tmp = new Thread(CheckStatusThread);
                tmp.IsBackground = true;
                tmp.Start();
            }
        }
        private void SendStopCmd()
        {
            MakeSendArgument("stop", null);
            SendArgsCmd();
            PlayStopArgs = SendArgs;
            if (IsCheckStatusSuccess)
            {
                Thread tmp = new Thread(CheckStatusThread);
                tmp.IsBackground = true;
                tmp.Start();
            }
        }

        private void SendPlayCmd()
        {
            MakeSendArgument("play", null);
            SendArgsCmd();
            PlayStopArgs = SendArgs;
            if (IsCheckStatusSuccess)
            {
                Thread tmp = new Thread(CheckStatusThread);
                tmp.IsBackground = true;
                tmp.Start();
            }
        }
        private void CheckStatusThread()
        {
            // start Time
            Log += "=========================================================";
            WriteLog("Start Check Status Thread");
            bool isKeepWork = true;
            int cnt = 0;
            while (isKeepWork)
            {
                MakeSendArgument("status", null);
                _client.SendArgs(SendArgs);
                ReceiveArgs = _client.ReceiveArgs();

                WriteLog(ReceiveArgs);
                var splitRes = SplitSpecificWord(ReceiveArgs);
                if (CheckStatusResult(splitRes[(int)ArgsOrder.ARGS1]))
                {
                    isKeepWork = false;
                    break;
                }

                if (cnt++ > 5)
                {
                    isKeepWork = false;
                    break;
                }
                Thread.Sleep(100);
            }
            // end Time
            WriteLog("End Check Status Thread");
            Log += "=========================================================";
        }
        #endregion
        #region Make Args

        private void MakeSendArgument(string method, string args)
        {
            string clientID = "0";
            string tmpArgs = $"$^{clientID}^^apc^{method}";

            if (string.IsNullOrEmpty(args)==false)
            {

                tmpArgs += "^" + MakeArgumentRule(args);
            }
                tmpArgs += "^#";
            SendArgs = tmpArgs;
            curCommand = method;
        }
        private string MakeArgumentRule(string args)
        {
            string tmpArgs = args.Replace("~", "~~");
            tmpArgs = args.Replace("#", "~#");
            tmpArgs = args.Replace("^", "~^");
            tmpArgs = args.Replace("$", "~$");

            return tmpArgs;
        }
        #endregion


        #region CUE, IN, OUT
        private void BtnOutClickedCmd()
        {
            MakeSendArgument("stop", null);
            SendArgsCmd();
            PlayStopArgs = SendArgs;
            
        }

        private void BtnInClickedCmd()
        {
            MakeSendArgument("play", null);
            SendArgsCmd();
            PlayStopArgs = SendArgs;
        }

        private void BtnCueClickedCmd()
        {
            if (string.IsNullOrEmpty(LoadTemplatePath) || string.IsNullOrEmpty(FindPageNum))
            {
                return;
            }
            
            CueTask();
        }
        private async void CueTask()
        {
            int num;
            if (int.TryParse(FindPageNum, out num) == false)
            {
                return;
            }
            // load
            MakeSendArgument("load", LoadTemplatePath);
            SendArgsCmd();
            LoadArgs = SendArgs;

            // complete check 
            var splitRes = SplitSpecificWord(ReceiveArgs);
            int cnt = 0;
            while (true)
            {
                MakeSendArgument("status", null);
                _client.SendArgs(SendArgs);
                ReceiveArgs = _client.ReceiveArgs();
                if (ParseStatusCode(splitRes[(int)ArgsOrder.ARGS1]))
                {
                    break;
                }
                else
                {
                    if (cnt++ > 5)
                        break;
                }
                Thread.Sleep(100);
            }
            
            // find
            MakeSendArgument("find", num.ToString());
            SendArgsCmd();
            FindArgs = SendArgs;
        }
        #endregion
        //private void ShowMakeCommand()
        //{
        //    if(childWindow != null)
        //    {
        //        return;
        //    }
        //    childWindow = new SetCommandWindow();
        //    childWindow.DataContext = new SetCommandWindowViewModel();

        //    childWindow.Show();

        //}

        private bool CheckStatusResult(string code)
        {
            
            return ParseStatusCode(code);

            //return false;
        }

        private void WriteLog(string strMessage)
        {
            string strLog = string.Format("{0} \t{1}", DateTime.Now.ToString("HH:mm:ss.fff"), strMessage);
            Log += strLog+"\n";
            LogFile(strLog);
        }
        private void LogFile(string msg)
        {
            string filename = "log.txt";
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"\\LOG\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + filename;
            string DirPath = AppDomain.CurrentDomain.BaseDirectory + @"\\LOG\\" + DateTime.Now.ToString("yyyyMMdd");

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            if (di.Exists != true) Directory.CreateDirectory(DirPath);
            if (fi.Exists != true)
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    sw.WriteLine(msg);
                    sw.Close();
                }
            }
            else
            {

                //using (FileStream fs = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                //{
                //    byte[] tmpByte = Encoding.Default.GetBytes(msg);
                //    fs.Write(tmpByte, 0, tmpByte.Length);
                //    fs.Close();
                //}
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    sw.WriteLine(msg);
                    sw.Close();
                }
            }           
        }
        
    }
}
