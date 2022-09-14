// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Hike.HikeSummaryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
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
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Hike
{
  [PageTaxonomy(new string[] {"Fitness", "Hike", "Summary Map"})]
  public class HikeSummaryViewModel : RouteBasedExerciseEventSummaryViewModelBase<HikeEvent>
  {
    private const string SharePreviewImageFileName = "Hiking.png";
    private readonly ISmoothNavService smoothNavService;
    private readonly IFormattingService formattingService;
    private readonly IRouteBasedExerciseEventProvider<HikeEvent> hikeProvider;
    private HealthCommand openFullMapCommand;
    private bool? hasGps;
    private bool enableShare;

    public HikeSummaryViewModel(
      ISmoothNavService smoothNavService,
      IBestEventProvider bestEventProvider,
      IErrorHandlingService cargoExceptionUtils,
      INetworkService networkUtils,
      IHistoryProvider historyProvider,
      IMessageBoxService messageBoxService,
      IFormattingService formattingService,
      IShareService shareService,
      IHealthCloudClient healthCloudClient,
      IRouteBasedExerciseEventProvider<HikeEvent> hikeProvider,
      IMessageSender messageSender)
      : base(networkUtils, smoothNavService, cargoExceptionUtils, bestEventProvider, historyProvider, messageBoxService, healthCloudClient, shareService, formattingService, hikeProvider, messageSender, CoreColors.HikePath, true, true)
    {
      Assert.ParamIsNotNull((object) hikeProvider, "provider");
      Assert.ParamIsNotNull((object) smoothNavService, nameof (smoothNavService));
      this.smoothNavService = smoothNavService;
      this.formattingService = formattingService;
      this.hikeProvider = hikeProvider;
      this.GoalType = GoalType.HikeGoal;
      this.EventType = EventType.Hike;
      this.DeletionMessage = AppResources.HikeDeleteMessage;
    }

    public bool? HasGps
    {
      get => this.hasGps;
      set => this.SetProperty<bool?>(ref this.hasGps, value, nameof (HasGps));
    }

    public bool EnableShare
    {
      get => this.enableShare;
      set => this.SetProperty<bool>(ref this.enableShare, value, nameof (EnableShare));
    }

    protected override bool IsShareImageIncluded => true;

    private async Task<string> GetShareMessageAsync() => await Task.FromResult<string>(string.Format(AppResources.ShareHikeMessage, new object[2]
    {
      (object) Formatter.GetMonthDayString(this.Model.StartTime),
      (object) (string) this.formattingService.FormatCalories(new int?(this.Model.CaloriesBurned))
    }));

    protected override async Task<ShareRequest> CreateShareRequestAsync()
    {
      ShareRequest request = await base.CreateShareRequestAsync();
      request.EventType = this.EventType;
      request.ChooserTitle = AppResources.ChooserShareHike;
      request.Title = AppResources.ShareHikeTitle;
      request.Message = ShareMessageFormatter.FormatHikeMessage(this.Model, this.formattingService);
      request.SharePreviewImageFileName = "Hiking.png";
      ShareRequest shareRequest = request;
      ShareUrlsRequestFormData urlsRequestFormData1 = new ShareUrlsRequestFormData();
      urlsRequestFormData1.EventType = this.EventType.ToString();
      urlsRequestFormData1.EventId = this.Model.EventId;
      ShareUrlsRequestFormData urlsRequestFormData2 = urlsRequestFormData1;
      string shareMessageAsync = await this.GetShareMessageAsync();
      urlsRequestFormData2.Title = shareMessageAsync;
      urlsRequestFormData1.TwitterDescription = AppResources.ShareTwitterDescription;
      urlsRequestFormData1.ActivityName = this.Model.Name != null ? this.Model.Name : AppResources.TileTitle_Hike;
      urlsRequestFormData1.MetricLabel1 = AppResources.PanelStatisticLabelDistance;
      urlsRequestFormData1.MetricContent1 = this.formattingService.FormatDistanceMileKMValue(this.Model.TotalDistance);
      urlsRequestFormData1.MetricUnit1 = this.formattingService.FormatDistanceMileKMUnit();
      urlsRequestFormData1.MetricLabel2 = AppResources.PanelStatisticLabelCaloriesBurned;
      urlsRequestFormData1.MetricContent2 = this.formattingService.FormatCaloriesValue(this.Model.CaloriesBurned);
      urlsRequestFormData1.MetricUnit2 = this.formattingService.FormatCaloriesUnit();
      urlsRequestFormData1.MetricLabel3 = AppResources.PanelStatisticLabelElevationGain;
      urlsRequestFormData1.MetricContent3 = (string) this.formattingService.FormatElevation(new Length?(this.Model.TotalAltitudeGain));
      urlsRequestFormData1.MetricUnit3 = this.formattingService.FormatElevationUnit();
      urlsRequestFormData1.MetricLabel4 = AppResources.PanelStatisticLabelDuration;
      urlsRequestFormData1.MetricContent4 = this.formattingService.FormatDurationValue(this.Model.Duration);
      urlsRequestFormData1.MetricUnit4 = this.formattingService.FormatDurationUnit();
      shareRequest.ShareUrlsRequestFormData = urlsRequestFormData1;
      shareRequest = (ShareRequest) null;
      urlsRequestFormData2 = (ShareUrlsRequestFormData) null;
      urlsRequestFormData1 = (ShareUrlsRequestFormData) null;
      return request;
    }

    public override ICommand OpenFullMapCommand => (ICommand) this.openFullMapCommand ?? (ICommand) (this.openFullMapCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (HikeFullMapViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "ID",
        this.Id.ToString()
      },
      {
        "IsLowPowerGps",
        this.IsLowPowerGps.ToString()
      }
    }))));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.DisplayNamingTextBox = false;
      await base.LoadDataAsync(parameters);
      if (parameters != null && parameters.ContainsKey("ID"))
      {
        this.Model = await this.hikeProvider.GetEventAsync(parameters["ID"]);
      }
      else
      {
        HikeEvent lastEventAsync = await this.hikeProvider.GetLastEventAsync();
        if (lastEventAsync != null)
          this.Model = await this.hikeProvider.GetEventAsync(lastEventAsync.EventId);
      }
      if (this.Model == null)
        throw new NoDataException();
      this.Stats.Clear();
      this.AddDurationStat();
      this.AddActiveTimeStat();
      this.AddAvgAscentRateStat();
      this.AddCaloriesStat();
      this.AddElevationGainStat();
      this.AddElevationLossStat();
      this.AddHeartRateStat();
      this.AddEndingHeartRateStat();
      this.AddRecoveryTimeStat();
      this.AddFitnessBenefitStat();
      this.AddCardioMinutesStat();
      this.AddUvExposureStat();
      this.HasGps = new bool?(this.Model.GpsState == GpsState.AtLeastOnePoint);
      this.Initialized = true;
    }

    private void AddActiveTimeStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelActiveTime,
      Glyph = "\uE024",
      Value = (object) this.Model.ActiveTime,
      ValueType = StatValueType.DurationWithoutSeconds,
      SubStat1 = new SubStatViewModel()
      {
        Label = AppResources.PanelStatisticLabelRestingTime,
        Value = (object) this.Model.RestingTime,
        ValueType = SubStatValueType.DurationWithText
      }
    });

    private void AddAvgAscentRateStat() => this.Stats.Add(new StatViewModel()
    {
      Label = AppResources.PanelStatisticLabelAvgClimbRate,
      Glyph = "\uE416",
      Value = (object) this.Model.AscentSpeed,
      ValueType = StatValueType.ClimbRate,
      ShowNotAvailableOnZero = false
    });
  }
}
