// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Rounds.GolfRoundHole
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.Providers.Golf.Rounds
{
  public sealed class GolfRoundHole
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Providers\\Golf\\Rounds\\GolfRoundHole.cs");
    private readonly int? differenceFromPar;
    private readonly int? holeDifficultyIndex;
    private readonly Length distanceToPin;
    private readonly int number;
    private readonly int par;
    private readonly int? score;
    private readonly TimeSpan? duration;
    private readonly Length? distanceWalked;
    private readonly int? stepsTaken;
    private readonly int? caloriesBurned;
    private readonly int? lowestHeartRate;
    private readonly int? peakHeartRate;
    private readonly int? averageHeartRate;
    private readonly Uri shotOverlayImageUrl;

    public GolfRoundHole(
      int number,
      int par,
      Length distanceToPin,
      int? holeDifficultyIndex,
      int? score,
      int? differenceFromPar,
      TimeSpan? duration,
      Length? distanceWalked,
      int? stepsTaken,
      int? caloriesBurned,
      int? lowestHeartRate,
      int? peakHeartRate,
      int? averageHeartRate,
      string shotOverlayImageUrl)
    {
      this.number = number;
      this.par = par;
      this.distanceToPin = distanceToPin;
      this.holeDifficultyIndex = holeDifficultyIndex;
      this.score = score;
      this.differenceFromPar = differenceFromPar;
      this.duration = duration;
      this.distanceWalked = distanceWalked;
      this.stepsTaken = stepsTaken;
      this.caloriesBurned = caloriesBurned;
      this.lowestHeartRate = lowestHeartRate;
      this.peakHeartRate = peakHeartRate;
      this.averageHeartRate = averageHeartRate;
      if (Uri.TryCreate(shotOverlayImageUrl, UriKind.Absolute, out this.shotOverlayImageUrl))
        return;
      GolfRoundHole.Logger.Error((object) ("Invalid shot overlay image url " + shotOverlayImageUrl));
    }

    public int? AverageHeartRate => this.averageHeartRate;

    public int? CaloriesBurned => this.caloriesBurned;

    public Length? DistanceWalked => this.distanceWalked;

    public int? DifferenceFromPar => this.differenceFromPar;

    public Length DistanceToPin => this.distanceToPin;

    public TimeSpan? Duration => this.duration;

    public int? HoleDifficultyIndex => this.holeDifficultyIndex;

    public int? LowestHeartRate => this.lowestHeartRate;

    public int Number => this.number;

    public int Par => this.par;

    public int? PeakHeartRate => this.peakHeartRate;

    public int? Score => this.score;

    public int? StepsTaken => this.stepsTaken;

    public Uri ShotOverlayImageUrl => this.shotOverlayImageUrl;
  }
}
