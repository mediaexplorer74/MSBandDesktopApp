// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.ChartDateSeriesInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Models
{
  public class ChartDateSeriesInfo : ChartSeriesInfoBase<DateChartPoint>
  {
    public TimeSpan Interval { get; set; }

    protected override void RefreshMetadata()
    {
      base.RefreshMetadata();
      this.SeriesDateWithAverage = DateTime.MinValue;
      this.SeriesDateWithHigh = DateTime.MinValue;
      this.SeriesDateWithLow = DateTime.MinValue;
      if (this.SeriesData.Count > 0)
      {
        this.SeriesStartDate = this.SeriesData[0].Date;
        this.SeriesEndDate = this.SeriesData[this.SeriesData.Count - 1].Date;
        this.SeriesDateWithLow = this.SeriesData.First<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Value == this.MinValue)).Date;
        this.SeriesDateWithHigh = this.SeriesData.First<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Value == this.MaxValue)).Date;
        this.SeriesDateWithAverage = new DateTimeOffset((long) this.AverageValue, TimeSpan.Zero).LocalDateTime;
        this.SetSleepTimes();
        if (this.SeriesType == ChartSeriesType.Steps || this.SeriesType == ChartSeriesType.Calories)
        {
          this.SetStepsAndCaloriesDayYAxisRange();
        }
        else
        {
          if (this.SeriesType != ChartSeriesType.WeeklySteps && this.SeriesType != ChartSeriesType.WeeklyCalories)
            return;
          this.SetStepsAndCaloriesWeekYAxisRange();
        }
      }
      else
      {
        this.SeriesStartDate = DateTime.MinValue;
        this.SeriesEndDate = DateTime.MinValue;
        this.End = 0.0;
      }
    }

    private void SetSleepTimes()
    {
      DateChartPoint dateChartPoint1 = this.SeriesData.FirstOrDefault<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Classification != DateChartValueClassification.Awake && d.Classification != DateChartValueClassification.Unknown));
      if (dateChartPoint1 != null)
      {
        this.FallAsleepDate = new DateTime?(dateChartPoint1.Date);
        DateTimeOffset dateTimeOffset1 = new DateTimeOffset(this.FallAsleepDate.Value);
        if (this.StartTime != DateTimeOffset.MinValue)
        {
          DateTimeOffset dateTimeOffset2 = this.StartTime.ToLocalTime();
          if (dateTimeOffset2.Hour < 5)
            dateTimeOffset2 = dateTimeOffset2.Subtract(TimeSpan.FromDays(1.0));
          this.ShowMonthDay = dateTimeOffset1.Month != dateTimeOffset2.Month || dateTimeOffset1.Day != dateTimeOffset2.Day;
        }
      }
      DateChartPoint dateChartPoint2 = this.SeriesData.LastOrDefault<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Classification != DateChartValueClassification.Awake && d.Classification != DateChartValueClassification.Unknown));
      if (dateChartPoint2 == null)
        return;
      this.WakeUpDate = new DateTime?(dateChartPoint2.Date);
    }

    private void SetStepsAndCaloriesDayYAxisRange()
    {
      double num1 = (double) (this.GoalValue / 16L);
      double num2 = num1 / 3.0;
      int num3 = (int) ((this.MaxValue <= num1 + num2 ? num1 + num2 : this.MaxValue) / 4.0).RoundUp(10.0);
      int num4 = num3 * 4;
      this.SeriesYAxisMin = 0.0;
      this.SeriesYAxisMax = (double) num4;
      this.SeriesYAxisStep = (double) num3;
    }

    private void SetStepsAndCaloriesWeekYAxisRange()
    {
      this.SeriesYAxisMin = 0.0;
      this.SeriesYAxisMax = (100.0 - this.MaxValue / 4.0 % 100.0 + this.MaxValue / 4.0) * 4.0;
      this.SeriesYAxisStep = this.SeriesYAxisMax / 5.0;
    }

    public override double GetPointXValue(DateChartPoint chartPoint) => (double) chartPoint.DateWithOffset.UtcTicks;

    public IList<SleepEventSequence> RawSeriesData { get; set; }

    public DateTime SeriesStartDate { get; set; }

    public DateTime SeriesEndDate { get; set; }

    public DateTime SeriesDateWithAverage { get; private set; }

    public DateTime SeriesDateWithLow { get; private set; }

    public DateTime SeriesDateWithHigh { get; private set; }

    public long GoalValue { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTime? FallAsleepDate { get; private set; }

    public DateTime? WakeUpDate { get; private set; }

    public bool ShowMonthDay { get; private set; }

    public double SeriesYAxisMin { get; private set; }

    public double SeriesYAxisMax { get; private set; }

    public double SeriesYAxisStep { get; private set; }
  }
}
