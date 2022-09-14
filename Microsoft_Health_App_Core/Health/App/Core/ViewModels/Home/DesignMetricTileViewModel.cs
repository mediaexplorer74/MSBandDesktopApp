// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.DesignMetricTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Utilities;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public sealed class DesignMetricTileViewModel
  {
    public string TopText => "My action plan";

    public string Header => "72 (-2)";

    public bool IsBest => true;

    public FormattedMetricViewModel Metric => new FormattedMetricViewModel(new FormattedMetric("72", "(-2)"));

    public IList<PivotDefinition> Pivots => (IList<PivotDefinition>) new List<PivotDefinition>()
    {
      new PivotDefinition("scorecard", (object) null)
      {
        IsSelected = true
      },
      new PivotDefinition("summary", (object) null)
    };

    public string Subheader => "Mon 6/1\nBellevue Golf Course";

    public string TileIcon => "\uE151";
  }
}
