// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.UserDailySummaryProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class UserDailySummaryProvider : IUserDailySummaryProvider
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Providers\\UserDailySummaryProvider.cs");
    private readonly IDateTimeService dateTimeService;
    private readonly IHealthCloudClient healthCloudClient;

    public UserDailySummaryProvider(
      IHealthCloudClient healthCloudClient,
      IDateTimeService dateTimeService)
    {
      UserDailySummaryProvider.Logger.Debug((object) "Instantiating the [UserDailySummaryProvider]...");
      if (healthCloudClient == null)
        throw new ArgumentNullException(nameof (healthCloudClient));
      if (dateTimeService == null)
        throw new ArgumentNullException(nameof (dateTimeService));
      this.healthCloudClient = healthCloudClient;
      this.dateTimeService = dateTimeService;
      UserDailySummaryProvider.Logger.Debug((object) "Instantiating the [UserDailySummaryProvider]... [complete]");
    }

    public async Task<IEnumerable<UserDailySummaryGroup>> GetPreviousUserDailySummaryGroupsAsync(
      DateTimeOffset day,
      int numberOfDays,
      string deviceId = null)
    {
      DateTimeOffset date = (DateTimeOffset) this.dateTimeService.ToLocalTime(day).Date;
      Range<DateTimeOffset> requestPeriod = Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) this.dateTimeService.AddDays(date, -numberOfDays).Date, date);
      UserDailySummaryProvider.Logger.Debug("Getting Continuous UTC Time Zone Offset Periods in Daily Summaries Request Period [{0}]...", (object) requestPeriod);
      IList<Range<DateTimeOffset>> list = (IList<Range<DateTimeOffset>>) this.dateTimeService.SplitByMidnightUtcOffset(requestPeriod).ToList<Range<DateTimeOffset>>();
      UserDailySummaryProvider.Logger.Debug("Getting Continuous UTC Time Zone Offset Periods in Daily Summaries Request Period [{0}]... [complete]", (object) requestPeriod);
      List<UserDailySummaryGroup> userDailySummaryGroups = new List<UserDailySummaryGroup>();
      UserDailySummaryProvider.Logger.Debug("Creating User Summary Groups by Day for [{0}]...", (object) requestPeriod);
      foreach (Range<DateTimeOffset> split in (IEnumerable<Range<DateTimeOffset>>) list)
      {
        IList<UserDailySummary> userDailySummaries;
        IList<UserDailySummary> userDailySummaryList = userDailySummaries;
        userDailySummaries = await this.GetUserDailySummariesAsync(split, deviceId);
        this.dateTimeService.ForEachTimeZoneDay(split, (Action<int, Range<DateTimeOffset>>) ((index, range) =>
        {
          if (userDailySummaries.Count <= index)
            return;
          UserDailySummaryProvider.Logger.Debug("Creating Day User Summary Group for [{0}]...", (object) range);
          userDailySummaryGroups.Add(new List<UserDailySummary>()
          {
            userDailySummaries[index]
          }.ToUserDailySummaryGroup(range));
        }));
      }
      UserDailySummaryProvider.Logger.Debug("Creating User Summary Groups by Day for [{0}]... [complete]", (object) requestPeriod);
      UserDailySummaryProvider.Logger.Debug("Created [{0}] User Summary Groups.", (object) userDailySummaryGroups.Count);
      return (IEnumerable<UserDailySummaryGroup>) userDailySummaryGroups;
    }

    public async Task<IEnumerable<UserDailySummaryGroup>> GetPreviousUserDailySummaryWeekGroupsAsync(
      DateTimeOffset day,
      int numberOfWeeks,
      string deviceId = null)
    {
      DateTimeOffset date = (DateTimeOffset) this.dateTimeService.ToLocalTime(day).Date;
      Range<DateTimeOffset> requestPeriod = Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) this.dateTimeService.AddWeeks(date, -numberOfWeeks).Date, date);
      UserDailySummaryProvider.Logger.Debug("Getting Continuous UTC Time Zone Offset Periods in Daily Summaries Request Period [{0}]...", (object) requestPeriod);
      IList<Range<DateTimeOffset>> list = (IList<Range<DateTimeOffset>>) this.dateTimeService.SplitByMidnightUtcOffset(requestPeriod).ToList<Range<DateTimeOffset>>();
      UserDailySummaryProvider.Logger.Debug("Getting Continuous UTC Time Zone Offset Periods in Daily Summaries Request Period [{0}]... [complete]", (object) requestPeriod);
      List<UserDailySummaryGroup> userDailySummaryGroups = new List<UserDailySummaryGroup>();
      List<UserDailySummary> userDailySummaries = new List<UserDailySummary>();
      foreach (Range<DateTimeOffset> range in (IEnumerable<Range<DateTimeOffset>>) list)
      {
        List<UserDailySummary> userDailySummaryList = userDailySummaries;
        IList<UserDailySummary> dailySummariesAsync = await this.GetUserDailySummariesAsync(range, deviceId);
        userDailySummaryList.AddRange((IEnumerable<UserDailySummary>) dailySummariesAsync);
        userDailySummaryList = (List<UserDailySummary>) null;
      }
      UserDailySummaryProvider.Logger.Debug("Creating User Summary Groups by Week for [{0}]...", (object) requestPeriod);
      this.dateTimeService.ForEachTimeZoneWeek(requestPeriod, (Action<Range<int>, Range<DateTimeOffset>>) ((weekIndexRange, timeZoneOffsetRange) =>
      {
        if (!userDailySummaries.Any<UserDailySummary>())
          return;
        UserDailySummaryProvider.Logger.Debug("Creating Week User Summary Group for [{0}]...", (object) timeZoneOffsetRange);
        userDailySummaryGroups.Add(userDailySummaries.GetRange<UserDailySummary>(weekIndexRange).ToUserDailySummaryGroup(timeZoneOffsetRange));
      }));
      UserDailySummaryProvider.Logger.Debug("Creating User Summary Groups by Week for [{0}]... [complete]", (object) requestPeriod);
      UserDailySummaryProvider.Logger.Debug("Created [{0}] User Summary Groups.", (object) userDailySummaryGroups.Count);
      return (IEnumerable<UserDailySummaryGroup>) userDailySummaryGroups;
    }

    public async Task<UserDailySummaryGroup> GetUserDailySummaryGroupAsync(
      DateTimeOffset day,
      string deviceId = null)
    {
      Range<DateTimeOffset> requestPeriod = Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) this.dateTimeService.ToLocalTime(day).Date, this.dateTimeService.AddDays(day, 1));
      IList<UserDailySummary> dailySummariesAsync = await this.GetUserDailySummariesAsync(requestPeriod, deviceId);
      UserDailySummaryProvider.Logger.Debug("Creating Day User Summary Group for [{0}]...", (object) requestPeriod);
      return dailySummariesAsync.ToUserDailySummaryGroup(requestPeriod);
    }

    public async Task<UserDailySummaryGroup> GetUserDailySummaryWeekGroupAsync(
      DateTimeOffset day,
      string deviceId = null)
    {
      Range<DateTimeOffset> requestPeriod = Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) this.dateTimeService.ToLocalTime(day).Date, this.dateTimeService.AddWeeks(day, 1));
      UserDailySummaryProvider.Logger.Debug("Getting Continuous UTC Time Zone Offset Periods in Daily Summaries Request Period [{0}]...", (object) requestPeriod);
      IList<Range<DateTimeOffset>> list = (IList<Range<DateTimeOffset>>) this.dateTimeService.SplitByMidnightUtcOffset(requestPeriod).ToList<Range<DateTimeOffset>>();
      UserDailySummaryProvider.Logger.Debug("Getting Continuous UTC Time Zone Offset Periods in Daily Summaries Request Period [{0}]... [complete]", (object) requestPeriod);
      List<UserDailySummary> userDailySummaries = new List<UserDailySummary>();
      foreach (Range<DateTimeOffset> range in (IEnumerable<Range<DateTimeOffset>>) list)
      {
        List<UserDailySummary> userDailySummaryList = userDailySummaries;
        IList<UserDailySummary> dailySummariesAsync = await this.GetUserDailySummariesAsync(range, deviceId);
        userDailySummaryList.AddRange((IEnumerable<UserDailySummary>) dailySummariesAsync);
        userDailySummaryList = (List<UserDailySummary>) null;
      }
      UserDailySummaryProvider.Logger.Debug("Creating Week User Summary Group for [{0}]...", (object) requestPeriod);
      return userDailySummaries.ToUserDailySummaryGroup(requestPeriod);
    }

    private async Task<IList<UserDailySummary>> GetUserDailySummariesAsync(
      Range<DateTimeOffset> range,
      string deviceId)
    {
      UserDailySummaryProvider.Logger.Debug("Getting Daily Summaries for [{0}] with UTC Time Zone Offset [{1}] in ...", (object) range, (object) range.Low.Offset.ToStringWithNegative("h\\:mm"));
      IList<UserDailySummary> dailySummariesAsync = await this.healthCloudClient.GetDailySummariesAsync(range.Low, range.High, range.Low.Offset, CancellationToken.None, deviceId);
      UserDailySummaryProvider.Logger.Debug("Getting Daily Summaries for [{0}] with UTC Time Zone Offset [{1}]... [complete]", (object) range, (object) range.Low.Offset.ToStringWithNegative("h\\:mm"));
      UserDailySummaryProvider.Logger.Debug("Got [{0}] Daily Summaries.", (object) dailySummariesAsync.Count);
      return dailySummariesAsync;
    }
  }
}
