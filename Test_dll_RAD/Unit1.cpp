//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 *Form1;
//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
	: TForm(Owner)
{

	HRESULT hr;
	hr = CoInitialize(NULL);
	if(FAILED(hr)){
		LogList->Lines->Append(L"Error1");
		return;
	}
		 hr = CoCreateInstance(CLSID_ClientProtocolLib,
		 NULL, CLSCTX_INPROC_SERVER,
		IID_IClientProtocolLib, reinterpret_cast<void**>(&cpi));

	if(FAILED(hr))
	{       if(cpi != NULL){
				cpi->Release();
			}

			CoUninitialize();
				LogList->Lines->Append(L"Error");
	}else{
		LogList->Lines->Append(L"Success to Create Client");
	}


}
__fastcall TForm1::~TForm1(){

	// disconnect
	if(cpi!=NULL){
		cpi->DisConnect();
	}
	 CoUninitialize();
}
//---------------------------------------------------------------------------
//---------------------------------------------------------------------------



void __fastcall TForm1::ButtonConnectClick(TObject *Sender)
{
	try {
		   LogList->Lines->Append(L"ButtonConnectClick");

		   if(cpi->IsConnected()){
				cpi->DisConnect();
		   }
		   LogList->Lines->Append(String().sprintf(L"Protocol : %s, IP : %s, Port : %s"
				,m_protocol,LabelIPAddr->Text.c_str(),LabelPortNum->Text ));

			BSTR ipAddr = SysAllocString(LabelIPAddr->Text.c_str());

			VARIANT_BOOL res = cpi->Connect((ProtocolKind)(ComboBoxProtocol->ItemIndex),
				ipAddr,//LabelIPAddr->Text.c_str(),
				StrToInt64Def(LabelPortNum->Text,5000));
			SysFreeString(ipAddr);
			if(res == VARIANT_FALSE){
				 LogList->Lines->Append(L"Failed to Connect");
					wchar_t* errorMsg;
		cpi->GetErrorMsg(&errorMsg);
		LogList->Lines->Append(errorMsg);
			}else{
                LogList->Lines->Append(L"Success to Connect");
            }
	} catch (...) {
		LogList->Lines->Append(L"Failed to Connect. Catch Error");
		wchar_t* errorMsg;
		cpi->GetErrorMsg(&errorMsg);
		LogList->Lines->Append(errorMsg);
	}

}

void __fastcall TForm1::ButtonDisonnectClick(TObject *Sender)
{
	try {
	   LogList->Lines->Append(L"ButtonDisonnectClick");
	   if(cpi==NULL){
			return;
	   }
		if(cpi->IsConnected()){
			VARIANT_BOOL res = cpi->DisConnect();
			if(res == VARIANT_TRUE){
				 LogList->Lines->Append(L"Success to Disconnect.");
			}else{
				LogList->Lines->Append(L"Failed to Disconnect.");
			}
	   }
	} catch (...) {
		LogList->Lines->Append(L"Failed to Disconnect. Catch Error");
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::ChangeProtocol(TObject *Sender)
{
	switch(ComboBoxProtocol->ItemIndex){
		case 1:
				m_protocol = L"TCP";
				break;
				case 2:
				m_protocol = L"UDP";
				break;
				case 3:
				m_protocol = L"TCP";
				break;
	}

}
//---------------------------------------------------------------------------

void __fastcall TForm1::ButtonSendMsgClick(TObject *Sender)
{
	if(cpi == NULL){
		LogList->Lines->Append(L"Failed to Send. COM is NULL");
	}
	if(cpi->IsConnected()){
		  LogList->Lines->Append(String().sprintf(L"Protocol : %s, IP : %S, Port : %s"
				,m_protocol,LabelIPAddr->Text,LabelPortNum->Text ));
		  LogList->Lines->Append(String().sprintf(L"Send Message : %s"
				,LabelSendMessage->Text.c_str()));
		try {
          	BSTR sendMsg = SysAllocString(LabelSendMessage->Text.c_str());
		  VARIANT_BOOL res = cpi->SendMsg(sendMsg);
		  SysFreeString(sendMsg);
		  if(res == VARIANT_FALSE){
				wchar_t *errorMsg;
				cpi->GetErrorMsg(&errorMsg);
				LogList->Lines->Append(errorMsg);
				return;
		  }


		  wchar_t *receivMsg;
		   cpi->ReceiveMsg(&receivMsg);

		  LogList->Lines->Append(String().sprintf(L"Receive Message : %s"
				,receivMsg));
		} catch (...) {
              LogList->Lines->Append(L"Error Occur Send Message. Catch Error");
		}

	}else{
		LogList->Lines->Append(L"Not Connected");
		wchar_t *errorMsg;
		cpi->GetErrorMsg(&errorMsg);
		LogList->Lines->Append(errorMsg);
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::ButtonTestClick(TObject *Sender)
{
	LogList->Lines->Append(LabelTest->Text);
}
//---------------------------------------------------------------------------

