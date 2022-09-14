// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.ChartUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class ChartUtilities
  {
    public static string GetHourStringForChartAxis(DateTime d)
    {
      if (RegionUtilities.Is24HourTime)
        return d.ToString("%H");
      return d.Hour == 0 || d.Hour == 12 ? d.ToString("ht").ToLower() : d.ToString("%h");
    }

    public static double ConvertPaceToUserUnits(Speed v, DistanceUnitType unitType)
    {
      if (unitType == DistanceUnitType.Imperial)
        return v.MinutesPerMile;
      if (unitType == DistanceUnitType.Metric)
        return v.MinutesPerKilometer;
      throw new ArgumentOutOfRangeException();
    }

    public static double ConvertSpeedToUserUnits(Speed v, DistanceUnitType unitType)
    {
      if (unitType == DistanceUnitType.Imperial)
        return v.MilesPerHour;
      if (unitType == DistanceUnitType.Metric)
        return v.KilometersPerHour;
      throw new ArgumentOutOfRangeException();
    }

    public static double ConvertDistanceToUserUnits(Length l, DistanceUnitType unitType)
    {
      if (unitType == DistanceUnitType.Imperial)
        return l.TotalMiles;
      if (unitType == DistanceUnitType.Metric)
        return l.TotalKilometers;
      throw new ArgumentOutOfRangeException();
    }

    public static double ConvertElevationToUserUnits(Length l, DistanceUnitType unitType)
    {
      if (unitType == DistanceUnitType.Imperial)
        return l.TotalFeet;
      if (unitType == DistanceUnitType.Metric)
        return l.TotalMeters;
      throw new ArgumentOutOfRangeException();
    }

    public static IList<PartitionedEventChartPoint> GetGolfScoreData(
      GolfRound golfModel)
    {
      return golfModel == null ? (IList<PartitionedEventChartPoint>) new List<PartitionedEventChartPoint>() : (IList<PartitionedEventChartPoint>) golfModel.Holes.Select<GolfRoundHole, PartitionedEventChartPoint>((Func<GolfRoundHole, PartitionedEventChartPoint>) (p =>
      {
        return new PartitionedEventChartPoint()
        {
          Partition = p.Number,
          Value = (double) p.DifferenceFromPar ?? double.MinValue
        };
      })).ToList<PartitionedEventChartPoint>();
    }

    public static IList<PartitionedEventChartPoint> GetGolfHeartRateData(
      GolfRound golfModel)
    {
      return golfModel == null ? (IList<PartitionedEventChartPoint>) new List<PartitionedEventChartPoint>() : (IList<PartitionedEventChartPoint>) golfModel.Holes.Select<GolfRoundHole, PartitionedEventChartPoint>((Func<GolfRoundHole, PartitionedEventChartPoint>) (p =>
      {
        return new PartitionedEventChartPoint()
        {
          Partition = p.Number,
          Value = !p.Score.HasValue || !p.PeakHeartRate.HasValue || p.PeakHeartRate.Value <= 0 ? double.MinValue : (double) p.PeakHeartRate.Value
        };
      })).ToList<PartitionedEventChartPoint>();
    }
  }
}
