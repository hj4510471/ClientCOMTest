//---------------------------------------------------------------------------

#ifndef Unit1H
#define Unit1H
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include <Vcl.ExtCtrls.hpp>

#include "ClientLib_TLB.h"
//---------------------------------------------------------------------------


class TForm1 : public TForm
{
__published:	// IDE-managed Components
//public 과는 달리  선언된 데이터 멤버와 속성에 대해 델파이 스타일RTTI(런타임 유형정보)가 생성됨
// https://docwiki.embarcadero.com/RADStudio/Tokyo/en/Published
	TButton *ButtonConnect;
	TComboBox *ComboBoxProtocol;
	TButton *ButtonDisconnect;
	TLabeledEdit *LabelIPAddr;
	TMemo *LogList;
	TLabeledEdit *LabelSendMessage;
	TButton *Button1;
	TLabeledEdit *LabelPortNum;
	TLabeledEdit *LabelTest;
	TButton *ButtonTest;

	void __fastcall ButtonConnectClick(TObject *Sender);
	void __fastcall ButtonDisonnectClick(TObject *Sender);
	void __fastcall ChangeProtocol(TObject *Sender);
	void __fastcall ButtonSendMsgClick(TObject *Sender);
	void __fastcall ButtonTestClick(TObject *Sender);
private:	// User declarations
	IClientProtocolLib *cpi;
	String m_protocol;
public:		// User declarations
	__fastcall TForm1(TComponent* Owner);
	__fastcall ~TForm1();

};
//---------------------------------------------------------------------------
extern PACKAGE TForm1 *Form1;
//---------------------------------------------------------------------------
#endif
