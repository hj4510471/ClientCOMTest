using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{

    [Guid("694E481D-F386-43B7-B1B2-6BDC573FDD3F")]
    public class NetInfo
    {
        public IClientProtocolLib Client;
        public int ID;
    }

    [Guid("B34C4288-7D7E-436D-965C-64B136DFB7BA")]
    public interface IMultiClientLib
    {
        bool Connect(ProtocolKind protocol, string ipAddr, int portNum, out int id);
        bool DisConnect(int id);
        bool IsConnect(int id);
        bool SendMessage(int id, string msg);
    }
    [Guid("8DD289B5-B04C-4F07-9DD8-13C0DFFD76BB")]
    public class MultiClientLib : IMultiClientLib
    {
        private List<NetInfo> _list;
        public struct MyId
        {
            public static int id=0;
        };
        private int _curId;

        public MultiClientLib()
        {
            if(_list == null)
                _list = new List<NetInfo>();
            //MyId.id = _curId;
        }

        public bool Connect(ProtocolKind protocol, string ipAddr, int portNum, out int id)
        {
            NetInfo tmp = new NetInfo();
            bool res = false;
            try
            {
                tmp.Client = new ClientProtocolLib();
                tmp.Client.Connect(protocol, ipAddr, portNum);

                tmp.ID = _curId++;//MyId.id++;
                _list.Add(tmp);
                id = tmp.ID;
                res = true;
            }
            catch
            {
                id = -1;
                res = false;
            }      
            return res;
        }

        public bool DisConnect(int id)
        {
            bool res = false;
            int idx = FindList(id);
            if (idx>-1)
            {
                try
                {
                    res = _list[idx].Client.DisConnect();
                }
                catch
                {
                    res = false;
                }
                finally
                {
                    _list.RemoveAt(idx);
                }
           
            }
            return res;
        }

        public bool IsConnect(int id)
        {
            bool res = false;
            int idx = FindList(id);

            if (idx>-1)
            {
                res = _list[idx].Client.IsConnected();
            }
            return res;
        }

        public bool SendMessage(int id, string msg)
        {
            bool res = false;
            int idx = FindList(id);
            if (idx > -1)
            {
                res = _list[idx].Client.SendMsg(msg);
            }
            return res;
        }
        public string ReceivMessage(int id)
        {
            string res = null;
            int idx = FindList(id);
            if (idx > -1)
            {
                res = _list[idx].Client.ReceiveMsg();
            }
            return res;
        }

        private int FindList(int id)
        {
            int res = -1;
            for(int i=0; i<_list.Count(); ++i)
            {
                if(_list[i].ID == id)
                {
                    res = i;
                    break;
                }
            }
            return res;
        }
    }
}
