// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.WorkoutPlanScheduleViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  [PageTaxonomy(new string[] {"Fitness", "Guided Workouts", "Plan", "Schedule"})]
  public class WorkoutPlanScheduleViewModel : PanelViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Workouts\\Panels\\WorkoutPlanScheduleViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IFileObjectStorageService isoObjectStore;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navigation;
    private readonly IWorkoutsProvider provider;
    private readonly IUserProfileService userProfileService;
    private bool isFavorite;
    private bool isProcessing;
    private bool isSubscribed;
    private int subscribedInstanceId;
    private string subscribedWorkoutPlanId;
    private HealthCommand<WorkoutPlanScheduleWeekGroupingViewModel> toggleExpansion;
    private HealthCommand<WorkoutPlanScheduleDayViewModel> viewExercisesCommand;
    private IList<WorkoutPlanScheduleWeekGroupingViewModel> weekGrouping;
    private string workoutPlanId;
    private string workoutPlanName;

    public bool IsProcessing
    {
      get => this.isProcessing;
      set => this.SetProperty<bool>(ref this.isProcessing, value, nameof (IsProcessing));
    }

    public bool IsSubscribed
    {
      get => this.isSubscribed;
      set => this.SetProperty<bool>(ref this.isSubscribed, value, nameof (IsSubscribed));
    }

    public bool IsFavorite
    {
      get => this.isFavorite;
      set => this.SetProperty<bool>(ref this.isFavorite, value, nameof (IsFavorite));
    }

    public IList<WorkoutPlanScheduleWeekGroupingViewModel> WeekGrouping
    {
      get => this.weekGrouping;
      set => this.SetProperty<IList<WorkoutPlanScheduleWeekGroupingViewModel>>(ref this.weekGrouping, value, nameof (WeekGrouping));
    }

    public bool ShowBandItems => this.userProfileService.IsRegisteredBandPaired;

    public ICommand ViewExercisesCommand => (ICommand) this.viewExercisesCommand ?? (ICommand) (this.viewExercisesCommand = new HealthCommand<WorkoutPlanScheduleDayViewModel>((Action<WorkoutPlanScheduleDayViewModel>) (day => this.ViewWorkoutExercises(day.GetRawModel()))));

    public ICommand ToggleExpansionCommand => (ICommand) this.toggleExpansion ?? (ICommand) (this.toggleExpansion = new HealthCommand<WorkoutPlanScheduleWeekGroupingViewModel>((Action<WorkoutPlanScheduleWeekGroupingViewModel>) (group => group.ToggleExpansion())));

    public WorkoutPlanScheduleViewModel(
      IWorkoutsProvider provider,
      IMessageBoxService messageBoxService,
      IFileObjectStorageService isoObjectStore,
      ISmoothNavService navigation,
      IUserProfileService userProfileService,
      IErrorHandlingService errorHandlingService,
      INetworkService networkService,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.provider = provider;
      this.messageBoxService = messageBoxService;
      this.isoObjectStore = isoObjectStore;
      this.navigation = navigation;
      this.userProfileService = userProfileService;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
      this.LoadState = LoadState.Loading;
      this.messageSender.Register<PanelRefreshMessage>((object) this, new Action<PanelRefreshMessage>(this.OnPageRefresh));
    }

    private async Task InvokeWithBlockingUIAsync(Func<Task> func)
    {
      this.SetProcessing(true);
      try
      {
        await func();
      }
      finally
      {
        this.SetProcessing(false);
      }
    }

    private void OnPageRefresh(PanelRefreshMessage message) => this.Refresh();

    private void ViewWorkoutExercises(WorkoutScheduleDay day)
    {
      if (day == null || day.IsRestDay)
        return;
      this.navigation.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "WorkoutPlanId",
          this.workoutPlanId
        },
        {
          "WorkoutPlanName",
          this.workoutPlanName
        },
        {
          "WorkoutIndex",
          day.WorkoutIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "WorkoutDetailName",
          day.PlanValues.Workouts[0]
        },
        {
          "WeekId",
          day.WeekId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "DayId",
          day.PlanValues.DayId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "Type",
          "WorkoutDetail"
        }
      });
    }

    public async void CheckSyncStatus(WorkoutScheduleDay day)
    {
      if (!this.userProfileService.IsBandRegistered)
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.WorkoutUploadBandNotRegistered, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
      }
      else if (!this.IsSubscribed && !string.IsNullOrWhiteSpace(this.subscribedWorkoutPlanId))
      {
        if (await this.messageBoxService.ShowAsync(AppResources.SyncASingleWorkoutMessage, AppResources.SyncASingleWorkoutTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
          return;
        await this.SyncToDeviceAsync(day, GuidedWorkoutSyncMode.SyncOutOfPlan);
      }
      else
        await this.SyncToDeviceAsync(day, GuidedWorkoutSyncMode.SyncPlan);
    }

    private async Task SkipPreviousWorkoutsAsync(WorkoutScheduleDay syncedDay)
    {
      bool doneSkipping = false;
      foreach (WorkoutPlanScheduleWeekGroupingViewModel groupingViewModel in (IEnumerable<WorkoutPlanScheduleWeekGroupingViewModel>) this.WeekGrouping)
      {
        foreach (WorkoutPlanScheduleDayViewModel scheduleDayViewModel in groupingViewModel)
        {
          WorkoutPlanScheduleDayViewModel day = scheduleDayViewModel;
          WorkoutScheduleDay model = day.GetRawModel();
          if (syncedDay.WeekId == model.WeekId && syncedDay.PlanValues.DayId == model.PlanValues.DayId)
          {
            doneSkipping = true;
            break;
          }
          if (!day.IsRestDay && model.State == WorkoutDayState.ReadyToSync)
          {
            await this.provider.SkipWorkoutAsync(this.workoutPlanId, model.WorkoutIndex, model.WeekId, model.PlanValues.DayId);
            model.State = WorkoutDayState.Skipped;
          }
          else if (day.IsRestDay)
            model.State = WorkoutDayState.Completed;
          model = (WorkoutScheduleDay) null;
          day = (WorkoutPlanScheduleDayViewModel) null;
        }
        if (doneSkipping)
          break;
      }
    }

    private async Task SyncToDeviceAsync(WorkoutScheduleDay day, GuidedWorkoutSyncMode mode) => await this.InvokeWithBlockingUIAsync((Func<Task>) (async () =>
    {
      Microsoft.Health.App.Core.Models.GuidedWorkoutTileState state = await this.provider.GetGuidedWorkoutTileStateAsync();
      switch (state)
      {
        case Microsoft.Health.App.Core.Models.GuidedWorkoutTileState.Unknown:
          this.SetProcessing(false);
          break;
        case Microsoft.Health.App.Core.Models.GuidedWorkoutTileState.Disabled:
          this.SetProcessing(false);
          if (await this.messageBoxService.ShowAsync(AppResources.WorkoutNavigateManageTiles, AppResources.ManageTilesLabel, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
            break;
          this.navigation.Navigate(typeof (ManageTilesViewModel));
          break;
        default:
          try
          {
            if (!this.IsSubscribed && !this.IsFavorite)
            {
              await this.provider.FavoriteWorkoutAsync(this.workoutPlanId);
              this.IsFavorite = true;
            }
            await this.provider.SetNextWorkoutAsync(this.workoutPlanId, day.WorkoutIndex, day.WeekId, day.PlanValues.DayId);
            await this.provider.UploadWorkoutBandFileAsync(this.workoutPlanId, day.WorkoutIndex, day.WeekId, day.PlanValues.DayId, this.subscribedInstanceId, CancellationToken.None, mode);
            if (this.IsSubscribed)
              await this.SkipPreviousWorkoutsAsync(day);
            PanelRefreshMessage.Send(this.messageSender);
          }
          catch (Exception ex)
          {
            WorkoutPlanScheduleViewModel.Logger.Error((object) "Syncing workout file to device failed.", ex);
            await this.errorHandlingService.HandleExceptionAsync(ex);
          }
          break;
      }
    }));

    private async Task SetFavoriteStateAsync(string workoutPlanId)
    {
      foreach (FavoriteWorkout favoriteWorkout in (IEnumerable<FavoriteWorkout>) await this.provider.GetFavoriteWorkoutsAsync(CancellationToken.None))
      {
        if (favoriteWorkout.IsSubscribed)
          this.subscribedWorkoutPlanId = favoriteWorkout.WorkoutPlanId;
        if (favoriteWorkout.WorkoutPlanId == workoutPlanId)
        {
          this.IsFavorite = true;
          this.IsSubscribed = favoriteWorkout.IsSubscribed;
          if (favoriteWorkout.IsSubscribed)
            this.subscribedInstanceId = favoriteWorkout.CurrentInstanceId;
        }
      }
    }

    private async Task<IList<WorkoutWeekGrouping>> LoadWorkoutsAsync(
      string workoutPlanId,
      int subscribedInstanceId)
    {
      IList<WorkoutWeekGrouping> workoutWeekGroupingList = await this.provider.GetScheduleAsync(workoutPlanId, subscribedInstanceId, this.IsSubscribed).ConfigureAwait(false);
      if (workoutWeekGroupingList.Count > 0)
        this.workoutPlanName = workoutWeekGroupingList[0].PlanName;
      return workoutWeekGroupingList;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.workoutPlanId = parameters.ContainsKey("WorkoutPlanId") ? parameters["WorkoutPlanId"] : throw new NoDataException();
      await this.SetFavoriteStateAsync(this.workoutPlanId);
      if (parameters.ContainsKey("Instance"))
        this.subscribedInstanceId = int.Parse(parameters["Instance"]);
      this.WeekGrouping = (IList<WorkoutPlanScheduleWeekGroupingViewModel>) new ObservableCollection<WorkoutPlanScheduleWeekGroupingViewModel>((await this.LoadWorkoutsAsync(this.workoutPlanId, this.subscribedInstanceId)).Select<WorkoutWeekGrouping, WorkoutPlanScheduleWeekGroupingViewModel>((Func<WorkoutWeekGrouping, WorkoutPlanScheduleWeekGroupingViewModel>) (s => new WorkoutPlanScheduleWeekGroupingViewModel(this, s))));
    }

    private void SetProcessing(bool value)
    {
      this.IsProcessing = value;
      this.messageSender.Send<BlockUserInteractionMessage>(new BlockUserInteractionMessage()
      {
        IsBlocked = value
      });
    }
  }
}
