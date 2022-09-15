// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfScorecardHoleViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class GolfScorecardHoleViewModel : GolfScorecardScoredElementViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfScorecardHoleViewModel.cs");
    private readonly int? differenceFromPar;
    private readonly string handicapDisplay;
    private readonly int holeNumber;
    private readonly StyledSpan duration;
    private readonly StyledSpan distanceWalked;
    private readonly StyledSpan stepsTaken;
    private readonly StyledSpan caloriesBurned;
    private readonly StyledSpan lowestHeartRate;
    private readonly StyledSpan peakHeartRate;
    private readonly StyledSpan averageHeartRate;
    private readonly Uri shotOverlayImageUrl;

    public GolfScorecardHoleViewModel(
      int holeNumber,
      int par,
      Length distanceToPin,
      string distanceToPinDisplay,
      string handicap,
      int? score,
      int? differenceFromPar,
      StyledSpan duration,
      StyledSpan distanceWalked,
      StyledSpan stepsTaken,
      StyledSpan caloriesBurned,
      StyledSpan lowestHeartRate,
      StyledSpan peakHeartRate,
      StyledSpan averageHeartRate,
      Uri shotOverlayImageUrl)
      : base(par, distanceToPin, distanceToPinDisplay, score)
    {
      this.holeNumber = holeNumber;
      this.handicapDisplay = handicap;
      this.differenceFromPar = differenceFromPar;
      this.duration = duration;
      this.distanceWalked = distanceWalked;
      this.stepsTaken = stepsTaken;
      this.caloriesBurned = caloriesBurned;
      this.lowestHeartRate = lowestHeartRate;
      this.peakHeartRate = peakHeartRate;
      this.averageHeartRate = averageHeartRate;
      this.shotOverlayImageUrl = shotOverlayImageUrl;
    }

    public StyledSpan AverageHeartRate => this.averageHeartRate;

    public StyledSpan CaloriesBurned => this.caloriesBurned;

    public int? DifferenceFromPar => this.differenceFromPar;

    public ArgbColor32 DifferenceFromParColor
    {
      get
      {
        if (!this.DifferenceFromPar.HasValue)
          return CoreColors.Transparent;
        int? differenceFromPar1 = this.DifferenceFromPar;
        if (differenceFromPar1.HasValue)
        {
          switch (differenceFromPar1.GetValueOrDefault())
          {
            case -2:
              return CoreColors.BelowParMedium;
            case -1:
              return CoreColors.BelowParLight;
            case 0:
              return CoreColors.Par;
            case 1:
              return CoreColors.AboveParLight;
            case 2:
              return CoreColors.AboveParMedium;
          }
        }
        int? differenceFromPar2 = this.DifferenceFromPar;
        int num = -3;
        return (differenceFromPar2.GetValueOrDefault() <= num ? (differenceFromPar2.HasValue ? 1 : 0) : 0) != 0 ? CoreColors.BelowParDark : CoreColors.AboveParDark;
      }
    }

    public StyledSpan DistanceWalked => this.distanceWalked;

    public StyledSpan Duration => this.duration;

    public string HandicapDisplay => this.handicapDisplay;

    public int HoleNumber => this.holeNumber;

    public StyledSpan LowestHeartRate => this.lowestHeartRate;

    public StyledSpan PeakHeartRate => this.peakHeartRate;

    public StyledSpan StepsTaken => this.stepsTaken;

    public Uri ShotOverlayImageUrl => this.shotOverlayImageUrl;
  }
}
