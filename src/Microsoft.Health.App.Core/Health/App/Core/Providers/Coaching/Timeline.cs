// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.Timeline
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class Timeline
  {
    public Timeline(
      IList<Reminder> reminders,
      SleepNotificationInfo sleepNotificationInfo,
      LightExposureNotificationInfo lightExposureNotificationInfo)
    {
      this.Reminders = reminders;
      this.SleepNotificationInfo = sleepNotificationInfo;
      this.LightExposureNotificationInfo = lightExposureNotificationInfo;
    }

    public IList<Reminder> Reminders { get; }

    public SleepNotificationInfo SleepNotificationInfo { get; }

    public LightExposureNotificationInfo LightExposureNotificationInfo { get; }
  }
}
