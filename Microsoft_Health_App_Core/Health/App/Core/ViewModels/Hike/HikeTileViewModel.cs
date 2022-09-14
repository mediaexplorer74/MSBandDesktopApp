// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Hike.HikeTileViewModel
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
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Hike
{
  [PageTaxonomy(new string[] {"Hike"})]
  public class HikeTileViewModel : EventTileViewModelBase<HikeEvent>
  {
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(14.0);
    private readonly IRouteBasedExerciseEventProvider<HikeEvent> provider;
    private readonly IBestEventProvider bestEventProvider;
    private readonly IFormattingService formatter;

    public HikeTileViewModel(
      IRouteBasedExerciseEventProvider<HikeEvent> provider,
      IBestEventProvider bestEventProvider,
      IEventTrackingService eventViewTracker,
      IFormattingService formatter,
      INetworkService networkUtils,
      IMessageSender messageSender,
      HikeSummaryViewModel hikeSummaryViewModel,
      ISmoothNavService smoothNavService,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventViewTracker, EventType.Hike, networkUtils, messageSender, smoothNavService, (EventSummaryViewModelBase<HikeEvent>) hikeSummaryViewModel, firstTimeUse)
    {
      this.TileIcon = "\uE407";
      this.provider = provider;
      this.bestEventProvider = bestEventProvider;
      this.formatter = formatter;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) hikeSummaryViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.Message = AppResources.TileFirstTimeUseHikeMessage;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Hike;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EventTileViewModelBase<HikeEvent>.Logger.Debug((object) "<START> loading the hike tile");
      if (parameters != null && parameters.ContainsKey("ID"))
        this.Event = await this.provider.GetEventAsync(parameters["ID"]);
      else
        this.Event = await this.provider.GetLastEventAsync();
      this.RefreshColorLevel();
      EventTileViewModelBase<HikeEvent>.Logger.Debug((object) "<END> loading the hike tile");
      return this.Event != null;
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(HikeTileViewModel.AgeCurrentThreshold);
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.IsBest = (await this.bestEventProvider.GetLabelForEventAsync(this.Event.EventId, EventType.Hike)).Key;
      this.Header = this.formatter.FormatDistance(new Length?(this.Event.TotalDistance), appendUnit: true);
      if (this.Pivots.All<PivotDefinition>((Func<PivotDefinition, bool>) (p => (object) p.Content.GetType() != (object) typeof (HikeWaypointsViewModel))))
        this.Pivots.Add(new PivotDefinition(AppResources.PivotPointsOfInterest, (object) ServiceLocator.Current.GetInstance<HikeWaypointsViewModel>()));
      this.Subheader = Formatter.FormatTileTime(this.Event.StartTime);
      this.CanOpen = true;
      if (string.IsNullOrEmpty(this.Event.Name))
        return;
      this.Subheader += string.Format("{0}{1}", new object[2]
      {
        (object) Environment.NewLine,
        (object) this.Event.Name
      });
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.HikeTileNoContent;
    }
  }
}
