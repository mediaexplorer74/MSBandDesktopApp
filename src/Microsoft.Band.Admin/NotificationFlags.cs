// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.NotificationFlags
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  [Flags]
  public enum NotificationFlags : byte
  {
    UnmodifiedNotificationSettings = 0,
    ForceNotificationDialog = 1,
    SuppressNotificationDialog = 2,
    SuppressSmsReply = 4,
    AutoResponseAvailable = 8,
    MaxValue = AutoResponseAvailable | SuppressSmsReply | SuppressNotificationDialog | ForceNotificationDialog, // 0x0F
  }
}
