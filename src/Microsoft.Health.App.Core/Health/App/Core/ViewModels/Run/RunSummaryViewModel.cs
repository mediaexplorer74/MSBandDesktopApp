// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  [PageTaxonomy(new string[] {"Fitness", "Run", "Summary Map"})]
  public class RunSummaryViewModel : RouteBasedExerciseEventSummaryViewModelBase<RunEvent>
  {
    public const string SharePreviewImageFileName = "Running.png";
    private readonly ISmoothNavService smoothNavService;
    private readonly IFormattingService formattingService;
    private HealthCommand openFullMapCommand;
    private bool hasGps;
    private Speed bestSplit;

    public RunSummaryViewModel(
      ISmoothNavService smoothNavService,
      IBestEventProvider bestEventProvider,
      IShareService shareService,
      INetworkService networkService,
      IErrorHandlingService cargoExceptionUtils,
      IFormattingService formattingService,
      IMessageBoxService messageBoxService,
      IHistoryProvider historyProvider,
      IHealthCloudClient healthCloudClient,
      IRouteBasedExerciseEventProvider<RunEvent> runProvider,
      IMessageSender messageSender)
      : base(networkService, smoothNavService, cargoExceptionUtils, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, runProvider, messageSender)
    {
      this.smoothNavService = smoothNavService;
      this.formattingService = formattingService;
      this.GoalType = GoalType.RunGoal;
      this.EventType = EventType.Running;
      this.DeletionMessage = AppResources.RunDeleteMessage;
    }

    public bool HasGps
    {
      get => this.hasGps;
      set => this.SetProperty<bool>(ref this.hasGps, value, nameof (HasGps));
    }

    protected override bool IsShareImageIncluded => this.HasGps;

    private async Task<string> GetShareMessageAsync()
    {
      string str = (string) this.formattingService.FormatDistance(new Length?(this.Model.TotalDistance), appendUnit: true);
      return await Task.FromResult<string>(string.Format(AppResources.ShareRunMessage, new object[2]
      {
        (object) Formatter.GetMonthDayString(this.Model.StartTime),
        (object) str
      }));
    }

    protected override async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest request = await base.CreateShareRequestAsync();
      request.EventType = this.EventType;
      request.ChooserTitle = AppResources.ChooserShareRun;
      request.Title = AppResources.ShareRunTitle;
      request.Message = ShareMessageFormatter.FormatRunMessage(this.Model, this.formattingService);
      request.SharePreviewImageFileName = "Running.png";
      ShareRequest shareRequest = request;
      ShareUrlsRequestFormData urlsRequestFormData1 = new ShareUrlsRequestFormData();
      urlsRequestFormData1.EventType = this.EventType.ToString();
      urlsRequestFormData1.EventId = this.Model.EventId;
      ShareUrlsRequestFormData urlsRequestFormData2 = urlsRequestFormData1;
      string shareMessageAsync = await this.GetShareMessageAsync();
      urlsRequestFormData2.Title = shareMessageAsync;
      urlsRequestFormData1.MetricLabel1 = AppResources.PanelStatisticLabelDuration;
      urlsRequestFormData1.MetricContent1 = this.FormattingService.FormatDurationValue(this.Model.Duration);
      urlsRequestFormData1.MetricUnit1 = this.FormattingService.FormatDurationUnit();
      urlsRequestFormData1.MetricLabel2 = AppResources.PanelStatisticLabelDistance;
      urlsRequestFormData1.MetricContent2 = this.FormattingService.FormatDistanceMileKMValue(this.Model.TotalDistance);
      urlsRequestFormData1.MetricUnit2 = this.FormattingService.FormatDistanceMileKMUnit();
      urlsRequestFormData1.MetricLabel3 = AppResources.ShareMetricLabelAverageHeartRate;
      urlsRequestFormData1.MetricContent3 = this.FormattingService.FormatHeartRateValue(this.Model.AverageHeartRate);
      urlsRequestFormData1.MetricUnit3 = this.FormattingService.FormatHeartRateUnit();
      urlsRequestFormData1.MetricLabel4 = AppResources.PanelStatisticLabelBestSplit;
      urlsRequestFormData1.MetricContent4 = (string) this.formattingService.FormatPace(new Speed?(this.bestSplit));
      urlsRequestFormData1.MetricUnit4 = this.FormattingService.ShareMetricFormatPaceUnit();
      urlsRequestFormData1.TwitterDescription = AppResources.ShareTwitterDescription;
      urlsRequestFormData1.ActivityName = string.IsNullOrEmpty(this.Name) ? AppResources.TileTitle_Run : this.Name;
      shareRequest.ShareUrlsRequestFormData = urlsRequestFormData1;
      shareRequest = (ShareRequest) null;
      urlsRequestFormData2 = (ShareUrlsRequestFormData) null;
      urlsRequestFormData1 = (ShareUrlsRequestFormData) null;
      return request;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.DisplayNamingTextBox = false;
      await base.LoadDataAsync(parameters);
      this.AddDurationStat();
      this.AddCaloriesStat();
      this.AddElevationGainStat();
      this.AddElevationLossStat();
      this.bestSplit = this.GetBestSplit();
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelBestSplit,
        Glyph = "\uE048",
        Value = (object) this.bestSplit,
        ValueType = StatValueType.Pace
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelPace,
        Glyph = "\uE029",
        Value = (object) this.Model.Pace,
        ValueType = StatValueType.Pace
      });
      this.AddHeartRateStat();
      this.AddEndingHeartRateStat();
      this.AddRecoveryTimeStat();
      this.AddFitnessBenefitStat();
      this.AddUvExposureStat();
      this.AddCardioMinutesStat();
      this.HasGps = this.Model.IsLowPowerGps || this.Model.MapPoints.HasEnoughGpsToMap(MapType.Run, this.Model.TotalDistance);
      this.Initialized = true;
    }

    private Speed GetBestSplit() => this.Model.Sequences != null && this.Model.Sequences.Count > 1 ? this.Model.Sequences.Select<RunEventSequence, Speed>((Func<RunEventSequence, Speed>) (sequence => sequence.SplitPace)).Take<Speed>(this.Model.Sequences.Count - 1).Max<Speed>() : Speed.Zero;

    public override ICommand OpenFullMapCommand => (ICommand) this.openFullMapCommand ?? (ICommand) (this.openFullMapCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (RunFullMapViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "ID",
        this.Id
      },
      {
        "IsLowPowerGps",
        this.IsLowPowerGps.ToString()
      }
    })), (Func<bool>) (() => this.HasGps)));
  }
}
