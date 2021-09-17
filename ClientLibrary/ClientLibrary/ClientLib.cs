using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientLibrary
{
    public enum KIND { NONE, TCPIP, UDPKIND, SOCKETKIND }

    interface IClientLib
    {
        bool Connect(KIND kind, string ipAddr, int portNum);
        bool DisConnect();
        bool IsConnected();
        bool SendMsg(PacketData data);
        PacketData ReceiveMsg();
    }

    public class ClientLib : IClientLib
    {
        // dll -> COM  //https://hwanschoi.tistory.com/118
        

        private string _ipAdddress;
        private int _portNum;
        private KIND _kind;

        private TcpClient _tcpClient;
        private UdpClient _udpClient;
        private Socket _socketClient;   //https://jinjae.tistory.com/50

        public ClientLib() { }
        public bool Connect(KIND kind,string ipAddr,int portNum)
        {
            if (string.IsNullOrEmpty(ipAddr))
            {
                return false;
            }

            if (IsConnected())
            {
                DisConnect();
            }

            _ipAdddress = ipAddr;
            _portNum = portNum;
            _kind = kind;
            
            try
            {
                switch (kind)
                {
                    case KIND.TCPIP:
                        _tcpClient = new TcpClient();
                        _tcpClient.Connect(ipAddr, portNum);
                        break;
                    case KIND.UDPKIND:
                        _udpClient = new UdpClient();
                        _udpClient.Connect(ipAddr, portNum); // 연결을 해주나??
                        break;
                    case KIND.SOCKETKIND:
                        _socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Unspecified);
                        _socketClient.Connect(ipAddr, portNum);
                        break;
                }
            }
            catch 
            {
                return false;
            }
         

            return true;
        }
        public bool DisConnect()
        {
            try
            {
                if ((bool)(_tcpClient?.Connected))
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                }
                if(_udpClient != null)
                {
                    _udpClient.Close();
                    _udpClient = null;
                }
                if (_socketClient != null)
                {
                    _socketClient.Close();
                    _socketClient = null;
                }
            }
            catch 
            {
                return false;
            }
            finally
            {
                _kind = KIND.NONE;
            }
            return true;
   
        }
        public bool IsConnected()
        {
            if (_kind == KIND.NONE)
                return false;
            return true;
        }

        public bool SendMsg(PacketData data)
        {
            if (IsConnected() == false)
            {
                // ERROR
                return false;
            }

            byte[] dataToByte = Encoding.Default.GetBytes(data.text);

            try
            {
                switch (_kind)
                {
                    case KIND.TCPIP:
                        _tcpClient.GetStream().Write(dataToByte, 0, dataToByte.Length);
                        break;
                    case KIND.UDPKIND:
                        _udpClient.Send(dataToByte, dataToByte.Length);
                        break;
                    case KIND.SOCKETKIND:
                        _socketClient.Send(dataToByte);
                        break;
                }
            }
            catch 
            {
                return false;
            }
      
            return true;
        }

        public PacketData ReceiveMsg()
        {
            if(IsConnected() == false)
            {
                return null;
            }

            PacketData res = new PacketData();
            try
            {
                switch (_kind)
                {
                    case KIND.TCPIP:
                        {
                            byte[] msg = new byte[1024];
                            _tcpClient.GetStream().Read(msg, 0, msg.Length);
                            res.text = msg.ToString();
                        }
                        break;
                    case KIND.UDPKIND:
                        {
                            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                            var msg = _udpClient.Receive(ref RemoteIpEndPoint);
                            res.text = msg.ToString();
                        }
                        break;
                    case KIND.SOCKETKIND:
                        {
                            byte[] msg = new byte[1024];
                            _socketClient.Receive(msg);
                            res.text = msg.ToString();
                        }
                        break;
                }
            }
            catch
            {
                res.text = "Occur Error.";
                
            }
  

            return res;
        }


    }
}
