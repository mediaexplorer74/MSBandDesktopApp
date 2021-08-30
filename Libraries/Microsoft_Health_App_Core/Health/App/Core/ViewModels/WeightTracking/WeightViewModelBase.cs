// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WeightTracking.WeightViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.WeightTracking
{
  public abstract class WeightViewModelBase : PanelViewModelBase
  {
    private readonly IWeightProvider weightProvider;
    private readonly IUserProfileService userProfileService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IWeightSavingService weightSavingService;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WeightTracking\\WeightViewModelBase.cs");
    private ICommand saveWeightCommand;
    private IList<Microsoft.Health.App.Core.Providers.WeightSensor> history;
    private IList<Microsoft.Health.App.Core.Providers.WeightSensor> windowedHistory;
    private Range<DateTimeOffset> timeWindow;
    private Weight currentWeight;
    private HealthCommand<Microsoft.Health.App.Core.Providers.WeightSensor> openWeightCommand;
    private IList<ChartDateSeriesInfo> weightSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();

    protected WeightViewModelBase(
      INetworkService networkService,
      IWeightProvider weightProvider,
      IUserProfileService userProfileService,
      ISmoothNavService smoothNavService,
      IWeightSavingService weightSavingService)
      : base(networkService)
    {
      this.weightProvider = weightProvider;
      this.userProfileService = userProfileService;
      this.smoothNavService = smoothNavService;
      this.weightSavingService = weightSavingService;
    }

    public IList<StatViewModel> Stats { get; } = (IList<StatViewModel>) new ObservableCollection<StatViewModel>();

    public IList<Microsoft.Health.App.Core.Providers.WeightSensor> History
    {
      get => this.history;
      protected set => this.SetProperty<IList<Microsoft.Health.App.Core.Providers.WeightSensor>>(ref this.history, value, nameof (History));
    }

    public IList<Microsoft.Health.App.Core.Providers.WeightSensor> WindowedHistory
    {
      get => this.windowedHistory;
      protected set => this.SetProperty<IList<Microsoft.Health.App.Core.Providers.WeightSensor>>(ref this.windowedHistory, value, nameof (WindowedHistory));
    }

    public Range<DateTimeOffset> TimeWindow
    {
      get => this.timeWindow;
      protected set => this.SetProperty<Range<DateTimeOffset>>(ref this.timeWindow, value, nameof (TimeWindow));
    }

    public IList<ChartDateSeriesInfo> WeightSeriesInformation => this.weightSeriesInformation;

    public abstract string ChangedWeightHeader { get; }

    protected abstract Range<DateTimeOffset> GetTimeWindow();

    protected virtual IList<Microsoft.Health.App.Core.Providers.WeightSensor> GetHistoryWindowPlusOne()
    {
      List<Microsoft.Health.App.Core.Providers.WeightSensor> weightSensorList = new List<Microsoft.Health.App.Core.Providers.WeightSensor>();
      foreach (Microsoft.Health.App.Core.Providers.WeightSensor weightSensor in (IEnumerable<Microsoft.Health.App.Core.Providers.WeightSensor>) this.History)
      {
        weightSensorList.Add(weightSensor);
        if (!this.TimeWindow.Contains(weightSensor.Timestamp))
          break;
      }
      return (IList<Microsoft.Health.App.Core.Providers.WeightSensor>) weightSensorList;
    }

    public ICommand OpenWeightCommand => (ICommand) this.openWeightCommand ?? (ICommand) (this.openWeightCommand = new HealthCommand<Microsoft.Health.App.Core.Providers.WeightSensor>((Action<Microsoft.Health.App.Core.Providers.WeightSensor>) (weightSensor => this.smoothNavService.Navigate(typeof (EditWeightViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "WeightSensor.Weight",
        ((int) weightSensor.Weight.TotalGrams).ToString()
      },
      {
        "WeightSensor.TimeStamp",
        weightSensor.Timestamp.ToString("o")
      }
    }))));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      using (CancellationTokenSource tokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        try
        {
          this.History = await this.weightProvider.GetAllWeightsAsync(tokenSource.Token);
          this.TimeWindow = this.GetTimeWindow();
          this.WindowedHistory = this.GetHistoryWindowPlusOne();
          Weight? nullable = new Weight?();
          string str = string.Empty;
          this.currentWeight = this.WindowedHistory.First<Microsoft.Health.App.Core.Providers.WeightSensor>().Weight;
          if (this.History.Count > 1)
          {
            DateTimeOffset dateTimeOffset = this.TimeWindow.Low;
            DateTime lowDate = dateTimeOffset.Date;
            Microsoft.Health.App.Core.Providers.WeightSensor weightSensor1 = this.WindowedHistory.LastOrDefault<Microsoft.Health.App.Core.Providers.WeightSensor>((Func<Microsoft.Health.App.Core.Providers.WeightSensor, bool>) (w => lowDate == w.Timestamp.Date));
            if (weightSensor1 == null)
            {
              weightSensor1 = this.WindowedHistory.Last<Microsoft.Health.App.Core.Providers.WeightSensor>();
              if (weightSensor1.Timestamp < (DateTimeOffset) lowDate)
              {
                Microsoft.Health.App.Core.Providers.WeightSensor weightSensor2 = this.WindowedHistory[this.windowedHistory.Count - 2];
                TimeSpan timeSpan1 = weightSensor2.Timestamp - weightSensor1.Timestamp;
                TimeSpan timeSpan2 = (DateTimeOffset) lowDate - weightSensor1.Timestamp;
                double num = (weightSensor2.Weight - weightSensor1.Weight).TotalGrams * (double) timeSpan2.Ticks / (double) timeSpan1.Ticks;
                dateTimeOffset = weightSensor1.Timestamp;
                Microsoft.Health.App.Core.Providers.WeightSensor weightSensor3 = new Microsoft.Health.App.Core.Providers.WeightSensor(dateTimeOffset.Add(timeSpan2), Weight.FromGrams(weightSensor1.Weight.TotalGrams + num));
                weightSensor3.IsCalculated = true;
                this.WindowedHistory.Insert(this.WindowedHistory.Count - 1, weightSensor3);
                weightSensor1 = weightSensor3;
              }
            }
            nullable = new Weight?(this.currentWeight - weightSensor1.Weight);
            str = nullable.Value.TotalGrams > 0.0 ? "\uE201" : "\uE202";
          }
          this.Stats.Clear();
          this.Stats.Add(new StatViewModel()
          {
            Label = this.ChangedWeightHeader,
            GlyphPrefix = str,
            Value = (object) nullable,
            ValueType = StatValueType.Weight
          });
          double bmi = this.CalculateBmi(this.currentWeight.TotalKilograms, Length.FromMillimeters((double) this.userProfileService.CurrentUserProfile.HeightMM).TotalMeters);
          string bmiClassString = this.GetBmiClassString(bmi);
          this.Stats.Add(new StatViewModel()
          {
            Label = AppResources.WeightTrackingBmiLabel,
            Value = (object) bmi,
            ValueType = StatValueType.Double,
            SubStat1 = new SubStatViewModel()
            {
              Label = bmiClassString
            }
          });
          this.Stats.Add(new StatViewModel()
          {
            Label = AppResources.WeightTrackingLastWeight,
            Value = (object) this.currentWeight,
            ValueType = StatValueType.Weight
          });
          this.PrepareWeightChart();
        }
        catch (Exception ex)
        {
          WeightViewModelBase.Logger.Error((object) string.Format("Unable to retrieve Weights - {0}", new object[1]
          {
            (object) this.GetType().Name
          }), ex);
          throw ex;
        }
      }
      this.RaisePropertyChanged("WeightSeriesInformation");
    }

    protected void PrepareWeightChart()
    {
      bool isMetric = this.userProfileService.MassUnitType == MassUnitType.Metric;
      List<DateChartPoint> list = this.WindowedHistory.Reverse<Microsoft.Health.App.Core.Providers.WeightSensor>().Select<Microsoft.Health.App.Core.Providers.WeightSensor, DateChartPoint>((Func<Microsoft.Health.App.Core.Providers.WeightSensor, DateChartPoint>) (w =>
      {
        return new DateChartPoint()
        {
          Date = w.Timestamp.LocalDateTime,
          Value = isMetric ? w.Weight.TotalKilograms : w.Weight.TotalPounds,
          Classification = w.IsCalculated ? DateChartValueClassification.Unknown : DateChartValueClassification.Activity
        };
      })).ToList<DateChartPoint>();
      ChartDateSeriesInfo chartDateSeriesInfo = new ChartDateSeriesInfo();
      chartDateSeriesInfo.Name = "Weight";
      chartDateSeriesInfo.SeriesType = ChartSeriesType.Weight;
      chartDateSeriesInfo.SeriesData = (IList<DateChartPoint>) list;
      chartDateSeriesInfo.Selected = true;
      chartDateSeriesInfo.ShowHighMarker = false;
      chartDateSeriesInfo.ShowAverageLine = false;
      chartDateSeriesInfo.ShowAverageMarker = false;
      chartDateSeriesInfo.ShowLastMarker = true;
      chartDateSeriesInfo.ShowLowMarker = false;
      chartDateSeriesInfo.SeriesStartDate = this.TimeWindow.Low.LocalDateTime;
      chartDateSeriesInfo.SeriesEndDate = this.TimeWindow.High.LocalDateTime;
      this.weightSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>()
      {
        chartDateSeriesInfo
      };
    }

    public ICommand SaveWeightCommand => this.saveWeightCommand ?? (this.saveWeightCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () => await this.weightSavingService.OpenSaveWeightAsync(this.currentWeight))));

    private double CalculateBmi(double kilograms, double meters) => kilograms / (meters * meters);

    private string GetBmiClassString(double bmi)
    {
      if (bmi < 16.5)
        return AppResources.WeightTrackingBmiUnderweight;
      if (bmi < 25.0)
        return AppResources.WeightTrackingBmiNormal;
      return bmi < 30.0 ? AppResources.WeightTrackingBmiBorderline : AppResources.WeightTrackingBmiHigh;
    }
  }
}
