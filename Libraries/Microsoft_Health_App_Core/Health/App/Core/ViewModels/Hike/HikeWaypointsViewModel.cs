// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Hike.HikeWaypointsViewModel
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
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Hike
{
  [PageTaxonomy(new string[] {"Fitness", "Hike", "Waypoints"})]
  public class HikeWaypointsViewModel : PanelViewModelBase
  {
    private readonly IHeartRateService heartRateService;
    private readonly IRouteBasedExerciseEventProvider<HikeEvent> provider;
    private readonly IUserProfileService userProfileService;
    private IList<ExpandableWaypointItemViewModel> waypoints = (IList<ExpandableWaypointItemViewModel>) new List<ExpandableWaypointItemViewModel>();
    private IList<ChartDistanceSeriesInfo> chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) new List<ChartDistanceSeriesInfo>();

    public IList<ChartDistanceSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public HikeWaypointsViewModel(
      IRouteBasedExerciseEventProvider<HikeEvent> provider,
      IUserProfileService userProfileService,
      INetworkService networkService,
      IHeartRateService heartRateService)
      : base(networkService)
    {
      this.provider = provider;
      this.userProfileService = userProfileService;
      this.heartRateService = heartRateService;
    }

    public IList<ExpandableWaypointItemViewModel> Waypoints => this.waypoints;

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) new List<ChartDistanceSeriesInfo>();
      this.waypoints = (IList<ExpandableWaypointItemViewModel>) new List<ExpandableWaypointItemViewModel>();
      HikeEvent hikeEvent;
      if (parameters != null && parameters.ContainsKey("ID"))
        hikeEvent = await this.provider.GetEventAsync(parameters["ID"]);
      else
        hikeEvent = await this.provider.GetLastEventAsync();
      if (hikeEvent == null)
        throw new NoDataException();
      HikeEvent eventAsync = await this.provider.GetEventAsync(hikeEvent.EventId);
      this.PrepareDataForList(eventAsync);
      await this.PrepareDataForChartsAsync(eventAsync);
      this.RaisePropertyChanged("ChartSeriesInformation");
      this.RaisePropertyChanged("Waypoints");
    }

    private async Task PrepareDataForChartsAsync(HikeEvent hikeEvent)
    {
      List<ChartDistanceSeriesInfo> localChartSeriesInfo = new List<ChartDistanceSeriesInfo>();
      DistanceUnitType unitType = this.userProfileService.DistanceUnitType;
      if (hikeEvent.HeartRateZones != null && hikeEvent.MapPoints != null && hikeEvent.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.HeartRate > 0)))
      {
        HeartRateZone heartRateZonesAsync = await this.heartRateService.GetEventHeartRateZonesAsync(hikeEvent.HeartRateZones);
        ChartDistanceSeriesInfo distanceSeriesInfo = new ChartDistanceSeriesInfo();
        distanceSeriesInfo.Name = "HeartRate";
        distanceSeriesInfo.SeriesType = ChartSeriesType.DistanceHeartRate;
        distanceSeriesInfo.SeriesData = hikeEvent.GetHeartRateData(this.userProfileService);
        distanceSeriesInfo.DisplayedMaxValue = (double) hikeEvent.PeakHeartRate;
        distanceSeriesInfo.HRZones = heartRateZonesAsync;
        distanceSeriesInfo.ShowHighMarker = true;
        distanceSeriesInfo.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo);
      }
      if (hikeEvent.MapPoints != null && hikeEvent.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.Location != null)))
      {
        ChartDistanceSeriesInfo distanceSeriesInfo1 = new ChartDistanceSeriesInfo();
        distanceSeriesInfo1.Name = "Elevation";
        distanceSeriesInfo1.SeriesType = ChartSeriesType.Elevation;
        distanceSeriesInfo1.SeriesData = hikeEvent.GetElevationData(this.userProfileService, true);
        distanceSeriesInfo1.DisplayedMaxValue = ChartUtilities.ConvertElevationToUserUnits(hikeEvent.MaxAltitude, unitType);
        distanceSeriesInfo1.ShowHighMarker = true;
        distanceSeriesInfo1.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo1);
        ChartDistanceSeriesInfo distanceSeriesInfo2 = new ChartDistanceSeriesInfo();
        distanceSeriesInfo2.Name = "Waypoint";
        distanceSeriesInfo2.SeriesType = ChartSeriesType.Waypoint;
        distanceSeriesInfo2.SeriesData = hikeEvent.GetHikeElevationData(this.userProfileService);
        distanceSeriesInfo2.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo2);
      }
      this.chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) localChartSeriesInfo;
    }

    private void PrepareDataForList(HikeEvent hikeEventDetails)
    {
      if (hikeEventDetails.Sequences == null)
        return;
      List<ExpandableWaypointItemViewModel> waypointItemViewModelList = new List<ExpandableWaypointItemViewModel>();
      foreach (HikeEventSequence sequence in (IEnumerable<HikeEventSequence>) hikeEventDetails.Sequences)
      {
        Location location = sequence.Location;
        waypointItemViewModelList.Add(new ExpandableWaypointItemViewModel()
        {
          Current = sequence,
          TimeOfDayFormatted = (string) Formatter.FormatShortTimeString(sequence.StartTime),
          DistanceFormatted = (string) Formatter.FormatDistance(sequence.TotalDistance, this.userProfileService.DistanceUnitType, appendUnit: true),
          IsUserWaypoint = sequence.PoiType == PointOfInterestType.UserGenerated,
          WaypointDesignator = this.GetGlyphForType(sequence),
          WaypointTitle = this.GetTitleForType(sequence),
          WaypointSubtitle = this.GetSubtitleForType(sequence),
          WaypointSubmetric = this.GetSubmetricForType(sequence),
          WaypointLatitudeSubmetric = location != null ? MapUtilities.ToNiceLatLongString((double) sequence.Location.Latitude, (double) sequence.Location.Longitude, true) : string.Empty,
          WaypointLongitudeSubmetric = location != null ? MapUtilities.ToNiceLatLongString((double) sequence.Location.Latitude, (double) sequence.Location.Longitude, false) : string.Empty,
          WaypointAltitudeGain = (string) Formatter.FormatDistanceMetersOrFeet(sequence.AltitudeGain, this.userProfileService.DistanceUnitType, true),
          WaypointAltitudeLoss = (string) Formatter.FormatDistanceMetersOrFeet(sequence.AltitudeLoss, this.userProfileService.DistanceUnitType, true),
          WaypointClimbRate = (string) Formatter.FormatClimbRate(sequence.ClimbSpeed, 2, this.userProfileService.DistanceUnitType, true)
        });
      }
      this.waypoints = (IList<ExpandableWaypointItemViewModel>) waypointItemViewModelList;
    }

    private string GetSubtitleForType(HikeEventSequence waypoint) => waypoint.PoiType != PointOfInterestType.UserGenerated ? string.Empty : waypoint.UserPoiOrdinal.ToString();

    private string GetSubmetricForType(HikeEventSequence waypoint)
    {
      string str = string.Empty;
      switch (waypoint.PoiType)
      {
        case PointOfInterestType.UserGenerated:
          str = this.GetSubtitleForType(waypoint);
          break;
        case PointOfInterestType.ElevationMax:
        case PointOfInterestType.ElevationMin:
        case PointOfInterestType.TimeMidPoint:
          str = (string) Formatter.FormatDistanceMetersOrFeet(waypoint.Elevation.Absolute.Value, this.userProfileService.DistanceUnitType, true);
          break;
      }
      return str;
    }

    private string GetTitleForType(HikeEventSequence waypoint)
    {
      switch (waypoint.PoiType)
      {
        case PointOfInterestType.ElevationMax:
          return AppResources.PointOfInterestMaxElevation;
        case PointOfInterestType.ElevationMin:
          return AppResources.PointOfInterestMinElevation;
        case PointOfInterestType.TimeMidPoint:
          return AppResources.PointOfInterestMidpoint;
        case PointOfInterestType.PauseAuto:
          return AppResources.PointOfInterestPause;
        case PointOfInterestType.Sunrise:
          return AppResources.PointOfInterestSunrise;
        case PointOfInterestType.Sunset:
          return AppResources.PointOfInterestSunset;
        case PointOfInterestType.Start:
          return AppResources.PointOfInterestStart;
        case PointOfInterestType.End:
          return AppResources.PointOfInterestFinish;
        default:
          return AppResources.PointOfInterestUserGenerated;
      }
    }

    private string GetGlyphForType(HikeEventSequence waypoint)
    {
      switch (waypoint.PoiType)
      {
        case PointOfInterestType.ElevationMax:
          return "\uE415";
        case PointOfInterestType.ElevationMin:
          return "\uE414";
        case PointOfInterestType.TimeMidPoint:
          return "\uE411";
        case PointOfInterestType.PauseAuto:
          return "\uE409";
        case PointOfInterestType.Sunrise:
        case PointOfInterestType.Sunset:
          return "\uE410";
        case PointOfInterestType.Start:
          return "\uE413";
        case PointOfInterestType.End:
          return "\uE412";
        default:
          return string.Empty;
      }
    }
  }
}
