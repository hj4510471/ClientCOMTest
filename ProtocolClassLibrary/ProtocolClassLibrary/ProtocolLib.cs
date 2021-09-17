using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ProtocolClassLibrary
{
    public enum ProtocolKind
    {
        NULL = 0,
        Socket = 1,
        UDP = 2,
        TCP = 3
    }

    //여기가 C++과 연동하기 위한 interface부분이다.    
    [Guid("9AE22444-7188-45D0-B1F2-C05F4AEA80FC")]
    public interface IProtocolLib
    {
        //여기에 C++에서 사용할 메소드들의 프로토타입을 지정해 주면 된다.
        //C++에서 사용하게 될 public 형태의 메소드들이 여기에 해당된다.
        //private 모드의 메소드는 여기에 지정이 불가능하다. 
        //생성자는 여기서 지정해 줄 필요가 없다.
        //메소드들만 지정해 주면 된다.
        bool ConnectLib(ProtocolKind protocol, string host, int port);
        bool DisconnectLib();
        string SendBufLib(string buf);
        string GetInfoLib();
    }


    //여기가 사용자가 원하는 기능을 구현한 클래스이다.
    [Guid("81B5DC06-BDAC-4439-9D14-4BDB7DA3A588")]
    public class ProtocolLib : IProtocolLib
    {
        //여기서 주의 해야할 사항은 SerialPort나 Thread 같은경우 처음 사용 후에 
        //포트를 닫는 동작이 필요하거나 Thread를 종료시키는 동작이 필요할 경우 등
        //여러 메소드들에서 사용을 해야하는 멤버 변수들은 반드시 static으로
        //선언해 주어야 한다. 왜냐하면 비록 전역 변수 형태로 선언되었다 할지라도
        //처음 사용했던 메소드가 아닌 다른 메소드에서 또 사용되어질 경우는
        //해당 멤버 변수(Thread, SerialPort...)가 null 상태가 되어 버린다.
        //따라서 C++에서 계속해서 해당 멤버 변수를사용할수 있도록 하기 위해서는
        //반드시 static으로 선언해 주어야 한다.

        private ProtocolKind _protocol;
        private string _host;
        private int _port;

        // server
        

        private void InitLib()
        {
            _protocol = ProtocolKind.NULL;
            _host = string.Empty;
            _port = 0;
        }

        public bool ConnectLib(ProtocolKind protocol, string host, int port)
        {


            _protocol = protocol;
            _host = host;
            _port = port;

            
            return true;

        }

        public bool DisconnectLib()
        {
            InitLib();

            return true;
        }

        public string SendBufLib(string buf)
        {

            string ret = string.Empty;

            ret = buf + " ret";

            return ret;
        }

        public string GetInfoLib()
        {
            string ret = string.Format($"Host:{_host}:{_port} (Protocol:{_protocol.ToString()})");

            return ret;

        }

        public void ShowInfoLib()
        {
            
        }
    }
}
