// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfScorecardScoredElementViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.Cloud.Client;
using System;
using System.Globalization;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public abstract class GolfScorecardScoredElementViewModel : GolfScorecardElementViewModel
  {
    private readonly int par;
    private readonly Length distance;
    private readonly string distanceDisplay;
    private readonly int? score;

    public GolfScorecardScoredElementViewModel(
      int par,
      Length distance,
      string distanceDisplay,
      int? score)
    {
      this.par = par;
      this.distance = distance;
      this.distanceDisplay = distanceDisplay;
      this.score = score;
    }

    public Length Distance => this.distance;

    public string DistanceDisplay => this.distanceDisplay;

    public int Par => this.par;

    public string ScoreDisplay => !this.score.HasValue ? AppResources.NotAvailable : this.score.Value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
  }
}
