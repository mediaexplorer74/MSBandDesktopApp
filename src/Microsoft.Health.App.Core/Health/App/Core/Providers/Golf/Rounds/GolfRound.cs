// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Rounds.GolfRound
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Events.Golf;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Providers.Golf.Rounds
{
  public sealed class GolfRound
  {
    private readonly string courseName;
    private readonly string name;
    private readonly List<GolfRoundHole> holes;
    private readonly GolfEvent rawEvent;
    private readonly DateTimeOffset startTime;
    private readonly int totalDifferenceFromPar;
    private readonly int parOrBetterCount;
    private readonly Length longestDrive;
    private readonly TimeSpan paceOfPlay;
    private readonly int totalSteps;
    private readonly Length totalDistance;
    private readonly int averageHeartRate;
    private readonly int peakHeartRate;
    private readonly int lowestHeartRate;
    private readonly string teesPlayed;
    private readonly int totalScore;

    public GolfRound(
      string courseName,
      string name,
      DateTimeOffset startTime,
      int totalScore,
      int totalDifferenceFromPar,
      int parOrBetterCount,
      Length longestDrive,
      TimeSpan paceOfPlay,
      int totalSteps,
      Length totalDistance,
      int averageHeartRate,
      int peakHeartRate,
      int lowestHeartRate,
      string teesPlayed,
      IEnumerable<GolfRoundHole> holes,
      GolfEvent rawEvent = null)
    {
      this.courseName = courseName;
      this.name = name;
      this.startTime = startTime;
      this.totalScore = totalScore;
      this.totalDifferenceFromPar = totalDifferenceFromPar;
      this.parOrBetterCount = parOrBetterCount;
      this.longestDrive = longestDrive;
      this.paceOfPlay = paceOfPlay;
      this.totalSteps = totalSteps;
      this.totalDistance = totalDistance;
      this.averageHeartRate = averageHeartRate;
      this.peakHeartRate = peakHeartRate;
      this.lowestHeartRate = lowestHeartRate;
      this.teesPlayed = teesPlayed;
      this.holes = new List<GolfRoundHole>(holes);
      this.rawEvent = rawEvent;
    }

    public string CourseName => this.courseName;

    public string Name => this.name;

    public IReadOnlyList<GolfRoundHole> Holes => (IReadOnlyList<GolfRoundHole>) this.holes;

    public DateTimeOffset StartTime => this.startTime;

    public int TotalDifferenceFromPar => this.totalDifferenceFromPar;

    public int TotalScore => this.totalScore;

    public int ParOrBetterCount => this.parOrBetterCount;

    public Length LongestDrive => this.longestDrive;

    public TimeSpan PaceOfPlay => this.paceOfPlay;

    public string TeesPlayed => this.teesPlayed;

    public int TotalSteps => this.totalSteps;

    public Length TotalDistance => this.totalDistance;

    public int AverageHeartRate => this.averageHeartRate;

    public int PeakHeartRate => this.peakHeartRate;

    public int LowestHeartRate => this.lowestHeartRate;

    public GolfEvent GetRawEvent() => this.rawEvent;
  }
}
