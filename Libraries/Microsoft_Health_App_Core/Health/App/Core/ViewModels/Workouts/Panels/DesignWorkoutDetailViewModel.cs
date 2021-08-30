// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.DesignWorkoutDetailViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.ViewModels.Workouts.Pre;
using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  public sealed class DesignWorkoutDetailViewModel
  {
    public IEnumerable<WorkoutItemViewModel> ViewList { get; private set; }

    public DesignWorkoutDetailViewModel() => this.ViewList = (IEnumerable<WorkoutItemViewModel>) DesignWorkoutDetailViewModel.CreateCustomGuidedWorkout();

    public static WorkoutBlockViewModel CreateWarmUpBlock()
    {
      WorkoutBlockViewModel workoutBlockViewModel = new WorkoutBlockViewModel();
      workoutBlockViewModel.CircuitType = CircuitType.Warmup;
      workoutBlockViewModel.Text = "WarmUp";
      return workoutBlockViewModel;
    }

    public static WorkoutBlockViewModel CreateWorkoutBlock()
    {
      WorkoutBlockViewModel workoutBlockViewModel = new WorkoutBlockViewModel();
      workoutBlockViewModel.CircuitType = CircuitType.Regular;
      workoutBlockViewModel.Text = "Workout";
      return workoutBlockViewModel;
    }

    public static WorkoutBlockViewModel CreateCoolDownBlock()
    {
      WorkoutBlockViewModel workoutBlockViewModel = new WorkoutBlockViewModel();
      workoutBlockViewModel.CircuitType = CircuitType.Cooldown;
      workoutBlockViewModel.Text = "Cool Down";
      return workoutBlockViewModel;
    }

    public static WorkoutGroupViewModel CreateGroupWithList(
      CircuitType circuitType)
    {
      WorkoutGroupViewModel workoutGroupViewModel = new WorkoutGroupViewModel();
      workoutGroupViewModel.CircuitType = CircuitType.Warmup;
      workoutGroupViewModel.Children = DesignWorkoutDetailViewModel.CreateExerciseItems();
      workoutGroupViewModel.Text = "List";
      workoutGroupViewModel.Glyph = "\uE046";
      workoutGroupViewModel.Metric = string.Empty;
      return workoutGroupViewModel;
    }

    public static WorkoutGroupViewModel CreateGroupWithCircuit(
      CircuitType circuitType)
    {
      WorkoutGroupViewModel workoutGroupViewModel = new WorkoutGroupViewModel();
      workoutGroupViewModel.CircuitType = CircuitType.Regular;
      workoutGroupViewModel.Children = DesignWorkoutDetailViewModel.CreateExerciseItems();
      workoutGroupViewModel.Glyph = "\uE046";
      workoutGroupViewModel.Text = "Circuit";
      workoutGroupViewModel.Metric = "2 seconds";
      return workoutGroupViewModel;
    }

    public static WorkoutGroupViewModel CreateGroupWithInterval(
      CircuitType circuitType)
    {
      WorkoutGroupViewModel workoutGroupViewModel = new WorkoutGroupViewModel();
      workoutGroupViewModel.CircuitType = CircuitType.Regular;
      workoutGroupViewModel.Children = DesignWorkoutDetailViewModel.CreateExerciseItems();
      workoutGroupViewModel.Glyph = "\uE046";
      workoutGroupViewModel.Text = "Interval";
      workoutGroupViewModel.Metric = "2min";
      return workoutGroupViewModel;
    }

    public static WorkoutGroupViewModel CreateGroupWithRest(
      CircuitType circuitType)
    {
      WorkoutGroupViewModel workoutGroupViewModel = new WorkoutGroupViewModel();
      workoutGroupViewModel.CircuitType = CircuitType.Regular;
      workoutGroupViewModel.Text = "Interval";
      workoutGroupViewModel.Glyph = "\uE046";
      workoutGroupViewModel.Metric = "3 min";
      return workoutGroupViewModel;
    }

    public static ExerciseExplanationViewModel CreateExerciseExplanationItem()
    {
      ExerciseExplanationViewModel explanationViewModel = new ExerciseExplanationViewModel();
      explanationViewModel.Text = "Complete each exercise one after the another";
      return explanationViewModel;
    }

    public static IList<WorkoutItemViewModel> CreateExerciseItems()
    {
      List<WorkoutItemViewModel> workoutItemViewModelList = new List<WorkoutItemViewModel>();
      workoutItemViewModelList.Add((WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateExerciseExplanationItem());
      ExerciseItemViewModel exerciseItemViewModel1 = new ExerciseItemViewModel();
      exerciseItemViewModel1.Text = "Workout1";
      exerciseItemViewModel1.Completion = "10 mins";
      exerciseItemViewModel1.Image = new EmbeddedOrRemoteImageSource(EmbeddedAsset.ThumbnailDefault);
      workoutItemViewModelList.Add((WorkoutItemViewModel) exerciseItemViewModel1);
      ExerciseItemViewModel exerciseItemViewModel2 = new ExerciseItemViewModel();
      exerciseItemViewModel2.Text = "Workout2";
      exerciseItemViewModel2.Completion = "50%";
      exerciseItemViewModel2.VideoId = "5";
      workoutItemViewModelList.Add((WorkoutItemViewModel) exerciseItemViewModel2);
      ExerciseItemViewModel exerciseItemViewModel3 = new ExerciseItemViewModel();
      exerciseItemViewModel3.Text = "Workout3";
      exerciseItemViewModel3.Completion = "1 hour";
      exerciseItemViewModel3.Image = new EmbeddedOrRemoteImageSource(EmbeddedAsset.ThumbnailDefault);
      workoutItemViewModelList.Add((WorkoutItemViewModel) exerciseItemViewModel3);
      return (IList<WorkoutItemViewModel>) workoutItemViewModelList;
    }

    public static IList<WorkoutItemViewModel> CreateCustomGuidedWorkout() => (IList<WorkoutItemViewModel>) new List<WorkoutItemViewModel>()
    {
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateWarmUpBlock(),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateGroupWithList(CircuitType.Warmup),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateWorkoutBlock(),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateGroupWithCircuit(CircuitType.Regular),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateGroupWithCircuit(CircuitType.Regular),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateGroupWithList(CircuitType.Regular),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateCoolDownBlock(),
      (WorkoutItemViewModel) DesignWorkoutDetailViewModel.CreateGroupWithList(CircuitType.Cooldown)
    };
  }
}
