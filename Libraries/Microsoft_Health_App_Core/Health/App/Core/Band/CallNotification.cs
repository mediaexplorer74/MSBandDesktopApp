// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.CallNotification
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System;

namespace Microsoft.Health.App.Core.Band
{
  public class CallNotification
  {
    public CallNotification(
      int callerId,
      string callerName,
      DateTimeOffset timestamp,
      CallNotificationType type,
      NotificationFlags flags = NotificationFlags.UnmodifiedNotificationSettings)
    {
      this.CallerId = callerId;
      this.CallerName = callerName;
      this.Timestamp = timestamp;
      this.Type = type;
      this.Flags = flags;
    }

    public int CallerId { get; }

    public string CallerName { get; }

    public DateTimeOffset Timestamp { get; }

    public CallNotificationType Type { get; }

    public NotificationFlags Flags { get; }
  }
}
