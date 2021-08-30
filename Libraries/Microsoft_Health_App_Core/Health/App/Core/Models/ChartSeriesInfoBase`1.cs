// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.ChartSeriesInfoBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Models
{
  public abstract class ChartSeriesInfoBase<T> where T : ChartPointBase
  {
    private IList<T> seriesData;

    public string Name { get; set; }

    public ChartSeriesType SeriesType { get; set; }

    public bool SkipZeroes { get; set; }

    public IList<T> SeriesData
    {
      get => this.seriesData;
      set
      {
        this.seriesData = value;
        this.RefreshMetadata();
      }
    }

    public bool Selected { get; set; }

    public string LabelGlyph
    {
      get
      {
        switch (this.SeriesType)
        {
          case ChartSeriesType.Steps:
          case ChartSeriesType.WeeklySteps:
            return "\uE008";
          case ChartSeriesType.HeartRate:
          case ChartSeriesType.DistanceHeartRate:
          case ChartSeriesType.ExerciseHeartRate:
          case ChartSeriesType.SleepHeartRate:
          case ChartSeriesType.GolfHeartRate:
            return "\uE006";
          case ChartSeriesType.Pace:
            return "\uE004";
          case ChartSeriesType.Elevation:
            return "\uE103";
          case ChartSeriesType.Calories:
          case ChartSeriesType.WeeklyCalories:
            return "\uE009";
          case ChartSeriesType.Sleep:
            return "\uE005";
          case ChartSeriesType.Speed:
            return "\uE137";
          case ChartSeriesType.Golf:
            return "\uE158";
          case ChartSeriesType.Weight:
            return "\uE203";
          case ChartSeriesType.Waypoint:
            return "\uE408";
          default:
            return "\uE046";
        }
      }
    }

    public string SubLabel { get; set; }

    public bool ShowAverageLine { get; set; }

    public bool ShowVerticalAxis => this.Selected;

    public string LabelBrushResourceName => !this.Selected ? "ChartUnselectedIconBrush" : "ChartSelectedIconBrush";

    public bool Hidden { get; set; }

    public HeartRateZone HRZones { get; set; }

    public bool ShowAverageMarker { get; set; }

    public bool ShowHighMarker { get; set; }

    public bool ShowLowMarker { get; set; }

    public bool ShowLastMarker { get; set; }

    public bool ShowWaypoints { get; set; }

    public double MinValue { get; protected set; }

    public double MaxValue { get; protected set; }

    public double DisplayMaxValue { get; set; }

    public double DisplayMinValue { get; set; }

    public TimeSpan Duration { get; set; }

    public double AverageValue { get; protected set; }

    public double MedianValue { get; protected set; }

    public double LastValue { get; protected set; }

    public double StdDev { get; protected set; }

    public double Start { get; protected set; }

    public double End { get; protected set; }

    public double XWithHigh { get; protected set; }

    public double XWithLow { get; protected set; }

    public double XWithAverage { get; protected set; }

    protected virtual void RefreshMetadata()
    {
      this.MedianValue = double.NaN;
      this.XWithAverage = double.MinValue;
      this.XWithHigh = double.MinValue;
      this.XWithLow = double.MinValue;
      this.MinValue = 0.0;
      this.MaxValue = 0.0;
      this.StdDev = 0.0;
      if (this.SeriesData.Count <= 0)
        return;
      List<double> doubleList = new List<double>(this.SeriesData.Select<T, double>((Func<T, double>) (d => d.Value)));
      doubleList.Sort();
      this.MedianValue = ChartSeriesInfoBase<T>.CalculateMedianValue((IList<double>) doubleList);
      this.AverageValue = ChartSeriesInfoBase<T>.CalculateAverageValue((ICollection<double>) doubleList);
      this.StdDev = ChartSeriesInfoBase<T>.CalculateStDev((ICollection<double>) doubleList, this.AverageValue);
      this.Start = this.GetPointXValue(this.SeriesData[0]);
      this.End = this.GetPointXValue(this.SeriesData[this.SeriesData.Count - 1]);
      this.MinValue = this.SeriesData[0].Value;
      this.XWithLow = this.GetPointXValue(this.SeriesData[0]);
      this.MaxValue = this.SeriesData[0].Value;
      this.XWithHigh = this.GetPointXValue(this.SeriesData[0]);
      this.LastValue = this.SeriesData[this.SeriesData.Count - 1].Value;
      int index1 = -1;
      int index2 = -1;
      for (int index3 = 1; index3 < this.SeriesData.Count; ++index3)
      {
        T chartPoint = this.SeriesData[index3];
        T obj = this.SeriesData[index3 - 1];
        if (chartPoint.Value <= this.MinValue && this.PointIsValidForMin(chartPoint))
        {
          this.MinValue = chartPoint.Value;
          this.XWithLow = this.GetPointXValue(chartPoint);
        }
        else if (chartPoint.Value >= this.MaxValue)
        {
          this.MaxValue = chartPoint.Value;
          this.XWithHigh = this.GetPointXValue(chartPoint);
        }
        if (this.XWithAverage == double.MinValue)
        {
          if (chartPoint.Value == this.AverageValue)
            this.XWithAverage = this.GetPointXValue(chartPoint);
          else if (index1 == -1 && index2 == -1)
          {
            if (chartPoint.Value > this.AverageValue && obj.Value < this.AverageValue)
            {
              index1 = index3 - 1;
              index2 = index3;
            }
            else if (obj.Value > this.AverageValue && chartPoint.Value < this.AverageValue)
            {
              index1 = index3 - 1;
              index2 = index3;
            }
          }
        }
      }
      if (index1 == -1 || index2 == -1)
        return;
      double num1 = this.GetPointXValue(this.SeriesData[index2]) - this.GetPointXValue(this.SeriesData[index1]);
      double num2 = Math.Abs(this.SeriesData[index2].Value - this.SeriesData[index1].Value);
      this.XWithAverage = this.GetPointXValue(this.SeriesData[index1]) + Math.Abs(this.AverageValue - this.SeriesData[index1].Value) / num2 * num1;
    }

    private static double CalculateStDev(ICollection<double> values, double averageValue)
    {
      double num1 = 0.0;
      foreach (double num2 in (IEnumerable<double>) values)
        num1 += Math.Pow(num2 - averageValue, 2.0);
      return Math.Sqrt(num1 / (double) values.Count);
    }

    private static double CalculateAverageValue(ICollection<double> values)
    {
      IList<double> list = (IList<double>) values.Where<double>((Func<double, bool>) (v => v > 0.0)).ToList<double>();
      return list.Any<double>() ? list.Average() : 0.0;
    }

    private static double CalculateMedianValue(IList<double> values) => values.Count % 2 == 0 ? 0.5 * (values[(values.Count >> 1) - 1] + values[values.Count >> 1]) : values[values.Count >> 1];

    public abstract double GetPointXValue(T chartPoint);

    protected virtual bool PointIsValidForMin(T chartPoint) => true;
  }
}
