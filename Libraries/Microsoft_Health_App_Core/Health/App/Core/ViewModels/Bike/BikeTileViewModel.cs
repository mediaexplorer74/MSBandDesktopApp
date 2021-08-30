// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Bike.BikeTileViewModel
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

namespace Microsoft.Health.App.Core.ViewModels.Bike
{
  [PageTaxonomy(new string[] {"Bike"})]
  public class BikeTileViewModel : EventTileViewModelBase<BikeEvent>
  {
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(14.0);
    private readonly IRouteBasedExerciseEventProvider<BikeEvent> provider;
    private readonly IBestEventProvider bestEventProvider;
    private readonly IFormattingService formatter;
    private readonly ISmoothNavService smoothNavService;

    public BikeTileViewModel(
      IRouteBasedExerciseEventProvider<BikeEvent> provider,
      IBestEventProvider bestEventProvider,
      IEventTrackingService eventViewTracker,
      IFormattingService formatter,
      INetworkService networkUtils,
      IMessageSender messageSender,
      BikeSummaryViewModel bikeSummaryViewModel,
      ISmoothNavService smoothNavService,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventViewTracker, EventType.Biking, networkUtils, messageSender, smoothNavService, (EventSummaryViewModelBase<BikeEvent>) bikeSummaryViewModel, firstTimeUse)
    {
      this.TileIcon = "\uE133";
      this.provider = provider;
      this.bestEventProvider = bestEventProvider;
      this.formatter = formatter;
      this.smoothNavService = smoothNavService;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) bikeSummaryViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.Message = AppResources.TileFirstTimeUseBikeMessage;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Bike;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EventTileViewModelBase<BikeEvent>.Logger.Debug((object) "<START> loading the bike tile");
      if (parameters != null && parameters.ContainsKey("ID"))
        this.Event = await this.provider.GetEventAsync(parameters["ID"]);
      else
        this.Event = await this.provider.GetLastEventAsync();
      this.RefreshColorLevel();
      EventTileViewModelBase<BikeEvent>.Logger.Debug((object) "<END> loading the bike tile");
      return this.Event != null;
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(BikeTileViewModel.AgeCurrentThreshold);
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.IsBest = (await this.bestEventProvider.GetLabelForEventAsync(this.Event.EventId, EventType.Biking)).Key;
      if (this.Event.GpsState == GpsState.AtLeastOnePoint)
      {
        this.Header = this.formatter.FormatDistance(new Length?(this.Event.TotalDistance), appendUnit: true);
        if (this.Pivots.All<PivotDefinition>((Func<PivotDefinition, bool>) (p => (object) p.Content.GetType() != (object) typeof (BikeSplitsViewModel))))
          this.Pivots.Add(new PivotDefinition(AppResources.PivotSplits, (object) ServiceLocator.Current.GetInstance<BikeSplitsViewModel>()));
      }
      else
      {
        this.Header = Formatter.FormatCalories(new int?(this.Event.CaloriesBurned), true);
        for (int index = 0; index < this.Pivots.Count; ++index)
        {
          if ((object) this.Pivots[index].Content.GetType() == (object) typeof (BikeSplitsViewModel))
          {
            this.Pivots.RemoveAt(index);
            break;
          }
        }
      }
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
      this.Subheader = AppResources.BikeTileNoContent;
    }
  }
}
