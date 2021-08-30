// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutSearchFilterViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  public class WorkoutSearchFilterViewModel
  {
    private WorkoutSearchFilter filter;

    public WorkoutSearchFilterViewModel(WorkoutSearchFilter filter)
      : this(filter, new EmbeddedAsset?())
    {
    }

    public WorkoutSearchFilterViewModel(WorkoutSearchFilter filter, EmbeddedAsset? backgroundImage)
    {
      Assert.ParamIsNotNull((object) filter, nameof (filter));
      this.filter = filter;
      this.BackgroundImage = backgroundImage;
    }

    public EmbeddedAsset? BackgroundImage { get; set; }

    public string Image => this.filter.Image;

    public string Name => this.filter.Name;

    public WorkoutSearchFilter Filter => this.filter;
  }
}
