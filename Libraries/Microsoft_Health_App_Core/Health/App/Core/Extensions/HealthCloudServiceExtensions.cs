// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.HealthCloudServiceExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class HealthCloudServiceExtensions
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Extensions\\HealthCloudServiceExtensions.cs");

    public static async Task<IList<UserActivity>> GetUserActivitiesAsync(
      this IHealthCloudClient healthCloudClient,
      Range<DateTimeOffset> range,
      ActivityPeriod activityPeriod,
      string deviceId = null)
    {
      HealthCloudServiceExtensions.Logger.Debug("Getting User Activities for [{0}] with Activity Period [{1}]...", (object) range, (object) activityPeriod);
      IList<UserActivity> userActivitiesAsync = await healthCloudClient.GetUserActivitiesAsync(range.Low, range.High, activityPeriod, CancellationToken.None, deviceId);
      HealthCloudServiceExtensions.Logger.Debug("Getting User Activities for [{0}] with Activity Period [{1}]...", (object) range, (object) activityPeriod);
      return userActivitiesAsync;
    }
  }
}
