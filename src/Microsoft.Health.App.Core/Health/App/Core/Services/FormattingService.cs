// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.FormattingService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Globalization;

namespace Microsoft.Health.App.Core.Services
{
  public class FormattingService : IFormattingService
  {
    private const int ValidYearThreshold = 1900;
    private readonly Func<DistanceUnitType> distanceUnitTypeProvider;
    private readonly Func<MassUnitType> massUnitTypeProvider;

    public FormattingService(IUserProfileService userProfileService)
      : this((Func<DistanceUnitType>) (() => userProfileService.DistanceUnitType), (Func<MassUnitType>) (() => userProfileService.MassUnitType))
    {
      Assert.ParamIsNotNull((object) userProfileService, nameof (userProfileService));
    }

    public FormattingService(DistanceUnitType distanceUnitType, MassUnitType massUnitType)
      : this((Func<DistanceUnitType>) (() => distanceUnitType), (Func<MassUnitType>) (() => massUnitType))
    {
    }

    private FormattingService(
      Func<DistanceUnitType> distanceUnitTypeProvider,
      Func<MassUnitType> massUnitTypeProvider)
    {
      this.distanceUnitTypeProvider = distanceUnitTypeProvider;
      this.massUnitTypeProvider = massUnitTypeProvider;
    }

    private DistanceUnitType DistanceUnitType => this.distanceUnitTypeProvider();

    private MassUnitType MassUnitType => this.massUnitTypeProvider();

    public string ShortMassUnit => Formatter.GetShortMassUnit(this.MassUnitType);

    public string ShortDistanceUnit => Formatter.GetShortDistanceUnit(this.DistanceUnitType);

    public string ShortSpeedUnit => Formatter.GetShortSpeedUnit(this.DistanceUnitType);

    public string ShortElevationUnit => Formatter.GetShortElevationUnit(this.DistanceUnitType);

    public StyledSpan FormatPace(Speed? speed) => speed.HasValue ? Formatter.FormatPace(speed.Value, this.DistanceUnitType) : Formatter.NotAvailableStyledSpan;

    public StyledSpan FormatSpeed(
      Speed? speed,
      bool showNotAvailableOnZero = false,
      bool appendUnit = false)
    {
      return !speed.HasValue || speed.Value == Speed.Zero & showNotAvailableOnZero ? Formatter.NotAvailableStyledSpan : Formatter.FormatSpeed(speed.Value, this.DistanceUnitType, appendUnit);
    }

    public StyledSpan FormatElevation(
      Length? length,
      bool showNotAvailableOnZero = false,
      bool appendUnit = false)
    {
      return !length.HasValue || length.Value == Length.Zero & showNotAvailableOnZero ? Formatter.NotAvailableStyledSpan : Formatter.FormatElevation(length.Value, this.DistanceUnitType, appendUnit);
    }

    public StyledSpan FormatDistance(
      Length? distance,
      int digits = 2,
      bool appendUnit = false,
      bool abbreviateUnit = true)
    {
      return distance.HasValue ? Formatter.FormatDistance(distance.Value, this.DistanceUnitType, digits, appendUnit, abbreviateUnit) : Formatter.NotAvailableStyledSpan;
    }

    public StyledSpan FormatDistanceDynamic(
      Length? distance,
      bool appendUnit = false,
      bool useLockedResource = false)
    {
      return Formatter.FormatDistanceDynamic(distance, this.DistanceUnitType, appendUnit, useLockedResource);
    }

    public StyledSpan FormatDistanceMetersOrYards(Length? distance, bool appendUnit = false) => distance.HasValue ? Formatter.FormatDistanceMetersOrYards(distance.Value, this.DistanceUnitType, appendUnit) : Formatter.NotAvailableStyledSpan;

    public StyledSpan FormatDistanceMetersOrFeet(
      Length? distance,
      bool appendUnit = false,
      bool useLockedResource = false)
    {
      return distance.HasValue ? Formatter.FormatDistanceMetersOrFeet(distance.Value, this.DistanceUnitType, appendUnit, useLockedResource) : Formatter.NotAvailableStyledSpan;
    }

    public StyledSpan FormatSplitDistanceHeader(Length distance) => Formatter.FormatSplitDistanceHeader(distance, this.DistanceUnitType);

    public StyledSpan FormatSplitDistance(Length distance, int digits = 0) => Formatter.FormatSplitDistance(distance, digits, this.DistanceUnitType);

    public StyledSpan FormatClimbRateValue(Speed climbSpeed, int digits, bool appendUnit = true) => Formatter.FormatClimbRate(climbSpeed, digits, this.DistanceUnitType, appendUnit);

    public StyledSpan FormatClimbRateUnit() => Formatter.FormatClimbRateUnit(this.DistanceUnitType);

    public StyledSpan FormatPaceUnit() => Formatter.FormatPaceUnit(this.DistanceUnitType);

    public StyledSpan FormatSubStat(
      object value,
      SubStatValueType valueType,
      bool showNotAvailableOnZero = true)
    {
      if (value == null)
        return Formatter.NotAvailableStyledSpan;
      switch (valueType)
      {
        case SubStatValueType.Integer:
          return this.FormatInteger((int?) value, showNotAvailableOnZero);
        case SubStatValueType.Double:
          return this.FormatDouble(value is string ? new double?(double.Parse((string) value)) : (double?) value, showNotAvailableOnZero);
        case SubStatValueType.Time:
          return this.FormatTimeStat((DateTimeOffset?) value);
        case SubStatValueType.DurationWithText:
          return this.FormatDurationWithText((TimeSpan?) value, showNotAvailableOnZero);
        case SubStatValueType.Elevation:
          return this.FormatElevation((Length?) value, showNotAvailableOnZero, false);
        case SubStatValueType.CardioBonusMinutes:
          return this.FormatCardioBonusMinutes((int) value, true);
        default:
          Assert.EnumIsDefined<SubStatValueType>(valueType, "SubStatValueType");
          return Formatter.FormatValueStyledSpan(string.Empty);
      }
    }

    public StyledSpan FormatStat(
      object value,
      StatValueType valueType,
      bool showNotAvailableOnZero = true)
    {
      if (value == null)
        return Formatter.NotAvailableStyledSpan;
      switch (valueType)
      {
        case StatValueType.Text:
          return Formatter.FormatValueStyledSpan((string) value);
        case StatValueType.SmallText:
          return Formatter.FormatSmallTextStyledSpan((string) value);
        case StatValueType.Percentage:
          return Formatter.ToStringAsPercentage((int?) value);
        case StatValueType.Integer:
          return this.FormatInteger((int?) value, showNotAvailableOnZero);
        case StatValueType.Double:
          return this.FormatDouble(value is string ? new double?(double.Parse((string) value)) : (double?) value, showNotAvailableOnZero);
        case StatValueType.DurationWithText:
          return this.FormatDurationWithText((TimeSpan?) value, showNotAvailableOnZero);
        case StatValueType.DurationWithTextWithoutSeconds:
          return this.FormatDurationWithTextWithoutSeconds((TimeSpan?) value, showNotAvailableOnZero);
        case StatValueType.DurationFull:
          return Formatter.FormatTimeSpanNoText((TimeSpan?) value, useLeadingZero: true);
        case StatValueType.DurationWithoutSeconds:
          return Formatter.FormatTimeSpanNoText((TimeSpan?) value, false);
        case StatValueType.DurationTicks:
          return Formatter.FormatTimeTicks((TimeSpan?) value);
        case StatValueType.Count:
          return this.FormatCount((int?) value);
        case StatValueType.Pace:
          return this.FormatPace((Speed?) value);
        case StatValueType.Speed:
          return this.FormatSpeed((Speed?) value, showNotAvailableOnZero, true);
        case StatValueType.Distance:
          return this.FormatDistance((Length?) value, 2, true, true);
        case StatValueType.DistanceShort:
          return this.FormatDistanceDynamic((Length?) value, true, false);
        case StatValueType.MinorDistance:
          return this.FormatDistanceMetersOrYards((Length?) value, true);
        case StatValueType.Elevation:
          return this.FormatElevation((Length?) value, showNotAvailableOnZero, true);
        case StatValueType.Time:
          return this.FormatTimeStat((DateTimeOffset?) value);
        case StatValueType.Calories:
          return this.FormatCalories((int?) value, true);
        case StatValueType.HeartRate:
          return this.FormatHeartRate((int?) value);
        case StatValueType.Weight:
          return this.FormatWeight((Weight?) value, true);
        case StatValueType.ClimbRate:
          return this.FormatClimbRate((Speed?) value);
        default:
          Assert.EnumIsDefined<StatValueType>(valueType, "StatValueType");
          return Formatter.FormatValueStyledSpan(string.Empty);
      }
    }

    private StyledSpan FormatHeartRate(int? heartRate) => heartRate.HasValue ? Formatter.FormatHeartRate(heartRate.Value, true) : Formatter.NotAvailableStyledSpan;

    private StyledSpan FormatCount(int? count)
    {
      if (!count.HasValue)
        return Formatter.NotAvailableStyledSpan;
      return Formatter.FormatValueStyledSpan(string.Format((IFormatProvider) CultureInfo.CurrentCulture, AppResources.CountFormat, new object[1]
      {
        (object) count
      }));
    }

    private StyledSpan FormatDurationWithTextWithoutSeconds(
      TimeSpan? duration,
      bool showNotAvailableOnZero)
    {
      return !duration.HasValue || duration.Value == TimeSpan.Zero & showNotAvailableOnZero ? Formatter.NotAvailableStyledSpan : Formatter.FormatTimeSpan(duration.Value, Formatter.TimeSpanFormat.OneChar, false);
    }

    private StyledSpan FormatDurationWithText(
      TimeSpan? duration,
      bool showNotAvailableOnZero)
    {
      return !duration.HasValue || duration.Value == TimeSpan.Zero & showNotAvailableOnZero ? Formatter.NotAvailableStyledSpan : Formatter.FormatTimeSpan(duration.Value, Formatter.TimeSpanFormat.OneChar);
    }

    private StyledSpan FormatDouble(double? d, bool showNotAvailableOnZero) => !d.HasValue || d.Value == 0.0 & showNotAvailableOnZero ? Formatter.NotAvailableStyledSpan : Formatter.FormatValueStyledSpan(d.Value.ToString("n", (IFormatProvider) CultureInfo.CurrentCulture));

    private StyledSpan FormatInteger(int? i, bool showNotAvailableOnZero) => !i.HasValue || i.Value == 0 & showNotAvailableOnZero ? Formatter.NotAvailableStyledSpan : Formatter.FormatValueStyledSpan(i.Value.ToString("n0", (IFormatProvider) CultureInfo.CurrentCulture));

    private StyledSpan FormatTimeStat(DateTimeOffset? time) => time.HasValue && time.Value.Year >= 1900 ? Formatter.FormatShortTimeString(time.Value) : Formatter.NotAvailableStyledSpan;

    public StyledSpan FormatCalories(int? calories, bool appendUnit = false) => Formatter.FormatCalories(calories, appendUnit);

    public string FormatTileTime(DateTimeOffset time) => Formatter.FormatTileTime(time);

    public string GetMonthDayFormatString() => Formatter.GetMonthDayFormatString();

    public StyledSpan FormatGolfScore(int score, int differenceFromPar) => Formatter.FormatGolfScore(score, differenceFromPar);

    public StyledSpan FormatWeight(Weight? weight, bool absoluteValue = true)
    {
      if (!weight.HasValue)
        return Formatter.NotAvailableStyledSpan;
      if (absoluteValue)
        weight = new Weight?(Weight.FromGrams(Math.Abs(weight.Value.TotalGrams)));
      return Formatter.FormatWeight(weight.Value, this.MassUnitType, true);
    }

    public string FormatTimeIntervalMonthDay(DateTimeOffset begin, DateTimeOffset end) => Formatter.GetMonthDayString(begin) + " - " + Formatter.GetMonthDayString(end);

    public StyledSpan FormatSocialMetric(double metric, string localizedUnits) => Formatter.FormatSocialMetric(metric, localizedUnits);

    public StyledSpan FormatCardioBonusMinutes(int cardioMinutes, bool appendUnit = true) => Formatter.FormatCardioBonusMinutes(cardioMinutes, appendUnit);

    public string FormatDurationValue(TimeSpan duration) => (string) Formatter.FormatTimeSpanNoText(new TimeSpan?(duration));

    public string FormatDurationUnit() => (string) null;

    public string FormatDistanceYardMeterValue(Length distance) => (string) this.FormatDistanceMetersOrYards(new Length?(distance), false);

    public string FormatDistanceYardMeterUnit()
    {
      switch (this.DistanceUnitType)
      {
        case DistanceUnitType.Metric:
          return AppResources.MeterDistanceUnitAbbr;
        default:
          return AppResources.YardDistanceUnitAbbr;
      }
    }

    public string FormatDistanceMileKMValue(Length distance) => (string) this.FormatDistance(new Length?(distance), 2, false, true);

    public string FormatDistanceMileKMUnit()
    {
      switch (this.DistanceUnitType)
      {
        case DistanceUnitType.Metric:
          return AppResources.KilometersAbbreviation;
        default:
          return AppResources.MilesAbbreviation;
      }
    }

    public string ShareMetricFormatPaceUnit() => (string) null;

    public string FormatSpeedValue(Speed speed) => (string) this.FormatSpeed(new Speed?(speed), false, false);

    public string FormatSpeedUnit() => Formatter.GetShortSpeedUnit(this.DistanceUnitType);

    public string FormatHeartRateValue(int heartRate) => (string) Formatter.FormatHeartRate(heartRate);

    public string FormatHeartRateUnit() => AppResources.BeatsPerMinuteAbbrString;

    public string FormatCaloriesValue(int calories) => (string) Formatter.FormatCalories(new int?(calories));

    public string FormatCaloriesUnit() => (string) null;

    public string FormatElevationUnit() => this.DistanceUnitType != DistanceUnitType.Metric ? AppResources.FeetAbbreviation : AppResources.MeterDistanceUnitAbbr;

    public string FormatPercentageValue(double percentage) => Math.Round(percentage, 0).ToString();

    public string FormatPercentageUnit() => NumberFormatInfo.CurrentInfo.PercentSymbol;

    public string FormatCountValue(int count) => count.ToString();

    public string FormatCountUnit() => AppResources.CountUnit;

    public string FormatFitnessBenefitValue(string fitnessBenefitMessage) => !string.IsNullOrWhiteSpace(fitnessBenefitMessage) ? fitnessBenefitMessage : AppResources.NotAvailable;

    public StyledSpan FormatClimbRate(Speed? climbRate) => climbRate.HasValue ? Formatter.FormatClimbRate(climbRate.Value, 2, this.DistanceUnitType, true) : Formatter.NotAvailableStyledSpan;

    public string FormatClimbRateValue(Speed climbRate) => (string) Formatter.FormatClimbRate(climbRate, 2, this.DistanceUnitType, false);
  }
}
