// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.SleepNotificationComparer
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services
{
  internal class SleepNotificationComparer : IEqualityComparer<SleepNotification>
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\SleepNotificationComparer.cs");

    public bool Equals(SleepNotification s1, SleepNotification s2) => s1 == null && s2 == null || s1 != null && s2 != null && SleepNotificationComparer.CompareSleepNotifications(s1, s2);

    public int GetHashCode(SleepNotification sleepNotification) => sleepNotification.GetHashCode();

    private static bool CompareSleepNotifications(SleepNotification cached, SleepNotification fresh)
    {
      bool flag = false;
      try
      {
        flag = cached.Header == fresh.Header && cached.Body == fresh.Body && SleepNotificationComparer.CompareDay(cached.Monday, fresh.Monday) && SleepNotificationComparer.CompareDay(cached.Tuesday, fresh.Tuesday) && SleepNotificationComparer.CompareDay(cached.Wednesday, fresh.Wednesday) && SleepNotificationComparer.CompareDay(cached.Thursday, fresh.Thursday) && SleepNotificationComparer.CompareDay(cached.Friday, fresh.Friday) && SleepNotificationComparer.CompareDay(cached.Saturday, fresh.Saturday) && SleepNotificationComparer.CompareDay(cached.Sunday, fresh.Sunday);
      }
      catch (Exception ex)
      {
        SleepNotificationComparer.Logger.Error(ex, "<FAILED> Comparing sleep notification.");
      }
      return flag;
    }

    private static bool CompareDay(NotificationTime cachedDay, NotificationTime freshDay)
    {
      if (cachedDay == null || freshDay == null)
        return false;
      bool flag = false;
      try
      {
        flag = (int) cachedDay.Hour == (int) freshDay.Hour && (int) cachedDay.Minute == (int) freshDay.Minute && cachedDay.Enabled == freshDay.Enabled;
      }
      catch (Exception ex)
      {
        SleepNotificationComparer.Logger.Error(ex, "<FAILED> Comparing sleep notification Day.");
      }
      return flag;
    }
  }
}
