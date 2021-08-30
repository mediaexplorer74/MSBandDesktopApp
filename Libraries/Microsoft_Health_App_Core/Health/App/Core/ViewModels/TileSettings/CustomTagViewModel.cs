// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.CustomTagViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class CustomTagViewModel : HealthObservableObject
  {
    private readonly ExerciseTag rawExerciseTag;
    private string text;

    public CustomTagViewModel(string text, ExerciseTag exerciseTag)
    {
      this.text = text;
      this.rawExerciseTag = exerciseTag;
    }

    public string Text
    {
      get => this.text;
      set => this.SetProperty<string>(ref this.text, value, nameof (Text));
    }

    public ExerciseTag RawExerciseTag
    {
      get
      {
        this.rawExerciseTag.Text = this.Text;
        return this.rawExerciseTag;
      }
    }
  }
}
