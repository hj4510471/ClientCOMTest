#pragma once
#import "ClientLib.tlb" no_namespace named_guids

//struct IClientProtocolLib;

class ClientClassWrapper {
public:
	ClientClassWrapper(void);
	virtual ~ClientClassWrapper(void);

	bool IsConnected();

	bool Connect(ProtocolKind kind, BSTR ipAddr, int portNum);
	bool Disconnect();
	bool SendMsg(BSTR msg);
	BSTR ReceiveMsg();
	BSTR GetErrorMsg();

private:
	bool m_isConnected;
	IClientProtocolLib* mp_ClientClass;
	
};