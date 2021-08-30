// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ExerciseSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
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

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Fitness", "Exercise", "Summary"})]
  public class ExerciseSummaryViewModel : ExerciseEventSummaryViewModelBase<ExerciseEvent>
  {
    private const string SharePreviewImageFileName = "Workout.png";
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IHeartRateService heartRateService;
    private readonly IBestEventProvider goalProvider;
    private readonly IExerciseProvider exerciseProvider;
    private readonly IMessageSender messageSender;
    private readonly IList<ChartDurationSeriesInfo> chartSeriesInformation = (IList<ChartDurationSeriesInfo>) new List<ChartDurationSeriesInfo>();
    private ICommand assignNameCommand;
    private bool isPersonalBest;
    private string personalBestMessage;

    public ExerciseSummaryViewModel(
      IExerciseProvider service,
      IBestEventProvider bestEventProvider,
      IShareService shareService,
      IFormattingService formattingService,
      INetworkService networkService,
      IErrorHandlingService errorHandlingService,
      IMessageBoxService messageBoxService,
      ISmoothNavService smoothNavService,
      IHistoryProvider historyProvider,
      IHealthCloudClient healthCloudClient,
      IHeartRateService heartRateService,
      IMessageSender messageSender)
      : base(networkService, smoothNavService, errorHandlingService, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, messageSender)
    {
      this.exerciseProvider = service != null ? service : throw new ArgumentNullException(nameof (service), "You must provide a valid exercise data provider to instantiate this class");
      this.goalProvider = bestEventProvider;
      this.errorHandlingService = errorHandlingService;
      this.heartRateService = heartRateService;
      this.messageSender = messageSender;
      this.GoalType = GoalType.WorkoutGoal;
      this.EventType = EventType.Workout;
      this.DeletionMessage = AppResources.ExerciseDeleteMessage;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.DisplayNamingTextBox = false;
      await base.LoadDataAsync(parameters);
      if (parameters != null && parameters.ContainsKey("ID"))
        this.Model = await this.exerciseProvider.GetExerciseEventAsync(parameters["ID"]);
      else
        this.Model = await this.exerciseProvider.GetLastExerciseEventAsync(true);
      if (this.Model == null)
        throw new NoDataException();
      KeyValuePair<bool, string> labelForEventAsync = await this.goalProvider.GetLabelForEventAsync(this.Model.EventId, EventType.Workout);
      this.IsPersonalBest = labelForEventAsync.Key;
      this.PersonalBestMessage = labelForEventAsync.Value;
      this.Name = this.Model.Name;
      this.SetEventProperties();
      this.AddDurationStat();
      this.AddCaloriesStat();
      this.AddHeartRateStat();
      this.AddEndingHeartRateStat();
      this.AddRecoveryTimeStat();
      this.AddFitnessBenefitStat();
      this.AddUvExposureStat();
      this.AddCardioMinutesStat();
      await this.PrepareDataForChartAsync();
      await this.SetHistoryIfValidAsync(parameters);
      this.RaisePropertyChanged("ChartSeriesInformation");
    }

    private async Task PrepareDataForChartAsync()
    {
      if (this.chartSeriesInformation.Any<ChartDurationSeriesInfo>())
        this.chartSeriesInformation.Clear();
      IList<DurationChartPoint> heartRateChartData = this.Model.Sequences.GetDurationHeartRateData<ExerciseEventSequence>(this.Model.Info, this.Model.StartTime);
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

    protected override bool IsShareImageIncluded => true;

    protected override async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest shareRequestAsync = await base.CreateShareRequestAsync();
      shareRequestAsync.EventType = this.EventType;
      shareRequestAsync.ChooserTitle = AppResources.ChooserShareExercise;
      shareRequestAsync.Title = AppResources.ShareWorkoutTitle;
      shareRequestAsync.Message = ShareMessageFormatter.FormatExerciseMessage(this.Model);
      shareRequestAsync.SharePreviewImageFileName = "Workout.png";
      string str = string.IsNullOrEmpty(this.Name) ? (this.Model.Tags == null || this.Model.Tags.Count <= 0 || string.IsNullOrEmpty(this.Model.Tags[0].Text) ? AppResources.TileTitle_Exercise : this.Model.Tags[0].Text) : this.Name;
      shareRequestAsync.ShareUrlsRequestFormData = new ShareUrlsRequestFormData()
      {
        EventType = this.EventType.ToString(),
        EventId = this.Model.EventId,
        Title = ShareMessageFormatter.FormatExerciseMessage(this.Model),
        MetricLabel1 = AppResources.PanelStatisticLabelCaloriesBurned,
        MetricContent1 = this.FormattingService.FormatCaloriesValue(this.Model.CaloriesBurned),
        MetricUnit1 = this.FormattingService.FormatCaloriesUnit(),
        MetricLabel2 = AppResources.PanelStatisticLabelDuration,
        MetricContent2 = this.FormattingService.FormatDurationValue(this.Model.Duration),
        MetricUnit2 = this.FormattingService.FormatDurationUnit(),
        MetricLabel3 = AppResources.ShareMetricLabelAverageHeartRate,
        MetricContent3 = this.FormattingService.FormatHeartRateValue(this.Model.AverageHeartRate),
        MetricUnit3 = this.FormattingService.FormatHeartRateUnit(),
        MetricLabel4 = AppResources.PanelStatisticLabelFitnessBenefit,
        MetricContent4 = this.FormattingService.FormatFitnessBenefitValue(this.Model.FitnessBenefitMsg),
        TwitterDescription = AppResources.ShareTwitterDescription,
        ActivityName = str
      };
      return shareRequestAsync;
    }

    public override async Task AssignNameAsync()
    {
      try
      {
        this.IsBeingEdited = true;
        await this.exerciseProvider.PatchExerciseEventAsync(this.Model.EventId, this.Name);
        this.DisplayNamingTextBox = false;
        this.messageSender.Send<EventChangedMessage>(new EventChangedMessage()
        {
          Event = (UserEvent) this.Model,
          Operation = EventOperation.Rename,
          Target = EventType.Workout,
          IsRefreshCanceled = !this.HasHistory
        });
      }
      catch (Exception ex)
      {
        EventSummaryViewModelBase<ExerciseEvent>.Logger.Error(ex, "Exception encountered during naming.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.IsBeingEdited = false;
      }
    }

    public IList<ChartDurationSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public string PersonalBestMessage
    {
      get => this.personalBestMessage;
      set => this.SetProperty<string>(ref this.personalBestMessage, value, nameof (PersonalBestMessage));
    }

    public bool IsPersonalBest
    {
      get => this.isPersonalBest;
      set => this.SetProperty<bool>(ref this.isPersonalBest, value, nameof (IsPersonalBest));
    }
  }
}
