// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.NotificationFlagsExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Notifications;
using System;

namespace Microsoft.Band.Admin
{
  internal static class NotificationFlagsExtensions
  {
    internal static BandNotificationFlags ToBandNotificationFlags(
      this NotificationFlags flags)
    {
      BandNotificationFlags notificationFlags = BandNotificationFlags.UnmodifiedNotificationSettings;
      if (flags.HasFlag((Enum) NotificationFlags.ForceNotificationDialog))
        notificationFlags |= BandNotificationFlags.ForceNotificationDialog;
      if (flags.HasFlag((Enum) NotificationFlags.SuppressNotificationDialog))
        notificationFlags |= BandNotificationFlags.SuppressNotificationDialog;
      if (flags.HasFlag((Enum) NotificationFlags.SuppressSmsReply))
        notificationFlags |= BandNotificationFlags.SuppressSmsReply;
      return notificationFlags;
    }
  }
}
