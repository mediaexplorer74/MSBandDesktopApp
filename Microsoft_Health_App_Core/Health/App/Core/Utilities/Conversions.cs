// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Conversions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class Conversions
  {
    private const int StepsDivisorMale = 227;
    private const int StepsDivisorFemale = 244;
    private const int WeightSecondPerCaloriesBurned = 837;

    public static TimeSpan StepsToWalkTime(int steps, Gender gender) => TimeSpan.FromSeconds((double) steps * 100.0 / (gender == Gender.Male ? 227.0 : 244.0));

    public static TimeSpan CaloriesToWalkTime(int calories, BandUserProfile profile)
    {
      Weight weight = Weight.FromGrams((double) profile.Weight);
      return TimeSpan.FromSeconds((double) calories / (weight.TotalKilograms / 837.0));
    }

    public static Length DistanceGivenSteps(int steps, Gender gender, float heightMM)
    {
      float num = 0.0f;
      switch (gender)
      {
        case Gender.Male:
          num = 0.48f * heightMM;
          break;
        case Gender.Female:
          num = 0.47f * heightMM;
          break;
      }
      return Length.FromMillimeters((double) num * (double) steps);
    }

    public static TimeSpan WalkTimeGivenSteps(int steps, Gender gender)
    {
      float num = 0.0f;
      switch (gender)
      {
        case Gender.Male:
          num = 142f;
          break;
        case Gender.Female:
          num = 146f;
          break;
      }
      return TimeSpan.FromSeconds((double) steps / (double) num * 100.0);
    }
  }
}
