// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Bike.BikeSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Bike
{
  [PageTaxonomy(new string[] {"Fitness", "Bike", "Summary Map"})]
  public class BikeSummaryViewModel : RouteBasedExerciseEventSummaryViewModelBase<BikeEvent>
  {
    private const string SharePreviewImageFileName = "Biking.png";
    private readonly IList<ChartDurationSeriesInfo> chartSeriesInformation = (IList<ChartDurationSeriesInfo>) new List<ChartDurationSeriesInfo>();
    private readonly ISmoothNavService smoothNavService;
    private readonly IFormattingService formattingService;
    private readonly IHeartRateService heartRateService;
    private HealthCommand openFullMapCommand;
    private int splitGroupSize;
    private bool? hasGps;
    private bool enableShare;

    public BikeSummaryViewModel(
      ISmoothNavService smoothNavService,
      IBestEventProvider bestEventProvider,
      IErrorHandlingService cargoExceptionUtils,
      INetworkService networkUtils,
      IHistoryProvider historyProvider,
      IMessageBoxService messageBoxService,
      IFormattingService formattingService,
      IShareService shareService,
      IHealthCloudClient healthCloudClient,
      IHeartRateService heartRateService,
      IRouteBasedExerciseEventProvider<BikeEvent> bikeProvider,
      IMessageSender messageSender)
      : base(networkUtils, smoothNavService, cargoExceptionUtils, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, bikeProvider, messageSender)
    {
      Assert.ParamIsNotNull((object) bikeProvider, "provider");
      Assert.ParamIsNotNull((object) smoothNavService, nameof (smoothNavService));
      this.smoothNavService = smoothNavService;
      this.formattingService = formattingService;
      this.heartRateService = heartRateService;
      this.splitGroupSize = 1;
      this.GoalType = GoalType.BikeGoal;
      this.EventType = EventType.Biking;
      this.DeletionMessage = AppResources.BikeDeleteMessage;
    }

    public bool? HasGps
    {
      get => this.hasGps;
      set => this.SetProperty<bool?>(ref this.hasGps, value, nameof (HasGps));
    }

    public IList<ChartDurationSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public int SplitGroupSize
    {
      get => this.splitGroupSize;
      set => this.SetProperty<int>(ref this.splitGroupSize, value, nameof (SplitGroupSize));
    }

    public bool EnableShare
    {
      get => this.enableShare;
      set => this.SetProperty<bool>(ref this.enableShare, value, nameof (EnableShare));
    }

    protected override bool IsShareImageIncluded => true;

    private async Task<string> GetShareMessageAsync()
    {
      string monthDayString = Formatter.GetMonthDayString(this.Model.StartTime);
      bool? hasGps = this.HasGps;
      string str;
      if (hasGps.HasValue)
      {
        hasGps = this.HasGps;
        if (hasGps.Value)
        {
          str = (string) this.formattingService.FormatDistance(new Length?(this.Model.TotalDistance), appendUnit: true);
          goto label_4;
        }
      }
      str = (string) Formatter.FormatTimeSpan(this.Model.Duration, Formatter.TimeSpanFormat.Abbreviated);
label_4:
      return await Task.FromResult<string>(string.Format(AppResources.ShareBikeMessage, new object[2]
      {
        (object) monthDayString,
        (object) str
      }));
    }

    protected override async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest request = await base.CreateShareRequestAsync();
      request.EventType = this.EventType;
      request.ChooserTitle = AppResources.ChooserShareBike;
      request.Title = AppResources.ShareBikeTitle;
      request.Message = ShareMessageFormatter.FormatBikeMessage(this.Model, this.formattingService);
      request.SharePreviewImageFileName = "Biking.png";
      ShareRequest shareRequest = request;
      ShareUrlsRequestFormData urlsRequestFormData1 = new ShareUrlsRequestFormData();
      urlsRequestFormData1.EventType = this.EventType.ToString();
      urlsRequestFormData1.EventId = this.Model.EventId;
      ShareUrlsRequestFormData urlsRequestFormData2 = urlsRequestFormData1;
      string shareMessageAsync = await this.GetShareMessageAsync();
      urlsRequestFormData2.Title = shareMessageAsync;
      urlsRequestFormData1.TwitterDescription = AppResources.ShareTwitterDescription;
      urlsRequestFormData1.ActivityName = string.IsNullOrEmpty(this.Name) ? AppResources.TileTitle_Bike : this.Name;
      urlsRequestFormData1.MetricLabel1 = AppResources.PanelStatisticLabelDuration;
      urlsRequestFormData1.MetricContent1 = this.FormattingService.FormatDurationValue(this.Model.Duration);
      urlsRequestFormData1.MetricUnit1 = this.FormattingService.FormatDurationUnit();
      urlsRequestFormData1.MetricLabel3 = AppResources.ShareMetricLabelAverageHeartRate;
      urlsRequestFormData1.MetricContent3 = this.FormattingService.FormatHeartRateValue(this.Model.AverageHeartRate);
      urlsRequestFormData1.MetricUnit3 = this.FormattingService.FormatHeartRateUnit();
      shareRequest.ShareUrlsRequestFormData = urlsRequestFormData1;
      shareRequest = (ShareRequest) null;
      urlsRequestFormData2 = (ShareUrlsRequestFormData) null;
      urlsRequestFormData1 = (ShareUrlsRequestFormData) null;
      bool? hasGps = this.HasGps;
      if (hasGps.HasValue)
      {
        hasGps = this.HasGps;
        if (hasGps.Value)
        {
          request.ShareUrlsRequestFormData.MetricLabel2 = AppResources.PanelStatisticLabelDistance;
          request.ShareUrlsRequestFormData.MetricContent2 = this.FormattingService.FormatDistanceMileKMValue(this.Model.TotalDistance);
          request.ShareUrlsRequestFormData.MetricUnit2 = this.FormattingService.FormatDistanceMileKMUnit();
          request.ShareUrlsRequestFormData.MetricLabel4 = AppResources.PanelStatisticLabelAverageSpeed;
          request.ShareUrlsRequestFormData.MetricContent4 = this.FormattingService.FormatSpeedValue(this.Model.AverageSpeed);
          request.ShareUrlsRequestFormData.MetricUnit4 = this.FormattingService.FormatSpeedUnit();
          goto label_6;
        }
      }
      request.ShareUrlsRequestFormData.MetricLabel2 = AppResources.PanelStatisticLabelCaloriesBurned;
      request.ShareUrlsRequestFormData.MetricContent2 = this.FormattingService.FormatCaloriesValue(this.Model.CaloriesBurned);
      request.ShareUrlsRequestFormData.MetricUnit2 = this.FormattingService.FormatCaloriesUnit();
      request.ShareUrlsRequestFormData.MetricLabel4 = AppResources.PanelStatisticLabelEndingHeartRate;
      request.ShareUrlsRequestFormData.MetricContent4 = this.FormattingService.FormatHeartRateValue(this.Model.FinishHeartRate);
      request.ShareUrlsRequestFormData.MetricUnit4 = this.FormattingService.FormatHeartRateUnit();
label_6:
      return request;
    }

    public override ICommand OpenFullMapCommand => (ICommand) this.openFullMapCommand ?? (ICommand) (this.openFullMapCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (BikeFullMapViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "ID",
        this.Id.ToString()
      },
      {
        "SplitGroupSize",
        this.SplitGroupSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)
      },
      {
        "IsLowPowerGps",
        this.IsLowPowerGps.ToString()
      }
    }))));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.chartSeriesInformation.Clear();
      this.DisplayNamingTextBox = false;
      await base.LoadDataAsync(parameters);
      this.SplitGroupSize = this.Model.SplitGroupSize;
      if (this.SplitGroupSize <= 0)
        this.SplitGroupSize = 1;
      this.Stats.Clear();
      this.AddDurationStat();
      this.AddCaloriesStat();
      if (this.Model.GpsState == GpsState.AtLeastOnePoint)
      {
        this.HasGps = new bool?(true);
        this.Stats.Add(new StatViewModel()
        {
          Label = AppResources.PanelStatisticLabelAverageSpeed,
          Glyph = "\uE137",
          Value = (object) this.Model.AverageSpeed,
          ValueType = StatValueType.Speed
        });
        this.Stats.Add(new StatViewModel()
        {
          Label = AppResources.PanelStatisticLabelTopSpeed,
          Glyph = "\uE137",
          Value = (object) this.Model.MaxSpeed,
          ValueType = StatValueType.Speed
        });
        this.AddElevationGainStat();
        this.AddElevationLossStat();
        IList<BikeEventSequence> sequences = this.Model.Sequences;
        TimeSpan timeSpan = sequences == null || sequences.Count <= 1 ? TimeSpan.Zero : this.GetBestSplitPace(sequences);
        this.Stats.Add(new StatViewModel()
        {
          Label = AppResources.PanelStatisticLabelBestSplit,
          Glyph = "\uE048",
          Value = (object) timeSpan,
          ValueType = StatValueType.DurationTicks
        });
        TimeSpan pace = this.GetPace(this.Model.Duration, this.Model.TotalDistance);
        this.Stats.Add(new StatViewModel()
        {
          Label = AppResources.PanelStatisticLabelPace,
          Glyph = "\uE029",
          Value = (object) pace,
          ValueType = StatValueType.DurationTicks
        });
        this.Initialized = true;
      }
      else
      {
        this.HasGps = new bool?(false);
        await this.PrepareDataForChartAsync();
        this.RaisePropertyChanged("ChartSeriesInformation");
      }
      this.AddHeartRateStat();
      this.AddEndingHeartRateStat();
      this.AddRecoveryTimeStat();
      this.AddFitnessBenefitStat();
      this.AddUvExposureStat();
      this.AddCardioMinutesStat();
    }

    private TimeSpan GetBestSplitPace(IList<BikeEventSequence> sequences)
    {
      int order = sequences[sequences.Count - 1].Order;
      int length = (int) Math.Ceiling((double) order / (double) this.SplitGroupSize);
      TimeSpan[] timeSpanArray = new TimeSpan[length];
      Length[] lengthArray = new Length[length];
      int index1 = 0;
      for (int index2 = 1; index2 <= order; ++index2)
      {
        BikeEventSequence sequence = sequences[index1];
        if (sequence.Order < index2)
          throw new InvalidOperationException("Sequences are out of order.");
        if (sequence.Order == index2)
        {
          int index3 = (index2 - 1) / this.SplitGroupSize;
          timeSpanArray[index3] += sequence.Duration;
          lengthArray[index3] += sequence.SplitDistance;
          ++index1;
        }
      }
      TimeSpan timeSpan = TimeSpan.Zero;
      for (int index4 = 0; index4 < length; ++index4)
      {
        TimeSpan pace = this.GetPace(timeSpanArray[index4], lengthArray[index4]);
        if (pace > TimeSpan.Zero && (pace < timeSpan || timeSpan == TimeSpan.Zero))
          timeSpan = pace;
      }
      return timeSpan;
    }

    private async Task PrepareDataForChartAsync()
    {
      if (this.chartSeriesInformation.Any<ChartDurationSeriesInfo>())
        this.chartSeriesInformation.Clear();
      IList<DurationChartPoint> heartRateChartData = this.Model.Sequences.GetDurationHeartRateData<BikeEventSequence>(this.Model.Info, this.Model.StartTime);
      HeartRateZone heartRateZonesAsync = await this.heartRateService.GetEventHeartRateZonesAsync(this.Model.HeartRateZones);
      ChartDurationSeriesInfo durationSeriesInfo = new ChartDurationSeriesInfo();
      durationSeriesInfo.Name = "HeartRate";
      durationSeriesInfo.SeriesType = ChartSeriesType.ExerciseHeartRate;
      durationSeriesInfo.SeriesData = heartRateChartData;
      durationSeriesInfo.Selected = true;
      durationSeriesInfo.ShowHighMarker = true;
      durationSeriesInfo.ShowAverageLine = false;
      durationSeriesInfo.ShowAverageMarker = false;
      durationSeriesInfo.ShowLowMarker = false;
      durationSeriesInfo.HRZones = heartRateZonesAsync;
      durationSeriesInfo.DisplayMaxValue = (double) this.Model.PeakHeartRate;
      durationSeriesInfo.Duration = this.Model.Duration;
      this.chartSeriesInformation.Add(durationSeriesInfo);
    }

    private TimeSpan GetPace(TimeSpan duration, Length distance)
    {
      if (distance <= Length.Zero)
        return TimeSpan.Zero;
      double num = this.Model.SplitDistance.TotalCentimeters * (double) this.SplitGroupSize / distance.TotalCentimeters;
      return TimeSpan.FromMilliseconds(duration.TotalMilliseconds * num);
    }
  }
}
