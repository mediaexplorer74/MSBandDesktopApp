// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.ChartPartitionedEventSeriesInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Models
{
  public class ChartPartitionedEventSeriesInfo : ChartSeriesInfoBase<PartitionedEventChartPoint>
  {
    private const double GolfParYAxisMin = -3.5;
    private const int GolfParYAxisMax = 3;
    private const double GolfParYAxisStep = 0.5;

    public double SeriesYAxisMin { get; private set; }

    public double SeriesYAxisMax { get; private set; }

    public double SeriesYAxisStep { get; private set; }

    public override double GetPointXValue(PartitionedEventChartPoint chartPoint) => (double) chartPoint.Partition;

    protected override void RefreshMetadata()
    {
      base.RefreshMetadata();
      switch (this.SeriesType)
      {
        case ChartSeriesType.Golf:
          this.SetGolfParYAxisRange();
          break;
        case ChartSeriesType.GolfHeartRate:
          this.SetGolfHeartRateYAxisRange();
          break;
      }
    }

    private void SetGolfParYAxisRange()
    {
      this.SeriesYAxisMin = -3.5;
      this.SeriesYAxisMax = 3.0;
      this.SeriesYAxisStep = 0.5;
    }

    private void SetGolfHeartRateYAxisRange()
    {
      IList<PartitionedEventChartPoint> seriesData1 = this.SeriesData;
      PartitionedEventChartPoint seed1 = new PartitionedEventChartPoint();
      seed1.Partition = 1;
      seed1.Value = 0.0;
      this.SeriesYAxisMin = (double) (int) (seriesData1.Aggregate<PartitionedEventChartPoint, PartitionedEventChartPoint>(seed1, (Func<PartitionedEventChartPoint, PartitionedEventChartPoint, PartitionedEventChartPoint>) ((current, minimum) => current.Value != 0.0 && minimum.Value >= current.Value || minimum.Value <= 0.0 ? current : minimum)).Value - 10.0).RoundDown(5.0);
      if (this.SeriesYAxisMin < 0.0)
        this.SeriesYAxisMin = 0.0;
      IList<PartitionedEventChartPoint> seriesData2 = this.SeriesData;
      PartitionedEventChartPoint seed2 = new PartitionedEventChartPoint();
      seed2.Partition = 1;
      seed2.Value = 0.0;
      this.SeriesYAxisStep = (double) (int) (((seriesData2.Aggregate<PartitionedEventChartPoint, PartitionedEventChartPoint>(seed2, (Func<PartitionedEventChartPoint, PartitionedEventChartPoint, PartitionedEventChartPoint>) ((current, maximum) => maximum.Value <= current.Value ? current : maximum)).Value + 10.0).RoundUp(5.0) - this.SeriesYAxisMin) / 5.0).RoundUp(5.0);
      this.SeriesYAxisMax = this.SeriesYAxisStep * 5.0 + this.SeriesYAxisMin;
    }
  }
}
