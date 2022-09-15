// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.LightExposureNotificationComparer
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
  internal class LightExposureNotificationComparer : IEqualityComparer<LightExposureNotification>
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\LightExposureNotificationComparer.cs");

    public bool Equals(LightExposureNotification l1, LightExposureNotification l2) => l1 == null && l2 == null || l1 != null && l2 != null && LightExposureNotificationComparer.CompareLightExposureNotifications(l1, l2);

    public int GetHashCode(
      LightExposureNotification lightExposureNotification)
    {
      return lightExposureNotification.GetHashCode();
    }

    private static bool CompareLightExposureNotifications(
      LightExposureNotification cached,
      LightExposureNotification fresh)
    {
      bool flag = false;
      try
      {
        flag = cached.Header == fresh.Header && cached.Body == fresh.Body && LightExposureNotificationComparer.CompareDay(cached.Monday, fresh.Monday) && LightExposureNotificationComparer.CompareDay(cached.Tuesday, fresh.Tuesday) && LightExposureNotificationComparer.CompareDay(cached.Wednesday, fresh.Wednesday) && LightExposureNotificationComparer.CompareDay(cached.Thursday, fresh.Thursday) && LightExposureNotificationComparer.CompareDay(cached.Friday, fresh.Friday) && LightExposureNotificationComparer.CompareDay(cached.Saturday, fresh.Saturday) && LightExposureNotificationComparer.CompareDay(cached.Sunday, fresh.Sunday);
      }
      catch (Exception ex)
      {
        LightExposureNotificationComparer.Logger.Error(ex, "<FAILED> Comparing light exposure notification.");
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
        LightExposureNotificationComparer.Logger.Error(ex, "<FAILED> Comparing light exposure notification day.");
      }
      return flag;
    }
  }
}
