// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.OobeStage
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin
{
  public enum OobeStage : ushort
  {
    AskPhoneType = 0,
    DownloadMessage = 1,
    WaitingOnPhoneToEnterCode = 2,
    WaitingOnPhoneToAcceptPairing = 3,
    PairingSuccess = 4,
    CheckingForUpdate = 5,
    StartingUpdate = 6,
    UpdateComplete = 7,
    WaitingOnPhoneToCompleteOobe = 8,
    PressActionButton = 9,
    ErrorState = 10, // 0x000A
    PairMessage = 11, // 0x000B
    PreStateCharging = 100, // 0x0064
    PreStateLanguageSelect = 101, // 0x0065
  }
}
