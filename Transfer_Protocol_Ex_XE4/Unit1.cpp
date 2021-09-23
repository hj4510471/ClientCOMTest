// ---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"
// ---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 * Form1;

// ---------------------------------------------------------------------------
__fastcall TForm1::TForm1( TComponent * Owner )
	: TForm( Owner )
{
}

// ---------------------------------------------------------------------------
void __fastcall TForm1::btnUDPServerOpenClick( TObject * Sender )
{
	if ( btnUDPServerOpen->Caption == "OPEN" )
	{
		try
		{
			IdUDPServer1->DefaultPort = StrToIntDef( edtUDPServerPort->Text, 9999 );
			IdUDPServer1->Active      = true;
			btnUDPServerOpen->Caption = "CLOSE";
		}
		catch ( ... )
		{
			ShowMessage( "���� ���� ����" );
		}
	}
	else
	{
		try
		{
			IdUDPServer1->Active      = false;
			btnUDPServerOpen->Caption = "OPEN";
		}
		catch ( ... )
		{
			ShowMessage( "���� ���� ����" );
		}
	}
}

// ---------------------------------------------------------------------------
void __fastcall TForm1::IdUDPServer1UDPRead( TIdUDPListenerThread * AThread, const TIdBytes AData,
	TIdSocketHandle * ABinding )
{
	int    nDataLen = AData.get_length( );
	String strRecv = BytesToString( AData, 0, -1, TIdTextEncoding_UTF8 );

	// ���� ���� ǥ��
	txtUDPRecvMsg->Lines->Text = strRecv;

	// ���� ������
	ABinding->SendTo( ABinding->PeerIP, ABinding->PeerPort, txtUDPResponseMsg->Lines->Text, Id_IPv4, TIdTextEncoding_UTF8 );

	// �޽��� ó��
	Application->ProcessMessages( );

}

// ---------------------------------------------------------------------------
void __fastcall TForm1::btnUDPSendMsgClick( TObject * Sender )
{
	IdUDPClient1->Host = edtUDPTargetIP->Text;
	IdUDPClient1->Port = StrToIntDef( edtUDPTargetPort->Text, 9999 );

	String stReceive;

	try
	{
		// �����͸� �۽�
		IdUDPClient1->Send( txtUDPSendMsg->Lines->Text, TIdTextEncoding_UTF8 );

		// Read �̺�Ʈ�� ���� ����� �� �ֵ��� �޽��� ���μ���
		// �� Ŭ���̾�Ʈ�ȿ� Ŭ���̾�Ʈ/���� ����� ���ÿ� ������� �ʴٸ� �ʿ䰡 �����ϴ�
		// �� ���� ������ �̺�Ʈ ������ ���� ������ �ѹ��� �з��� ó���ǰ� �˴ϴ�.
		Application->ProcessMessages( );

		// �ִ� 100ms ��ŭ ���� �����͸� �����
		stReceive = IdUDPClient1->ReceiveString( RESPONSE_WAITING_TIME, TIdTextEncoding_UTF8 );

		ShowMessage( "�۽��� �����߽��ϴ�\n���� ���� : " + stReceive );
	}
	catch ( ... )
	{
		ShowMessage( "�۽��� �����߽��ϴ�" );
	}
}


// ---------------------------------------------------------------------------
void __fastcall TForm1::btnSocketServerOpenClick( TObject * Sender )
{
	if ( btnSocketServerOpen->Caption == "OPEN" )
	{
		try
		{
			ServerSocket1->Port          = StrToIntDef( edtSocketServerPort->Text, 9999 );
			ServerSocket1->Active        = true;
			btnSocketServerOpen->Caption = "CLOSE";
		}
		catch ( ... )
		{
			ShowMessage( "���� ���� ����" );
		}
	}
	else
	{
		try
		{
			ServerSocket1->Active        = false;
			btnSocketServerOpen->Caption = "OPEN";
		}
		catch ( ... )
		{
			ShowMessage( "���� ���� ����" );
		}
	}
}


// ---------------------------------------------------------------------------
void __fastcall TForm1::btnSocketSendMsgClick( TObject * Sender )
{
	ClientSocket1->Host = edtSocketTargetIP->Text;
	ClientSocket1->Port = StrToIntDef( edtSocketTargetPort->Text, 9999 );
	if ( ClientSocket1->Active )
	{
		ClientSocket1->Active = false;
	}
	ClientSocket1->Active = true;

	String stReceive;

	try
	{
		// �����͸� �۽�
		AnsiString strAnsi = AnsiString( txtSocketSendMsg->Lines->Text );

		SOCKET_PACKET stSockPacket;
		memset( & stSockPacket, 0, sizeof( SOCKET_PACKET ) );
		stSockPacket.Magic  = MGC_SOCK_PACK;
		stSockPacket.Length = strAnsi.Length( );
		strcpy( stSockPacket.Text, strAnsi.c_str( ) );
		ClientSocket1->Socket->SendBuf( & stSockPacket, sizeof( SOCKET_PACKET ) );

		// �޽��� ó��
		Application->ProcessMessages( );

		char chBuf[ 2048 ] =
		{
			0,
		} ;
		int nRecvLen = ClientSocket1->Socket->ReceiveLength( );
		if ( nRecvLen )
		{
			ClientSocket1->Socket->ReceiveBuf( chBuf, nRecvLen );
			SOCKET_PACKET stSockPacket_Recv;
			memcpy( & stSockPacket_Recv, chBuf, sizeof( SOCKET_PACKET ) );
			ShowMessage( "�۽��� �����߽��ϴ�\n���� ���� : " + String( stSockPacket_Recv.Text ) );
		}

	}
	catch ( ... )
	{
		ShowMessage( "�۽��� �����߽��ϴ�" );
	}

	ClientSocket1->Active = false;
}
// ---------------------------------------------------------------------------

void __fastcall TForm1::ServerSocket1ClientRead( TObject * Sender, TCustomWinSocket * Socket )

{
	SOCKET_PACKET stSockPacket_Send, stSockPacket_Recv;
	memset( & stSockPacket_Send, 0, sizeof( SOCKET_PACKET ) );
	memset( & stSockPacket_Recv, 0, sizeof( SOCKET_PACKET ) );

	char chBuf[ 2048 ] =
	{
		0,
	} ;
	int nRecvLen = Socket->ReceiveLength( );
	if ( nRecvLen > 0 )
	{
		Socket->ReceiveBuf( chBuf, nRecvLen );
		memcpy( & stSockPacket_Recv, chBuf, sizeof( SOCKET_PACKET ) );

		if ( stSockPacket_Recv.Magic == MGC_SOCK_PACK )
		{
			// ���� ���� ǥ��
			txtSocketRecvMsg->Lines->Text = String( stSockPacket_Recv.Text );

			// ���� ������
			AnsiString strAnsi = AnsiString( txtSocketResponseMsg->Lines->Text );
			stSockPacket_Send.Magic  = MGC_SOCK_PACK;
			stSockPacket_Send.Length = strAnsi.Length( );
			strcpy( stSockPacket_Send.Text, strAnsi.c_str( ) );
			Socket->SendBuf( & stSockPacket_Send, sizeof( SOCKET_PACKET ) );

		}
		else
		{
			String AnsiData = Utf8ToAnsi(chBuf);
			txtSocketRecvMsg->Lines->Text = AnsiData;
			Socket->SendBuf( chBuf, nRecvLen );
		}

		// �޽��� ó��
		Application->ProcessMessages( );
	}
}
// ---------------------------------------------------------------------------

void __fastcall TForm1::btnTCPServerOpenClick(TObject *Sender)
{
	if ( btnTCPServerOpen->Caption == "OPEN" )
	{
		try
		{
			IdTCPServer1->DefaultPort          = StrToIntDef( edtTCPServerPort->Text, 9999 );
			IdTCPServer1->Active        = true;
			btnTCPServerOpen->Caption = "CLOSE";
		}
		catch ( ... )
		{
			ShowMessage( "���� ���� ����" );
		}
	}
	else
	{
		try
		{
			IdTCPServer1->Active        = false;
			btnTCPServerOpen->Caption = "OPEN";
		}
		catch ( ... )
		{
			ShowMessage( "���� ���� ����" );
		}
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::btnTCPSendMsgClick(TObject *Sender)
{
	IdTCPClient1->Host = edtTCPTargetIP->Text;
	IdTCPClient1->Port = StrToIntDef( edtTCPTargetPort->Text, 9999 );
	if ( IdTCPClient1->Connected() )
	{
		IdTCPClient1->Disconnect();
	}

	try
	{
		IdTCPClient1->Connect();
	}
	catch(Exception &ex)
	{
		ShowMessage(ex.ToString());
	}




	String stReceive = "";

	try
	{
		// �����͸� �۽�
		AnsiString strAnsi = AnsiString( txtTCPSendMsg->Lines->Text );

		TEXT_PACKET stSockPacket;
		memset( & stSockPacket, 0, sizeof( TEXT_PACKET ) );
		strcpy( stSockPacket.Text, strAnsi.c_str( ) );
		IdTCPClient1->Socket->WriteLnRFC( txtTCPSendMsg->Lines->Text,  TIdTextEncoding_UTF8 );

		// �޽��� ó��
		Application->ProcessMessages( );



		stReceive = IdTCPClient1->IOHandler->ReadLn(TIdTextEncoding_UTF8);
		ShowMessage( "�۽��� �����߽��ϴ�\n���� ���� : " + stReceive );

	}
	catch ( ... )
	{
		ShowMessage( "�۽��� �����߽��ϴ�" );
	}

	IdTCPClient1->Disconnect();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::IdTCPServer1Execute(TIdContext *AContext)
{

	if(!AContext->Connection->IOHandler->InputBufferIsEmpty())
	{

		String strRecv  = AContext->Connection->IOHandler->InputBuffer->Extract(-1, TIdTextEncoding_UTF8);


		txtTCPRecvMsg->Lines->Text = strRecv;

		AContext->Binding->SendTo( AContext->Binding->PeerIP, AContext->Binding->PeerPort, txtTCPResponseMsg->Lines->Text, Id_IPv4, TIdTextEncoding_UTF8 );

		Application->ProcessMessages( );

	}

}
//---------------------------------------------------------------------------



