// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunDetails
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  public class RunDetails
  {
    public RunSummary RunSummary { get; set; }

    public IList<RunSplit> RunSplits { get; set; }

    public static RunDetails Create(
      RunEvent runEvent,
      DistanceUnitType unitType,
      IFormattingService formatter,
      bool runSummary = true)
    {
      RunDetails runDetails = new RunDetails();
      if (runSummary)
        runDetails.RunSummary = RunSummary.Create(runEvent, formatter);
      runDetails.RunSplits = (IList<RunSplit>) new List<RunSplit>();
      runEvent.Sequences = (IList<RunEventSequence>) runEvent.Sequences.OrderBy<RunEventSequence, RunEventSequence>((Func<RunEventSequence, RunEventSequence>) (s => s)).ToList<RunEventSequence>();
      if (runEvent.Sequences.Count == 1 && runEvent.Sequences[0].TotalDistance <= Length.Zero)
        return (RunDetails) null;
      if (runEvent.Sequences.Count > 0)
      {
        RunEventSequence sequence1 = runEvent.Sequences[runEvent.Sequences.Count - 1];
        Speed splitPace1 = runEvent.Sequences[0].SplitPace;
        int index1 = 0;
        Speed splitPace2 = runEvent.Sequences[0].SplitPace;
        int index2 = 0;
        for (int index3 = 0; index3 < runEvent.Sequences.Count; ++index3)
        {
          RunEventSequence sequence2 = runEvent.Sequences[index3];
          bool isLastSplit = sequence2 == sequence1;
          if (isLastSplit)
          {
            Length splitDistance = sequence2.SplitDistance;
            if ((unitType == DistanceUnitType.Metric ? splitDistance.TotalKilometers : splitDistance.TotalMiles) < 0.01)
              continue;
          }
          else if (sequence2.SplitPace > splitPace1)
          {
            splitPace1 = sequence2.SplitPace;
            index1 = index3;
          }
          else if (sequence2.SplitPace < splitPace2)
          {
            splitPace2 = sequence2.SplitPace;
            index2 = index3;
          }
          RunSplit split = RunDetails.CreateSplit(sequence2, index3, isLastSplit, unitType, formatter);
          runDetails.RunSplits.Add(split);
        }
        if (runEvent.Sequences.Count > 2 && index2 != index1)
        {
          runDetails.RunSplits[index2].SpeedMarker = SpeedMarker.Slowest;
          runDetails.RunSplits[index1].SpeedMarker = SpeedMarker.Fastest;
        }
      }
      return runDetails;
    }

    private static RunSplit CreateSplit(
      RunEventSequence sequence,
      int splitIndex,
      bool isLastSplit,
      DistanceUnitType unitType,
      IFormattingService formatter)
    {
      string str = string.Empty;
      if (isLastSplit)
      {
        Length splitDistance = sequence.SplitDistance;
        if ((unitType == DistanceUnitType.Metric ? sequence.TotalDistance.TotalKilometers : sequence.TotalDistance.TotalMiles) - (double) (splitIndex + 1) < 0.0)
          str = (string) formatter.FormatSplitDistance(sequence.TotalDistance, 2);
      }
      if (string.IsNullOrEmpty(str))
      {
        Length distance = unitType == DistanceUnitType.Metric ? Length.FromKilometers((double) (splitIndex + 1)) : Length.FromMiles((double) (splitIndex + 1));
        str = (string) formatter.FormatSplitDistance(distance);
      }
      return new RunSplit()
      {
        LapTime = (string) formatter.FormatPace(new Speed?(sequence.SplitPace)),
        SplitSensorData = new RunSummary()
        {
          DistanceRun = str,
          RunSensorData = new RunSensorData()
          {
            AverageHeartRate = (string) Formatter.FormatHeartRate(sequence.AverageHeartRate)
          }
        },
        SpeedMarker = SpeedMarker.None
      };
    }
  }
}
