using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{
 
    public enum ProtocolKind
    {
        NULL = 0,
        Socket = 1,
        UDP = 2,
        TCP = 3
    }

    [Guid("32565504-2D50-4B35-AD1C-69882186EC34")]
    public interface IClientProtocolLib
    {
        bool Connect(ProtocolKind kind, string ipAddr, int portNum);
        bool DisConnect();
        bool IsConnected();
        bool SendMsg(string data);
        string ReceiveMsg();
        string GetErrorMsg();
        bool SendArgs(string sendArgs);
        string ReceiveArgs();
    }

    [Guid("28FF5549-40A2-468D-A34D-5927D8D5A8A6")]
    public class ClientProtocolLib : IClientProtocolLib
    {

        private string _ipAdddress;
        private int _portNum;
        private ProtocolKind _kind;

        private TcpClient _tcpClient;
        private UdpClient _udpClient;
        private Socket _socketClient;   //https://jinjae.tistory.com/50

        private string _errorMsg;
     
        public bool Connect(ProtocolKind kind, string ipAddr, int portNum)
        {
            if(kind == ProtocolKind.NULL)
            {
                _errorMsg = "Protocol is null";
                return false;
            }
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
                    case ProtocolKind.TCP:
                        _tcpClient = new TcpClient();
                        _tcpClient.Connect(ipAddr, portNum);
                        break;
                    case ProtocolKind.UDP:                       
                        _udpClient = new UdpClient();
                        //byte[] addrByte = Encoding.Default.GetBytes(_ipAdddress);
                        //IPAddress iPAddress = new IPAddress(addrByte);
                        //IPEndPoint pEndPoint = new IPEndPoint(iPAddress, _portNum);
                        _udpClient.Connect(IPAddress.Parse(ipAddr), portNum);
                        break;
                    case ProtocolKind.Socket:
                        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(_ipAdddress), _portNum);
                        _socketClient = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        _socketClient.Connect(ipe);
                        break;
                }
            }
            catch(Exception e)
            {
                _errorMsg = $"Failed To Connect ip : {ipAddr}  port : {portNum}\n{e.ToString()}\n";
                DisConnect();
                return false;
            }
  

            return true;
        }

        private void Initialize()
        {
            _kind = ProtocolKind.NULL;
            _ipAdddress = string.Empty;
            _portNum = 0;
        }

        public bool DisConnect()
        {
            Initialize();
            if(_tcpClient != null)
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


            return true;
        }


        public bool IsConnected()
        {
            if(_kind == ProtocolKind.NULL)
            {
                return false;
            }

            switch (_kind)
            {
                case ProtocolKind.TCP:
                    return _tcpClient.Connected;
                case ProtocolKind.UDP:
                    if (_udpClient == null)
                    {
                        return false;
                    }
                    break;
                case ProtocolKind.Socket:
                    if (_socketClient == null)
                    {
                        return false;
                    }
                    return _socketClient.Connected;

            }
            return true;
        }

        public string ReceiveMsg()
        {
            if (IsConnected() == false)
            {
                return null;
            }
            byte[] buf = new byte[1024];
            string recvMsg = null;
            try
            {
                switch (_kind)
                {
                    case ProtocolKind.TCP:
                        _tcpClient.GetStream().Read(buf, 0, buf.Length);
                        //recvMsg = buf.ToString();
                        break;
                    case ProtocolKind.UDP:
                        //byte[] addrByte = Encoding.Default.GetBytes(_ipAdddress);
                        //IPAddress iPAddress = new IPAddress(addrByte);
                        IPEndPoint pEndPoint = new IPEndPoint(IPAddress.Any, _portNum);
                        buf = _udpClient.Receive(ref pEndPoint);
                        break;
                    case ProtocolKind.Socket:
                        _socketClient.Receive(buf);
                        break;
                }
            }
            catch(Exception e)
            {
                _errorMsg = $"Failed to Receive Message\n{e.ToString()}\n";
                return _errorMsg;
            }


            recvMsg = Encoding.UTF8.GetString(buf).TrimEnd('\0');//buf.ToString();
            return recvMsg;
        }

        public bool SendMsg(string msg)
        {
            if(IsConnected() == false)
            {
                _errorMsg = $"Not Connected. {_kind.ToString()}  Addr : {_ipAdddress}   Port : {_portNum}";
                return false;
            }
            try
            {
                byte[] buf = Encoding.Default.GetBytes(msg);
                switch (_kind)
                {
                    case ProtocolKind.TCP:
                        _tcpClient.GetStream().Write(buf, 0, buf.Length);
                        break;
                    case ProtocolKind.UDP:
                        //byte[] addrByte = Encoding.Default.GetBytes(_ipAdddress);
                        //IPAddress iPAddress = new IPAddress(addrByte);
                        //IPEndPoint pEndPoint = new IPEndPoint(iPAddress, _portNum);
                        _udpClient.Send(buf, buf.Length);                      
                        break;
                    case ProtocolKind.Socket:
                        _socketClient.Send(buf, buf.Length, SocketFlags.None);
                        break;
                }

                return true;
            }
            catch (Exception e)
            {
                _errorMsg = $"Failed To Send Message  Protocol : {_kind.ToString()}  IP : {_ipAdddress}  Port : {_portNum}\n{e.ToString()}\n";
                return false;
            }

        }
        public string GetErrorMsg()
        {
            string tmp = _errorMsg;

            _errorMsg = string.Empty;

            return tmp;
        }

        public bool SendArgs(string sendArgs)
        {
            if (IsConnected() == false)
            {
                _errorMsg = $"Not Connected. {_kind.ToString()}  Addr : {_ipAdddress}   Port : {_portNum}";
                return false;
            }
            bool res = false;
            switch (_kind)
            {
                case ProtocolKind.TCP:
                    try
                    {
                        byte[] buf = Encoding.UTF8.GetBytes(sendArgs);
                        _tcpClient.GetStream().Write(buf, 0, buf.Length);
                        res = true;
                    }
                    catch(Exception e)
                    {
                        _errorMsg = $"Error to Send Argument.\n{e.ToString()}";
                        res = false;
                    }
                    break;
                default:
                    _errorMsg = $"Only use TCP Protocol. Current : {_kind.ToString()}";
                    res = false;
                    break;
            }
            return res;
        }

        public string ReceiveArgs()
        {
            if (IsConnected() == false)
            {
                _errorMsg = $"Not Connected. {_kind.ToString()}  Addr : {_ipAdddress}   Port : {_portNum}";
                return null;
            }
       
            string recvArgs = string.Empty;

            switch (_kind)
            {
                case ProtocolKind.TCP:
                    try
                    {
                        byte[] buf = new byte[1024];
                        _tcpClient.GetStream().Read(buf, 0, buf.Length);
                        recvArgs = Encoding.UTF8.GetString(buf).TrimEnd('\0');
                    }catch(Exception e)
                    {
                        _errorMsg = $"Error to Send Argument.\n{e.ToString()}";
                    }
                    break;
                default:
                    _errorMsg = $"Only use TCP Protocol. Current : {_kind.ToString()}";
                    break;
            }

            return recvArgs;
        }
    }
}
