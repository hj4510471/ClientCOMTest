//---------------------------------------------------------------------------

#ifndef Unit1H
#define Unit1H
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include <IdBaseComponent.hpp>
#include <IdComponent.hpp>
#include <IdContext.hpp>
#include <IdCustomTCPServer.hpp>
#include <IdSocketHandle.hpp>
#include <IdTCPClient.hpp>
#include <IdTCPConnection.hpp>
#include <IdTCPServer.hpp>
#include <IdUDPBase.hpp>
#include <IdUDPClient.hpp>
#include <IdUDPServer.hpp>
#include <System.Win.ScktComp.hpp>
#include <Vcl.ExtCtrls.hpp>
//#include <Web.Win.Sockets.hpp>
#include <IdGlobal.hpp>

#define RESPONSE_WAITING_TIME 3000
#define MGC_SOCK_PACK (ULONG)'MGSP'


typedef struct _TEXT_PACKET
{
	char        Text[2048];

}TEXT_PACKET, *PTEXT_PACKET;

typedef struct _SOCKET_PACKET
{
	ULONG       Magic;
	int         Length;
	char        Text[2048];

}SOCKET_PACKET, *PSOCKET_PACKET;

typedef struct _SOCKET_LONG_PACKET
{
	ULONG       Magic;
	int         Length;
	char*       Text;

}SOCKET_LONG_PACKET, *PSOCKET_LONG_PACKET;
//---------------------------------------------------------------------------
class TForm1 : public TForm
{
__published:	// IDE-managed Components
	TGroupBox *GroupBox1;
	TLabel *Label1;
	TLabel *Label2;
	TBevel *Bevel1;
	TLabel *Label4;
	TMemo *txtUDPSendMsg;
	TEdit *edtUDPTargetIP;
	TEdit *edtUDPTargetPort;
	TButton *btnUDPServerOpen;
	TEdit *edtUDPServerPort;
	TMemo *txtUDPRecvMsg;
	TMemo *txtUDPResponseMsg;
	TButton *btnUDPSendMsg;
	TIdUDPServer *IdUDPServer1;
	TIdUDPClient *IdUDPClient1;
	TGroupBox *GroupBox2;
	TLabel *Label3;
	TLabel *Label5;
	TBevel *Bevel2;
	TLabel *Label6;
	TMemo *txtTCPSendMsg;
	TEdit *edtTCPTargetIP;
	TEdit *edtTCPTargetPort;
	TButton *btnTCPServerOpen;
	TEdit *edtTCPServerPort;
	TMemo *txtTCPRecvMsg;
	TMemo *txtTCPResponseMsg;
	TButton *btnTCPSendMsg;
	TGroupBox *GroupBox3;
	TLabel *Label7;
	TLabel *Label8;
	TBevel *Bevel3;
	TLabel *Label9;
	TMemo *txtSocketSendMsg;
	TEdit *edtSocketTargetIP;
	TEdit *edtSocketTargetPort;
	TButton *btnSocketServerOpen;
	TEdit *edtSocketServerPort;
	TMemo *txtSocketRecvMsg;
	TMemo *txtSocketResponseMsg;
	TButton *btnSocketSendMsg;
	TClientSocket *ClientSocket1;
	TServerSocket *ServerSocket1;
	TIdTCPClient *IdTCPClient1;
	TIdTCPServer *IdTCPServer1;
	void __fastcall btnUDPServerOpenClick(TObject *Sender);
	void __fastcall IdUDPServer1UDPRead(TIdUDPListenerThread *AThread, const TIdBytes AData,
          TIdSocketHandle *ABinding);
	void __fastcall btnUDPSendMsgClick(TObject *Sender);
	void __fastcall btnSocketServerOpenClick(TObject *Sender);
	void __fastcall btnSocketSendMsgClick(TObject *Sender);
	void __fastcall ServerSocket1ClientRead(TObject *Sender, TCustomWinSocket *Socket);
	void __fastcall btnTCPServerOpenClick(TObject *Sender);
	void __fastcall btnTCPSendMsgClick(TObject *Sender);
	void __fastcall IdTCPServer1Execute(TIdContext *AContext);







private:	// User declarations
public:		// User declarations
	__fastcall TForm1(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TForm1 *Form1;
//---------------------------------------------------------------------------
#endif
