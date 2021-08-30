// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Bike.BikeMultiSplit
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Bike
{
  public class BikeMultiSplit : HealthObservableObject
  {
    private StyledSpan speedSpan;
    private StyledSpan elevationSpan;
    private HealthCommand toggleExpandCommand;
    private bool expanded;
    private SpeedMarker speedMarker;

    public BikeMultiSplit() => this.SingleSplits = (IList<BikeSingleSplit>) new List<BikeSingleSplit>();

    public ICommand ToggleExpandCommand => (ICommand) this.toggleExpandCommand ?? (ICommand) (this.toggleExpandCommand = new HealthCommand((Action) (() => this.Expanded = !this.Expanded)));

    public bool Expanded
    {
      get => this.expanded;
      set
      {
        this.SetProperty<bool>(ref this.expanded, value, nameof (Expanded));
        this.RaisePropertyChanged("ExpandGlyph");
      }
    }

    public string ExpandGlyph => !this.Expanded ? "\uE100" : "\uE099";

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

    public Length Distance { get; set; }

    public string TotalDistanceDisplay { get; set; }

    public TimeSpan Duration { get; set; }

    public Speed Speed => Speed.FromDistanceAndTime(this.Distance, this.Duration);

    public StyledSpan SpeedSpan
    {
      get => this.speedSpan;
      set => this.SetProperty<StyledSpan>(ref this.speedSpan, value, nameof (SpeedSpan));
    }

    public Length Elevation { get; set; }

    public StyledSpan ElevationSpan
    {
      get => this.elevationSpan;
      set => this.SetProperty<StyledSpan>(ref this.elevationSpan, value, nameof (ElevationSpan));
    }

    public bool ShowGpsLossWarning { get; set; }

    public IList<BikeSingleSplit> SingleSplits { get; private set; }
  }
}
