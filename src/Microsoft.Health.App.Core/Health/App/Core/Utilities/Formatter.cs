// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Formatter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class Formatter
  {
    public static readonly StyledSpan NotAvailableStyledSpan = Formatter.FormatValueStyledSpan(AppResources.NotAvailable);

    public static StyledSpan FormatDistance(
      Length distance,
      DistanceUnitType unit,
      int digits = 2,
      bool appendUnit = false,
      bool abbreviateUnit = true,
      bool useLockedResource = false)
    {
      double num1 = Formatter.IsMetric(unit) ? distance.TotalKilometers : distance.TotalMiles;
      int num2 = (int) Math.Pow(10.0, (double) digits);
      string str1 = (Math.Truncate(num1 * (double) num2) / (double) num2).ToString("n" + (object) digits);
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str1);
      string str2 = abbreviateUnit ? Formatter.GetShortDistanceUnit(unit, useLockedResource) : Formatter.GetLongDistanceUnit(unit);
      return new StyledSpan(AppResources.DistanceValueUnitStyledFormat, new object[2]
      {
        (object) str1,
        (object) str2
      });
    }

    public static StyledSpan FormatDistanceDynamic(
      Length? distance,
      DistanceUnitType unit,
      bool appendUnit = false,
      bool useLockedResource = false)
    {
      if (!distance.HasValue)
        return Formatter.NotAvailableStyledSpan;
      string dynamicWithoutUnit = Formatter.GetDistanceDynamicWithoutUnit(Formatter.IsMetric(unit) ? distance.Value.TotalKilometers : distance.Value.TotalMiles);
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(dynamicWithoutUnit);
      return new StyledSpan(AppResources.DistanceValueUnitStyledFormat, new object[2]
      {
        (object) dynamicWithoutUnit,
        (object) Formatter.GetShortDistanceUnit(unit, useLockedResource)
      });
    }

    public static StyledSpan FormatDistanceMetersOrYards(
      Length distance,
      DistanceUnitType unit,
      bool appendUnit = false)
    {
      string str = (Formatter.IsMetric(unit) ? distance.TotalMeters : distance.TotalYards).ToString("n0");
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str);
      return new StyledSpan(AppResources.DistanceStyledFormat, new object[2]
      {
        (object) str,
        (object) Formatter.GetShortMinorDistanceUnitMeterorYard(unit)
      });
    }

    public static StyledSpan FormatDistanceMetersOrFeet(
      Length distance,
      DistanceUnitType unit,
      bool appendUnit = false,
      bool useLockedResource = false)
    {
      string str = (Formatter.IsMetric(unit) ? distance.TotalMeters : distance.TotalFeet).ToString("n0");
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str);
      return new StyledSpan(useLockedResource ? AppResources.DistanceStyledFormatLocked : AppResources.DistanceStyledFormat, new object[2]
      {
        (object) str,
        (object) Formatter.GetShortMinorDistanceUnitMeterOrFeet(unit, useLockedResource)
      });
    }

    private static string GetDistanceDynamicWithoutUnit(double value)
    {
      value = Math.Truncate(value * 100.0) / 100.0;
      int num = value < 100.0 ? (value < 10.0 ? 2 : 1) : 0;
      return value.ToString("n" + (object) num);
    }

    public static StyledSpan FormatSplitDistanceHeader(
      Length distance,
      DistanceUnitType unit)
    {
      return new StyledSpan(AppResources.DistanceStyledFormat, new object[2]
      {
        (object) (Math.Truncate((Formatter.IsMetric(unit) ? distance.TotalKilometers : distance.TotalMiles) * 100.0) / 100.0).ToString("g"),
        (object) Formatter.GetShortDistanceUnit(unit)
      });
    }

    public static StyledSpan FormatSplitDistance(
      Length distance,
      int digits,
      DistanceUnitType unit)
    {
      double num1 = Formatter.IsMetric(unit) ? distance.TotalKilometers : distance.TotalMiles;
      int num2 = (int) Math.Pow(10.0, (double) digits);
      return new StyledSpan(AppResources.SplitDistanceStyledFormat, new object[2]
      {
        (object) (Math.Truncate(num1 * (double) num2) / (double) num2).ToString("g"),
        (object) Formatter.GetShortDistanceUnit(unit)
      });
    }

    public static string GetFullDistanceUnit(DistanceUnitType unit) => !Formatter.IsMetric(unit) ? AppResources.Miles : AppResources.Kilometers;

    public static string GetShortDistanceUnit(DistanceUnitType unit, bool useLockedResource = false) => Formatter.IsMetric(unit) ? (!useLockedResource ? AppResources.KilometersAbbreviation : AppResources.KilometersAbbreviationLocked) : (!useLockedResource ? AppResources.MilesAbbreviation : AppResources.MilesAbbreviationLocked);

    public static string GetShortMassUnit(MassUnitType unit, bool useLockedResource = false) => Formatter.IsMetric(unit) ? AppResources.KilogramsAbbreviation : AppResources.PoundsPluralAbbreviation;

    public static string GetShortMinorDistanceUnitMeterorYard(DistanceUnitType unit) => !Formatter.IsMetric(unit) ? AppResources.YardDistanceUnitAbbr : AppResources.MeterDistanceUnitAbbr;

    public static string GetShortMinorDistanceUnitMeterOrFeet(
      DistanceUnitType unit,
      bool useLockedResource = false)
    {
      return Formatter.IsMetric(unit) ? (!useLockedResource ? AppResources.MeterDistanceUnitAbbr : AppResources.MeterDistanceUnitAbbrLocked) : (!useLockedResource ? AppResources.FeetAbbreviation : AppResources.FeetAbbreviationLocked);
    }

    public static string GetShortSpeedUnit(DistanceUnitType unit) => !Formatter.IsMetric(unit) ? AppResources.MilesPerHourAbbreviation : AppResources.KilometersPerHourAbbreviation;

    public static string GetLongDistanceUnit(DistanceUnitType unit) => !Formatter.IsMetric(unit) ? AppResources.Miles : AppResources.Kilometers;

    public static string GetShortElevationUnit(DistanceUnitType unit) => !Formatter.IsMetric(unit) ? AppResources.FeetAbbreviation : AppResources.MeterDistanceUnitAbbr;

    public static StyledSpan FormatPace(Speed speed, DistanceUnitType unit) => Formatter.FormatTimeTicks(new TimeSpan?(TimeSpan.FromMinutes(Formatter.IsMetric(unit) ? speed.MinutesPerKilometer : speed.MinutesPerMile)));

    public static StyledSpan FormatPaceUnit(DistanceUnitType unit) => new StyledSpan(AppResources.PerUnitStyledFormat, new object[1]
    {
      (object) Formatter.GetShortDistanceUnit(unit)
    });

    public static StyledSpan FormatTimeTicks(TimeSpan? time)
    {
      if (time.HasValue)
      {
        int totalMinutes = (int) time.Value.TotalMinutes;
        if (totalMinutes < 1000)
        {
          string empty = string.Empty;
          string str1;
          if (totalMinutes > 0)
          {
            string str2;
            if (totalMinutes >= 100)
              str2 = string.Format(AppResources.ElapsedTimeAsTickOnlyMinutes, new object[1]
              {
                (object) totalMinutes
              });
            else
              str2 = string.Format(AppResources.ElapsedTimeAsTickWithMinutes, new object[2]
              {
                (object) totalMinutes,
                (object) time.Value.Seconds
              });
            str1 = str2;
          }
          else
          {
            string str3;
            if (time.Value.Seconds <= 0)
              str3 = AppResources.NotAvailable;
            else
              str3 = string.Format(AppResources.ElapsedTimeAsTickOnlySeconds, new object[1]
              {
                (object) time.Value.Seconds
              });
            str1 = str3;
          }
          return Formatter.FormatValueStyledSpan(str1);
        }
      }
      return Formatter.NotAvailableStyledSpan;
    }

    public static StyledSpan FormatSpeed(
      Speed speed,
      DistanceUnitType unit,
      bool appendUnit = false)
    {
      string str1 = Formatter.FormatDouble(Formatter.IsMetric(unit) ? speed.KilometersPerHour : speed.MilesPerHour, 1);
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str1);
      string str2 = Formatter.IsMetric(unit) ? AppResources.KilometersPerHourAbbreviation : AppResources.MilesPerHourAbbreviation;
      return new StyledSpan(AppResources.SpeedStyledFormat, new object[2]
      {
        (object) str1,
        (object) str2
      });
    }

    public static string FormatSpeedUnstyled(Speed speed, DistanceUnitType unit) => string.Format(AppResources.SpeedFormat, new object[2]
    {
      (object) Formatter.FormatDouble(Formatter.IsMetric(unit) ? speed.KilometersPerHour : speed.MilesPerHour, 1),
      (object) (Formatter.IsMetric(unit) ? AppResources.KilometersPerHourAbbreviation : AppResources.MilesPerHourAbbreviation)
    });

    public static StyledSpan FormatElevation(
      Length length,
      DistanceUnitType unit,
      bool appendUnit = false)
    {
      string str1 = Math.Floor(Formatter.IsMetric(unit) ? length.TotalMeters : length.TotalFeet).ToString("n0", (IFormatProvider) CultureInfo.CurrentCulture);
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str1);
      string str2 = Formatter.IsMetric(unit) ? AppResources.MeterDistanceUnitAbbr : AppResources.FeetAbbreviation;
      return new StyledSpan(AppResources.ElevationStyledFormat, new object[2]
      {
        (object) str1,
        (object) str2
      });
    }

    public static StyledSpan FormatDurationMinutesLocked(TimeSpan duration) => Formatter.FormatTimeSpan(duration, Formatter.TimeSpanFormat.Abbreviated, false, true);

    public static string FormatElevationUnstyled(Length length, DistanceUnitType unit) => string.Format(AppResources.ElevationUnstyledFormat, new object[2]
    {
      (object) Math.Floor(Formatter.IsMetric(unit) ? length.TotalMeters : length.TotalFeet).ToString("n0", (IFormatProvider) CultureInfo.CurrentCulture),
      (object) (Formatter.IsMetric(unit) ? AppResources.MeterDistanceUnitAbbr : AppResources.FeetAbbreviation)
    });

    public static StyledSpan ToStringAsPercentage(int? percent)
    {
      if (!percent.HasValue)
        return Formatter.NotAvailableStyledSpan;
      string str = string.Format("{0:P0}", new object[1]
      {
        (object) ((double) percent.Value / 100.0)
      });
      string percentSymbol = NumberFormatInfo.CurrentInfo.PercentSymbol;
      Assert.IsTrue(str.StartsWith(percentSymbol) || str.EndsWith(percentSymbol), "The percent symbol should be at the beginning or at the end of the string.");
      StyledRun styledRun = new StyledRun(percentSymbol, StyledRunType.Unit);
      StyledRun[] styledRunArray = new StyledRun[2];
      if (str.StartsWith(percentSymbol))
      {
        styledRunArray[0] = styledRun;
        styledRunArray[1] = new StyledRun(str.Substring(percentSymbol.Length), StyledRunType.Value);
      }
      else if (str.EndsWith(percentSymbol))
      {
        styledRunArray[0] = new StyledRun(str.Substring(0, str.Length - percentSymbol.Length), StyledRunType.Value);
        styledRunArray[1] = styledRun;
      }
      return new StyledSpan(styledRunArray);
    }

    public static StyledSpan FormatCalories(int? calories, bool appendUnit = false)
    {
      if (!calories.HasValue)
        return Formatter.NotAvailableStyledSpan;
      string str1 = calories.Value.ToString("n0");
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str1);
      string str2 = Formatter.SingularOrPlural(calories.Value, AppResources.CaloriesAbbrStringSingular, AppResources.CaloriesAbbrString);
      return new StyledSpan(AppResources.CaloriesValueUnitStyledFormat, new object[2]
      {
        (object) str1,
        (object) str2
      });
    }

    public static StyledSpan FormatHeartRate(int heartRate, bool appendUnit = false)
    {
      if (heartRate == 0)
        return Formatter.NotAvailableStyledSpan;
      string str = heartRate.ToString("n0");
      string minuteAbbrString = AppResources.BeatsPerMinuteAbbrString;
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str);
      return new StyledSpan(AppResources.HeartRateStyledFormat, new object[2]
      {
        (object) str,
        (object) minuteAbbrString
      });
    }

    public static StyledSpan FormatSteps(int steps, bool appendUnit = false)
    {
      string str1 = steps.ToString("n0");
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str1);
      string str2 = Formatter.SingularOrPlural(steps, AppResources.StepsSingular, AppResources.StepsPlural);
      return new StyledSpan(AppResources.StepsValueUnitStyledFormat, new object[2]
      {
        (object) str1,
        (object) str2
      });
    }

    public static string FormatGoalValue(int goalValue) => goalValue.ToString("N0");

    public static string FormatTime(DateTimeOffset time) => time.ToString("t");

    public static StyledSpan FormatTimeLower(DateTimeOffset time)
    {
      string text = Formatter.FormatTime(time).Replace(" ", string.Empty);
      string str = time.DateTime.ToString("tt").Replace(" ", string.Empty);
      string lower = str.ToLower();
      int num = text.IndexOf(str);
      Assert.IsTrue(num < 0 || text.StartsWith(str) || text.EndsWith(str), "The designator is located at unexpected location.");
      List<StyledRun> styledRunList = new List<StyledRun>();
      if (num < 0 || string.IsNullOrEmpty(str))
        styledRunList.Add(new StyledRun(text, StyledRunType.Value));
      else if (text.StartsWith(str))
      {
        styledRunList.Add(new StyledRun(lower, StyledRunType.Unit));
        styledRunList.Add(new StyledRun(text.Substring(str.Length), StyledRunType.Value));
      }
      else if (text.EndsWith(str))
      {
        styledRunList.Add(new StyledRun(text.Substring(0, text.Length - str.Length), StyledRunType.Value));
        styledRunList.Add(new StyledRun(lower, StyledRunType.Unit));
      }
      return new StyledSpan((IEnumerable<StyledRun>) styledRunList);
    }

    public static string FormatTileTime(DateTimeOffset time, bool convertToLocalTime = true) => convertToLocalTime ? time.ToLocalTime().ToString("ddd " + Formatter.GetMonthDayFormatString(), (IFormatProvider) CultureInfo.CurrentCulture) : time.ToString("ddd " + Formatter.GetMonthDayFormatString(), (IFormatProvider) CultureInfo.CurrentCulture);

    public static string FormatSleepTime(DateTimeOffset time, bool tileTime = false)
    {
      if (time.Hour < 5)
        time = time.Subtract(TimeSpan.FromDays(1.0));
      return tileTime ? Formatter.FormatTileTime(time) : Formatter.GetMonthDayString(time);
    }

    internal static StyledSpan FormatCardioBonusMinutes(
      int cardioMinutes,
      bool appendUnit)
    {
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(cardioMinutes.ToString());
      return new StyledSpan(AppResources.CoachingPlanActivityBonusMinutesStyledFormat, new object[1]
      {
        (object) cardioMinutes
      });
    }

    internal static string FormatCardioMinutes(int cardioMinutes) => cardioMinutes.ToString();

    public static StyledSpan FormatGolfScore(int score, int differenceFromPar)
    {
      string empty = string.Empty;
      return new StyledSpan(differenceFromPar <= 0 ? AppResources.GolfCourseUnderParValueUnitStyledFormat : AppResources.GolfCourseOverParValueUnitStyledFormat, new object[2]
      {
        (object) score,
        (object) differenceFromPar
      });
    }

    public static StyledSpan FormatWeight(
      Weight weight,
      MassUnitType unit,
      bool appendUnit = false)
    {
      string str1 = (Formatter.IsMetric(unit) ? weight.TotalKilograms : weight.TotalPounds).ToString("N1");
      if (!appendUnit)
        return Formatter.FormatValueStyledSpan(str1);
      string str2 = Formatter.IsMetric(unit) ? AppResources.KilogramsAbbreviation : AppResources.PoundsPluralAbbreviation;
      return new StyledSpan(AppResources.WeightStyledFormat, new object[2]
      {
        (object) str1,
        (object) str2
      });
    }

    public static StyledSpan FormatSocialMetric(double metric, string localizedUnits) => new StyledSpan(AppResources.ValuePlusUnitLabelStyledFormat, new object[2]
    {
      (object) metric.ToString("N0"),
      (object) localizedUnits
    });

    public static string FormatTrackerWeekday(DateTimeOffset time) => time.ToLocalTime().ToString("ddd");

    public static StyledSpan FormatTimeSpan(
      TimeSpan time,
      Formatter.TimeSpanFormat format,
      bool includeSeconds = true,
      bool useLockedResource = false)
    {
      if (format == Formatter.TimeSpanFormat.NoText)
        return Formatter.FormatValueStyledSpan((string) Formatter.FormatTimeSpanNoText(new TimeSpan?(time), includeSeconds));
      int totalHours = (int) time.TotalHours;
      int minutes = time.Minutes;
      int seconds = time.Seconds;
      if (!includeSeconds && time.TotalMinutes < 1.0 && time > TimeSpan.Zero)
        minutes = 1;
      string hoursLabel;
      string minutesLabel;
      string secondsLabel;
      string labelFormat;
      Formatter.FormatTimeSpanLabels(totalHours, minutes, seconds, format, useLockedResource, out hoursLabel, out minutesLabel, out secondsLabel, out labelFormat);
      return time.TotalHours >= 1.0 ? (minutes == 0 ? new StyledSpan(labelFormat, new object[2]
      {
        (object) totalHours,
        (object) hoursLabel
      }) : new StyledSpan(useLockedResource ? AppResources.TimeSpanFormatLockedStyledFormat : AppResources.TimeSpanStyledFormat, new object[2]
      {
        (object) new StyledSpan(labelFormat, new object[2]
        {
          (object) totalHours,
          (object) hoursLabel
        }),
        (object) new StyledSpan(labelFormat, new object[2]
        {
          (object) minutes,
          (object) minutesLabel
        })
      })) : (time.TotalMinutes >= 1.0 ? (seconds == 0 || !includeSeconds ? new StyledSpan(labelFormat, new object[2]
      {
        (object) minutes,
        (object) minutesLabel
      }) : new StyledSpan(useLockedResource ? AppResources.TimeSpanFormatLockedStyledFormat : AppResources.TimeSpanStyledFormat, new object[2]
      {
        (object) new StyledSpan(labelFormat, new object[2]
        {
          (object) minutes,
          (object) minutesLabel
        }),
        (object) new StyledSpan(labelFormat, new object[2]
        {
          (object) seconds,
          (object) secondsLabel
        })
      })) : (time.TotalSeconds >= 1.0 & includeSeconds ? new StyledSpan(labelFormat, new object[2]
      {
        (object) seconds,
        (object) secondsLabel
      }) : (includeSeconds ? new StyledSpan(labelFormat, new object[2]
      {
        (object) 0,
        (object) minutesLabel
      }) : new StyledSpan(labelFormat, new object[2]
      {
        (object) 0,
        (object) hoursLabel
      }))));
    }

    private static void FormatTimeSpanLabels(
      int hours,
      int minutes,
      int seconds,
      Formatter.TimeSpanFormat format,
      bool useLockedResource,
      out string hoursLabel,
      out string minutesLabel,
      out string secondsLabel,
      out string labelFormat)
    {
      switch (format)
      {
        case Formatter.TimeSpanFormat.OneChar:
          hoursLabel = AppResources.HourSuffix;
          minutesLabel = AppResources.MinuteSuffix;
          secondsLabel = AppResources.SecondSuffix;
          break;
        case Formatter.TimeSpanFormat.Abbreviated:
          hoursLabel = AppResources.HourAbbreviation;
          minutesLabel = AppResources.MinuteAbbreviation;
          secondsLabel = AppResources.SecondAbbreviation;
          break;
        case Formatter.TimeSpanFormat.Full:
          hoursLabel = Formatter.SingularOrPlural(hours, useLockedResource ? AppResources.HourSingularFullLocked : AppResources.HourSingularFull, useLockedResource ? AppResources.HourPluralFullLocked : AppResources.HourPluralFull);
          minutesLabel = Formatter.SingularOrPlural(minutes, useLockedResource ? AppResources.MinuteSingularFullLocked : AppResources.MinuteSingularFull, useLockedResource ? AppResources.MinutePluralFullLocked : AppResources.MinutePluralFull);
          secondsLabel = Formatter.SingularOrPlural(seconds, useLockedResource ? AppResources.SecondSingularFullLocked : AppResources.SecondSingularFull, useLockedResource ? AppResources.SecondPluralFullLocked : AppResources.SecondPluralFull);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (format));
      }
      if (format == Formatter.TimeSpanFormat.OneChar)
        labelFormat = AppResources.ValuePlusUnitLabelTimeSpanOneCharStyledFormat;
      else
        labelFormat = AppResources.ValuePlusUnitLabelStyledFormat;
    }

    public static StyledSpan FormatTimeSpanNoText(
      TimeSpan? time,
      bool includeSeconds = true,
      bool useLeadingZero = false)
    {
      if (!time.HasValue)
        return Formatter.NotAvailableStyledSpan;
      string empty = string.Empty;
      string str;
      if (!includeSeconds)
      {
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        object[] objArray = new object[2];
        TimeSpan timeSpan = time.Value;
        objArray[0] = (object) (int) timeSpan.TotalHours;
        timeSpan = time.Value;
        objArray[1] = (object) timeSpan.Minutes;
        str = string.Format((IFormatProvider) currentCulture, "{0}:{1:d2}", objArray);
      }
      else if (time.Value.TotalHours >= 1.0)
      {
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        object[] objArray = new object[3];
        TimeSpan timeSpan = time.Value;
        objArray[0] = (object) (int) timeSpan.TotalHours;
        timeSpan = time.Value;
        objArray[1] = (object) timeSpan.Minutes;
        timeSpan = time.Value;
        objArray[2] = (object) timeSpan.Seconds;
        str = string.Format((IFormatProvider) currentCulture, "{0}:{1:d2}:{2:d2}", objArray);
      }
      else
      {
        string format = useLeadingZero ? "mm\\:ss" : "m\\:ss";
        str = time.Value.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      return Formatter.FormatValueStyledSpan(str);
    }

    public static string FormatMonthDay(DateTimeOffset date) => date.ToString("M");

    public static string FormatShortDate(DateTimeOffset date) => date.ToString("d");

    public static string FormatDateAndTime(DateTimeOffset date) => date.ToString("G");

    public static string FormatSyncTime(DateTimeOffset date) => date.ToString("d MMMM hh:mm tt");

    public static string GetMonthDayFormatString()
    {
      string str = Regex.Replace(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, "y", string.Empty);
      if (str.StartsWith("/") || str.StartsWith("-") || str.StartsWith(".") || str.StartsWith(","))
        str = str.Substring(1);
      if (str.EndsWith("/") || str.EndsWith("-") || str.StartsWith(".") || str.StartsWith(","))
        str = str.Substring(0, str.Length - 1);
      return str;
    }

    public static string GetMonthNameDayFormatString() => Regex.Replace(CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern, "MMMM", "MMM");

    public static string GetMonthNameDayString(DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString(Formatter.GetMonthNameDayFormatString());

    public static string GetMonthNameShortDayFormatString() => Regex.Replace(Regex.Replace(CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern, "MMMM", "MMM"), "dd", "d");

    public static string GetMonthNameShortDayString(DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString(Formatter.GetMonthNameShortDayFormatString());

    public static StyledSpan FormatShortTimeString(DateTimeOffset dateTimeOffset) => new StyledSpan(AppResources.ValuePlusUnitLabelTimeSpanOneCharStyledFormat, new object[2]
    {
      (object) Formatter.GetHoursMinutesString(dateTimeOffset),
      (object) Formatter.GetSingleCharacterAMOrPMString(dateTimeOffset)
    });

    public static string GetHoursMinutesString(DateTimeOffset dateTimeOffset)
    {
      string format = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt", string.Empty).Trim(' ');
      return dateTimeOffset.ToLocalTime().ToString(format);
    }

    public static string GetMonthDayString(DateTimeOffset dateTimeOffset, bool convertToLocalTime = true) => convertToLocalTime ? dateTimeOffset.ToLocalTime().ToString(Formatter.GetMonthDayFormatString()) : dateTimeOffset.ToString(Formatter.GetMonthDayFormatString());

    public static string GetFriendlyMonthDayString(DateTimeOffset dateTimeOffset)
    {
      dateTimeOffset = dateTimeOffset.ToLocalTime();
      if (dateTimeOffset.Date == DateTime.Today)
        return AppResources.TodayDateString;
      return dateTimeOffset.Date == DateTime.Today.AddDays(-1.0) ? AppResources.YesterdayDateString : Formatter.GetMonthDayString(dateTimeOffset);
    }

    public static string GetDateOnlyString(DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString("%d");

    public static string GetSingleCharacterAMOrPMString(DateTimeOffset dateTimeOffset) => RegionUtilities.Is24HourTime ? string.Empty : dateTimeOffset.ToLocalTime().ToString("%t").ToLowerInvariant();

    public static string GetTimeStringWithoutSpaceBeforeAMorPM(string str) => str.Replace(" ", string.Empty);

    public static string FormatTimeWithSingleCharacterAMOrPM(
      DateTimeOffset dateTimeOffset,
      bool includeMonthDay = false)
    {
      string oldValue = dateTimeOffset.ToString("tt");
      string str = Formatter.FormatTime(dateTimeOffset);
      if (!string.IsNullOrEmpty(oldValue))
      {
        string newValue = dateTimeOffset.ToString("%t");
        str = Formatter.GetTimeStringWithoutSpaceBeforeAMorPM(str).Replace(oldValue, newValue).ToLowerInvariant();
      }
      if (!includeMonthDay)
        return str;
      return string.Format(AppResources.DateAndTimeString, new object[2]
      {
        (object) Formatter.GetMonthDayString(dateTimeOffset),
        (object) str
      });
    }

    public static string FormatTimeWithAMOrPM(DateTimeOffset dateTimeOffset) => Formatter.GetTimeStringWithoutSpaceBeforeAMorPM(Formatter.FormatTime(dateTimeOffset)).ToLowerInvariant();

    public static string FormatDouble(double number, int digits)
    {
      int num = (int) Math.Pow(10.0, (double) digits);
      return (Math.Truncate(number * (double) num) / (double) num).ToString("n" + (object) digits);
    }

    public static string FormatStatDateParameter(DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString("yyyy-MM-dd");

    public static string FormatDateWithFullYear(DateTimeOffset dateTimeOffset)
    {
      DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
      return dateTimeOffset.ToString("d", (IFormatProvider) dateTimeFormat);
    }

    public static string SingularOrPlural(int count, string singular, string plural) => count != 1 ? plural : singular;

    public static string FormatDateForLabel(DateTimeOffset dateTimeOffset)
    {
      string monthDayString = Formatter.GetMonthDayString(dateTimeOffset);
      return string.Format("{0}\n{1}", new object[2]
      {
        (object) dateTimeOffset.ToString("ddd"),
        Formatter.IsToday(dateTimeOffset) ? (object) AppResources.Today : (object) monthDayString
      });
    }

    public static string FormatExerciseTimeLabel(double timeMs, bool usePartialMinutes)
    {
      string str;
      if (usePartialMinutes)
        str = string.Format("{0:0.0}", new object[1]
        {
          (object) (Math.Round(TimeSpan.FromMilliseconds(timeMs).TotalMinutes * 2.0, MidpointRounding.AwayFromZero) / 2.0)
        });
      else
        str = string.Format("{0}", new object[1]
        {
          (object) Math.Round(TimeSpan.FromMilliseconds(timeMs).TotalMinutes)
        });
      return string.Format(AppResources.Charts_MinuteValueCompositeFormatString, new object[1]
      {
        (object) str
      });
    }

    private static bool IsToday(DateTimeOffset dateTime)
    {
      DateTimeOffset localTime = DateTimeOffset.Now.ToLocalTime();
      DateTimeOffset dateTime1 = localTime.AddDays(1.0);
      DateTimeOffset dateTimeOffset1 = localTime.RoundDown(TimeSpan.FromDays(1.0));
      TimeSpan interval = TimeSpan.FromDays(1.0);
      DateTimeOffset dateTimeOffset2 = dateTime1.RoundDown(interval);
      if (dateTime == dateTimeOffset1)
        return true;
      return dateTime > dateTimeOffset1 && dateTime < dateTimeOffset2;
    }

    private static bool IsMetric(DistanceUnitType unit) => unit == DistanceUnitType.Metric;

    private static bool IsMetric(MassUnitType unit) => unit == MassUnitType.Metric;

    public static StyledSpan FormatValueStyledSpan(string value) => Formatter.FormatStyledSpan(value, StyledRunType.Value);

    public static StyledSpan FormatSmallTextStyledSpan(string value) => Formatter.FormatStyledSpan(value, StyledRunType.SmallText);

    private static StyledSpan FormatStyledSpan(string value, StyledRunType type) => new StyledSpan(new StyledRun[1]
    {
      new StyledRun(value, type)
    });

    public static StyledSpan FormatClimbRate(
      Speed climbSpeed,
      int digits,
      DistanceUnitType unit,
      bool appendUnit)
    {
      bool flag = Formatter.IsMetric(unit);
      string str1;
      if (!flag)
        str1 = string.Format(AppResources.RatioFormatString, new object[2]
        {
          (object) AppResources.FeetAbbreviation,
          (object) AppResources.HourAbbreviation
        });
      else
        str1 = string.Format(AppResources.RatioFormatString, new object[2]
        {
          (object) AppResources.MeterDistanceUnitAbbr,
          (object) AppResources.HourAbbreviation
        });
      string str2 = str1;
      return new StyledSpan(AppResources.SpeedStyledFormat, new object[2]
      {
        (object) (flag ? climbSpeed.MetersPerHour : climbSpeed.FeetPerHour).ToString(string.Format("N{0}", new object[1]
        {
          (object) digits
        })),
        appendUnit ? (object) str2 : (object) string.Empty
      });
    }

    public static StyledSpan FormatClimbRateUnit(DistanceUnitType distanceUnitType)
    {
      string format;
      if (!Formatter.IsMetric(distanceUnitType))
        format = string.Format(AppResources.RatioFormatString, new object[2]
        {
          (object) AppResources.FeetAbbreviation,
          (object) AppResources.HourAbbreviation
        });
      else
        format = string.Format(AppResources.RatioFormatString, new object[2]
        {
          (object) AppResources.MeterDistanceUnitAbbr,
          (object) AppResources.HourAbbreviation
        });
      object[] objArray = new object[0];
      return new StyledSpan(format, objArray);
    }

    public enum TimeSpanFormat
    {
      OneChar,
      Abbreviated,
      Full,
      NoText,
    }
  }
}
