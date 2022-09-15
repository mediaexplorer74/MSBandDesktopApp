// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.WorkoutDetailViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
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
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Workouts.Pre;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  [PageTaxonomy(new string[] {"Fitness", "Guided Workouts", "Plan", "Workout Detail"})]
  public class WorkoutDetailViewModel : PanelViewModelBase
  {
    private const int MinMinorImperialValueInFeet = 500;
    private const int MinMinorMetricValueInMeters = 999;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Workouts\\Panels\\WorkoutDetailViewModel.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService navigation;
    private readonly IEnvironmentService environmentService;
    private readonly IWorkoutsProvider provider;
    private readonly IUserProfileService userProfileService;
    private readonly IErrorHandlingService errorHandlingService;
    private HealthCommand browseCommand;
    private HealthCommand openPlanCommand;
    private HealthCommand uploadWorkoutToBand;
    private IList<WorkoutItemViewModel> viewList;
    private IList<WorkoutItemViewModel> treeList;
    private int dayId;
    private bool isRestDay;
    private bool isSyncedToBand;
    private bool showDownloadState = true;
    private bool showFooter;
    private bool isProcessing;
    private int weekId;
    private string workoutDetailName;
    private int workoutIndex;
    private string workoutInstructions;
    private string workoutPlanId;

    public WorkoutDetailViewModel(
      IWorkoutsProvider provider,
      ISmoothNavService navigation,
      IMessageBoxService messageBoxService,
      IUserProfileService userProfileService,
      INetworkService networkService,
      IEnvironmentService environmentService,
      IMessageSender messageSender,
      IErrorHandlingService errorHandlingService)
      : base(networkService)
    {
      this.provider = provider;
      this.navigation = navigation;
      this.messageBoxService = messageBoxService;
      this.userProfileService = userProfileService;
      this.environmentService = environmentService;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
      this.messageSender.Register<PanelRefreshMessage>((object) this, new Action<PanelRefreshMessage>(this.OnPageRefresh));
      this.ViewList = (IList<WorkoutItemViewModel>) new List<WorkoutItemViewModel>();
      this.TreeList = (IList<WorkoutItemViewModel>) new List<WorkoutItemViewModel>();
    }

    public IList<WorkoutItemViewModel> ViewList
    {
      get => this.viewList;
      private set => this.SetProperty<IList<WorkoutItemViewModel>>(ref this.viewList, value, nameof (ViewList));
    }

    public IList<WorkoutItemViewModel> TreeList
    {
      get => this.treeList;
      private set => this.SetProperty<IList<WorkoutItemViewModel>>(ref this.treeList, value, nameof (TreeList));
    }

    public string WorkoutDetailName
    {
      get => this.workoutDetailName;
      set => this.SetProperty<string>(ref this.workoutDetailName, value, nameof (WorkoutDetailName));
    }

    public bool IsRestDay
    {
      get => this.isRestDay;
      set => this.SetProperty<bool>(ref this.isRestDay, value, nameof (IsRestDay));
    }

    public bool ShowDownloadState
    {
      get => this.showDownloadState;
      set => this.SetProperty<bool>(ref this.showDownloadState, value, nameof (ShowDownloadState));
    }

    public bool ShowFooter
    {
      get => this.showFooter;
      set => this.SetProperty<bool>(ref this.showFooter, value, nameof (ShowFooter));
    }

    public bool IsSyncedToBand
    {
      get => this.isSyncedToBand;
      set => this.SetProperty<bool>(ref this.isSyncedToBand, value, nameof (IsSyncedToBand));
    }

    public bool IsProcessing
    {
      get => this.isProcessing;
      set => this.SetProperty<bool>(ref this.isProcessing, value, nameof (IsProcessing));
    }

    public string WorkoutInstructions
    {
      get => this.workoutInstructions;
      set => this.SetProperty<string>(ref this.workoutInstructions, value, nameof (WorkoutInstructions));
    }

    public ICommand BrowseCommand => (ICommand) this.browseCommand ?? (ICommand) (this.browseCommand = new HealthCommand((Action) (() => this.navigation.Navigate(typeof (WorkoutPlanLandingViewModel)))));

    public ICommand OpenPlanCommand => (ICommand) this.openPlanCommand ?? (ICommand) (this.openPlanCommand = new HealthCommand((Action) (() => this.navigation.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      ["WorkoutPlanId"] = this.workoutPlanId,
      ["Type"] = "WorkoutPlanDetail"
    }))));

    public ICommand UploadWorkoutToBandCommand => (ICommand) this.uploadWorkoutToBand ?? (ICommand) (this.uploadWorkoutToBand = new HealthCommand((Action) (async () =>
    {
      if (!this.userProfileService.IsBandRegistered)
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.WorkoutUploadBandNotRegistered, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
      }
      else
        await this.InvokeWithBlockingUIAsync((Func<Task>) (async () => await this.UploadWorkoutAsync()));
    })));

    public async void OpenExerciseVideo(ExerciseItemViewModel exercise)
    {
      if (string.IsNullOrEmpty(exercise.VideoId))
        return;
      this.LoadState = LoadState.Loading;
      PixelScreenSize pixelScreenSize = this.environmentService.PixelScreenSize;
      double height = (double) pixelScreenSize.Height;
      double width = (double) pixelScreenSize.Width;
      try
      {
        this.navigation.Navigate(typeof (VideoViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Url",
            await this.provider.GetVideoUrlAsync(exercise.VideoId, height, width)
          }
        });
        ApplicationTelemetry.LogWatchedVideo(exercise.Id, exercise.Number, exercise.VideoId);
      }
      catch (Exception ex)
      {
        WorkoutDetailViewModel.Logger.Error((object) "Could not open exercise video.", ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.LoadState = LoadState.Loaded;
    }

    public void ToggleExpanded(WorkoutItemViewModel workoutItem)
    {
      workoutItem.Expanded = !workoutItem.Expanded;
      bool expanded = workoutItem.Expanded;
      foreach (WorkoutItemViewModel child in (IEnumerable<WorkoutItemViewModel>) workoutItem.Children)
      {
        if (child.Children != null && child.Expanded)
          this.RefreshVisibleRecursive(child, expanded);
        else
          child.Visible = expanded;
      }
    }

    private void RefreshVisibleRecursive(WorkoutItemViewModel item, bool expanded)
    {
      if (item.Children != null && item.Expanded)
      {
        foreach (WorkoutItemViewModel child in (IEnumerable<WorkoutItemViewModel>) item.Children)
          this.RefreshVisibleRecursive(child, expanded);
      }
      item.Visible = expanded;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.ShowDownloadState = this.userProfileService.IsBandRegistered;
      if (parameters == null || !parameters.ContainsKey("WorkoutPlanId"))
      {
        GuidedWorkoutState workoutStateAsync = await this.provider.GetWorkoutStateAsync(CancellationToken.None);
        if (workoutStateAsync == null || workoutStateAsync.WorkoutInfo == null)
          throw new NoDataException();
        this.workoutPlanId = workoutStateAsync.WorkoutInfo.WorkoutPlanId;
        if (workoutStateAsync.State == GuidedWorkoutSyncState.RestDay)
        {
          this.IsRestDay = true;
        }
        else
        {
          this.IsRestDay = false;
          this.workoutIndex = workoutStateAsync.WorkoutInfo.WorkoutIndex;
          this.weekId = workoutStateAsync.WorkoutInfo.WeekId;
          this.dayId = workoutStateAsync.WorkoutInfo.DayId;
          await this.LoadDetailsAsync();
        }
      }
      else if (parameters.ContainsKey("IsRestDay"))
      {
        this.IsRestDay = true;
        this.workoutPlanId = parameters["WorkoutPlanId"];
      }
      else
      {
        if (!parameters.ContainsKey("WorkoutIndex") || !parameters.ContainsKey("WeekId") || !parameters.ContainsKey("DayId"))
          throw new NoDataException();
        this.IsRestDay = false;
        this.workoutPlanId = parameters["WorkoutPlanId"];
        int.TryParse(parameters["WorkoutIndex"], out this.workoutIndex);
        int.TryParse(parameters["WeekId"], out this.weekId);
        int.TryParse(parameters["DayId"], out this.dayId);
        await this.LoadDetailsAsync();
      }
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

    private async Task UploadWorkoutAsync()
    {
      bool isFavorite = false;
      int instanceId = 0;
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
          try
          {
            GuidedWorkoutInfo subscribedWorkoutAsync = await this.provider.GetSubscribedWorkoutAsync();
            if (subscribedWorkoutAsync != null && subscribedWorkoutAsync.WorkoutPlanId != this.workoutPlanId)
            {
              if (await this.messageBoxService.ShowAsync(AppResources.SyncASingleWorkoutMessage, AppResources.SyncASingleWorkoutTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
                break;
            }
            foreach (FavoriteWorkout favoriteWorkout in (IEnumerable<FavoriteWorkout>) await this.provider.GetFavoriteWorkoutsAsync(CancellationToken.None))
            {
              if (favoriteWorkout.WorkoutPlanId == this.workoutPlanId)
              {
                isFavorite = true;
                instanceId = favoriteWorkout.CurrentInstanceId;
                break;
              }
            }
            if (!isFavorite)
              await this.provider.FavoriteWorkoutAsync(this.workoutPlanId);
            await this.provider.SetNextWorkoutAsync(this.workoutPlanId, this.workoutIndex, this.weekId, this.dayId);
            await this.provider.UploadWorkoutBandFileAsync(this.workoutPlanId, this.workoutIndex, this.weekId, this.dayId, instanceId, CancellationToken.None);
            this.IsSyncedToBand = true;
            PanelRefreshMessage.Send(this.messageSender);
          }
          catch (Exception ex)
          {
            WorkoutDetailViewModel.Logger.Error((object) "Workout upload failed.", ex);
            await this.errorHandlingService.HandleExceptionAsync(ex);
          }
          break;
      }
    }

    private async Task LoadDetailsAsync()
    {
      WorkoutPlanDetail workout = await this.provider.GetWorkoutAsync(this.workoutPlanId);
      WorkoutDetailItem workoutDetail = workout.Details[this.workoutIndex];
      this.WorkoutDetailName = workoutDetail.Name;
      WorkoutStatus syncedWorkoutAsync = await this.provider.GetLastSyncedWorkoutAsync(CancellationToken.None);
      if (syncedWorkoutAsync != null && syncedWorkoutAsync.WorkoutPlanId == this.workoutPlanId && syncedWorkoutAsync.WeekId == this.weekId && syncedWorkoutAsync.Day == this.dayId && syncedWorkoutAsync.WorkoutIndex == workout.Details.IndexOf(workoutDetail))
        this.IsSyncedToBand = true;
      this.PopulateTreeList(workoutDetail.Circuit);
      this.PopulateViewListFromTreeList();
    }

    private void PopulateTreeList(IList<CircuitDetails> circuits)
    {
      Dictionary<CircuitType, List<WorkoutGroupViewModel>> blocks = new Dictionary<CircuitType, List<WorkoutGroupViewModel>>();
      foreach (CircuitDetails circuit in (IEnumerable<CircuitDetails>) circuits)
      {
        WorkoutGroupViewModel group = this.CreateGroup(circuit);
        if (!blocks.ContainsKey(group.CircuitType))
          blocks.Add(group.CircuitType, new List<WorkoutGroupViewModel>());
        blocks[group.CircuitType].Add(group);
      }
      this.TreeList = (IList<WorkoutItemViewModel>) new List<WorkoutItemViewModel>();
      this.AddBlockToTreeListIfExists(blocks, CircuitType.Warmup);
      this.AddBlockToTreeListIfExists(blocks, CircuitType.Regular);
      this.AddBlockToTreeListIfExists(blocks, CircuitType.Cooldown);
    }

    private void AddBlockToTreeListIfExists(
      Dictionary<CircuitType, List<WorkoutGroupViewModel>> blocks,
      CircuitType circuitType)
    {
      if (!blocks.ContainsKey(circuitType))
        return;
      this.TreeList.Add((WorkoutItemViewModel) this.CreateBlock((IList<WorkoutGroupViewModel>) blocks[circuitType]));
    }

    private WorkoutBlockViewModel CreateBlock(
      IList<WorkoutGroupViewModel> groups)
    {
      WorkoutBlockViewModel workoutBlockViewModel = new WorkoutBlockViewModel();
      CircuitType circuitType = groups[0].CircuitType;
      workoutBlockViewModel.Parent = this;
      workoutBlockViewModel.CircuitType = circuitType;
      workoutBlockViewModel.Text = WorkoutResultViewModel.GetCircuitTypeText(circuitType);
      foreach (IGrouping<CircuitGroupType, WorkoutGroupViewModel> source in groups.GroupBy<WorkoutGroupViewModel, CircuitGroupType>((Func<WorkoutGroupViewModel, CircuitGroupType>) (g => g.CircuitGroupType == CircuitGroupType.CircuitTime ? CircuitGroupType.CircuitTask : g.CircuitGroupType)))
      {
        switch (source.Key)
        {
          case CircuitGroupType.List:
          case CircuitGroupType.CircuitTask:
          case CircuitGroupType.CircuitTime:
          case CircuitGroupType.Interval:
            List<WorkoutGroupViewModel> list = source.ToList<WorkoutGroupViewModel>();
            if (list.Count == 1)
            {
              using (List<WorkoutGroupViewModel>.Enumerator enumerator = list.GetEnumerator())
              {
                while (enumerator.MoveNext())
                  enumerator.Current.Text = WorkoutResultViewModel.GetCircuitGroupTypeText(source.Key);
                continue;
              }
            }
            else
            {
              for (int index = 0; index < list.Count; ++index)
                list[index].Text = string.Format(AppResources.WorkoutNumberedHeaderFormat, new object[2]
                {
                  (object) WorkoutResultViewModel.GetCircuitGroupTypeText(source.Key),
                  (object) (index + 1)
                });
              continue;
            }
          case CircuitGroupType.Rest:
            using (IEnumerator<WorkoutGroupViewModel> enumerator = source.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.Text = AppResources.WorkoutRest;
              continue;
            }
          case CircuitGroupType.FreePlay:
            using (IEnumerator<WorkoutGroupViewModel> enumerator = source.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.Text = AppResources.WorkoutFree;
              continue;
            }
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      foreach (WorkoutGroupViewModel group in (IEnumerable<WorkoutGroupViewModel>) groups)
      {
        if (group.Text != null)
          group.Text = group.Text.ToUpper();
        workoutBlockViewModel.Children.Add((WorkoutItemViewModel) group);
      }
      return workoutBlockViewModel;
    }

    private WorkoutGroupViewModel CreateGroup(CircuitDetails circuit)
    {
      WorkoutGroupViewModel workoutGroupViewModel = new WorkoutGroupViewModel();
      workoutGroupViewModel.Parent = this;
      workoutGroupViewModel.CircuitGroupType = circuit.GroupType;
      workoutGroupViewModel.CircuitType = circuit.CircuitType;
      workoutGroupViewModel.Glyph = WorkoutResultViewModel.GetGroupGlyph(circuit.GroupType);
      WorkoutGroupViewModel result = workoutGroupViewModel;
      WorkoutDetailViewModel.AddInstructionToGroup(circuit, result);
      WorkoutDetailViewModel.PopulateMetricOnGroup(circuit, result);
      WorkoutDetailViewModel.AddExercisesToGroup(this, circuit, result);
      WorkoutDetailViewModel.AddDropLastRestNoteIfNeeded(circuit, result);
      return result;
    }

    private static void AddInstructionToGroup(CircuitDetails circuit, WorkoutGroupViewModel result)
    {
      switch (circuit.GroupType)
      {
        case CircuitGroupType.List:
          if (circuit.Exercises.Count <= 1)
          {
            WorkoutDetailViewModel.AddInstructionToGroup(AppResources.WorkoutSingleExerciseInstructions, result);
            break;
          }
          WorkoutDetailViewModel.AddInstructionToGroup(AppResources.WorkoutListMultipleExerciseInstructions, result);
          break;
        case CircuitGroupType.CircuitTask:
          if (circuit.CompletionValue <= 1)
          {
            WorkoutDetailViewModel.AddInstructionToGroup(AppResources.WorkoutCircuitTaskNoRepeatInstructions, result);
            break;
          }
          WorkoutDetailViewModel.AddInstructionToGroup(string.Format(AppResources.WorkoutCircuitTaskRepeatInstructions, new object[1]
          {
            (object) circuit.CompletionValue
          }), result);
          break;
        case CircuitGroupType.CircuitTime:
          if (circuit.CompletionType == CompletionType.Seconds)
          {
            WorkoutDetailViewModel.AddInstructionToGroup(string.Format(AppResources.WorkoutCircuitTimeInstructions, new object[1]
            {
              (object) (string) Formatter.FormatTimeSpan(TimeSpan.FromSeconds((double) circuit.CompletionValue), Formatter.TimeSpanFormat.Full, useLockedResource: true)
            }), result);
            break;
          }
          WorkoutDetailViewModel.Warn("CircuitTime group had completion type " + (object) circuit.CompletionType + " instead of Seconds.");
          break;
        case CircuitGroupType.Interval:
          if (circuit.Exercises.Count <= 1)
          {
            WorkoutDetailViewModel.AddInstructionToGroup(AppResources.WorkoutSingleExerciseInstructions, result);
            break;
          }
          WorkoutDetailViewModel.AddInstructionToGroup(AppResources.WorkoutIntervalsMultipleExerciseInstructions, result);
          break;
        case CircuitGroupType.Rest:
          break;
        case CircuitGroupType.FreePlay:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (circuit), "Unrecognized group type: " + (object) circuit.GroupType);
      }
    }

    private static void PopulateMetricOnGroup(CircuitDetails circuit, WorkoutGroupViewModel result)
    {
      switch (circuit.GroupType)
      {
        case CircuitGroupType.List:
          break;
        case CircuitGroupType.CircuitTask:
          if (circuit.CompletionType == CompletionType.Repetitions)
          {
            string format = circuit.CompletionValue == 1 ? AppResources.WorkoutRound : AppResources.WorkoutRounds;
            result.Metric = string.Format(format, new object[1]
            {
              (object) circuit.CompletionValue
            });
            break;
          }
          WorkoutDetailViewModel.Warn("CircuitTask group had completion type " + (object) circuit.CompletionType + " instead of Repetitions.");
          break;
        case CircuitGroupType.CircuitTime:
          if (circuit.CompletionType == CompletionType.Seconds)
          {
            TimeSpan time = TimeSpan.FromSeconds((double) circuit.CompletionValue);
            result.Metric = (string) Formatter.FormatTimeSpan(time, Formatter.TimeSpanFormat.Abbreviated, useLockedResource: true);
            break;
          }
          WorkoutDetailViewModel.Warn("CircuitTime group had completion type " + (object) circuit.CompletionType + " instead of Seconds.");
          break;
        case CircuitGroupType.Interval:
          if (circuit.Exercises.Count > 0)
          {
            int rangeUpperEnd = WorkoutDetailViewModel.GetRangeUpperEnd(circuit.Exercises[0].Sets);
            string format = rangeUpperEnd == 1 ? AppResources.WorkoutSet : AppResources.WorkoutSets;
            result.Metric = string.Format(format, new object[1]
            {
              (object) rangeUpperEnd
            });
            break;
          }
          WorkoutDetailViewModel.Warn("Circuit had no exercises.");
          break;
        case CircuitGroupType.Rest:
          if (circuit.Exercises.Count > 0)
          {
            Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise exercise = circuit.Exercises[0];
            int rangeUpperEnd = WorkoutDetailViewModel.GetRangeUpperEnd(exercise.Rest);
            if (rangeUpperEnd == 0 && exercise.ShowInterstitial)
            {
              result.Metric = AppResources.WorkoutRestAsNeeded;
              break;
            }
            TimeSpan time = TimeSpan.FromSeconds((double) rangeUpperEnd);
            result.Metric = (string) Formatter.FormatTimeSpan(time, Formatter.TimeSpanFormat.Abbreviated, useLockedResource: true);
            break;
          }
          WorkoutDetailViewModel.Warn("Circuit had no exercises.");
          break;
        case CircuitGroupType.FreePlay:
          if (circuit.Exercises.Count == 0)
          {
            WorkoutDetailViewModel.Warn("Circuit had no exercises.");
            break;
          }
          Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise exercise1 = circuit.Exercises[0];
          if (exercise1.CompletionType != CompletionType.Seconds)
          {
            WorkoutDetailViewModel.Warn("FreePlay completion type was expected to be Seconds, but was " + (object) exercise1.CompletionType);
            break;
          }
          TimeSpan time1 = TimeSpan.FromSeconds((double) exercise1.CompletionValue);
          result.Metric = (string) Formatter.FormatTimeSpan(time1, Formatter.TimeSpanFormat.Abbreviated, useLockedResource: true);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static void AddExercisesToGroup(
      WorkoutDetailViewModel parent,
      CircuitDetails circuit,
      WorkoutGroupViewModel result)
    {
      switch (circuit.GroupType)
      {
        case CircuitGroupType.List:
        case CircuitGroupType.CircuitTask:
        case CircuitGroupType.CircuitTime:
        case CircuitGroupType.Interval:
          using (IEnumerator<Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise> enumerator = circuit.Exercises.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise current = enumerator.Current;
              if ((current.Id == null ? 0 : (current.Id.ToLowerInvariant() == "ex9000" ? 1 : 0)) == 0)
              {
                ExerciseItemViewModel exerciseItemViewModel1 = new ExerciseItemViewModel();
                exerciseItemViewModel1.Parent = parent;
                exerciseItemViewModel1.Text = current.Name;
                exerciseItemViewModel1.Image = current.Image == null ? (EmbeddedOrRemoteImageSource) null : new EmbeddedOrRemoteImageSource(current.Image);
                exerciseItemViewModel1.VideoId = current.VideoId;
                exerciseItemViewModel1.Id = current.Id;
                exerciseItemViewModel1.Number = current.Number;
                ExerciseItemViewModel exerciseItemViewModel2 = exerciseItemViewModel1;
                string str1 = WorkoutDetailViewModel.GetCompletionString(current);
                if (circuit.GroupType == CircuitGroupType.List)
                {
                  int rangeUpperEnd1 = WorkoutDetailViewModel.GetRangeUpperEnd(current.Sets);
                  if (rangeUpperEnd1 > 1)
                  {
                    str1 = string.Format(AppResources.WorkoutMultipleSets, new object[2]
                    {
                      (object) rangeUpperEnd1,
                      (object) str1
                    });
                    int rangeUpperEnd2 = WorkoutDetailViewModel.GetRangeUpperEnd(current.Rest);
                    if (rangeUpperEnd2 > 0 || current.ShowInterstitial)
                    {
                      string str2 = rangeUpperEnd2 > 0 ? (string) Formatter.FormatTimeSpan(TimeSpan.FromSeconds((double) rangeUpperEnd2), Formatter.TimeSpanFormat.Abbreviated, useLockedResource: true) : AppResources.WorkoutRestAsNeeded;
                      str1 = string.Format(AppResources.WorkoutRestBetween, new object[2]
                      {
                        (object) str1,
                        (object) str2
                      });
                    }
                  }
                }
                exerciseItemViewModel2.Completion = str1;
                result.Children.Add((WorkoutItemViewModel) exerciseItemViewModel2);
              }
              WorkoutDetailViewModel.GenerateRestItemIfNeeded(parent, current, circuit.GroupType, result);
            }
            break;
          }
        case CircuitGroupType.Rest:
          break;
        case CircuitGroupType.FreePlay:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (circuit), "Unrecognized group type: " + (object) circuit.GroupType);
      }
    }

    private static void AddDropLastRestNoteIfNeeded(
      CircuitDetails circuit,
      WorkoutGroupViewModel result)
    {
      if (!circuit.DropLastRest || circuit.GroupType != CircuitGroupType.CircuitTask || result.Children.Count < 2)
        return;
      WorkoutItemViewModel child1 = result.Children[result.Children.Count - 1];
      if (!(child1.Text == AppResources.WorkoutRest))
        return;
      child1.Text = string.Format(AppResources.WorkoutFootnote, new object[1]
      {
        (object) child1.Text
      });
      ExerciseExplanationViewModel child2 = (ExerciseExplanationViewModel) result.Children[0];
      child2.Text = string.Format(AppResources.WorkoutLastRestDroppedInstructions, new object[1]
      {
        (object) child2.Text
      });
    }

    private static void GenerateRestItemIfNeeded(
      WorkoutDetailViewModel parent,
      Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise exercise,
      CircuitGroupType groupType,
      WorkoutGroupViewModel group)
    {
      if (WorkoutDetailViewModel.GetRangeUpperEnd(exercise.Sets) > 1 && groupType == CircuitGroupType.List)
        return;
      int rangeUpperEnd = WorkoutDetailViewModel.GetRangeUpperEnd(exercise.Rest);
      if (rangeUpperEnd == 0)
      {
        if (!exercise.ShowInterstitial)
          return;
        group.Children.Add((WorkoutItemViewModel) WorkoutDetailViewModel.CreateRestExercise(parent, AppResources.WorkoutRestAsNeeded));
      }
      else
      {
        string completionString = (string) Formatter.FormatTimeSpan(TimeSpan.FromSeconds((double) rangeUpperEnd), Formatter.TimeSpanFormat.Abbreviated, useLockedResource: true);
        group.Children.Add((WorkoutItemViewModel) WorkoutDetailViewModel.CreateRestExercise(parent, completionString));
      }
    }

    private static void AddInstructionToGroup(string instruction, WorkoutGroupViewModel group)
    {
      IList<WorkoutItemViewModel> children = group.Children;
      ExerciseExplanationViewModel explanationViewModel = new ExerciseExplanationViewModel();
      explanationViewModel.Text = instruction;
      children.Add((WorkoutItemViewModel) explanationViewModel);
    }

    private static ExerciseItemViewModel CreateRestExercise(
      WorkoutDetailViewModel parent,
      string completionString)
    {
      ExerciseItemViewModel exerciseItemViewModel = new ExerciseItemViewModel();
      exerciseItemViewModel.Parent = parent;
      exerciseItemViewModel.Image = new EmbeddedOrRemoteImageSource(EmbeddedAsset.GuidedWorkoutsRest);
      exerciseItemViewModel.Text = AppResources.WorkoutRest;
      exerciseItemViewModel.Completion = completionString;
      return exerciseItemViewModel;
    }

    private static int GetRangeUpperEnd(string rangeString)
    {
      int result = 0;
      if (!int.TryParse(rangeString, out result) && rangeString != null && rangeString.Contains("-"))
      {
        string[] strArray = rangeString.Split('-');
        if (strArray.Length == 2)
          int.TryParse(strArray[1], out result);
      }
      return result;
    }

    private static string GetCompletionString(Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise exercise)
    {
      CompletionType completionType = exercise.CompletionType;
      int completionValue = exercise.CompletionValue;
      switch (completionType)
      {
        case CompletionType.Seconds:
          return (string) Formatter.FormatTimeSpan(TimeSpan.FromSeconds((double) completionValue), Formatter.TimeSpanFormat.Abbreviated, useLockedResource: true);
        case CompletionType.Repetitions:
          string format;
          string workoutMax;
          string str;
          switch (completionValue)
          {
            case 1:
              str = AppResources.WorkoutRep;
              break;
            case int.MaxValue:
              format = AppResources.WorkoutReps;
              workoutMax = AppResources.WorkoutMax;
              goto label_7;
            default:
              str = AppResources.WorkoutReps;
              break;
          }
          format = str;
          workoutMax = completionValue.ToString((IFormatProvider) CultureInfo.CurrentCulture);
label_7:
          return string.Format(format, new object[1]
          {
            (object) workoutMax
          });
        case CompletionType.Meters:
          return WorkoutDetailViewModel.GetDistanceString(exercise);
        default:
          WorkoutDetailViewModel.Warn("Unsupported completion type, cannot generate completion string: " + (object) completionType);
          return string.Empty;
      }
    }

    private static string GetDistanceString(Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise exercise)
    {
      Length distance = Length.FromMeters((double) exercise.CompletionValue);
      bool flag;
      DistanceUnitType unit;
      if (exercise.IsUseCustomaryUnits)
      {
        flag = distance.TotalFeet > 500.0;
        unit = DistanceUnitType.Imperial;
      }
      else
      {
        flag = distance.TotalMeters > 999.0;
        unit = DistanceUnitType.Metric;
      }
      return (string) (flag ? Formatter.FormatDistance(distance, unit, appendUnit: true, useLockedResource: true) : Formatter.FormatDistanceMetersOrFeet(distance, unit, true, true));
    }

    private void PopulateViewListFromTreeList()
    {
      List<WorkoutItemViewModel> workoutItemViewModelList = new List<WorkoutItemViewModel>();
      foreach (WorkoutItemViewModel tree in (IEnumerable<WorkoutItemViewModel>) this.TreeList)
      {
        workoutItemViewModelList.Add(tree);
        workoutItemViewModelList.AddRange((IEnumerable<WorkoutItemViewModel>) tree.Children);
      }
      this.ViewList = (IList<WorkoutItemViewModel>) workoutItemViewModelList;
    }

    private void SetProcessing(bool value)
    {
      this.IsProcessing = value;
      this.messageSender.Send<BlockUserInteractionMessage>(new BlockUserInteractionMessage()
      {
        IsBlocked = value
      });
    }

    private static void Warn(string message)
    {
      WorkoutDetailViewModel.Logger.Warn((object) message);
      DebugUtilities.Fail(message);
    }
  }
}
