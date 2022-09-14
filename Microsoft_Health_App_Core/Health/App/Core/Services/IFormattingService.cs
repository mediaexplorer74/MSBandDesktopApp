// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IFormattingService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.Services
{
  public interface IFormattingService
  {
    StyledSpan FormatDistance(
      Length? distance,
      int digits = 2,
      bool appendUnit = false,
      bool abbreviateUnit = true);

    StyledSpan FormatDistanceDynamic(
      Length? distance,
      bool appendUnit = false,
      bool useLockedResource = false);

    StyledSpan FormatDistanceMetersOrYards(Length? distance, bool appendUnit = false);

    StyledSpan FormatDistanceMetersOrFeet(
      Length? distance,
      bool appendUnit = false,
      bool useLockedResource = false);

    StyledSpan FormatSplitDistanceHeader(Length distance);

    StyledSpan FormatSplitDistance(Length distance, int digits = 0);

    string ShortDistanceUnit { get; }

    string ShortMassUnit { get; }

    string ShortSpeedUnit { get; }

    string ShortElevationUnit { get; }

    StyledSpan FormatPace(Speed? speed);

    StyledSpan FormatPaceUnit();

    StyledSpan FormatSpeed(Speed? speed, bool showNotAvailableOnZero = false, bool appendUnit = false);

    StyledSpan FormatElevation(
      Length? length,
      bool showNotAvailableOnZero = false,
      bool appendUnit = false);

    StyledSpan FormatStat(
      object value,
      StatValueType valueType,
      bool showNotAvailableOnZero = true);

    StyledSpan FormatSubStat(
      object value,
      SubStatValueType valueType,
      bool showNotAvailableOnZero = true);

    StyledSpan FormatCalories(int? calories, bool appendUnit = false);

    string FormatTileTime(DateTimeOffset time);

    string GetMonthDayFormatString();

    StyledSpan FormatGolfScore(int score, int differenceFromPar);

    StyledSpan FormatWeight(Weight? weight, bool absoluteValue = true);

    StyledSpan FormatCardioBonusMinutes(int minutes, bool appendUnit = true);

    string FormatTimeIntervalMonthDay(DateTimeOffset begin, DateTimeOffset end);

    StyledSpan FormatSocialMetric(double averageSteps, string localizedUnits);

    string FormatDistanceYardMeterValue(Length distance);

    string FormatDistanceYardMeterUnit();

    string FormatDistanceMileKMValue(Length distance);

    string FormatDistanceMileKMUnit();

    string FormatElevationUnit();

    string FormatClimbRateValue(Speed climbSpeed);

    string ShareMetricFormatPaceUnit();

    string FormatDurationValue(TimeSpan duration);

    string FormatDurationUnit();

    string FormatHeartRateValue(int heartRate);

    string FormatHeartRateUnit();

    string FormatCaloriesValue(int calories);

    string FormatCaloriesUnit();

    string FormatSpeedValue(Speed speed);

    string FormatSpeedUnit();

    string FormatPercentageValue(double percentage);

    string FormatPercentageUnit();

    string FormatCountValue(int count);

    string FormatCountUnit();

    string FormatFitnessBenefitValue(string fitnessBenefitMessage);
  }
}
