// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Sleep.SleepTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Sleep
{
  [PageTaxonomy(new string[] {"Sleep"})]
  public class SleepTileViewModel : EventTileViewModelBase<SleepEvent>
  {
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(7.0);
    private readonly ISleepProvider provider;

    public SleepTileViewModel(
      ISleepProvider provider,
      IEventTrackingService eventViewTracker,
      INetworkService networkService,
      IMessageSender messageSender,
      ISmoothNavService smoothNavService,
      SleepSummaryViewModel sleepSummaryViewModel,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventViewTracker, EventType.Sleeping, networkService, messageSender, smoothNavService, (EventSummaryViewModelBase<SleepEvent>) sleepSummaryViewModel, firstTimeUse)
    {
      this.TileIcon = "\uE005";
      this.provider = provider;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) sleepSummaryViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.Message = AppResources.TileFirstTimeUseSleepMessage;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Sleep;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EventTileViewModelBase<SleepEvent>.Logger.Debug((object) "<START> loading the sleep tile");
      if (parameters != null && parameters.ContainsKey("ID"))
        this.Event = await this.provider.GetSleepEventAsync(parameters["ID"]);
      else
        this.Event = await this.provider.GetLastSleepEventAsync();
      this.RefreshColorLevel();
      EventTileViewModelBase<SleepEvent>.Logger.Debug((object) "<END loading the sleep tile");
      return this.Event != null;
    }

    private string GetSubheader()
    {
      string str = Formatter.FormatSleepTime(this.Event.StartTime.ToLocalTime(), true);
      return this.Event.IsAutoDetected ? string.Format(AppResources.SleepTile_Subheader_AutoDetectedSleep_FormatString, new object[1]
      {
        (object) str
      }) : string.Format(AppResources.SleepTile_Subheader_UserDetectedSleep_FormatString, new object[1]
      {
        (object) str
      });
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(SleepTileViewModel.AgeCurrentThreshold);
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.CanOpen = true;
      this.Header = Formatter.FormatTimeSpan(this.Event.SleepTime, Formatter.TimeSpanFormat.OneChar, false);
      this.Subheader = this.GetSubheader();
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.SleepTileNoContent;
    }
  }
}
