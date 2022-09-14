// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Bike.BikeSingleSplit
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.ViewModels.Bike
{
  public class BikeSingleSplit : HealthObservableObject
  {
    private SpeedMarker speedMarker;

    public Length TotalDistance { get; set; }

    public string TotalDistanceDisplay { get; set; }

    public Length SplitDistance { get; set; }

    public SpeedMarker SpeedMarker
    {
      get => this.speedMarker;
      set
      {
        this.SetProperty<SpeedMarker>(ref this.speedMarker, value, nameof (SpeedMarker));
        this.RaisePropertyChanged("SpeedGlyph");
      }
    }

    public string SpeedGlyph => this.SpeedMarker.ToGlyph();

    public StyledSpan SpeedSpan { get; set; }

    public Length Elevation { get; set; }

    public StyledSpan ElevationSpan { get; set; }
  }
}
