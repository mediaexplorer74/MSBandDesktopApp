// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.WorkoutResultViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Workouts.Post;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  [PageTaxonomy(new string[] {"Fitness", "Guided Workouts", "Completed Workout", "Reps"})]
  public class WorkoutResultViewModel : PanelViewModelBase
  {
    private readonly IWorkoutsProvider provider;
    private readonly IFormattingService formattingService;
    private IList<WorkoutResultItemViewModel> viewList;
    private IList<WorkoutResultItemViewModel> treeList;
    private WorkoutResultData totalResultData;
    private WorkoutResultTotalViewModel totalResultViewModel;
    private bool eventSupportsEstimatedReps;
    private LabeledItem<WorkoutResultMetric> selectedResultMetric;
    private IList<LabeledItem<WorkoutResultMetric>> workoutResultMetricTypes;

    public WorkoutResultViewModel(
      IWorkoutsProvider provider,
      INetworkService networkService,
      IFormattingService formattingService)
      : base(networkService)
    {
      this.provider = provider;
      this.formattingService = formattingService;
    }

    public IList<WorkoutResultItemViewModel> ViewList
    {
      get => this.viewList;
      private set => this.SetProperty<IList<WorkoutResultItemViewModel>>(ref this.viewList, value, nameof (ViewList));
    }

    public IList<WorkoutResultItemViewModel> TreeList
    {
      get => this.treeList;
      private set => this.SetProperty<IList<WorkoutResultItemViewModel>>(ref this.treeList, value, nameof (TreeList));
    }

    public LabeledItem<WorkoutResultMetric> SelectedResultMetric
    {
      get => this.selectedResultMetric;
      set
      {
        ApplicationTelemetry.LogWorkoutMetricToggle(value.Label.Substring(0, 1) + value.Label.Substring(1).ToLower());
        this.SetProperty<LabeledItem<WorkoutResultMetric>>(ref this.selectedResultMetric, value, nameof (SelectedResultMetric));
        this.RefreshResults();
        this.RefreshEstimatedRepsFootnote();
        this.RefreshCalculatedDistanceFootnote();
      }
    }

    public IList<LabeledItem<WorkoutResultMetric>> WorkoutResultMetricTypes
    {
      get => this.workoutResultMetricTypes;
      private set => this.SetProperty<IList<LabeledItem<WorkoutResultMetric>>>(ref this.workoutResultMetricTypes, value, nameof (WorkoutResultMetricTypes));
    }

    public void ToggleExpanded(WorkoutResultItemViewModel workoutItem)
    {
      workoutItem.Expanded = !workoutItem.Expanded;
      bool expanded = workoutItem.Expanded;
      foreach (WorkoutResultItemViewModel child in (IEnumerable<WorkoutResultItemViewModel>) workoutItem.Children)
      {
        if (child.Children != null && child.Expanded)
          this.RefreshVisibleRecursive(child, expanded);
        else
          child.Visible = expanded;
      }
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      WorkoutEvent eventDetails;
      if (parameters != null && parameters.ContainsKey("ID"))
        eventDetails = await this.provider.GetWorkoutEventAsync(parameters["ID"]);
      else
        eventDetails = await this.provider.GetLastCompletedWorkoutAsync();
      if (eventDetails == null || !this.LoadWorkoutResults(eventDetails))
        throw new NoDataException();
    }

    private bool LoadWorkoutResults(WorkoutEvent eventDetails)
    {
      if (eventDetails.Sequences == null || eventDetails.Sequences.Count == 0)
        return false;
      this.eventSupportsEstimatedReps = eventDetails.IsSupportedCounting;
      this.PopulateTreeList((IEnumerable<WorkoutEventSequence>) eventDetails.Sequences);
      this.PopulateTotalResultData(eventDetails);
      this.PopulateViewListFromTreeList();
      this.PopulateResultMetricList();
      this.RefreshResults();
      this.RefreshEstimatedRepsFootnote();
      this.RefreshCalculatedDistanceFootnote();
      return true;
    }

    private void PopulateResultMetricList()
    {
      List<LabeledItem<WorkoutResultMetric>> labeledItemList = new List<LabeledItem<WorkoutResultMetric>>()
      {
        LabeledItem<WorkoutResultMetric>.FromEnumValue(WorkoutResultMetric.Time),
        LabeledItem<WorkoutResultMetric>.FromEnumValue(WorkoutResultMetric.Calories)
      };
      if (this.totalResultData.Pace > Speed.Zero)
        labeledItemList.Add(LabeledItem<WorkoutResultMetric>.FromEnumValue(WorkoutResultMetric.Pace));
      if (this.totalResultData.Reps > 0 || this.totalResultData.RepsAreEstimated)
        labeledItemList.Add(LabeledItem<WorkoutResultMetric>.FromEnumValue(WorkoutResultMetric.Reps));
      if (this.totalResultData.Distance > Length.Zero || this.totalResultData.CalculatedDistance > Length.Zero)
        labeledItemList.Add(LabeledItem<WorkoutResultMetric>.FromEnumValue(WorkoutResultMetric.Distance));
      this.WorkoutResultMetricTypes = (IList<LabeledItem<WorkoutResultMetric>>) labeledItemList;
      this.selectedResultMetric = this.WorkoutResultMetricTypes[0];
    }

    private void PopulateTreeList(IEnumerable<WorkoutEventSequence> sequences)
    {
      IEnumerable<List<WorkoutEventSequence>> workoutEventSequenceLists = WorkoutResultViewModel.GroupSequences(sequences);
      Dictionary<CircuitType, List<WorkoutResultGroupViewModel>> dictionary = new Dictionary<CircuitType, List<WorkoutResultGroupViewModel>>();
      foreach (IList<WorkoutEventSequence> sequences1 in workoutEventSequenceLists)
      {
        WorkoutResultGroupViewModel group = this.CreateGroup(sequences1);
        if (!dictionary.ContainsKey(group.CircuitType))
          dictionary.Add(group.CircuitType, new List<WorkoutResultGroupViewModel>());
        dictionary[group.CircuitType].Add(group);
      }
      this.TreeList = (IList<WorkoutResultItemViewModel>) new List<WorkoutResultItemViewModel>();
      this.AddBlockToTreeListIfExists((IReadOnlyDictionary<CircuitType, List<WorkoutResultGroupViewModel>>) dictionary, CircuitType.Warmup);
      this.AddBlockToTreeListIfExists((IReadOnlyDictionary<CircuitType, List<WorkoutResultGroupViewModel>>) dictionary, CircuitType.Regular);
      this.AddBlockToTreeListIfExists((IReadOnlyDictionary<CircuitType, List<WorkoutResultGroupViewModel>>) dictionary, CircuitType.Cooldown);
    }

    private static IEnumerable<List<WorkoutEventSequence>> GroupSequences(
      IEnumerable<WorkoutEventSequence> sequences)
    {
      return (IEnumerable<List<WorkoutEventSequence>>) sequences.GroupBy<WorkoutEventSequence, int>((Func<WorkoutEventSequence, int>) (s => s.CircuitOrdinal)).Select<IGrouping<int, WorkoutEventSequence>, List<WorkoutEventSequence>>((Func<IGrouping<int, WorkoutEventSequence>, List<WorkoutEventSequence>>) (group => group.ToList<WorkoutEventSequence>())).ToList<List<WorkoutEventSequence>>();
    }

    private WorkoutResultBlockViewModel CreateBlock(
      IList<WorkoutResultGroupViewModel> groups)
    {
      WorkoutResultBlockViewModel resultBlockViewModel = new WorkoutResultBlockViewModel();
      CircuitType circuitType = groups[0].CircuitType;
      resultBlockViewModel.Parent = this;
      resultBlockViewModel.CircuitType = circuitType;
      resultBlockViewModel.Text = WorkoutResultViewModel.GetCircuitTypeText(circuitType);
      foreach (IGrouping<CircuitGroupType, WorkoutResultGroupViewModel> source in groups.GroupBy<WorkoutResultGroupViewModel, CircuitGroupType>((Func<WorkoutResultGroupViewModel, CircuitGroupType>) (g => g.CircuitGroupType == CircuitGroupType.CircuitTime ? CircuitGroupType.CircuitTask : g.CircuitGroupType)))
      {
        switch (source.Key)
        {
          case CircuitGroupType.List:
          case CircuitGroupType.CircuitTask:
          case CircuitGroupType.CircuitTime:
          case CircuitGroupType.Interval:
            List<WorkoutResultGroupViewModel> list = source.ToList<WorkoutResultGroupViewModel>();
            if (list.Count == 1)
            {
              using (List<WorkoutResultGroupViewModel>.Enumerator enumerator = list.GetEnumerator())
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
            using (IEnumerator<WorkoutResultGroupViewModel> enumerator = source.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.Text = AppResources.WorkoutRest;
              continue;
            }
          case CircuitGroupType.FreePlay:
            using (IEnumerator<WorkoutResultGroupViewModel> enumerator = source.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.Text = AppResources.WorkoutFree;
              continue;
            }
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      foreach (WorkoutResultGroupViewModel group in (IEnumerable<WorkoutResultGroupViewModel>) groups)
      {
        if (group.Text != null)
          group.Text = group.Text.ToUpper();
        resultBlockViewModel.Children.Add((WorkoutResultItemViewModel) group);
      }
      resultBlockViewModel.RollUpResultData();
      return resultBlockViewModel;
    }

    internal static string GetCircuitGroupTypeText(CircuitGroupType circuitGroupType)
    {
      switch (circuitGroupType)
      {
        case CircuitGroupType.List:
          return AppResources.WorkoutList;
        case CircuitGroupType.CircuitTask:
        case CircuitGroupType.CircuitTime:
          return AppResources.WorkoutCircuit;
        case CircuitGroupType.Interval:
          return AppResources.WorkoutIntervals;
        default:
          throw new ArgumentOutOfRangeException(nameof (circuitGroupType));
      }
    }

    internal static string GetCircuitTypeText(CircuitType circuitType)
    {
      switch (circuitType)
      {
        case CircuitType.Regular:
          return AppResources.WorkoutBlockWorkout;
        case CircuitType.Cooldown:
          return AppResources.WorkoutBlockCooldown;
        case CircuitType.Warmup:
          return AppResources.WorkoutBlockWarmup;
        default:
          throw new ArgumentOutOfRangeException(nameof (circuitType));
      }
    }

    private void AddBlockToTreeListIfExists(
      IReadOnlyDictionary<CircuitType, List<WorkoutResultGroupViewModel>> blocks,
      CircuitType circuitType)
    {
      if (!blocks.ContainsKey(circuitType))
        return;
      this.TreeList.Add((WorkoutResultItemViewModel) this.CreateBlock((IList<WorkoutResultGroupViewModel>) blocks[circuitType]));
    }

    private WorkoutResultGroupViewModel CreateGroup(
      IList<WorkoutEventSequence> sequences)
    {
      WorkoutResultGroupViewModel resultGroupViewModel = new WorkoutResultGroupViewModel();
      resultGroupViewModel.Parent = this;
      CircuitGroupType circuitGroupType = sequences[0].CircuitGroupType;
      resultGroupViewModel.CircuitGroupType = circuitGroupType;
      resultGroupViewModel.CircuitType = sequences[0].CircuitType;
      switch (circuitGroupType)
      {
        case CircuitGroupType.List:
          using (IEnumerator<WorkoutResultItemViewModel> enumerator = this.GetListGroupChildren(sequences).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              WorkoutResultItemViewModel current = enumerator.Current;
              resultGroupViewModel.Children.Add(current);
            }
            goto case CircuitGroupType.Rest;
          }
        case CircuitGroupType.CircuitTask:
        case CircuitGroupType.CircuitTime:
          using (IEnumerator<WorkoutResultItemViewModel> enumerator = this.GetCircuitGroupChildren((IEnumerable<WorkoutEventSequence>) sequences).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              WorkoutResultItemViewModel current = enumerator.Current;
              resultGroupViewModel.Children.Add(current);
            }
            goto case CircuitGroupType.Rest;
          }
        case CircuitGroupType.Interval:
          using (IEnumerator<WorkoutResultItemViewModel> enumerator = this.GetIntervalGroupChildren(sequences).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              WorkoutResultItemViewModel current = enumerator.Current;
              resultGroupViewModel.Children.Add(current);
            }
            goto case CircuitGroupType.Rest;
          }
        case CircuitGroupType.Rest:
        case CircuitGroupType.FreePlay:
          resultGroupViewModel.Glyph = WorkoutResultViewModel.GetGroupGlyph(circuitGroupType);
          if (circuitGroupType == CircuitGroupType.Rest || circuitGroupType == CircuitGroupType.FreePlay)
            resultGroupViewModel.ResultData = this.CreateResultData(sequences[0]);
          else
            resultGroupViewModel.RollUpResultData();
          return resultGroupViewModel;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    internal static string GetGroupGlyph(CircuitGroupType groupType)
    {
      switch (groupType)
      {
        case CircuitGroupType.List:
          return "\uE161";
        case CircuitGroupType.CircuitTask:
        case CircuitGroupType.CircuitTime:
          return "\uE162";
        case CircuitGroupType.Interval:
          return "\uE163";
        case CircuitGroupType.Rest:
          return "\uE164";
        case CircuitGroupType.FreePlay:
          return "\uE165";
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private IEnumerable<WorkoutResultItemViewModel> GetListGroupChildren(
      IList<WorkoutEventSequence> sequences)
    {
      List<WorkoutResultItemViewModel> resultItemViewModelList = new List<WorkoutResultItemViewModel>();
      bool flag = sequences.Max<WorkoutEventSequence>((Func<WorkoutEventSequence, int>) (s => s.ExercisePosition)) == 0;
      int num1 = 0;
      while (num1 < sequences.Count)
      {
        WorkoutEventSequence sequence = sequences[num1];
        if (sequence.IsRest | flag)
        {
          resultItemViewModelList.Add((WorkoutResultItemViewModel) this.CreateExerciseItem(sequence));
          ++num1;
        }
        else
        {
          int exercisePosition = sequence.ExercisePosition;
          int num2 = num1;
          for (int index = num1 + 1; index < sequences.Count; ++index)
          {
            if (!sequences[index].IsRest)
            {
              if (sequences[index].ExercisePosition == exercisePosition)
                num2 = index;
              else
                break;
            }
          }
          if (num1 == num2)
          {
            resultItemViewModelList.Add((WorkoutResultItemViewModel) this.CreateExerciseItem(sequence));
            ++num1;
          }
          else
          {
            List<WorkoutEventSequence> list = sequences.Skip<WorkoutEventSequence>(num1).Take<WorkoutEventSequence>(num2 - num1 + 1).ToList<WorkoutEventSequence>();
            resultItemViewModelList.Add((WorkoutResultItemViewModel) this.CreateExerciseSubgroup((IList<WorkoutEventSequence>) list));
            num1 = num2 + 1;
          }
        }
      }
      return (IEnumerable<WorkoutResultItemViewModel>) resultItemViewModelList;
    }

    private IEnumerable<WorkoutResultItemViewModel> GetCircuitGroupChildren(
      IEnumerable<WorkoutEventSequence> sequences)
    {
      List<WorkoutResultItemViewModel> resultItemViewModelList = new List<WorkoutResultItemViewModel>();
      foreach (List<WorkoutEventSequence> workoutEventSequenceList in sequences.GroupBy<WorkoutEventSequence, int>((Func<WorkoutEventSequence, int>) (s => s.RoundOrdinal)).Select<IGrouping<int, WorkoutEventSequence>, List<WorkoutEventSequence>>((Func<IGrouping<int, WorkoutEventSequence>, List<WorkoutEventSequence>>) (group => group.ToList<WorkoutEventSequence>())).ToList<List<WorkoutEventSequence>>())
      {
        string name = string.Format(AppResources.WorkoutNumberedHeaderFormat, new object[2]
        {
          (object) AppResources.WorkoutResultRound,
          (object) workoutEventSequenceList[0].RoundOrdinal
        });
        WorkoutResultSubgroupViewModel subgroup = this.CreateSubgroup((IEnumerable<WorkoutEventSequence>) workoutEventSequenceList, name);
        resultItemViewModelList.Add((WorkoutResultItemViewModel) subgroup);
      }
      return (IEnumerable<WorkoutResultItemViewModel>) resultItemViewModelList;
    }

    private IEnumerable<WorkoutResultItemViewModel> GetIntervalGroupChildren(
      IList<WorkoutEventSequence> sequences)
    {
      List<WorkoutResultItemViewModel> resultItemViewModelList = new List<WorkoutResultItemViewModel>();
      if (sequences[0].SetOrdinal == 0)
      {
        resultItemViewModelList.AddRange((IEnumerable<WorkoutResultItemViewModel>) sequences.Select<WorkoutEventSequence, ExerciseResultItemViewModel>(new Func<WorkoutEventSequence, ExerciseResultItemViewModel>(this.CreateExerciseItem)));
        return (IEnumerable<WorkoutResultItemViewModel>) resultItemViewModelList;
      }
      int setOrdinal = sequences[0].SetOrdinal;
      List<WorkoutEventSequence> workoutEventSequenceList = new List<WorkoutEventSequence>();
      foreach (WorkoutEventSequence sequence in (IEnumerable<WorkoutEventSequence>) sequences)
      {
        if (!sequence.IsRest && sequence.SetOrdinal != setOrdinal)
        {
          string name = string.Format(AppResources.WorkoutNumberedHeaderFormat, new object[2]
          {
            (object) AppResources.WorkoutResultSet,
            (object) setOrdinal
          });
          resultItemViewModelList.Add((WorkoutResultItemViewModel) this.CreateSubgroup((IEnumerable<WorkoutEventSequence>) workoutEventSequenceList, name));
          setOrdinal = sequence.SetOrdinal;
          workoutEventSequenceList = new List<WorkoutEventSequence>();
        }
        workoutEventSequenceList.Add(sequence);
      }
      string name1 = string.Format(AppResources.WorkoutNumberedHeaderFormat, new object[2]
      {
        (object) AppResources.WorkoutResultSet,
        (object) setOrdinal
      });
      resultItemViewModelList.Add((WorkoutResultItemViewModel) this.CreateSubgroup((IEnumerable<WorkoutEventSequence>) workoutEventSequenceList, name1));
      return (IEnumerable<WorkoutResultItemViewModel>) resultItemViewModelList;
    }

    private WorkoutResultSubgroupViewModel CreateSubgroup(
      IEnumerable<WorkoutEventSequence> sequences,
      string name)
    {
      WorkoutResultSubgroupViewModel subgroupViewModel1 = new WorkoutResultSubgroupViewModel();
      subgroupViewModel1.Parent = this;
      subgroupViewModel1.Text = name;
      WorkoutResultSubgroupViewModel subgroupViewModel2 = subgroupViewModel1;
      foreach (WorkoutEventSequence sequence in sequences)
        subgroupViewModel2.Children.Add((WorkoutResultItemViewModel) this.CreateExerciseItem(sequence));
      subgroupViewModel2.RollUpResultData();
      return subgroupViewModel2;
    }

    private WorkoutResultSubgroupViewModel CreateExerciseSubgroup(
      IList<WorkoutEventSequence> sequences)
    {
      WorkoutResultSubgroupViewModel subgroupViewModel1 = new WorkoutResultSubgroupViewModel();
      subgroupViewModel1.Text = sequences[0].Name;
      WorkoutResultSubgroupViewModel subgroupViewModel2 = subgroupViewModel1;
      int num = 1;
      foreach (WorkoutEventSequence sequence in (IEnumerable<WorkoutEventSequence>) sequences)
      {
        ExerciseResultItemViewModel exerciseItem = this.CreateExerciseItem(sequence);
        if (!sequence.IsRest)
        {
          exerciseItem.Text = string.Format(AppResources.WorkoutNumberedHeaderFormat, new object[2]
          {
            (object) AppResources.WorkoutResultSet,
            (object) num
          });
          ++num;
        }
        subgroupViewModel2.Children.Add((WorkoutResultItemViewModel) exerciseItem);
      }
      subgroupViewModel2.RollUpResultData();
      return subgroupViewModel2;
    }

    private ExerciseResultItemViewModel CreateExerciseItem(
      WorkoutEventSequence sequence)
    {
      string str = sequence.IsRest ? AppResources.WorkoutRest : sequence.Name;
      ExerciseResultItemViewModel resultItemViewModel = new ExerciseResultItemViewModel();
      resultItemViewModel.Text = str;
      resultItemViewModel.ResultData = this.CreateResultData(sequence);
      return resultItemViewModel;
    }

    private WorkoutResultData CreateResultData(WorkoutEventSequence sequence)
    {
      int num;
      bool flag;
      if (sequence.CompletionType == CompletionType.Repetitions)
      {
        num = sequence.CompletionValue == int.MaxValue ? 0 : sequence.CompletionValue;
        flag = false;
      }
      else if (sequence.CompletionType == CompletionType.Seconds && this.eventSupportsEstimatedReps)
      {
        num = sequence.CompletionValue == int.MaxValue ? 0 : sequence.ComputedCompletionValue;
        flag = true;
      }
      else
      {
        num = 0;
        flag = false;
      }
      Length length = sequence.CompletionType == CompletionType.Meters ? Length.FromMeters((double) sequence.CompletionValue) : Length.Zero;
      return new WorkoutResultData()
      {
        Time = sequence.Duration,
        Calories = sequence.CaloriesBurned,
        Reps = num,
        RepsAreEstimated = flag,
        Distance = length,
        Pace = sequence.Pace,
        CalculatedDistance = sequence.DistanceInCM
      };
    }

    private void PopulateViewListFromTreeList()
    {
      List<WorkoutResultItemViewModel> resultItemViewModelList = new List<WorkoutResultItemViewModel>();
      foreach (WorkoutResultItemViewModel tree in (IEnumerable<WorkoutResultItemViewModel>) this.TreeList)
      {
        resultItemViewModelList.Add(tree);
        resultItemViewModelList.AddRange((IEnumerable<WorkoutResultItemViewModel>) tree.Children);
      }
      this.totalResultViewModel = new WorkoutResultTotalViewModel();
      resultItemViewModelList.Add((WorkoutResultItemViewModel) this.totalResultViewModel);
      this.ViewList = (IList<WorkoutResultItemViewModel>) resultItemViewModelList;
    }

    private void PopulateTotalResultData(WorkoutEvent eventDetails)
    {
      this.totalResultData = new WorkoutResultData();
      this.totalResultData.Time = eventDetails.Duration;
      this.totalResultData.Calories = eventDetails.CaloriesBurned;
      this.totalResultData.CalculatedDistance = eventDetails.TotalDistanceInCM;
      this.totalResultData.Pace = eventDetails.AvgPace;
      foreach (WorkoutResultItemViewModel tree in (IEnumerable<WorkoutResultItemViewModel>) this.TreeList)
      {
        this.totalResultData.Reps += tree.ResultData.Reps;
        this.totalResultData.Distance += tree.ResultData.Distance;
        if (tree.ResultData.RepsAreEstimated)
          this.totalResultData.RepsAreEstimated = true;
      }
    }

    private void RefreshResults()
    {
      foreach (WorkoutResultItemViewModel tree in (IEnumerable<WorkoutResultItemViewModel>) this.TreeList)
        this.RefreshResultsRecursive(tree);
      if (this.SelectedResultMetric.Value.Equals((object) WorkoutResultMetric.Distance))
      {
        string metric = (string) null;
        string calculatedDistanceMetric = (string) null;
        this.FormatDistanceHeader(this.totalResultData, ref metric, ref calculatedDistanceMetric);
        this.totalResultViewModel.Metric = metric;
        this.totalResultViewModel.CalculatedDistanceMetric = calculatedDistanceMetric;
      }
      else
      {
        this.totalResultViewModel.Metric = this.FormatResultData(this.totalResultData, this.SelectedResultMetric.Value);
        this.totalResultViewModel.CalculatedDistanceMetric = string.Empty;
      }
    }

    private void RefreshResultsRecursive(WorkoutResultItemViewModel item)
    {
      if (item.Children != null)
      {
        foreach (WorkoutResultItemViewModel child in (IEnumerable<WorkoutResultItemViewModel>) item.Children)
          this.RefreshResultsRecursive(child);
      }
      item.CalculatedDistanceMetric = string.Empty;
      if (this.SelectedResultMetric.Value.Equals((object) WorkoutResultMetric.Distance))
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        this.FormatDistanceHeader(item.ResultData, ref empty1, ref empty2);
        item.Metric = empty1;
        item.CalculatedDistanceMetric = empty2;
      }
      else
        item.Metric = this.FormatResultData(item.ResultData, this.SelectedResultMetric.Value);
    }

    private void FormatDistanceHeader(
      WorkoutResultData resultData,
      ref string metric,
      ref string calculatedDistanceMetric)
    {
      if (resultData.Distance == Length.Zero && resultData.CalculatedDistance == Length.Zero)
        metric = AppResources.NotAvailable;
      else if (resultData.Distance == Length.Zero)
      {
        calculatedDistanceMetric = (string) this.formattingService.FormatDistanceDynamic(new Length?(resultData.CalculatedDistance), true) + "*";
      }
      else
      {
        metric = (string) this.formattingService.FormatDistanceDynamic(new Length?(resultData.Distance), true);
        if (!(resultData.CalculatedDistance != Length.Zero))
          return;
        calculatedDistanceMetric = (string) this.formattingService.FormatDistanceDynamic(new Length?(resultData.CalculatedDistance)) + "* / ";
      }
    }

    private string FormatResultData(WorkoutResultData resultData, WorkoutResultMetric metric)
    {
      switch (metric)
      {
        case WorkoutResultMetric.Time:
          return (string) Formatter.FormatTimeSpanNoText(new TimeSpan?(resultData.Time));
        case WorkoutResultMetric.Calories:
          return resultData.Calories == 0 ? AppResources.NotAvailable : resultData.Calories.ToString();
        case WorkoutResultMetric.Reps:
          if (resultData.Reps == 0)
            return AppResources.NotAvailable;
          if (!resultData.RepsAreEstimated)
            return resultData.Reps.ToString();
          return string.Format(AppResources.WorkoutEstimatedRepsFormat, new object[1]
          {
            (object) resultData.Reps
          });
        case WorkoutResultMetric.Pace:
          return resultData.Pace == Speed.Zero ? AppResources.NotAvailable : (string) this.formattingService.FormatPace(new Speed?(resultData.Pace));
        default:
          throw new ArgumentOutOfRangeException(nameof (metric));
      }
    }

    private void RefreshVisibleRecursive(WorkoutResultItemViewModel item, bool expanded)
    {
      if (item.Children != null && item.Expanded)
      {
        foreach (WorkoutResultItemViewModel child in (IEnumerable<WorkoutResultItemViewModel>) item.Children)
          this.RefreshVisibleRecursive(child, expanded);
      }
      item.Visible = expanded;
    }

    private void RefreshEstimatedRepsFootnote() => this.totalResultViewModel.ShowEstimatedRepsFootnote = this.SelectedResultMetric.Value == WorkoutResultMetric.Reps && this.totalResultData.RepsAreEstimated && this.totalResultData.Reps > 0;

    private void RefreshCalculatedDistanceFootnote() => this.totalResultViewModel.ShowCalculatedDistanceFootnote = this.SelectedResultMetric.Value == WorkoutResultMetric.Distance && this.totalResultData.CalculatedDistance > Length.Zero;
  }
}
