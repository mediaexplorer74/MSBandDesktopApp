// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
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

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  [PageTaxonomy(new string[] {"Run"})]
  public class RunTileViewModel : EventTileViewModelBase<RunEvent>
  {
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(14.0);
    private readonly IBestEventProvider bestEventProvider;
    private readonly IFormattingService formatter;
    private readonly IRouteBasedExerciseEventProvider<RunEvent> provider;
    private readonly IUserProfileService userProfileService;

    public RunTileViewModel(
      IRouteBasedExerciseEventProvider<RunEvent> provider,
      IBestEventProvider bestEventProvider,
      IEventTrackingService eventViewTracker,
      IFormattingService formatter,
      INetworkService networkService,
      IMessageSender messageSender,
      RunSummaryViewModel runSummaryViewModel,
      RunSplitsViewModel runSplitsViewModel,
      ISmoothNavService smoothNavService,
      IUserProfileService userProfileService,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventViewTracker, EventType.Running, networkService, messageSender, smoothNavService, (EventSummaryViewModelBase<RunEvent>) runSummaryViewModel, firstTimeUse)
    {
      this.TileIcon = "\uE004";
      this.provider = provider;
      this.bestEventProvider = bestEventProvider;
      this.formatter = formatter;
      this.userProfileService = userProfileService;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) runSummaryViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSplits, (object) runSplitsViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Run;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EventTileViewModelBase<RunEvent>.Logger.Debug((object) "<START> loading the run tile");
      string eventId = (string) null;
      string str;
      if (parameters != null && parameters.TryGetValue("ID", out str))
      {
        eventId = str;
      }
      else
      {
        RunEvent lastEventAsync = await this.provider.GetLastEventAsync();
        if (lastEventAsync != null)
          eventId = lastEventAsync.EventId;
      }
      if (!string.IsNullOrEmpty(eventId))
        this.Event = await this.provider.GetEventAsync(eventId);
      else
        this.Event = (RunEvent) null;
      this.RefreshColorLevel();
      EventTileViewModelBase<RunEvent>.Logger.Debug("<END> loading the run tile (ID: {0})", this.Event != null ? (object) this.Event.EventId : (object) "null");
      return this.Event != null;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.CanOpen = true;
      this.IsBest = (await this.bestEventProvider.GetLabelForEventAsync(this.Event.EventId, EventType.Running)).Key;
      this.Header = this.formatter.FormatDistance(new Length?(this.Event.TotalDistance), appendUnit: true);
      this.Subheader = Formatter.FormatTileTime(this.Event.StartTime);
      if (string.IsNullOrEmpty(this.Event.Name))
        return;
      this.Subheader += string.Format("\n{0}", new object[1]
      {
        (object) this.Event.Name
      });
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.RunTileNoContent;
      this.FirstTimeUse.Message = this.userProfileService.DistanceUnitType == DistanceUnitType.Metric ? AppResources.TileFirstTimeUseRunMetricMessage : AppResources.TileFirstTimeUseRunImperialMessage;
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(RunTileViewModel.AgeCurrentThreshold);
    }
  }
}
