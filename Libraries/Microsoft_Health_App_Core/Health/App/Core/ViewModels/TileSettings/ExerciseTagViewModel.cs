// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.ExerciseTagViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class ExerciseTagViewModel : HealthObservableObject
  {
    private readonly Guid exerciseActivityId = new Guid("24794770-5084-49BB-838C-504BC6663B30");
    private readonly ExerciseSettingsViewModel parentViewModel;
    private readonly string text;
    private readonly ExerciseTag rawExerciseTag;
    private bool isChecked;

    public ExerciseTagViewModel(
      ExerciseSettingsViewModel parentViewModel,
      string text,
      bool isChecked,
      ExerciseTag exerciseTag)
    {
      this.parentViewModel = parentViewModel;
      this.text = text;
      this.isChecked = isChecked;
      this.rawExerciseTag = exerciseTag;
    }

    public string Text => this.text;

    public bool IsChecked
    {
      get => this.isChecked;
      set
      {
        this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
        this.parentViewModel.SaveTagsToPendingSettings();
      }
    }

    public bool Visible => this.rawExerciseTag.ExerciseTypeId != this.exerciseActivityId;

    public ExerciseTag RawExerciseTag
    {
      get
      {
        this.rawExerciseTag.IsChecked = this.IsChecked;
        return this.rawExerciseTag;
      }
    }
  }
}
