object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'Form1'
  ClientHeight = 590
  ClientWidth = 788
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object GroupBox1: TGroupBox
    Left = 8
    Top = 8
    Width = 241
    Height = 521
    Caption = 'UDP'
    TabOrder = 0
    object Label1: TLabel
      Left = 24
      Top = 40
      Width = 58
      Height = 13
      Caption = #45824#49345' '#50500#51060#54588
    end
    object Label2: TLabel
      Left = 24
      Top = 65
      Width = 47
      Height = 13
      Caption = #45824#49345' '#54252#53944
    end
    object Bevel1: TBevel
      Left = 24
      Top = 222
      Width = 197
      Height = 2
    end
    object Label4: TLabel
      Left = 24
      Top = 238
      Width = 47
      Height = 13
      Caption = #49436#48260' '#54252#53944
    end
    object txtUDPSendMsg: TMemo
      Left = 24
      Top = 89
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #49569#49888' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 0
    end
    object edtUDPTargetIP: TEdit
      Left = 99
      Top = 36
      Width = 121
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 1
      Text = '127.0.0.1'
    end
    object edtUDPTargetPort: TEdit
      Left = 99
      Top = 60
      Width = 121
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 2
      Text = '9999'
    end
    object btnUDPServerOpen: TButton
      Left = 171
      Top = 227
      Width = 46
      Height = 25
      Caption = 'OPEN'
      TabOrder = 3
      OnClick = btnUDPServerOpenClick
    end
    object edtUDPServerPort: TEdit
      Left = 83
      Top = 233
      Width = 78
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 4
      Text = '9999'
    end
    object txtUDPRecvMsg: TMemo
      Left = 24
      Top = 257
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #49688#49888' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 5
    end
    object txtUDPResponseMsg: TMemo
      Left = 24
      Top = 353
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #51025#45813' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 6
    end
    object btnUDPSendMsg: TButton
      Left = 139
      Top = 187
      Width = 75
      Height = 25
      Caption = #49569#49888
      TabOrder = 7
      OnClick = btnUDPSendMsgClick
    end
  end
  object GroupBox2: TGroupBox
    Left = 264
    Top = 8
    Width = 241
    Height = 521
    Caption = 'TCP'
    TabOrder = 1
    object Label3: TLabel
      Left = 24
      Top = 40
      Width = 58
      Height = 13
      Caption = #45824#49345' '#50500#51060#54588
    end
    object Label5: TLabel
      Left = 24
      Top = 65
      Width = 47
      Height = 13
      Caption = #45824#49345' '#54252#53944
    end
    object Bevel2: TBevel
      Left = 24
      Top = 222
      Width = 197
      Height = 2
    end
    object Label6: TLabel
      Left = 24
      Top = 238
      Width = 47
      Height = 13
      Caption = #49436#48260' '#54252#53944
    end
    object txtTCPSendMsg: TMemo
      Left = 24
      Top = 89
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #49569#49888' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 0
    end
    object edtTCPTargetIP: TEdit
      Left = 99
      Top = 36
      Width = 121
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 1
      Text = '127.0.0.1'
    end
    object edtTCPTargetPort: TEdit
      Left = 99
      Top = 60
      Width = 121
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 2
      Text = '9999'
    end
    object btnTCPServerOpen: TButton
      Left = 171
      Top = 227
      Width = 46
      Height = 25
      Caption = 'OPEN'
      TabOrder = 3
      OnClick = btnTCPServerOpenClick
    end
    object edtTCPServerPort: TEdit
      Left = 83
      Top = 233
      Width = 78
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 4
      Text = '9999'
    end
    object txtTCPRecvMsg: TMemo
      Left = 24
      Top = 257
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #49688#49888' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 5
    end
    object txtTCPResponseMsg: TMemo
      Left = 24
      Top = 353
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #51025#45813' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 6
    end
    object btnTCPSendMsg: TButton
      Left = 139
      Top = 187
      Width = 75
      Height = 25
      Caption = #49569#49888
      TabOrder = 7
      OnClick = btnTCPSendMsgClick
    end
  end
  object GroupBox3: TGroupBox
    Left = 520
    Top = 8
    Width = 241
    Height = 521
    Caption = 'Socket'
    TabOrder = 2
    object Label7: TLabel
      Left = 24
      Top = 40
      Width = 58
      Height = 13
      Caption = #45824#49345' '#50500#51060#54588
    end
    object Label8: TLabel
      Left = 24
      Top = 65
      Width = 47
      Height = 13
      Caption = #45824#49345' '#54252#53944
    end
    object Bevel3: TBevel
      Left = 24
      Top = 222
      Width = 197
      Height = 2
    end
    object Label9: TLabel
      Left = 24
      Top = 238
      Width = 47
      Height = 13
      Caption = #49436#48260' '#54252#53944
    end
    object txtSocketSendMsg: TMemo
      Left = 24
      Top = 89
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #49569#49888' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 0
    end
    object edtSocketTargetIP: TEdit
      Left = 99
      Top = 36
      Width = 121
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 1
      Text = '127.0.0.1'
    end
    object edtSocketTargetPort: TEdit
      Left = 99
      Top = 60
      Width = 121
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 2
      Text = '9999'
    end
    object btnSocketServerOpen: TButton
      Left = 171
      Top = 227
      Width = 46
      Height = 25
      Caption = 'OPEN'
      TabOrder = 3
      OnClick = btnSocketServerOpenClick
    end
    object edtSocketServerPort: TEdit
      Left = 83
      Top = 233
      Width = 78
      Height = 21
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      TabOrder = 4
      Text = '9999'
    end
    object txtSocketRecvMsg: TMemo
      Left = 24
      Top = 257
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #49688#49888' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 5
    end
    object txtSocketResponseMsg: TMemo
      Left = 24
      Top = 353
      Width = 197
      Height = 89
      ImeName = #54620#44397#50612' '#51077#47141' '#49884#49828#53596' (IME 2000)'
      Lines.Strings = (
        #51025#45813' '#45236#50857)
      ScrollBars = ssVertical
      TabOrder = 6
    end
    object btnSocketSendMsg: TButton
      Left = 139
      Top = 184
      Width = 75
      Height = 25
      Caption = #49569#49888
      TabOrder = 7
      OnClick = btnSocketSendMsgClick
    end
  end
  object IdUDPServer1: TIdUDPServer
    Bindings = <>
    DefaultPort = 0
    OnUDPRead = IdUDPServer1UDPRead
    Left = 112
    Top = 472
  end
  object IdUDPClient1: TIdUDPClient
    Port = 0
    Left = 72
    Top = 472
  end
  object ClientSocket1: TClientSocket
    Active = False
    ClientType = ctBlocking
    Port = 0
    Left = 592
    Top = 480
  end
  object ServerSocket1: TServerSocket
    Active = False
    Port = 0
    ServerType = stNonBlocking
    OnClientRead = ServerSocket1ClientRead
    Left = 656
    Top = 480
  end
  object IdTCPClient1: TIdTCPClient
    ConnectTimeout = 0
    IPVersion = Id_IPv4
    Port = 0
    ReadTimeout = -1
    Left = 304
    Top = 496
  end
  object IdTCPServer1: TIdTCPServer
    Bindings = <>
    DefaultPort = 0
    OnExecute = IdTCPServer1Execute
    Left = 368
    Top = 496
  end
end
