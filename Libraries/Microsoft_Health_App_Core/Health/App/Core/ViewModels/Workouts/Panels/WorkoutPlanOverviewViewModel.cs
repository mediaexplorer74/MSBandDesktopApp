// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.WorkoutPlanOverviewViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  [PageTaxonomy(new string[] {"Fitness", "Guided Workouts", "Plan", "Summary"})]
  public class WorkoutPlanOverviewViewModel : PanelViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Workouts\\Panels\\WorkoutPlanOverviewViewModel.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navigation;
    private readonly IWorkoutsProvider provider;
    private readonly IUserProfileService userProfileService;
    private readonly IErrorHandlingService errorHandlingService;
    private HealthCommand browseCommand;
    private HealthCommand toggleFavoriteCommand;
    private bool isFavorite;
    private bool isProcessing;
    private bool isSingleWorkout;
    private bool isSubscribed;
    private bool isSyncedToBand;
    private bool showActions;
    private bool showFavoriteButton;
    private HealthCommand subscribeCommand;
    private int subscribedInstanceId;
    private string subscribedWorkoutPlanId;
    private HealthCommand undoSubscribeCommand;
    private HealthCommand uploadWorkoutToBand;
    private WorkoutPlanDetailViewModel workoutPlan;
    private string workoutPlanDescription;
    private string workoutPlanId;

    public WorkoutPlanOverviewViewModel(
      IWorkoutsProvider provider,
      IMessageBoxService messageBoxService,
      ISmoothNavService navigation,
      IUserProfileService userProfileService,
      INetworkService networkService,
      IMessageSender messageSender,
      IErrorHandlingService errorHandlingService)
      : base(networkService)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof (provider), "You must provide a valid exercise data provider to instantiate this class");
      if (messageBoxService == null)
        throw new ArgumentNullException(nameof (messageBoxService), "You must provide a valid message box provider to instantiate this class");
      this.provider = provider;
      this.messageBoxService = messageBoxService;
      this.navigation = navigation;
      this.userProfileService = userProfileService;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
      this.messageSender.Register<PanelRefreshMessage>((object) this, new Action<PanelRefreshMessage>(this.OnPageRefresh));
    }

    public bool IsSyncedToBand
    {
      get => this.isSyncedToBand;
      set => this.SetProperty<bool>(ref this.isSyncedToBand, value, nameof (IsSyncedToBand));
    }

    public bool IsSingleWorkout
    {
      get => this.isSingleWorkout;
      set => this.SetProperty<bool>(ref this.isSingleWorkout, value, nameof (IsSingleWorkout));
    }

    public bool IsProcessing
    {
      get => this.isProcessing;
      set => this.SetProperty<bool>(ref this.isProcessing, value, nameof (IsProcessing));
    }

    public bool IsFavorite
    {
      get => this.isFavorite;
      set => this.SetProperty<bool>(ref this.isFavorite, value, nameof (IsFavorite));
    }

    public bool IsSubscribed
    {
      get => this.isSubscribed;
      set => this.SetProperty<bool>(ref this.isSubscribed, value, nameof (IsSubscribed));
    }

    public bool ShowFavoriteButton
    {
      get => this.showFavoriteButton;
      set => this.SetProperty<bool>(ref this.showFavoriteButton, value, nameof (ShowFavoriteButton));
    }

    public string SubscribedWorkoutPlanId
    {
      get => this.subscribedWorkoutPlanId;
      set => this.SetProperty<string>(ref this.subscribedWorkoutPlanId, value, nameof (SubscribedWorkoutPlanId));
    }

    public int SubscribedInstanceId
    {
      get => this.subscribedInstanceId;
      set => this.SetProperty<int>(ref this.subscribedInstanceId, value, nameof (SubscribedInstanceId));
    }

    public WorkoutPlanDetailViewModel WorkoutPlan
    {
      get => this.workoutPlan;
      set => this.SetProperty<WorkoutPlanDetailViewModel>(ref this.workoutPlan, value, nameof (WorkoutPlan));
    }

    public string WorkoutPlanDescription
    {
      get => this.workoutPlanDescription;
      set => this.SetProperty<string>(ref this.workoutPlanDescription, value, nameof (WorkoutPlanDescription));
    }

    public bool ShowActions
    {
      get => this.showActions;
      set => this.SetProperty<bool>(ref this.showActions, value, nameof (ShowActions));
    }

    public ICommand BrowseCommand => (ICommand) this.browseCommand ?? (ICommand) (this.browseCommand = new HealthCommand(new Action(this.BrowseWorkouts)));

    public ICommand ToggleFavoriteCommand => (ICommand) this.toggleFavoriteCommand ?? (ICommand) (this.toggleFavoriteCommand = new HealthCommand(new Action(this.ToggleFavorite)));

    public ICommand SubscribeCommand => (ICommand) this.subscribeCommand ?? (ICommand) (this.subscribeCommand = new HealthCommand(new Action(this.Subscribe)));

    private async void Subscribe()
    {
      if (!this.userProfileService.IsBandRegistered)
      {
        int num1 = (int) await this.messageBoxService.ShowAsync(AppResources.WorkoutSubscribeBandNotRegistered, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
      }
      else
        await this.InvokeWithBlockingUIAsync((Func<Task>) (async () =>
        {
          Microsoft.Health.App.Core.Models.GuidedWorkoutTileState state = await this.provider.GetGuidedWorkoutTileStateAsync();
          switch (state)
          {
            case Microsoft.Health.App.Core.Models.GuidedWorkoutTileState.Enabled:
              int num3 = await this.SubscribeAsync();
              PanelRefreshMessage.Send(this.messageSender);
              break;
            case Microsoft.Health.App.Core.Models.GuidedWorkoutTileState.Disabled:
              if (await this.messageBoxService.ShowAsync(AppResources.WorkoutNavigateManageTiles, AppResources.ManageTilesLabel, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
                break;
              this.navigation.Navigate(typeof (ManageTilesViewModel));
              break;
          }
        }));
    }

    public ICommand UnsubscribeCommand => (ICommand) this.undoSubscribeCommand ?? (ICommand) (this.undoSubscribeCommand = new HealthCommand(new Action(this.Unsubscribe)));

    private async void Unsubscribe() => await this.InvokeWithBlockingUIAsync((Func<Task>) (async () =>
    {
      if (!await this.UnsubscribeAsync())
        return;
      PanelRefreshMessage.Send(this.messageSender);
    }));

    public ICommand UploadWorkoutToBandCommand => (ICommand) this.uploadWorkoutToBand ?? (ICommand) (this.uploadWorkoutToBand = new HealthCommand(new Action(this.DoUploadWorkoutToBand)));

    private async void DoUploadWorkoutToBand()
    {
      if (!this.userProfileService.IsBandRegistered)
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.WorkoutUploadBandNotRegistered, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
      }
      else
        await this.InvokeWithBlockingUIAsync((Func<Task>) (async () => await this.UploadSingleWorkoutAsync()));
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

    public void BrowseWorkouts() => this.navigation.Navigate(typeof (WorkoutPlanLandingViewModel));

    public void ToggleFavorite()
    {
      if (this.isFavorite)
        this.UnFavorite();
      else
        this.Favorite();
    }

    private async void Favorite()
    {
      this.LoadState = LoadState.Loading;
      try
      {
        await this.provider.FavoriteWorkoutAsync(this.WorkoutPlan.WorkoutPlanId);
        this.IsFavorite = true;
      }
      catch (Exception ex)
      {
        WorkoutPlanOverviewViewModel.Logger.Error(ex, "Workout favoriting failed.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.LoadState = LoadState.Loaded;
    }

    private async void UnFavorite()
    {
      this.LoadState = LoadState.Loading;
      try
      {
        await this.provider.UnFavoriteWorkoutAsync(this.WorkoutPlan.WorkoutPlanId);
        this.IsFavorite = false;
      }
      catch (Exception ex)
      {
        WorkoutPlanOverviewViewModel.Logger.Error(ex, "Unfavorite failed.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.LoadState = LoadState.Loaded;
    }

    public async Task<int> SubscribeAsync()
    {
      GuidedWorkoutSyncMode syncMode = GuidedWorkoutSyncMode.SyncPlan;
      Exception ex = (Exception) null;
      try
      {
        if (!string.IsNullOrWhiteSpace(this.SubscribedWorkoutPlanId))
        {
          WorkoutPlanOverviewViewModel.Logger.Debug("<FLAG> Unsubscirbing from previously subscribed {0}", (object) this.SubscribedWorkoutPlanId);
          if (await this.messageBoxService.ShowAsync(AppResources.WorkoutAlreadySubscribedMessage, AppResources.WorkoutAlreadySubscribedTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.Cancel)
            return 0;
          syncMode = GuidedWorkoutSyncMode.ReplacePlan;
          await this.provider.UnsubscribeWorkoutAsync(this.SubscribedWorkoutPlanId);
          this.SubscribedWorkoutPlanId = (string) null;
        }
        if (!this.IsFavorite)
        {
          WorkoutPlanOverviewViewModel.Logger.Debug("<FLAG> Favorite the workout {0}", (object) this.WorkoutPlan.WorkoutPlanId);
          await this.provider.FavoriteWorkoutAsync(this.WorkoutPlan.WorkoutPlanId);
          this.IsFavorite = true;
        }
        await this.provider.SubscribeWorkoutAsync(this.WorkoutPlan.WorkoutPlanId);
        foreach (FavoriteWorkout favoriteWorkout in (IEnumerable<FavoriteWorkout>) await this.provider.GetFavoriteWorkoutsAsync(CancellationToken.None))
        {
          if (favoriteWorkout.WorkoutPlanId == this.WorkoutPlan.WorkoutPlanId)
          {
            this.SubscribedInstanceId = favoriteWorkout.CurrentInstanceId;
            break;
          }
        }
        WorkoutPlanOverviewViewModel.Logger.Debug((object) "<START> Uploading first workout to band");
        bool flag;
        try
        {
          await this.provider.UploadWorkoutBandFileAsync(this.WorkoutPlan.WorkoutPlanId, 0, 1, 1, this.SubscribedInstanceId, CancellationToken.None, syncMode);
          flag = true;
        }
        catch (Exception ex1)
        {
          ex = ex1;
          flag = false;
        }
        if (!flag)
          await this.provider.UnsubscribeWorkoutAsync(this.WorkoutPlan.WorkoutPlanId);
      }
      catch (Exception ex2)
      {
        ex = ex2;
      }
      if (ex != null)
      {
        WorkoutPlanOverviewViewModel.Logger.Error(ex, "Workout subscribe failed.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        return 0;
      }
      WorkoutPlanOverviewViewModel.Logger.Debug((object) "<END> Uploading first workout to band");
      WorkoutPlanOverviewViewModel.Logger.Debug("<END> Subscribing to {0}", (object) this.WorkoutPlan.WorkoutPlanId);
      this.IsSubscribed = true;
      this.ShowFavoriteButton = false;
      return this.SubscribedInstanceId;
    }

    public async Task<bool> UnsubscribeAsync()
    {
      try
      {
        await this.provider.UnsubscribeWorkoutAsync(this.WorkoutPlan.WorkoutPlanId);
        this.IsSubscribed = false;
        this.SubscribedInstanceId = 0;
        this.SubscribedWorkoutPlanId = (string) null;
      }
      catch (Exception ex)
      {
        WorkoutPlanOverviewViewModel.Logger.Error(ex, "Unsubscribe failed.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        return false;
      }
      return true;
    }

    public async Task UploadSingleWorkoutAsync()
    {
      Microsoft.Health.App.Core.Models.GuidedWorkoutTileState state = await this.provider.GetGuidedWorkoutTileStateAsync();
      switch (state)
      {
        case Microsoft.Health.App.Core.Models.GuidedWorkoutTileState.Unknown:
          break;
        case Microsoft.Health.App.Core.Models.GuidedWorkoutTileState.Disabled:
          if (await this.messageBoxService.ShowAsync(AppResources.WorkoutNavigateManageTiles, AppResources.ManageTilesLabel, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
            break;
          this.navigation.Navigate(typeof (ManageTilesViewModel));
          break;
        default:
          if (!this.IsSubscribed && !string.IsNullOrWhiteSpace(this.subscribedWorkoutPlanId))
          {
            if (await this.messageBoxService.ShowAsync(AppResources.SyncASingleWorkoutMessage, AppResources.SyncASingleWorkoutTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
              break;
          }
          try
          {
            if (!this.IsFavorite)
              await this.provider.FavoriteWorkoutAsync(this.workoutPlanId);
            await this.provider.SetNextWorkoutAsync(this.workoutPlanId, 0, 1, 1);
            await this.provider.UploadWorkoutBandFileAsync(this.workoutPlanId, 0, 1, 1, 0, CancellationToken.None);
            this.IsSyncedToBand = true;
            PanelRefreshMessage.Send(this.messageSender);
          }
          catch (Exception ex)
          {
            WorkoutPlanOverviewViewModel.Logger.Error(ex, "Workout upload failed.");
            await this.errorHandlingService.HandleExceptionAsync(ex);
          }
          break;
      }
    }

    private async Task SetFavoriteStateAsync(string workoutPlanId)
    {
      this.ShowFavoriteButton = true;
      IList<FavoriteWorkout> favoriteWorkoutsAsync = await this.provider.GetFavoriteWorkoutsAsync(CancellationToken.None);
      if (favoriteWorkoutsAsync == null)
        throw new HealthCloudServerException("favorites can not be null.");
      foreach (FavoriteWorkout favoriteWorkout in (IEnumerable<FavoriteWorkout>) favoriteWorkoutsAsync)
      {
        if (favoriteWorkout.IsSubscribed)
          this.SubscribedWorkoutPlanId = favoriteWorkout.WorkoutPlanId;
        if (favoriteWorkout.WorkoutPlanId == workoutPlanId)
        {
          this.IsFavorite = true;
          this.IsSubscribed = favoriteWorkout.IsSubscribed;
          this.ShowFavoriteButton = !favoriteWorkout.IsSubscribed;
          if (favoriteWorkout.IsSubscribed)
            this.SubscribedInstanceId = favoriteWorkout.CurrentInstanceId;
        }
      }
      if (!this.ShowFavoriteButton)
        return;
      WorkoutStatus syncedWorkoutAsync = await this.provider.GetLastSyncedWorkoutAsync(CancellationToken.None);
      if (syncedWorkoutAsync == null)
        return;
      this.ShowFavoriteButton = syncedWorkoutAsync.WorkoutPlanId != workoutPlanId;
    }

    private async Task LoadWorkoutAsync(string workoutPlanId)
    {
      this.WorkoutPlan = !string.IsNullOrWhiteSpace(workoutPlanId) ? new WorkoutPlanDetailViewModel(await this.provider.GetWorkoutAsync(workoutPlanId) ?? throw new HealthCloudServerException("Workout can not be null.")) : throw new NoDataException();
      this.WorkoutPlanDescription = WebUtility.HtmlDecode((this.WorkoutPlan.Description ?? string.Empty).Replace("<p>", string.Empty).Replace("</p>", Environment.NewLine + Environment.NewLine).Replace("<br>", Environment.NewLine));
      if (this.WorkoutPlan.Weeks.Count == 1 && this.WorkoutPlan.Weeks[0].Days.Count == 1)
      {
        this.ShowActions = this.userProfileService.IsRegisteredBandPaired;
        this.IsSingleWorkout = true;
        WorkoutStatus syncedWorkoutAsync = await this.provider.GetLastSyncedWorkoutAsync(CancellationToken.None);
        if (syncedWorkoutAsync != null && syncedWorkoutAsync.WorkoutPlanId != null && syncedWorkoutAsync.WorkoutPlanId == this.WorkoutPlan.WorkoutPlanId && syncedWorkoutAsync.WorkoutIndex == 0 && syncedWorkoutAsync.WeekId == 1 && syncedWorkoutAsync.Day == 1)
          this.IsSyncedToBand = true;
      }
      else if (this.workoutPlan.Weeks.Count > 0 && this.workoutPlan.Weeks[0].Days.Count > 0)
        this.ShowActions = this.workoutPlan.Weeks[0].Days[0].DayId == 1 && this.userProfileService.IsRegisteredBandPaired;
      await this.SetFavoriteStateAsync(this.WorkoutPlan.WorkoutPlanId);
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.workoutPlanId = parameters != null && parameters.ContainsKey("WorkoutPlanId") ? parameters["WorkoutPlanId"] : throw new NoDataException();
      await this.LoadWorkoutAsync(this.workoutPlanId);
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
