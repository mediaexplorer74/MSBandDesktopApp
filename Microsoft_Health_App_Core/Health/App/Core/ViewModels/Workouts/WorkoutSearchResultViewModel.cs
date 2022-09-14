// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutSearchResultViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  public sealed class WorkoutSearchResultViewModel : HealthViewModelBase
  {
    private readonly WorkoutSearchResult result;
    private bool isFavorite;

    public WorkoutSearchResultViewModel(WorkoutSearchResult result, bool isFavorite)
    {
      Assert.ParamIsNotNull((object) result, nameof (result));
      this.result = result;
      this.isFavorite = isFavorite;
    }

    public WorkoutSearchResultViewModel(FavoriteWorkout result)
    {
      Assert.ParamIsNotNull((object) result, nameof (result));
      int result1 = 0;
      int.TryParse(result.WorkoutPlanBrowseDetails.Duration, out result1);
      this.result = new WorkoutSearchResult()
      {
        Id = result.WorkoutPlanId,
        Image = result.WorkoutPlanBrowseDetails.Image,
        Name = result.WorkoutPlanBrowseDetails.Name,
        Level = result.WorkoutPlanBrowseDetails.Level,
        Duration = result1,
        PartnerName = result.WorkoutPlanBrowseDetails.PartnerName
      };
      this.isFavorite = true;
    }

    public string DurationMinsFormatted => (string) Formatter.FormatDurationMinutesLocked(TimeSpan.FromMinutes((double) this.result.Duration));

    public int Duration => this.result.Duration;

    public string Id => this.result.Id;

    public string Image => this.result.Image;

    public bool IsCustomWorkout => this.result.IsCustomWorkout;

    public bool IsFavorite
    {
      get => this.isFavorite;
      set => this.SetProperty<bool>(ref this.isFavorite, value, nameof (IsFavorite));
    }

    public string Level => this.result.Level;

    public string Name => this.result.Name;

    public string PartnerName => this.result.PartnerName;

    public string PublishDate => Formatter.FormatShortDate(this.result.PublishDate);
  }
}
