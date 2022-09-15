// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.SearchFilter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;

namespace Microsoft.Health.App.Core.Models
{
  public class SearchFilter : HealthObservableObject
  {
    private bool isSelected;

    public string DisplayName { get; set; }

    public string FilterName { get; set; }

    public string FilterValueName { get; set; }

    public string Id { get; set; }

    public WorkoutSearchFilter WorkoutFilter { get; set; }

    public bool IsSelected
    {
      get => this.isSelected;
      set => this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
    }
  }
}
