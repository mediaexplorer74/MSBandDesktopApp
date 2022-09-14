// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Bike.BikeSplitsViewModel
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Bike
{
  [PageTaxonomy(new string[] {"Fitness", "Bike", "Splits"})]
  public class BikeSplitsViewModel : PanelViewModelBase
  {
    private const double MinimumSplitDistance = 0.01;
    private readonly IRouteBasedExerciseEventProvider<BikeEvent> provider;
    private readonly IUserProfileService userProfileService;
    private readonly IHeartRateService heartRateService;
    private readonly IFormattingService formattingService;
    private IList<ChartDistanceSeriesInfo> chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) new List<ChartDistanceSeriesInfo>();
    private bool showGroupedSplits;
    private int splitGroupSize;
    private bool showGpsLossWarning;

    public BikeSplitsViewModel(
      INetworkService networkService,
      IRouteBasedExerciseEventProvider<BikeEvent> provider,
      IUserProfileService userProfileService,
      IHeartRateService heartRateService,
      IFormattingService formattingService)
      : base(networkService)
    {
      this.provider = provider;
      this.userProfileService = userProfileService;
      this.heartRateService = heartRateService;
      this.formattingService = formattingService;
      this.SingleSplits = (IList<BikeSingleSplit>) new ObservableCollection<BikeSingleSplit>();
      this.GroupedSplits = (IList<BikeMultiSplit>) new ObservableCollection<BikeMultiSplit>();
    }

    public IList<BikeSingleSplit> SingleSplits { get; private set; }

    public IList<BikeMultiSplit> GroupedSplits { get; private set; }

    public bool ShowGroupedSplits
    {
      get => this.showGroupedSplits;
      set => this.SetProperty<bool>(ref this.showGroupedSplits, value, nameof (ShowGroupedSplits));
    }

    public int SplitGroupSize
    {
      get => this.splitGroupSize;
      set => this.SetProperty<int>(ref this.splitGroupSize, value, nameof (SplitGroupSize));
    }

    public bool ShowGpsLossWarning
    {
      get => this.showGpsLossWarning;
      set => this.SetProperty<bool>(ref this.showGpsLossWarning, value, nameof (ShowGpsLossWarning));
    }

    public IList<ChartDistanceSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      BikeEvent bikeEventDetails = (BikeEvent) null;
      if (parameters != null && parameters.ContainsKey("ID"))
      {
        bikeEventDetails = await this.provider.GetEventAsync(parameters["ID"]);
      }
      else
      {
        BikeEvent lastEventAsync = await this.provider.GetLastEventAsync();
        if (lastEventAsync != null)
          bikeEventDetails = await this.provider.GetEventAsync(lastEventAsync.EventId);
      }
      if (bikeEventDetails == null)
        throw new NoDataException();
      this.SplitGroupSize = bikeEventDetails.SplitGroupSize > 0 ? bikeEventDetails.SplitGroupSize : 1;
      this.ShowGroupedSplits = this.SplitGroupSize > 1;
      if (this.ShowGroupedSplits)
      {
        this.GroupedSplits.Clear();
        this.AddSplitGroups(bikeEventDetails);
        this.ShowGpsLossWarning = this.GroupedSplits.Any<BikeMultiSplit>((Func<BikeMultiSplit, bool>) (s => s.ShowGpsLossWarning));
      }
      else
      {
        this.SingleSplits.Clear();
        this.AddSingleSplits(bikeEventDetails);
        this.ShowGpsLossWarning = false;
      }
      await this.PrepareDataForChartsAsync(bikeEventDetails);
      this.RaisePropertyChanged("ChartSeriesInformation");
    }

    private async Task PrepareDataForChartsAsync(BikeEvent bikeEventDetails)
    {
      List<ChartDistanceSeriesInfo> localChartSeriesInfo = new List<ChartDistanceSeriesInfo>();
      DistanceUnitType unitType = this.userProfileService.DistanceUnitType;
      if (bikeEventDetails.MapPoints != null && bikeEventDetails.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.Speed > Speed.Zero)))
      {
        ChartDistanceSeriesInfo distanceSeriesInfo = new ChartDistanceSeriesInfo();
        distanceSeriesInfo.Name = "Speed";
        distanceSeriesInfo.SeriesType = ChartSeriesType.Speed;
        distanceSeriesInfo.SeriesData = bikeEventDetails.GetSpeedData(this.userProfileService);
        distanceSeriesInfo.PrecalculatedAverage = ChartUtilities.ConvertSpeedToUserUnits(bikeEventDetails.AverageSpeed, unitType);
        distanceSeriesInfo.DisplayedMaxValue = ChartUtilities.ConvertSpeedToUserUnits(bikeEventDetails.MaxSpeed, unitType);
        distanceSeriesInfo.ShowAverageLine = true;
        distanceSeriesInfo.ShowHighMarker = true;
        distanceSeriesInfo.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo);
      }
      if (bikeEventDetails.MapPoints != null && bikeEventDetails.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.HeartRate > 0)))
      {
        HeartRateZone heartRateZonesAsync = await this.heartRateService.GetEventHeartRateZonesAsync(bikeEventDetails.HeartRateZones);
        ChartDistanceSeriesInfo distanceSeriesInfo = new ChartDistanceSeriesInfo();
        distanceSeriesInfo.Name = "HeartRate";
        distanceSeriesInfo.SeriesType = ChartSeriesType.DistanceHeartRate;
        distanceSeriesInfo.SeriesData = bikeEventDetails.GetHeartRateData(this.userProfileService);
        distanceSeriesInfo.ShowSplitMarkers = false;
        distanceSeriesInfo.ShowHighMarker = true;
        distanceSeriesInfo.ShowAverageLine = false;
        distanceSeriesInfo.HRZones = heartRateZonesAsync;
        distanceSeriesInfo.DisplayedMaxValue = (double) bikeEventDetails.PeakHeartRate;
        distanceSeriesInfo.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        localChartSeriesInfo.Add(distanceSeriesInfo);
      }
      if (bikeEventDetails.MapPoints != null && bikeEventDetails.MapPoints.Any<MapPoint>((Func<MapPoint, bool>) (d => d.Location != null)))
      {
        ChartDistanceSeriesInfo distanceSeriesInfo1 = new ChartDistanceSeriesInfo();
        distanceSeriesInfo1.Name = "Elevation";
        distanceSeriesInfo1.SeriesType = ChartSeriesType.Elevation;
        distanceSeriesInfo1.SeriesData = bikeEventDetails.GetElevationData(this.userProfileService, false);
        distanceSeriesInfo1.DisplayedMaxValue = ChartUtilities.ConvertElevationToUserUnits(bikeEventDetails.MaxAltitude, unitType);
        distanceSeriesInfo1.ShowSplitMarkers = false;
        distanceSeriesInfo1.ShowAverageLine = false;
        distanceSeriesInfo1.ShowHighMarker = true;
        distanceSeriesInfo1.Selected = !localChartSeriesInfo.Any<ChartDistanceSeriesInfo>();
        ChartDistanceSeriesInfo distanceSeriesInfo2 = distanceSeriesInfo1;
        if (bikeEventDetails.MapPointElevationSource != MapPointElevationSource.BingMapsApi)
          distanceSeriesInfo2.SubLabel = AppResources.ChartEstimateElevationLabel;
        localChartSeriesInfo.Add(distanceSeriesInfo2);
      }
      this.chartSeriesInformation = (IList<ChartDistanceSeriesInfo>) localChartSeriesInfo;
    }

    private void AddSplitGroups(BikeEvent bikeEventDetails)
    {
      IList<BikeEventSequence> sequences = bikeEventDetails.Sequences;
      if (sequences.Count == 0)
        return;
      BikeMultiSplit bikeMultiSplit = (BikeMultiSplit) null;
      Length kilometerDistance = this.userProfileService.GetMileOrKilometerDistance();
      int order = sequences.Last<BikeEventSequence>().Order;
      int index = 0;
      for (int splitNumber = 1; splitNumber <= order; ++splitNumber)
      {
        BikeEventSequence sequence = bikeEventDetails.Sequences[index];
        if (sequence.Order < splitNumber)
          throw new InvalidOperationException("Sequences are out of order.");
        BikeSingleSplit bikeSingleSplit;
        Length length;
        TimeSpan timeSpan;
        if (sequence.Order > splitNumber)
        {
          bikeSingleSplit = this.CreateEmptySingleSplit(kilometerDistance, splitNumber);
          length = Length.Zero;
          timeSpan = TimeSpan.Zero;
        }
        else if (splitNumber != order || !this.ShouldIgnoreSplit(sequence))
        {
          bikeSingleSplit = this.CreateSingleSplit(sequence.SplitDistance, splitNumber == order ? sequence.TotalDistance : Length.FromMillimeters(kilometerDistance.TotalMillimeters * (double) splitNumber), sequence.AltitudeGain - sequence.AltitudeLoss, sequence.SplitSpeed);
          length = sequence.SplitDistance;
          timeSpan = sequence.Duration;
          ++index;
        }
        else
          continue;
        if (bikeMultiSplit == null)
          bikeMultiSplit = new BikeMultiSplit();
        bikeMultiSplit.SingleSplits.Add(bikeSingleSplit);
        bikeMultiSplit.Distance += length;
        bikeMultiSplit.Duration += timeSpan;
        bikeMultiSplit.Elevation += bikeSingleSplit.Elevation;
        bikeMultiSplit.TotalDistanceDisplay = (string) this.formattingService.FormatSplitDistanceHeader(bikeSingleSplit.TotalDistance);
        if (bikeMultiSplit.SingleSplits.Count >= this.SplitGroupSize)
        {
          this.GroupedSplits.Add(bikeMultiSplit);
          bikeMultiSplit = (BikeMultiSplit) null;
        }
      }
      if (bikeMultiSplit != null)
        this.GroupedSplits.Add(bikeMultiSplit);
      foreach (BikeMultiSplit groupedSplit in (IEnumerable<BikeMultiSplit>) this.GroupedSplits)
      {
        groupedSplit.SpeedSpan = this.formattingService.FormatSpeed(new Speed?(groupedSplit.Speed), true, true);
        groupedSplit.ElevationSpan = this.formattingService.FormatElevation(new Length?(groupedSplit.Elevation), true, true);
      }
      this.CalculateSplitGroupMaxAndMin(kilometerDistance);
      this.CalculateSplitGroupGpsLossWarning();
    }

    private void AddSingleSplits(BikeEvent bikeEventDetails)
    {
      IList<BikeEventSequence> sequences = bikeEventDetails.Sequences;
      if (sequences.Count == 0)
        return;
      int index1 = 0;
      Speed speed1 = sequences[0].SplitSpeed;
      int index2 = 0;
      Speed speed2 = speed1;
      Length kilometerDistance = this.userProfileService.GetMileOrKilometerDistance();
      int order = sequences.Last<BikeEventSequence>().Order;
      int index3 = 0;
      for (int splitNumber = 1; splitNumber <= order; ++splitNumber)
      {
        BikeEventSequence sequence = sequences[index3];
        if (sequence.Order < splitNumber)
          throw new InvalidOperationException("Sequences are out of order.");
        BikeSingleSplit bikeSingleSplit;
        if (sequence.Order > splitNumber)
          bikeSingleSplit = this.CreateEmptySingleSplit(kilometerDistance, splitNumber);
        else if (splitNumber != order || !this.ShouldIgnoreSplit(sequence))
        {
          Speed splitSpeed = sequence.SplitSpeed;
          if (splitNumber != order || sequence.SplitDistance == bikeEventDetails.SplitDistance)
          {
            if (splitSpeed > Speed.Zero && splitSpeed < speed1)
            {
              index1 = index3;
              speed1 = splitSpeed;
            }
            else if (splitSpeed > speed2)
            {
              index2 = index3;
              speed2 = splitSpeed;
            }
          }
          bikeSingleSplit = this.CreateSingleSplit(sequence.SplitDistance, splitNumber == order ? sequence.TotalDistance : Length.FromMillimeters(kilometerDistance.TotalMillimeters * (double) splitNumber), sequence.AltitudeGain - sequence.AltitudeLoss, splitSpeed);
          ++index3;
        }
        else
          continue;
        this.SingleSplits.Add(bikeSingleSplit);
      }
      if (index1 == index2)
        return;
      BikeEventSequence bikeEventSequence1 = sequences[index1];
      BikeEventSequence bikeEventSequence2 = sequences[index2];
      int index4 = bikeEventSequence1.Order - 1;
      int index5 = bikeEventSequence2.Order - 1;
      BikeSingleSplit singleSplit1 = this.SingleSplits[index4];
      BikeSingleSplit singleSplit2 = this.SingleSplits[index5];
      singleSplit1.SpeedMarker = SpeedMarker.Slowest;
      singleSplit2.SpeedMarker = SpeedMarker.Fastest;
    }

    private BikeSingleSplit CreateEmptySingleSplit(
      Length splitDistance,
      int splitNumber)
    {
      return this.CreateSingleSplit(splitDistance, Length.FromMillimeters(splitDistance.TotalMillimeters * (double) splitNumber), Length.Zero, Speed.Zero);
    }

    private BikeSingleSplit CreateSingleSplit(
      Length splitDistance,
      Length totalDistance,
      Length elevation,
      Speed speed)
    {
      return new BikeSingleSplit()
      {
        SplitDistance = splitDistance,
        TotalDistance = totalDistance,
        TotalDistanceDisplay = (string) this.formattingService.FormatSplitDistance(totalDistance, 2),
        Elevation = elevation,
        ElevationSpan = elevation != Length.Zero ? this.formattingService.FormatElevation(new Length?(elevation), appendUnit: true) : Formatter.NotAvailableStyledSpan,
        SpeedSpan = speed > Speed.Zero ? this.formattingService.FormatSpeed(new Speed?(speed), appendUnit: true) : Formatter.NotAvailableStyledSpan
      };
    }

    private void CalculateSplitGroupMaxAndMin(Length splitDistance)
    {
      if (this.GroupedSplits.Count == 0)
        return;
      Speed speed1 = this.GroupedSplits[0].Speed;
      BikeMultiSplit bikeMultiSplit1 = this.GroupedSplits[0];
      Speed speed2 = speed1;
      BikeMultiSplit bikeMultiSplit2 = bikeMultiSplit1;
      for (int index = 0; index < this.GroupedSplits.Count; ++index)
      {
        BikeMultiSplit groupedSplit = this.GroupedSplits[index];
        if (index != this.GroupedSplits.Count - 1 || groupedSplit.SingleSplits.Count == this.SplitGroupSize && groupedSplit.Distance == Length.FromMillimeters(splitDistance.TotalMillimeters * (double) this.SplitGroupSize))
        {
          Speed speed3 = groupedSplit.Speed;
          if (speed3 > Speed.Zero && speed3 < speed1)
          {
            speed1 = speed3;
            bikeMultiSplit1 = groupedSplit;
          }
          else if (speed3 > speed2)
          {
            speed2 = speed3;
            bikeMultiSplit2 = groupedSplit;
          }
        }
      }
      if (bikeMultiSplit1 == bikeMultiSplit2)
        return;
      bikeMultiSplit1.SpeedMarker = SpeedMarker.Slowest;
      bikeMultiSplit2.SpeedMarker = SpeedMarker.Fastest;
    }

    private void CalculateSplitGroupGpsLossWarning()
    {
      for (int index1 = 0; index1 < this.GroupedSplits.Count; ++index1)
      {
        BikeMultiSplit groupedSplit = this.GroupedSplits[index1];
        bool flag1 = false;
        bool flag2 = false;
        for (int index2 = 0; index2 < groupedSplit.SingleSplits.Count; ++index2)
        {
          if (groupedSplit.SingleSplits[index2].SplitDistance == Length.Zero)
          {
            flag2 = true;
            if (index1 > 0 && index2 == 0 && this.GroupedSplits[index1 - 1].Distance > Length.Zero)
              this.GroupedSplits[index1 - 1].ShowGpsLossWarning = true;
          }
          else
            flag1 = true;
        }
        if (flag1 & flag2)
          groupedSplit.ShowGpsLossWarning = true;
      }
    }

    private bool ShouldIgnoreSplit(BikeEventSequence sequence)
    {
      Length splitDistance = sequence.SplitDistance;
      return (this.userProfileService.DistanceUnitType == DistanceUnitType.Metric ? splitDistance.TotalKilometers : splitDistance.TotalMiles) < 0.01;
    }
  }
}
