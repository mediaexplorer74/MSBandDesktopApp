// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.SampleExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class SampleExtensions
  {
    public static IList<DateChartPoint> AggregateHourly(
      this IList<Sample> samples)
    {
      List<DateChartPoint> dateChartPointList1 = new List<DateChartPoint>();
      if (samples == null || samples.Count == 0)
        return (IList<DateChartPoint>) dateChartPointList1;
      List<DateChartPoint> dateChartPointList2 = dateChartPointList1;
      DateChartPoint dateChartPoint1 = new DateChartPoint();
      DateTimeOffset dateTimeOffset = samples[0].DateTimeOffset;
      dateChartPoint1.Date = dateTimeOffset.LocalDateTime;
      dateChartPoint1.Value = 0.0;
      dateChartPointList2.Add(dateChartPoint1);
      double num = 0.0;
      foreach (Sample sample in (IEnumerable<Sample>) samples)
      {
        num += sample.Value;
        List<DateChartPoint> dateChartPointList3 = dateChartPointList1;
        DateChartPoint dateChartPoint2 = new DateChartPoint();
        dateTimeOffset = sample.DateTimeOffset;
        dateTimeOffset = dateTimeOffset.AddHours(1.0);
        dateChartPoint2.Date = dateTimeOffset.LocalDateTime;
        dateChartPoint2.Value = num;
        dateChartPointList3.Add(dateChartPoint2);
      }
      return (IList<DateChartPoint>) dateChartPointList1;
    }

    public static IList<DateChartPoint> ToHourlyDateChartPoints(
      this IList<Sample> samples)
    {
      List<DateChartPoint> dateChartPointList = new List<DateChartPoint>();
      if (samples == null || samples.Count == 0)
        return (IList<DateChartPoint>) dateChartPointList;
      dateChartPointList.AddRange(samples.Select<Sample, DateChartPoint>((Func<Sample, DateChartPoint>) (sample =>
      {
        return new DateChartPoint()
        {
          DateWithOffset = sample.DateTimeOffset,
          Date = sample.DateTimeOffset.LocalDateTime,
          Value = sample.Value
        };
      })));
      return (IList<DateChartPoint>) dateChartPointList;
    }

    public static IList<DateChartPoint> ToHalfHourlySumDateChartPoints(
      this IList<Sample> samples)
    {
      return (IList<DateChartPoint>) samples.GroupBy<Sample, DateTimeOffset>((Func<Sample, DateTimeOffset>) (s => s.DateTimeOffset.AddMinutes((double) (-1 * s.DateTimeOffset.Minute % 30)))).OrderBy<IGrouping<DateTimeOffset, Sample>, DateTime>((Func<IGrouping<DateTimeOffset, Sample>, DateTime>) (block => block.Key.LocalDateTime)).Select<IGrouping<DateTimeOffset, Sample>, DateChartPoint>((Func<IGrouping<DateTimeOffset, Sample>, DateChartPoint>) (block =>
      {
        return new DateChartPoint()
        {
          Date = block.Key.LocalDateTime,
          Value = block.Sum<Sample>((Func<Sample, double>) (b => b.Value))
        };
      })).ToList<DateChartPoint>();
    }

    public static IList<DateChartPoint> ToHalfHourlyAverageDateChartPoints(
      this IList<Sample> samples)
    {
      return (IList<DateChartPoint>) samples.GroupBy<Sample, DateTimeOffset>((Func<Sample, DateTimeOffset>) (s => s.DateTimeOffset.AddMinutes((double) (-1 * s.DateTimeOffset.Minute % 30)))).OrderBy<IGrouping<DateTimeOffset, Sample>, DateTime>((Func<IGrouping<DateTimeOffset, Sample>, DateTime>) (block => block.Key.LocalDateTime)).Select<IGrouping<DateTimeOffset, Sample>, DateChartPoint>((Func<IGrouping<DateTimeOffset, Sample>, DateChartPoint>) (block =>
      {
        return new DateChartPoint()
        {
          Date = block.Key.LocalDateTime,
          Value = block.Max<Sample>((Func<Sample, double>) (b => b.Value))
        };
      })).ToList<DateChartPoint>();
    }
  }
}
