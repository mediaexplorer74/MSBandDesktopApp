// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.PushServiceType
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

namespace Microsoft.Band.Admin
{
  internal enum PushServiceType
  {
    WakeApp = 0,
    RemoteSubscription = 1,
    Sms = 100, // 0x00000064
    DismissCall = 101, // 0x00000065
    DismissCallThenSms = 102, // 0x00000066
    MuteCall = 103, // 0x00000067
    AnswerCall = 104, // 0x00000068
    SnoozeAlarm = 110, // 0x0000006E
    DismissAlarm = 111, // 0x0000006F
    VoicePacketBegin = 200, // 0x000000C8
    VoicePacketData = 201, // 0x000000C9
    VoicePacketEnd = 202, // 0x000000CA
    VoicePacketCancel = 203, // 0x000000CB
    TileEvent = 204, // 0x000000CC
    TileSyncRequest = 205, // 0x000000CD
    CortanaContext = 206, // 0x000000CE
    ActivityEvent = 208, // 0x000000D0
    Keyboard = 220, // 0x000000DC
    KeyboardSetContext = 222, // 0x000000DE
  }
}
