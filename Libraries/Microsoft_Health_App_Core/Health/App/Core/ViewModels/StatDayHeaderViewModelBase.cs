// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.StatDayHeaderViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class StatDayHeaderViewModelBase : IStatHeaderViewModel, ILoadableParameters
  {
    private readonly IDateTimeService dateTimeService;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;

    public StatHistoryEntry StatHistoryEntry { get; set; }

    public UserDailySummaryGroup SummaryGroup { get; set; }

    public string Header { get; set; }

    public string SubHeader { get; set; }

    public abstract string TileIcon { get; }

    public StatDayHeaderViewModelBase(
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider)
    {
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
    }

    public virtual async Task LoadAsync(IDictionary<string, string> parameters = null)
    {
      DateTimeOffset startDate = this.dateTimeService.GetToday();
      if (parameters != null && parameters.ContainsKey("StartDate"))
        startDate = DateTimeOffset.Parse(parameters["StartDate"]);
      this.SummaryGroup = (await this.userDailySummaryProvider.GetPreviousUserDailySummaryGroupsAsync(startDate, 1)).FirstOrDefault<UserDailySummaryGroup>();
      this.StatHistoryEntry = new StatHistoryEntry(startDate, 0, false);
    }
  }
}
