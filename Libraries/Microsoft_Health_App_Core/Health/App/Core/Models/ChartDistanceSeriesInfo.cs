// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.ChartDistanceSeriesInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Models
{
  public class ChartDistanceSeriesInfo : ChartSeriesInfoBase<DistanceChartPoint>
  {
    protected override void RefreshMetadata()
    {
      base.RefreshMetadata();
      this.TotalScaledDistance = 0.0;
      if (this.SeriesData.Count <= 0)
        return;
      this.TotalScaledDistance = this.SeriesData.Max<DistanceChartPoint>((Func<DistanceChartPoint, double>) (d => this.GetPointXValue(d)));
      this.Splits = (IList<DistanceChartPoint>) this.SeriesData.Where<DistanceChartPoint>((Func<DistanceChartPoint, bool>) (d => d.HasAnnotation(DistanceAnnotationType.Split))).ToList<DistanceChartPoint>();
    }

    public override double GetPointXValue(DistanceChartPoint chartPoint) => chartPoint.ScaledDistance;

    protected override bool PointIsValidForMin(DistanceChartPoint chartPoint) => !this.FilterScaledPace || chartPoint.ScaledPace >= 20.0;

    public double DisplayedMaxValue { get; set; }

    public double TotalScaledDistance { get; private set; }

    public IList<DistanceChartPoint> Splits { get; private set; }

    public bool ShowSplitMarkers { get; set; }

    public bool FilterScaledPace { get; set; }

    public double PrecalculatedAverage { get; set; }
  }
}
