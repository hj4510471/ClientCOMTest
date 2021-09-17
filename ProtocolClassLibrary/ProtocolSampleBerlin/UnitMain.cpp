//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "UnitMain.h"
#include "ProtocolClassLibrary_TLB.h"

//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"


TForm1 *Form1;
//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
	: TForm(Owner)
{
	CoInitialize(NULL); //Init COM library DLLs

	IProtocolLib* lib;

	HRESULT hr = CoCreateInstance ( CLSID_ProtocolLib,
									NULL,
									CLSCTX_INPROC_SERVER,
									IID_IProtocolLib,
									(void**) &lib );
	if (SUCCEEDED (hr))
	{
		lib->ConnectLib(ProtocolKind::ProtocolKind_Socket, L"127.0.0.1", 5500);

		WideString info;
		lib->GetInfoLib(&info);

		ShowMessage(info);


		delete info;
		lib->Release();
	}

	CoUninitialize();
}
//---------------------------------------------------------------------------
