// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunSplitsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  [PageTaxonomy(new string[] {"Fitness", "Run", "Splits"})]
  public class RunSplitsViewModel : PanelViewModelBase, INavigationListener
  {
    private readonly IFormattingService formatter;
    private readonly IHeartRateService heartRateService;
    private readonly IRouteBasedExerciseEventProvider<RunEvent> provider;
    private readonly IUserProfileService userProfileService;
    private IList<ChartDistanceSeriesInfo> chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) new List<ChartDistanceSeriesInfo>();
    private RunDetails lastRunEvent;
    private ChartDistanceSeriesInfo selectedChartSeriesInformation;

    public RunSplitsViewModel(
      IRouteBasedExerciseEventProvider<RunEvent> provider,
      IUserProfileService userProfileService,
      INetworkService networkService,
      IFormattingService formatter,
      IHeartRateService heartRateService)
      : base(networkService)
    {
      this.provider = provider;
      this.userProfileService = userProfileService;
      this.formatter = formatter;
      this.heartRateService = heartRateService;
    }

    public RunDetails LastRunEvent
    {
      get => this.lastRunEvent;
      set => this.SetProperty<RunDetails>(ref this.lastRunEvent, value, nameof (LastRunEvent));
    }

    public IList<ChartDistanceSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public ChartDistanceSeriesInfo SelectedChartSeriesInformation
    {
      get => this.selectedChartSeriesInformation;
      set => this.SetProperty<ChartDistanceSeriesInfo>(ref this.selectedChartSeriesInformation, value, nameof (SelectedChartSeriesInformation));
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) new List<ChartDistanceSeriesInfo>();
      RunEvent runEventDetails = (RunEvent) null;
      DistanceUnitType unitType = this.userProfileService.DistanceUnitType;
      if (parameters != null && parameters.ContainsKey("ID"))
      {
        runEventDetails = await this.provider.GetEventAsync(parameters["ID"]);
      }
      else
      {
        RunEvent lastEventAsync = await this.provider.GetLastEventAsync();
        if (lastEventAsync != null)
          runEventDetails = await this.provider.GetEventAsync(lastEventAsync.EventId);
      }
      if (runEventDetails == null)
        throw new NoDataException();
      this.LastRunEvent = RunDetails.Create(runEventDetails, unitType, this.formatter, false);
      await this.PrepareDataForChartsAsync(runEventDetails, unitType);
      this.RaisePropertyChanged("ChartSeriesInformation");
    }

    private async Task PrepareDataForChartsAsync(
      RunEvent runEventDetails,
      DistanceUnitType unitType)
    {
      List<ChartDistanceSeriesInfo> localChartSeriesInfo = new List<ChartDistanceSeriesInfo>();
      if (runEventDetails.MapPoints != null && runEventDetails.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.Pace > Speed.Zero)))
      {
        ChartDistanceSeriesInfo distanceSeriesInfo = new ChartDistanceSeriesInfo();
        distanceSeriesInfo.Name = "Pace";
        distanceSeriesInfo.SeriesType = ChartSeriesType.Pace;
        distanceSeriesInfo.SeriesData = runEventDetails.GetPaceData(this.userProfileService, 5);
        distanceSeriesInfo.PrecalculatedAverage = ChartUtilities.ConvertPaceToUserUnits(runEventDetails.Pace, unitType);
        distanceSeriesInfo.ShowAverageLine = true;
        distanceSeriesInfo.ShowSplitMarkers = false;
        distanceSeriesInfo.FilterScaledPace = true;
        distanceSeriesInfo.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo);
      }
      if (runEventDetails.MapPoints != null && runEventDetails.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.HeartRate > 0)))
      {
        HeartRateZone heartRateZonesAsync = await this.heartRateService.GetEventHeartRateZonesAsync(runEventDetails.HeartRateZones);
        ChartDistanceSeriesInfo distanceSeriesInfo = new ChartDistanceSeriesInfo();
        distanceSeriesInfo.Name = "HeartRate";
        distanceSeriesInfo.SeriesType = ChartSeriesType.DistanceHeartRate;
        distanceSeriesInfo.SeriesData = runEventDetails.GetHeartRateData(this.userProfileService);
        distanceSeriesInfo.ShowSplitMarkers = false;
        distanceSeriesInfo.FilterScaledPace = true;
        distanceSeriesInfo.ShowHighMarker = true;
        distanceSeriesInfo.ShowAverageLine = false;
        distanceSeriesInfo.HRZones = heartRateZonesAsync;
        distanceSeriesInfo.DisplayedMaxValue = (double) runEventDetails.PeakHeartRate;
        distanceSeriesInfo.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo);
      }
      if (runEventDetails.MapPoints != null && runEventDetails.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.Location != null)))
      {
        ChartDistanceSeriesInfo distanceSeriesInfo = new ChartDistanceSeriesInfo();
        distanceSeriesInfo.Name = "Elevation";
        distanceSeriesInfo.SeriesType = ChartSeriesType.Elevation;
        distanceSeriesInfo.SkipZeroes = true;
        distanceSeriesInfo.SeriesData = runEventDetails.GetElevationData(this.userProfileService, true, trimNullLocations: false);
        distanceSeriesInfo.ShowSplitMarkers = false;
        distanceSeriesInfo.FilterScaledPace = true;
        distanceSeriesInfo.ShowAverageLine = false;
        distanceSeriesInfo.ShowHighMarker = true;
        distanceSeriesInfo.SubLabel = AppResources.ChartEstimateElevationLabel;
        distanceSeriesInfo.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo);
      }
      this.chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) localChartSeriesInfo;
    }

    public void OnBackNavigation(bool isTileOpen)
    {
    }
  }
}
