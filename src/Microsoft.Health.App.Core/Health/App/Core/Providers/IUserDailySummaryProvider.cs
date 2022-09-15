// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.IUserDailySummaryProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public interface IUserDailySummaryProvider
  {
    Task<IEnumerable<UserDailySummaryGroup>> GetPreviousUserDailySummaryGroupsAsync(
      DateTimeOffset day,
      int numberOfDays,
      string deviceId = null);

    Task<IEnumerable<UserDailySummaryGroup>> GetPreviousUserDailySummaryWeekGroupsAsync(
      DateTimeOffset day,
      int numberOfWeeks,
      string deviceId = null);

    Task<UserDailySummaryGroup> GetUserDailySummaryGroupAsync(
      DateTimeOffset day,
      string deviceId = null);

    Task<UserDailySummaryGroup> GetUserDailySummaryWeekGroupAsync(
      DateTimeOffset day,
      string deviceId = null);
  }
}
