
#include "Main.h"
#include <iostream>


	
//https://silverlab.tistory.com/12
ClientClassWrapper::ClientClassWrapper(void) {

	HRESULT hr;
	hr = CoInitialize(NULL);
	hr = CoCreateInstance(CLSID_ClientProtocolLib,
		NULL,
		CLSCTX_INPROC_SERVER,
		IID__ClientProtocolLib,
		reinterpret_cast<void**>(&mp_ClientClass));

	if (FAILED(hr)) {
		CoUninitialize();
	}
	else {
		OutputDebugString(L"Succeded ClientClass\n");
	}
}
ClientClassWrapper::~ClientClassWrapper(void) {
	CoUninitialize();
}
bool ClientClassWrapper::IsConnected() {
	if (mp_ClientClass == NULL) {
		return false;
	}
	return mp_ClientClass->IsConnected();
}
bool ClientClassWrapper::Connect(ProtocolKind kind,BSTR ipAddr,int portNum) {
	if (IsConnected() == false) {
		return false;
	}
	try
	{
		mp_ClientClass->Connect(kind, ipAddr, portNum);
	}
	catch (const std::exception&)
	{
		Disconnect();
	}
}
bool ClientClassWrapper::Disconnect() {
	mp_ClientClass->DisConnect();
}
bool ClientClassWrapper::SendMsg(BSTR msg) {
	mp_ClientClass->SendMsg(msg);
}
BSTR ClientClassWrapper::ReceiveMsg() {
	return mp_ClientClass->ReceiveMsg();
}
int main() {
	auto client = ClientClassWrapper::ClientClassWrapper();
	
	std::cout << "Create Client" << std::endl;
	int inputNum;
	while (true) {
		std::cout << "1. connect  2.disconnect  3. send  4.quit" << std::endl;
		std::cin >> inputNum;

		switch (inputNum) {
		case 1:
			break;
		default:
			std::cout << "Invalid input." << std::endl;
			break;

		}
	}

	return 0;
}