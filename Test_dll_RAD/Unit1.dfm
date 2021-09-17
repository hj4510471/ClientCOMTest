object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'Form1'
  ClientHeight = 299
  ClientWidth = 635
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object ButtonConnect: TButton
    Left = 532
    Top = 16
    Width = 75
    Height = 25
    Caption = 'Connect'
    TabOrder = 0
    OnClick = ButtonConnectClick
  end
  object ComboBoxProtocol: TComboBox
    Left = 8
    Top = 18
    Width = 171
    Height = 21
    Style = csDropDownList
    DropDownCount = 20
    TabOrder = 1
    OnChange = ChangeProtocol
    Items.Strings = (
      'NONE'
      'SOCKET'
      'UDP'
      'TCP/IP')
  end
  object ButtonDisconnect: TButton
    Left = 444
    Top = 16
    Width = 75
    Height = 25
    Caption = 'Disconnect'
    TabOrder = 2
    OnClick = ButtonDisonnectClick
  end
  object LabelIPAddr: TLabeledEdit
    Left = 203
    Top = 18
    Width = 65
    Height = 21
    EditLabel.Width = 14
    EditLabel.Height = 13
    EditLabel.Caption = 'IP:'
    LabelPosition = lpLeft
    TabOrder = 3
    Text = '127.0.0.1'
  end
  object LogList: TMemo
    Left = 8
    Top = 72
    Width = 599
    Height = 209
    BorderStyle = bsNone
    ImeName = 'Microsoft Office IME 2007'
    ReadOnly = True
    ScrollBars = ssVertical
    TabOrder = 4
  end
  object LabelSendMessage: TLabeledEdit
    Left = 203
    Top = 45
    Width = 316
    Height = 21
    EditLabel.Width = 34
    EditLabel.Height = 13
    EditLabel.Caption = 'Send : '
    LabelPosition = lpLeft
    TabOrder = 5
  end
  object Button1: TButton
    Left = 532
    Top = 41
    Width = 75
    Height = 25
    Caption = 'Send'
    TabOrder = 6
    OnClick = ButtonSendMsgClick
  end
  object LabelPortNum: TLabeledEdit
    Left = 323
    Top = 18
    Width = 65
    Height = 21
    EditLabel.Width = 30
    EditLabel.Height = 13
    EditLabel.Caption = 'Port : '
    LabelPosition = lpLeft
    NumbersOnly = True
    TabOrder = 7
    Text = '5000'
  end
  object LabelTest: TLabeledEdit
    Left = 32
    Top = 45
    Width = 65
    Height = 21
    EditLabel.Width = 14
    EditLabel.Height = 13
    EditLabel.Caption = 'IP:'
    LabelPosition = lpLeft
    TabOrder = 8
    Text = '127.0.0.1'
  end
  object ButtonTest: TButton
    Left = 103
    Top = 41
    Width = 50
    Height = 25
    Caption = 'Test'
    TabOrder = 9
    OnClick = ButtonTestClick
  end
end
