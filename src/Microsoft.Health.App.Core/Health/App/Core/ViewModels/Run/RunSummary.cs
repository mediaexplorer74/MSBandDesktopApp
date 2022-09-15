// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunSummary
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  public class RunSummary
  {
    public string RunId { get; set; }

    public string RunName { get; set; }

    public string RunMessage { get; set; }

    public RunSensorData RunSensorData { get; set; }

    public Length DistanceRunRaw { get; set; }

    public string DistanceRun { get; set; }

    public string DistanceWithUnit => string.Format(AppResources.UnitFormat, new object[2]
    {
      (object) this.DistanceRun,
      (object) this.DistanceUnit
    });

    public TimeSpan TimeTaken { get; set; }

    public Speed AverageSpeedRaw { get; set; }

    public string AverageSpeed { get; set; }

    public string AverageSpeedWithUnit => string.Format(AppResources.UnitFormat, new object[2]
    {
      (object) this.AverageSpeed,
      (object) this.AverageSpeedUnit
    });

    public string DistanceUnit { get; set; }

    public string AverageSpeedUnit { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public string StartTimeString => Formatter.FormatShortDate(this.StartTime);

    public DateTimeOffset EndTime { get; set; }

    public TimeSpan RecoveryTime { get; set; }

    public HRZones RunHRZones { get; set; }

    public static RunSummary Create(RunEvent runEvent, IFormattingService formatter)
    {
      RunSummary runSummary = new RunSummary()
      {
        TimeTaken = runEvent.Duration,
        RunName = runEvent.Name,
        RunId = runEvent.EventId,
        StartTime = runEvent.StartTime.ToLocalTime(),
        RunSensorData = new RunSensorData()
        {
          CaloriesBurned = (double) runEvent.CaloriesBurned,
          AverageHeartRate = runEvent.AverageHeartRate > 0 ? (string) Formatter.FormatHeartRate(runEvent.AverageHeartRate) : AppResources.NoDataAvailable,
          PeakHeartRate = runEvent.PeakHeartRate > 0 ? (string) Formatter.FormatHeartRate(runEvent.PeakHeartRate) : AppResources.NoDataAvailable,
          RecoveryHeartRate = AppResources.NoDataAvailable,
          VO2 = runEvent.AverageVO2
        },
        DistanceRunRaw = runEvent.TotalDistance
      };
      runSummary.DistanceRun = (string) formatter.FormatDistance(new Length?(runSummary.DistanceRunRaw));
      runSummary.DistanceUnit = formatter.ShortDistanceUnit;
      runSummary.AverageSpeedRaw = runEvent.Pace;
      runSummary.AverageSpeed = (string) formatter.FormatPace(new Speed?(runSummary.AverageSpeedRaw));
      runSummary.AverageSpeedUnit = (string) formatter.FormatPaceUnit();
      runSummary.RunHRZones = runEvent.HeartRateZones;
      return runSummary;
    }

    public static IList<RunSummary> CreateCollection(
      IEnumerable<RunEvent> runEvents,
      IFormattingService formatter)
    {
      return (IList<RunSummary>) runEvents.Select<RunEvent, RunSummary>((Func<RunEvent, RunSummary>) (e => RunSummary.Create(e, formatter))).ToList<RunSummary>();
    }
  }
}
