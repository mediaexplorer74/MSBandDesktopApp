// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.SendFeedback;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Events.Golf;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Browse map"})]
  public class GolfSummaryViewModel : EventSummaryViewModelBase<GolfEvent>, IRenameableEventViewModel
  {
    private const string SharePreviewImageFileName = "Golf.png";
    private readonly ISmoothNavService smoothNavService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IFormattingService formattingService;
    private readonly IGolfRoundProvider golfRoundProvider;
    private readonly IReportProblemStore reportProblemStore;
    private readonly IPagePicker pagePicker;
    private readonly IMessageSender messageSender;
    private readonly IList<ChartPartitionedEventSeriesInfo> chartSeriesInformation = (IList<ChartPartitionedEventSeriesInfo>) new List<ChartPartitionedEventSeriesInfo>();
    private GolfRound round;
    private bool displayNamingTextBox;
    private string name;
    private ICommand renameGolfCommand;
    private ICommand assignNameCommand;
    private ICommand clearNameCommand;
    private ICommand findAGolfCourseCommand;
    private ICommand reportProblemCommand;
    private ActionViewModel renameAction;

    public event EventHandler RenameRequested;

    public GolfSummaryViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IBestEventProvider bestEventProvider,
      IHistoryProvider historyProvider,
      IFormattingService formattingService,
      IMessageBoxService messageBoxService,
      IHealthCloudClient healthCloudClient,
      IShareService shareService,
      IGolfRoundProvider golfRoundProvider,
      IPagePicker pagePicker,
      IReportProblemStore reportProblemStore,
      IMessageSender messageSender)
      : base(networkService, smoothNavService, errorHandlingService, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, messageSender)
    {
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.formattingService = formattingService;
      this.golfRoundProvider = golfRoundProvider;
      this.reportProblemStore = reportProblemStore;
      this.messageSender = messageSender;
      this.pagePicker = pagePicker;
      this.EventType = EventType.Golf;
      this.DeletionMessage = AppResources.GolfDeleteMessage;
      this.CanHaveBests = false;
    }

    protected override IList<ActionViewModel> InitializeActions()
    {
      this.renameAction = new ActionViewModel(string.Empty, this.RenameGolfCommand);
      this.InvalidateRenameText();
      IList<ActionViewModel> actionViewModelList = base.InitializeActions();
      actionViewModelList.Add(this.renameAction);
      actionViewModelList.Add(new ActionViewModel(AppResources.GolfSummaryFindAGolfCourseButton, this.FindAGolfCourseCommand));
      actionViewModelList.Add(new ActionViewModel(AppResources.GolfSummarySendFeedbackButtonText, this.ReportProblemCommand));
      return actionViewModelList;
    }

    private GolfRound Round
    {
      get => this.round;
      set
      {
        this.round = value;
        this.Model = this.round != null ? this.round.GetRawEvent() : (GolfEvent) null;
      }
    }

    public bool DisplayNamingTextBox
    {
      get => this.displayNamingTextBox;
      private set => this.SetProperty<bool>(ref this.displayNamingTextBox, value, nameof (DisplayNamingTextBox));
    }

    public string Name
    {
      get => this.name;
      set
      {
        this.SetProperty<string>(ref this.name, value, nameof (Name));
        this.InvalidateRenameText();
      }
    }

    private void InvalidateRenameText()
    {
      if (this.renameAction == null)
        return;
      this.renameAction.Text = string.IsNullOrEmpty(this.Name) ? AppResources.PanelNameButtonText : AppResources.PanelRenameButtonText;
    }

    public IList<ChartPartitionedEventSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    protected override bool IsShareImageIncluded => true;

    private async Task<string> GetShareMessageAsync() => await Task.FromResult<string>(string.Format(AppResources.ShareGolfMessage, new object[3]
    {
      (object) Formatter.GetMonthDayString(this.Model.StartTime),
      (object) this.formattingService.FormatGolfScore(this.Round.TotalScore, this.Round.TotalDifferenceFromPar),
      (object) this.Round.CourseName
    }));

    protected override async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest request = await base.CreateShareRequestAsync();
      request.EventType = this.EventType;
      request.ChooserTitle = AppResources.ChooserShareGolf;
      request.Title = AppResources.ShareGolfTitle;
      request.Message = ShareMessageFormatter.FormatGolfMessage(this.Model, this.Round, this.formattingService);
      request.SharePreviewImageFileName = "Golf.png";
      ShareRequest shareRequest = request;
      ShareUrlsRequestFormData urlsRequestFormData1 = new ShareUrlsRequestFormData();
      urlsRequestFormData1.EventType = this.EventType.ToString();
      urlsRequestFormData1.EventId = this.Model.EventId;
      ShareUrlsRequestFormData urlsRequestFormData2 = urlsRequestFormData1;
      string shareMessageAsync = await this.GetShareMessageAsync();
      urlsRequestFormData2.Title = shareMessageAsync;
      urlsRequestFormData1.MetricLabel1 = AppResources.ShareMetricLabelGolfScore;
      ShareUrlsRequestFormData urlsRequestFormData3 = urlsRequestFormData1;
      int num = this.Round.TotalScore;
      string str1 = num.ToString();
      urlsRequestFormData3.MetricContent1 = str1;
      urlsRequestFormData1.MetricUnit1 = string.Format("({0}{1})", new object[2]
      {
        this.Round.TotalDifferenceFromPar >= 0 ? (object) "+" : (object) string.Empty,
        (object) this.Round.TotalDifferenceFromPar
      });
      urlsRequestFormData1.MetricLabel2 = AppResources.PanelStatisticLabelParOrBetter;
      urlsRequestFormData1.MetricContent2 = this.FormattingService.FormatCountValue(this.Round.ParOrBetterCount);
      urlsRequestFormData1.MetricUnit2 = this.FormattingService.FormatCountUnit();
      urlsRequestFormData1.MetricLabel3 = AppResources.PanelStatisticLabelSteps;
      ShareUrlsRequestFormData urlsRequestFormData4 = urlsRequestFormData1;
      num = this.Model.TotalStepCount;
      string str2 = num.ToString();
      urlsRequestFormData4.MetricContent3 = str2;
      urlsRequestFormData1.MetricUnit3 = (string) null;
      urlsRequestFormData1.MetricLabel4 = AppResources.PanelStatisticLabelLongestDrive;
      urlsRequestFormData1.MetricContent4 = this.FormattingService.FormatDistanceYardMeterValue(this.Model.LongestDrive);
      urlsRequestFormData1.MetricUnit4 = this.FormattingService.FormatDistanceYardMeterUnit();
      urlsRequestFormData1.TwitterDescription = AppResources.ShareTwitterDescription;
      urlsRequestFormData1.ActivityName = string.IsNullOrEmpty(this.Name) ? AppResources.TileTitle_Golf : this.Name;
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
      if (parameters != null && parameters.ContainsKey("ID"))
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          this.Round = await this.golfRoundProvider.GetRoundAsync(parameters["ID"], cancellationTokenSource.Token);
      }
      else
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          this.Round = await this.golfRoundProvider.GetLastRoundAsync(cancellationTokenSource.Token);
          if (this.Round == null)
            throw new NoDataException("failed to get the most recent golf event");
        }
      }
      if (this.Round == null)
        throw new NoDataException();
      this.SetEventProperties();
      this.Name = this.Round.Name;
      this.LoadStats();
      this.AddUvExposureStat();
      this.PrepareDataForChart();
      await this.SetHistoryIfValidAsync(parameters);
      this.RaisePropertyChanged("ChartSeriesInformation");
    }

    private void LoadStats()
    {
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelLongestDrive,
        Glyph = "\uE159",
        Value = (object) this.Round.LongestDrive,
        ValueType = StatValueType.MinorDistance
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelParOrBetter,
        Glyph = "\uE027",
        Value = (object) this.Round.ParOrBetterCount,
        ValueType = StatValueType.Count
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.TrackerDistance,
        Glyph = "\uE030",
        Value = (object) this.Round.TotalDistance,
        ValueType = StatValueType.DistanceShort
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelPaceOfPlay,
        Glyph = "\uE048",
        Value = (object) this.Round.PaceOfPlay,
        ValueType = StatValueType.DurationWithText
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelSteps,
        Glyph = "\uE008",
        Value = (object) this.Round.TotalSteps,
        ValueType = StatValueType.Integer,
        ShowNotAvailableOnZero = false
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelCaloriesBurned,
        Glyph = "\uE009",
        Value = (object) this.Model.CaloriesBurned,
        ValueType = StatValueType.Integer
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelAverageHeartRate,
        Glyph = "\uE006",
        Value = (object) this.Round.AverageHeartRate,
        ValueType = StatValueType.Integer,
        SubStat1 = new SubStatViewModel()
        {
          Label = AppResources.PanelStatisticLabelPeakHeartRate,
          Value = (object) this.Round.PeakHeartRate,
          ValueType = SubStatValueType.Integer
        },
        SubStat2 = new SubStatViewModel()
        {
          Label = AppResources.PanelStatisticLabelLowestHeartRate,
          Value = (object) this.Round.LowestHeartRate,
          ValueType = SubStatValueType.Integer
        }
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.PanelStatisticLabelDuration,
        Glyph = "\uE025",
        Value = (object) this.Model.Duration,
        ValueType = StatValueType.DurationWithText,
        SubStat1 = new SubStatViewModel()
        {
          Label = AppResources.PanelStatisticLabelStartTime,
          Value = (object) this.Model.StartTime,
          ValueType = SubStatValueType.Time
        }
      });
    }

    private void PrepareDataForChart()
    {
      this.chartSeriesInformation.Clear();
      if (this.Round.Holes.Count <= 0)
        return;
      if (this.Round.Holes.Any<GolfRoundHole>((Func<GolfRoundHole, bool>) (p => p.DifferenceFromPar.HasValue)))
      {
        ChartPartitionedEventSeriesInfo partitionedEventSeriesInfo = new ChartPartitionedEventSeriesInfo();
        partitionedEventSeriesInfo.Name = "Par";
        partitionedEventSeriesInfo.SeriesType = ChartSeriesType.Golf;
        partitionedEventSeriesInfo.SeriesData = ChartUtilities.GetGolfScoreData(this.Round);
        partitionedEventSeriesInfo.Selected = true;
        this.chartSeriesInformation.Add(partitionedEventSeriesInfo);
      }
      if (!this.Round.Holes.Any<GolfRoundHole>((Func<GolfRoundHole, bool>) (p =>
      {
        if (!p.DifferenceFromPar.HasValue || !p.PeakHeartRate.HasValue)
          return false;
        int? peakHeartRate = p.PeakHeartRate;
        int num = 0;
        return peakHeartRate.GetValueOrDefault() > num && peakHeartRate.HasValue;
      })))
        return;
      ChartPartitionedEventSeriesInfo partitionedEventSeriesInfo1 = new ChartPartitionedEventSeriesInfo();
      partitionedEventSeriesInfo1.Name = "HeartRate";
      partitionedEventSeriesInfo1.SeriesType = ChartSeriesType.GolfHeartRate;
      partitionedEventSeriesInfo1.SeriesData = ChartUtilities.GetGolfHeartRateData(this.Round);
      partitionedEventSeriesInfo1.SubLabel = AppResources.PanelStatisticLabelMaxHeartRate;
      this.chartSeriesInformation.Add(partitionedEventSeriesInfo1);
    }

    public ICommand RenameGolfCommand => this.renameGolfCommand ?? (this.renameGolfCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.DisplayNamingTextBox = true;
      if (this.RenameRequested == null)
        return;
      this.RenameRequested((object) this, EventArgs.Empty);
    })));

    public ICommand AssignNameCommand => this.assignNameCommand ?? (this.assignNameCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      try
      {
        this.IsBeingEdited = true;
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          await this.golfRoundProvider.RenameRoundAsync(this.Model.EventId, this.Name, cancellationTokenSource.Token);
          this.DisplayNamingTextBox = false;
          this.messageSender.Send<EventChangedMessage>(new EventChangedMessage()
          {
            Event = (UserEvent) this.Model,
            Operation = EventOperation.Rename,
            Target = EventType.Golf,
            IsRefreshCanceled = !this.HasHistory
          });
        }
      }
      catch (Exception ex)
      {
        EventSummaryViewModelBase<GolfEvent>.Logger.Error(ex, "Exception encountered during naming.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.IsBeingEdited = false;
      }
    })));

    public ICommand ClearNameCommand => this.clearNameCommand ?? (this.clearNameCommand = (ICommand) new HealthCommand((Action) (() => this.Name = string.Empty)));

    public ICommand FindAGolfCourseCommand => this.findAGolfCourseCommand ?? (this.findAGolfCourseCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogGolfFindCourse("Post");
      this.smoothNavService.Navigate(typeof (GolfLandingViewModel));
    })));

    public ICommand ReportProblemCommand => this.reportProblemCommand ?? (this.reportProblemCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.reportProblemStore.Title = AppResources.GolfSummarySendFeedbackButtonText;
      this.reportProblemStore.UserFeedback = new DiagnosticsUserFeedback()
      {
        Category = new DiagnosticsCategory?(DiagnosticsCategory.Golf),
        Subcategory = new DiagnosticsSubcategory?(DiagnosticsSubcategory.EventProblem),
        Properties = (object) new DiagnosticsEventProperties()
        {
          EventId = this.Model.EventId.ToString()
        }
      };
      this.smoothNavService.Navigate(typeof (ReportProblemViewModel));
    })));
  }
}
