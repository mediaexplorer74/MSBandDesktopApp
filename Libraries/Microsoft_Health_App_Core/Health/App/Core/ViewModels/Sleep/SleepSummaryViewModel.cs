// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Sleep.SleepSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Diagnostics;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Authentication;
using Microsoft.Health.Cloud.Client.Models;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Sleep
{
  [PageTaxonomy(new string[] {"Fitness", "Sleep", "Summary"})]
  public class SleepSummaryViewModel : EventSummaryViewModelBase<SleepEvent>
  {
    private const string SharePreviewImageFileName = "Sleeping.png";
    private readonly ISleepProvider sleepProvider;
    private readonly IUserProfileService userProfileService;
    private readonly IConnectionInfoProvider connectionInfoProvider;
    private readonly IFormattingService formattingService;
    private IList<ChartDateSeriesInfo> chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
    private HealthCloudConnectionInfo healthCloudConnectionInfo;
    private bool hasSleepDataGap;
    private HealthCommand reportIncorrectSleepCommand;
    private IEnvironmentService environmentService;
    private IMessageBoxService messageBoxService;
    private IErrorHandlingService errorHandlingService;
    private IScreenshotService screenshotService;
    private IDiagnosticsService diagnosticsService;

    public SleepSummaryViewModel(
      ISleepProvider sleepProvider,
      INetworkService networkService,
      IUserProfileService userProfileService,
      IMessageBoxService messageBoxService,
      ISmoothNavService smoothNavService,
      IErrorHandlingService cargoExceptionUtils,
      IBestEventProvider bestEventProvider,
      IHistoryProvider historyProvider,
      IHealthCloudClient healthCloudClient,
      IConnectionInfoProvider connectionInfoProvider,
      IFormattingService formattingService,
      IShareService shareService,
      IMessageSender messageSender,
      IEnvironmentService environmentService,
      IErrorHandlingService errorHandlingService,
      IScreenshotService screenshotService,
      IDiagnosticsService diagnosticsService)
      : base(networkService, smoothNavService, cargoExceptionUtils, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, messageSender)
    {
      this.sleepProvider = sleepProvider;
      this.userProfileService = userProfileService;
      this.connectionInfoProvider = connectionInfoProvider;
      this.environmentService = environmentService;
      this.errorHandlingService = errorHandlingService;
      this.screenshotService = screenshotService;
      this.diagnosticsService = diagnosticsService;
      this.messageBoxService = messageBoxService;
      this.formattingService = formattingService;
      this.GoalType = GoalType.SleepGoal;
      this.EventType = EventType.Sleeping;
      this.DeletionMessage = AppResources.SleepDeleteMessage;
      this.CanHaveBests = false;
    }

    public IList<ChartDateSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public bool HasSleepDataGap
    {
      get => this.hasSleepDataGap;
      set => this.SetProperty<bool>(ref this.hasSleepDataGap, value, nameof (HasSleepDataGap));
    }

    protected override bool IsShareImageIncluded => true;

    private async Task<string> GetShareMessageAsync() => await Task.FromResult<string>(string.Format(AppResources.ShareSleepMessage, new object[2]
    {
      (object) Formatter.FormatSleepTime(this.Model.StartTime.ToLocalTime(), true),
      (object) this.Model.SleepEfficiencyPercentage
    }));

    protected override async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest request = await base.CreateShareRequestAsync();
      request.EventType = this.EventType;
      request.ChooserTitle = AppResources.ChooserShareSleep;
      request.Title = AppResources.ShareSleepTitle;
      request.Message = ShareMessageFormatter.FormatSleepMessage(this.Model, this.formattingService);
      request.SharePreviewImageFileName = "Sleeping.png";
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
      urlsRequestFormData1.MetricLabel2 = AppResources.SleepStatEfficiency;
      urlsRequestFormData1.MetricContent2 = this.FormattingService.FormatPercentageValue(this.Model.SleepEfficiencyPercentage);
      urlsRequestFormData1.MetricUnit2 = this.FormattingService.FormatPercentageUnit();
      urlsRequestFormData1.MetricLabel3 = AppResources.SleepStatRest;
      urlsRequestFormData1.MetricContent3 = this.FormattingService.FormatHeartRateValue(this.Model.RestingHeartRate);
      urlsRequestFormData1.MetricUnit3 = this.FormattingService.FormatHeartRateUnit();
      urlsRequestFormData1.MetricLabel4 = AppResources.SleepStatCals;
      urlsRequestFormData1.MetricContent4 = this.FormattingService.FormatCaloriesValue(this.Model.CaloriesBurned);
      urlsRequestFormData1.MetricUnit4 = this.FormattingService.FormatCaloriesUnit();
      urlsRequestFormData1.TwitterDescription = AppResources.ShareTwitterDescription;
      urlsRequestFormData1.ActivityName = this.GetActivityName();
      shareRequest.ShareUrlsRequestFormData = urlsRequestFormData1;
      shareRequest = (ShareRequest) null;
      urlsRequestFormData2 = (ShareUrlsRequestFormData) null;
      urlsRequestFormData1 = (ShareUrlsRequestFormData) null;
      return request;
    }

    protected override IList<ActionViewModel> InitializeActions()
    {
      IList<ActionViewModel> actionViewModelList = base.InitializeActions();
      if (this.Model != null && this.Model.IsAutoDetected && !this.environmentService.IsPublicRelease)
        actionViewModelList.Add(new ActionViewModel(AppResources.ReportIncorrectSleepTimingButtonText, this.ReportIncorrectSleepCommand));
      return actionViewModelList;
    }

    protected string GetActivityName() => string.Format(AppResources.ShareSleepTitleOnImage, new object[1]
    {
      (object) Formatter.FormatSleepTime(this.Model.StartTime.ToLocalTime(), true)
    });

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await base.LoadDataAsync(parameters);
      if (parameters != null && parameters.ContainsKey("ID"))
        this.Model = await this.sleepProvider.GetSleepEventAsync(parameters["ID"]);
      else
        this.Model = await this.sleepProvider.GetLastSleepEventAsync(true);
      if (this.Model == null)
        throw new NoDataException();
      this.PopulateSleepData();
      this.PrepareDataForCharts();
      await this.SetHistoryIfValidAsync(parameters);
      this.RaisePropertyChanged("ChartSeriesInformation");
      this.HealthCloudConnectionInfo = await this.connectionInfoProvider.GetConnectionInfoAsync(new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan).Token);
    }

    private void PrepareDataForCharts()
    {
      List<ChartDateSeriesInfo> chartDateSeriesInfoList = new List<ChartDateSeriesInfo>();
      if (this.Model.Info.Count > 0 && this.Model.Sequences.Count > 0)
      {
        ChartDateSeriesInfo chartDateSeriesInfo1 = new ChartDateSeriesInfo();
        chartDateSeriesInfo1.Name = "Sleep";
        chartDateSeriesInfo1.SeriesType = ChartSeriesType.Sleep;
        chartDateSeriesInfo1.Interval = TimeSpan.FromMinutes(1.0);
        chartDateSeriesInfo1.StartTime = this.Model.StartTime;
        chartDateSeriesInfo1.Duration = this.Model.Duration;
        chartDateSeriesInfo1.SeriesData = this.Model.GetSleepData();
        chartDateSeriesInfo1.RawSeriesData = (IList<SleepEventSequence>) this.Model.Sequences.OrderBy<SleepEventSequence, DateTimeOffset>((Func<SleepEventSequence, DateTimeOffset>) (p => p.StartTime)).ToList<SleepEventSequence>();
        chartDateSeriesInfo1.Selected = true;
        ChartDateSeriesInfo chartDateSeriesInfo2 = chartDateSeriesInfo1;
        chartDateSeriesInfoList.Add(chartDateSeriesInfo2);
        this.HasSleepDataGap = chartDateSeriesInfo2.SeriesData.Any<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Classification == DateChartValueClassification.Unknown));
        ChartDateSeriesInfo chartDateSeriesInfo3 = new ChartDateSeriesInfo();
        chartDateSeriesInfo3.Name = "HeartRate";
        chartDateSeriesInfo3.SeriesType = ChartSeriesType.SleepHeartRate;
        chartDateSeriesInfo3.Interval = TimeSpan.FromMinutes(1.0);
        chartDateSeriesInfo3.SeriesData = this.Model.GetSleepHeartRateData(this.userProfileService, 5);
        chartDateSeriesInfo3.Selected = false;
        chartDateSeriesInfo3.ShowAverageMarker = false;
        chartDateSeriesInfo3.ShowLowMarker = true;
        chartDateSeriesInfo3.ShowAverageLine = false;
        chartDateSeriesInfo3.DisplayMinValue = (double) this.Model.RestingHeartRate;
        chartDateSeriesInfo3.SeriesStartDate = chartDateSeriesInfo2.SeriesStartDate;
        chartDateSeriesInfo3.SeriesEndDate = chartDateSeriesInfo2.SeriesEndDate;
        ChartDateSeriesInfo chartDateSeriesInfo4 = chartDateSeriesInfo3;
        chartDateSeriesInfoList.Add(chartDateSeriesInfo4);
      }
      this.chartSeriesInformation = (IList<ChartDateSeriesInfo>) chartDateSeriesInfoList;
    }

    private void PopulateSleepData()
    {
      if (this.Model == null)
        return;
      this.SetEventProperties();
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatDeep,
        Glyph = "\uE070",
        Value = (object) this.Model.TotalRestfulSleep,
        ValueType = StatValueType.DurationWithText
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatLight,
        Glyph = "\uE070",
        Value = (object) this.Model.TotalRestlessSleep,
        ValueType = StatValueType.DurationWithText
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatNumWake,
        Glyph = "\uE049",
        Value = (object) this.Model.NumberOfWakeups,
        ValueType = StatValueType.Count
      });
      TimeSpan timeSpan = !(this.Model.TotalRestfulSleep == TimeSpan.Zero) || !(this.Model.TotalRestlessSleep == TimeSpan.Zero) ? this.Model.TimeToFallAsleep : TimeSpan.Zero;
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatTimeToFallAsleep,
        Glyph = "\uE092",
        Value = (object) timeSpan,
        ValueType = StatValueType.DurationWithTextWithoutSeconds
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatEfficiency,
        Glyph = "\uE011",
        Value = (object) (int) this.Model.SleepEfficiencyPercentage,
        ValueType = StatValueType.Percentage
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelDuration,
        Glyph = "\uE025",
        Value = (object) this.Model.Duration,
        ValueType = StatValueType.DurationWithText
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatRest,
        Glyph = "\uE006",
        Value = (object) this.Model.RestingHeartRate,
        ValueType = StatValueType.Integer
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.SleepStatCals,
        Glyph = "\uE009",
        Value = (object) this.Model.CaloriesBurned,
        ValueType = StatValueType.Integer
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelSleepRestoration,
        Glyph = "\uE150",
        Value = string.IsNullOrEmpty(this.Model.SleepRestorationMsg) ? (object) AppResources.NotAvailable : (object) this.Model.SleepRestorationMsg,
        ValueType = StatValueType.SmallText
      });
    }

    public HealthCloudConnectionInfo HealthCloudConnectionInfo
    {
      get => this.healthCloudConnectionInfo;
      private set => this.SetProperty<HealthCloudConnectionInfo>(ref this.healthCloudConnectionInfo, value, nameof (HealthCloudConnectionInfo));
    }

    protected override async Task DeleteAsync()
    {
      await base.DeleteAsync();
      if (!this.Model.IsAutoDetected)
        return;
      ApplicationTelemetry.LogDeleteAutoDetectedSleep(this.HealthCloudConnectionInfo.UserId, this.Model.EventId, this.HealthCloudConnectionInfo.BaseUri.ToString());
    }

    public ICommand ReportIncorrectSleepCommand => (ICommand) this.reportIncorrectSleepCommand ?? (ICommand) (this.reportIncorrectSleepCommand = new HealthCommand((Action) (() =>
    {
      if (this.environmentService.IsPublicRelease)
        return;
      this.errorHandlingService.HandleExceptionsAsync((Func<Task>) (async () =>
      {
        if (await this.messageBoxService.ShowAsync(AppResources.ReportIncorrectSleepTimingPrompt, (string) null, PortableMessageBoxButton.CasualBooleanChoice) != PortableMessageBoxResult.OK)
          return;
        ICollection<IFile> files3 = await this.screenshotService.CaptureScreenshotsAsync(CancellationToken.None);
        IDiagnosticsService diagnosticsService = this.diagnosticsService;
        DiagnosticsUserFeedback userFeedback = new DiagnosticsUserFeedback();
        userFeedback.Text = AppResources.ReportIncorrectSleepTimingFeedbackText;
        ICollection<IFile> files4 = files3;
        await this.diagnosticsService.SendFeedbackEmailAsync(await diagnosticsService.CaptureDiagnosisPackageAsync(userFeedback, (IEnumerable<IFile>) files4, false, true), AppResources.ReportIncorrectSleepTimingFeedbackText);
        ApplicationTelemetry.LogReportAutoDetectedSleepInvalid(this.HealthCloudConnectionInfo.UserId, this.Model.EventId, this.HealthCloudConnectionInfo.BaseUri.ToString());
      })).IgnoreException("Ignore Exceptions Not Handled By the ErrorHandlingService, Reporting incorrect sleep timing failed.", nameof (ReportIncorrectSleepCommand));
    })));
  }
}
