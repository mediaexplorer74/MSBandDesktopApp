// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.ChartExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class ChartExtensions
  {
    public static IList<DistanceChartPoint> GetSpeedData(
      this RouteBasedExerciseEvent routeBasedEvent,
      IUserProfileService userProfileService,
      int movingAvgPoints = 1)
    {
      DistanceUnitType unitType = userProfileService.DistanceUnitType;
      return ChartExtensions.GetDistanceChartPoints(routeBasedEvent, userProfileService, (Func<MapPoint, double?>) (p => p.Speed > Speed.Zero ? new double?(ChartUtilities.ConvertSpeedToUserUnits(p.Speed, unitType)) : new double?()), movingAvgPoints);
    }

    public static IList<DistanceChartPoint> GetPaceData(
      this RouteBasedExerciseEvent routeBasedEvent,
      IUserProfileService userProfileService,
      int movingAvgPoints = 1)
    {
      DistanceUnitType unitType = userProfileService.DistanceUnitType;
      return ChartExtensions.GetDistanceChartPoints(routeBasedEvent, userProfileService, (Func<MapPoint, double?>) (p => p.Pace > Speed.Zero ? new double?(ChartUtilities.ConvertPaceToUserUnits(p.Pace, unitType)) : new double?()), movingAvgPoints);
    }

    public static IList<DistanceChartPoint> GetElevationData(
      this RouteBasedExerciseEvent routeBasedEvent,
      IUserProfileService userProfileService,
      bool isPace,
      int movingAvgPoints = 1,
      bool trimNullLocations = true)
    {
      DistanceUnitType unitType = userProfileService.DistanceUnitType;
      return ChartExtensions.GetDistanceChartPoints(routeBasedEvent, userProfileService, (Func<MapPoint, double?>) (p =>
      {
        if (!trimNullLocations && p.Location == null)
          return new double?(double.MinValue);
        Elevation elevation = p.Elevation;
        Length? absolute;
        int num;
        if (elevation == null)
        {
          num = 0;
        }
        else
        {
          absolute = elevation.Absolute;
          num = absolute.HasValue ? 1 : 0;
        }
        if (num == 0 || (isPace ? (p.Pace > Speed.Zero ? 1 : 0) : (p.Speed > Speed.Zero ? 1 : 0)) == 0)
          return new double?();
        absolute = p.Elevation.Absolute;
        return new double?(ChartUtilities.ConvertElevationToUserUnits(absolute.Value, unitType));
      }), movingAvgPoints);
    }

    public static IList<DistanceChartPoint> GetHikeElevationData(
      this HikeEvent hikeEvent,
      IUserProfileService userProfileService)
    {
      Length length = Length.FromMillimeters(hikeEvent.TotalDistance.TotalMillimeters * 0.04);
      IOrderedEnumerable<MapPoint> source = hikeEvent.MapPoints.OrderBy<MapPoint, Length>((Func<MapPoint, Length>) (s => s.ActualDistance));
      DistanceUnitType distanceUnitType = userProfileService.DistanceUnitType;
      bool flag = false;
      if (!source.Any<MapPoint>())
        return (IList<DistanceChartPoint>) new List<DistanceChartPoint>();
      List<DistanceChartPoint> distanceChartPointList = new List<DistanceChartPoint>();
      DistanceChartPoint distanceChartPoint1 = (DistanceChartPoint) null;
      int userGeneratedOrdinal = 0;
      foreach (MapPoint mapPoint in (IEnumerable<MapPoint>) source)
      {
        if (!mapPoint.IsPaused)
        {
          double? nullable = new double?();
          Elevation elevation = mapPoint.Elevation;
          Length? absolute;
          int num;
          if (elevation == null)
          {
            num = 0;
          }
          else
          {
            absolute = elevation.Absolute;
            num = absolute.HasValue ? 1 : 0;
          }
          if (num != 0 && mapPoint.Pace > Speed.Zero)
          {
            ref double? local = ref nullable;
            absolute = mapPoint.Elevation.Absolute;
            double userUnits = ChartUtilities.ConvertElevationToUserUnits(absolute.Value, distanceUnitType);
            local = new double?(userUnits);
          }
          if (nullable.HasValue)
          {
            DistanceChartPoint distanceChartPoint2 = new DistanceChartPoint();
            distanceChartPoint2.Distance = mapPoint.TotalDistance;
            distanceChartPoint2.Value = nullable.Value;
            distanceChartPoint2.ElapsedSeconds = mapPoint.SecondsSinceStart;
            distanceChartPoint2.ScaledDistance = ChartUtilities.ConvertDistanceToUserUnits(mapPoint.TotalDistance, distanceUnitType);
            distanceChartPoint2.ScaledPace = (double) mapPoint.ScaledPace;
            DistanceChartPoint distanceChartPoint3 = distanceChartPoint2;
            if (distanceChartPoint1 == null || mapPoint.TotalDistance - distanceChartPoint1.Distance >= length)
              distanceChartPoint1 = distanceChartPoint3;
            DistanceAnnotationType distanceAnnotationType1 = mapPoint.MapPointType.ToDistanceAnnotationType();
            DistanceAnnotationType distanceAnnotationType2 = mapPoint.PoiType.ToDistanceAnnotationType();
            object annotationDisplayValue1 = distanceAnnotationType1.ToDistanceAnnotationDisplayValue<HikeEvent>(ref userGeneratedOrdinal);
            object annotationDisplayValue2 = distanceAnnotationType2.ToDistanceAnnotationDisplayValue<HikeEvent>(ref userGeneratedOrdinal);
            if (annotationDisplayValue1 != null)
              distanceChartPoint1.Annotations.Add(new DistanceAnnotation(distanceAnnotationType1, annotationDisplayValue1));
            if (annotationDisplayValue2 != null)
              distanceChartPoint1.Annotations.Add(new DistanceAnnotation(distanceAnnotationType2, annotationDisplayValue2));
            distanceChartPointList.Add(distanceChartPoint3);
          }
        }
        else if (!flag && mapPoint.MapPointType != MapPointType.End)
        {
          distanceChartPoint1?.Annotations.Add(new DistanceAnnotation(DistanceAnnotationType.Pause, DistanceAnnotationType.Pause.ToDistanceAnnotationDisplayValue<HikeEvent>(ref userGeneratedOrdinal)));
          flag = true;
        }
        if (flag && mapPoint.IsResume)
          flag = false;
      }
      return (IList<DistanceChartPoint>) distanceChartPointList;
    }

    public static IList<DistanceChartPoint> GetHeartRateData(
      this RouteBasedExerciseEvent routeBasedEvent,
      IUserProfileService userProfileService,
      int movingAvgPoints = 1)
    {
      return ChartExtensions.GetDistanceChartPoints(routeBasedEvent, userProfileService, (Func<MapPoint, double?>) (p => new double?((double) p.HeartRate)), movingAvgPoints);
    }

    private static IList<DistanceChartPoint> GetDistanceChartPoints(
      RouteBasedExerciseEvent routeBasedEvent,
      IUserProfileService userProfileService,
      Func<MapPoint, double?> valueSelector,
      int movingAvgPoints = 1)
    {
      List<MapPoint> source1 = new List<MapPoint>();
      if (routeBasedEvent.MapPoints != null)
        source1.AddRange((IEnumerable<MapPoint>) routeBasedEvent.MapPoints);
      IOrderedEnumerable<MapPoint> source2 = source1.OrderBy<MapPoint, Length>((Func<MapPoint, Length>) (s => s.ActualDistance));
      DistanceUnitType distanceUnitType = userProfileService.DistanceUnitType;
      if (movingAvgPoints < 1)
        movingAvgPoints = 1;
      LinkedList<double> source3 = new LinkedList<double>();
      bool flag = false;
      if (!source2.Any<MapPoint>())
        return (IList<DistanceChartPoint>) new List<DistanceChartPoint>();
      List<DistanceChartPoint> distanceChartPointList = new List<DistanceChartPoint>();
      DistanceChartPoint distanceChartPoint1 = (DistanceChartPoint) null;
      int userGeneratedOrdinal = 0;
      foreach (MapPoint mapPoint in (IEnumerable<MapPoint>) source2)
      {
        double? nullable = valueSelector(mapPoint);
        if (!mapPoint.IsPaused)
        {
          if (nullable.HasValue)
          {
            source3.AddFirst(nullable.Value);
            if (source3.Count > movingAvgPoints)
              source3.RemoveLast();
            DistanceChartPoint distanceChartPoint2 = new DistanceChartPoint();
            distanceChartPoint2.Distance = mapPoint.TotalDistance;
            distanceChartPoint2.Value = source3.Average();
            distanceChartPoint2.ElapsedSeconds = mapPoint.SecondsSinceStart;
            distanceChartPoint2.ScaledDistance = ChartUtilities.ConvertDistanceToUserUnits(mapPoint.TotalDistance, distanceUnitType);
            distanceChartPoint2.ScaledPace = (double) mapPoint.ScaledPace;
            DistanceChartPoint distanceChartPoint3 = distanceChartPoint2;
            DistanceAnnotationType distanceAnnotationType = mapPoint.MapPointType.ToDistanceAnnotationType();
            object annotationDisplayValue = distanceAnnotationType.ToDistanceAnnotationDisplayValue<RouteBasedExerciseEvent>(ref userGeneratedOrdinal);
            if (annotationDisplayValue != null)
              distanceChartPoint3.Annotations.Add(new DistanceAnnotation(distanceAnnotationType, annotationDisplayValue));
            distanceChartPoint1 = distanceChartPoint3;
            distanceChartPointList.Add(distanceChartPoint3);
          }
        }
        else if (!flag && mapPoint.MapPointType != MapPointType.End)
        {
          distanceChartPoint1?.Annotations.Add(new DistanceAnnotation(DistanceAnnotationType.Pause, DistanceAnnotationType.Pause.ToDistanceAnnotationDisplayValue<RouteBasedExerciseEvent>(ref userGeneratedOrdinal)));
          source3.Clear();
          flag = true;
        }
        if (flag && mapPoint.IsResume)
          flag = false;
      }
      return (IList<DistanceChartPoint>) distanceChartPointList;
    }

    public static IList<DateChartPoint> GetSleepHeartRateData(
      this SleepEvent sleepEvent,
      IUserProfileService userProfileService,
      int movingAvgPoints = 1)
    {
      List<DateChartPoint> dateChartPointList1 = new List<DateChartPoint>();
      if (sleepEvent == null || sleepEvent.Info == null || sleepEvent.Info.Count == 0)
        return (IList<DateChartPoint>) dateChartPointList1;
      if (movingAvgPoints < 1)
        movingAvgPoints = 0;
      LinkedList<double> source = new LinkedList<double>();
      bool flag = false;
      foreach (UserActivity userActivity in (IEnumerable<UserActivity>) sleepEvent.Info)
        userActivity.TimeOfDay = userActivity.TimeOfDay.ToLocalTime();
      List<UserActivity> list = sleepEvent.Info.OrderBy<UserActivity, DateTimeOffset>((Func<UserActivity, DateTimeOffset>) (d => d.TimeOfDay)).ToList<UserActivity>();
      TimeSpan offset1 = list.First<UserActivity>().TimeOfDay.Offset;
      TimeSpan offset2 = list.Last<UserActivity>().TimeOfDay.Offset;
      if (ChartExtensions.IsDaylightSavingsShiftBackwards(offset1, offset2))
      {
        ChartExtensions.FillInMissingInfoMinutes((IList<UserActivity>) list);
        ChartExtensions.ShiftInfoCollection((IList<UserActivity>) list, offset1, offset2);
      }
      DateTimeOffset timeOfDay;
      foreach (UserActivity userActivity in list)
      {
        if (userActivity.AverageHeartRate > 0)
        {
          flag = true;
          source.AddFirst((double) userActivity.AverageHeartRate);
          if (source.Count<double>() > movingAvgPoints)
            source.RemoveLast();
          List<DateChartPoint> dateChartPointList2 = dateChartPointList1;
          DateChartPoint dateChartPoint = new DateChartPoint();
          timeOfDay = userActivity.TimeOfDay;
          dateChartPoint.Date = timeOfDay.LocalDateTime;
          dateChartPoint.Duration = userActivity.TimeOfDay - sleepEvent.StartTime;
          dateChartPoint.Value = source.Average();
          dateChartPointList2.Add(dateChartPoint);
        }
        else if (!flag)
        {
          List<DateChartPoint> dateChartPointList3 = dateChartPointList1;
          DateChartPoint dateChartPoint = new DateChartPoint();
          timeOfDay = userActivity.TimeOfDay;
          dateChartPoint.Date = timeOfDay.LocalDateTime;
          dateChartPoint.Duration = userActivity.TimeOfDay - sleepEvent.StartTime;
          dateChartPoint.Value = 0.0;
          dateChartPointList3.Add(dateChartPoint);
        }
      }
      return (IList<DateChartPoint>) dateChartPointList1;
    }

    public static IList<DurationChartPoint> GetDurationHeartRateData<T>(
      this IList<T> sequences,
      IList<UserActivity> activities,
      DateTimeOffset startTime)
      where T : UserEventSequence
    {
      activities = (IList<UserActivity>) activities.OrderBy<UserActivity, DateTimeOffset>((Func<UserActivity, DateTimeOffset>) (d => d.TimeOfDay)).ToList<UserActivity>();
      if (sequences == null)
        return (IList<DurationChartPoint>) activities.Select<UserActivity, DurationChartPoint>((Func<UserActivity, DurationChartPoint>) (i =>
        {
          return new DurationChartPoint()
          {
            Duration = i.TimeOfDay - startTime,
            Value = (double) i.AverageHeartRate
          };
        })).ToList<DurationChartPoint>();
      List<DurationChartPoint> durationChartPointList1 = new List<DurationChartPoint>();
      TimeSpan zero = TimeSpan.Zero;
      DateTimeOffset dateTimeOffset = startTime;
      int index1 = 0;
      int index2 = 0;
      DateTimeOffset? nullable1 = new DateTimeOffset?();
      DateTimeOffset? nullable2 = new DateTimeOffset?();
      while (index1 < activities.Count)
      {
        UserActivity activity = activities[index1];
        if (index2 >= sequences.Count - 1 || activity.TimeOfDay < dateTimeOffset)
        {
          if (!nullable1.HasValue || activity.TimeOfDay <= nullable1.Value || activity.TimeOfDay >= nullable2.Value)
          {
            TimeSpan timeSpan = !nullable1.HasValue || !(activity.TimeOfDay <= nullable1.Value) ? zero : zero - (nullable2.Value - nullable1.Value);
            List<DurationChartPoint> durationChartPointList2 = durationChartPointList1;
            DurationChartPoint durationChartPoint = new DurationChartPoint();
            durationChartPoint.Duration = activity.TimeOfDay - startTime - timeSpan;
            durationChartPoint.Value = (double) activity.AverageHeartRate;
            durationChartPointList2.Add(durationChartPoint);
          }
          ++index1;
        }
        else
        {
          UserEventSequence sequence1 = (UserEventSequence) sequences[index2];
          UserEventSequence sequence2 = (UserEventSequence) sequences[index2 + 1];
          TimeSpan timeSpan = sequence2.StartTime - sequence1.StartTime - sequence1.Duration;
          if (timeSpan.TotalSeconds > 5.0)
          {
            nullable1 = new DateTimeOffset?(sequence1.StartTime + sequence1.Duration);
            nullable2 = new DateTimeOffset?(sequence2.StartTime);
            zero += timeSpan;
          }
          else
          {
            nullable1 = new DateTimeOffset?();
            nullable2 = new DateTimeOffset?();
          }
          dateTimeOffset = sequence2.StartTime + sequence2.Duration;
          ++index2;
        }
      }
      return (IList<DurationChartPoint>) durationChartPointList1;
    }

    public static IList<DateChartPoint> GetSleepData(this SleepEvent sleepEvent)
    {
      List<DateChartPoint> dateChartPointList = new List<DateChartPoint>();
      if (sleepEvent == null || sleepEvent.Sequences == null || sleepEvent.Sequences.Count == 0)
        return (IList<DateChartPoint>) dateChartPointList;
      DateTimeOffset dateTimeOffset1 = sleepEvent.StartTime.RoundDown(TimeSpan.FromMinutes(1.0));
      DateTimeOffset localTime = dateTimeOffset1.ToLocalTime();
      foreach (SleepEventSequence sequence in (IEnumerable<SleepEventSequence>) sleepEvent.Sequences)
      {
        dateTimeOffset1 = sequence.StartTime;
        sequence.StartTime = dateTimeOffset1.ToLocalTime();
      }
      List<SleepEventSequence> list1 = sleepEvent.Sequences.OrderBy<SleepEventSequence, DateTimeOffset>((Func<SleepEventSequence, DateTimeOffset>) (d => d.StartTime)).ToList<SleepEventSequence>();
      DateTimeOffset dateTimeOffset2 = list1.First<SleepEventSequence>().StartTime;
      TimeSpan offset1 = dateTimeOffset2.Offset;
      dateTimeOffset2 = list1.Last<SleepEventSequence>().StartTime;
      TimeSpan offset2 = dateTimeOffset2.Offset;
      List<DateChartPoint> list2 = list1.Select<SleepEventSequence, DateChartPoint>(new Func<SleepEventSequence, DateChartPoint>(ChartExtensions.CreateDateChartPoint)).ToList<DateChartPoint>();
      TimeSpan duration = sleepEvent.Duration;
      int num1 = 1;
      if (duration.TotalHours > 12.0)
        num1 = 5;
      else if (duration.TotalHours > 4.0)
        num1 = 2;
      DateChartPoint[] dateChartPointArray1 = new DateChartPoint[(int) Math.Floor(duration.TotalMinutes) + 1];
      using (List<DateChartPoint>.Enumerator enumerator = list2.GetEnumerator())
      {
label_18:
        while (enumerator.MoveNext())
        {
          DateChartPoint current = enumerator.Current;
          int num2 = (int) Math.Floor(((DateTimeOffset) current.Date - localTime).TotalMinutes);
          int num3 = 0;
          while (true)
          {
            if (num3 < (int) Math.Round(current.Duration.TotalMinutes) && num2 + num3 < dateChartPointArray1.Length)
            {
              DateChartPoint[] dateChartPointArray2 = dateChartPointArray1;
              int index = num2 + num3;
              DateChartPoint dateChartPoint = new DateChartPoint();
              dateChartPoint.Date = current.Date.AddMinutes((double) num3);
              dateTimeOffset2 = current.DateWithOffset;
              dateChartPoint.DateWithOffset = dateTimeOffset2.AddMinutes((double) num3);
              dateChartPoint.Duration = TimeSpan.FromMinutes((double) num1);
              dateChartPoint.Value = current.Value;
              dateChartPoint.Classification = current.Classification;
              dateChartPointArray2[index] = dateChartPoint;
              ++num3;
            }
            else
              goto label_18;
          }
        }
      }
      List<DateChartPoint> list3;
      if (((IEnumerable<DateChartPoint>) dateChartPointArray1).Any<DateChartPoint>((Func<DateChartPoint, bool>) (p => p == null)))
      {
        list3 = ((IEnumerable<DateChartPoint>) dateChartPointArray1).Where<DateChartPoint>((Func<DateChartPoint, bool>) (d => d != null)).ToList<DateChartPoint>();
        if (list3.Count > 0)
          ChartExtensions.FillInMissingPointMinutes((IList<DateChartPoint>) list3);
      }
      else
        list3 = ((IEnumerable<DateChartPoint>) dateChartPointArray1).ToList<DateChartPoint>();
      if (offset1 != offset2 && ChartExtensions.IsDaylightSavingsShiftBackwards(offset1, offset2))
        ChartExtensions.ShiftPointCollection((IList<DateChartPoint>) list3, offset1, offset2);
      return (IList<DateChartPoint>) list3;
    }

    private static DateChartPoint CreateDateChartPoint(
      SleepEventSequence sleepSequence)
    {
      DateChartPoint dateChartPoint = new DateChartPoint()
      {
        Date = sleepSequence.StartTime.LocalDateTime.RoundDown(TimeSpan.FromMinutes(1.0)),
        DateWithOffset = sleepSequence.StartTime.RoundDown(TimeSpan.FromMinutes(1.0)),
        Duration = sleepSequence.Duration
      };
      switch (sleepSequence.SequenceType)
      {
        case EventSequenceType.Dozing:
        case EventSequenceType.Snoozing:
        case EventSequenceType.Awake:
          dateChartPoint.Value = 1.0;
          dateChartPoint.Classification = DateChartValueClassification.Awake;
          break;
        case EventSequenceType.Sleep:
          if (sleepSequence.SleepType == SleepType.RestfulSleep)
          {
            dateChartPoint.Value = -4.0;
            dateChartPoint.Classification = DateChartValueClassification.DeepSleep;
            break;
          }
          dateChartPoint.Value = -2.0;
          dateChartPoint.Classification = DateChartValueClassification.LightSleep;
          break;
        default:
          dateChartPoint.Value = 0.0;
          dateChartPoint.Classification = DateChartValueClassification.Unknown;
          break;
      }
      return dateChartPoint;
    }

    public static IEnumerable<DateChartPoint> EnsurePointExistsForInterval(
      this IList<DateChartPoint> dateChartPoints,
      DateTimeOffset start,
      DateTimeOffset end,
      TimeSpan interval)
    {
      while (start < end)
      {
        DateChartPoint dateChartPoint1 = dateChartPoints.FirstOrDefault<DateChartPoint>((Func<DateChartPoint, bool>) (point => point.DateWithOffset >= start && point.DateWithOffset < start + interval));
        if (dateChartPoint1 != null)
        {
          yield return dateChartPoint1;
        }
        else
        {
          DateChartPoint dateChartPoint2 = new DateChartPoint();
          dateChartPoint2.Date = start.LocalDateTime;
          dateChartPoint2.DateWithOffset = start;
          dateChartPoint2.Value = 0.0;
          dateChartPoint2.Duration = interval;
          dateChartPoint2.Classification = DateChartValueClassification.Grey;
          yield return dateChartPoint2;
        }
        start += interval;
      }
    }

    public static IEnumerable<DateChartPoint> EnsurePointExistsForInterval(
      this IList<DateChartPoint> dateChartPoints,
      Range<DateTimeOffset> range,
      TimeSpan interval)
    {
      return dateChartPoints.EnsurePointExistsForInterval(range.Low, range.High, interval);
    }

    private static bool IsDaylightSavingsShiftBackwards(
      TimeSpan startingOffset,
      TimeSpan endingOffset)
    {
      return startingOffset > endingOffset;
    }

    private static DateTimeOffset ShiftTime(
      DateTimeOffset date,
      TimeSpan offsetDifference,
      int index,
      bool left)
    {
      return !left ? date.Add(TimeSpan.FromMinutes(offsetDifference.TotalMinutes / 2.0 - (double) index / 2.0)) : date.Subtract(TimeSpan.FromMinutes((double) index / 2.0));
    }

    internal static void FillInMissingInfoMinutes(IList<UserActivity> orderedInfo)
    {
      for (int index1 = 0; index1 < orderedInfo.Count; ++index1)
      {
        UserActivity userActivity1 = index1 > 0 ? orderedInfo[index1 - 1] : (UserActivity) null;
        if (userActivity1 != null)
        {
          DateTimeOffset timeOfDay;
          int num1;
          if (orderedInfo[index1] == null)
          {
            num1 = 2;
          }
          else
          {
            timeOfDay = orderedInfo[index1].TimeOfDay;
            DateTime dateTime1 = timeOfDay.DateTime;
            timeOfDay = userActivity1.TimeOfDay;
            DateTime dateTime2 = timeOfDay.DateTime;
            num1 = (int) (dateTime1 - dateTime2).TotalMinutes;
          }
          int num2 = num1;
          if (num2 > 1)
          {
            for (int index2 = 0; index2 < num2 - 1; ++index2)
            {
              UserActivity userActivity2 = new UserActivity();
              userActivity2.ActivityLevel = userActivity1.ActivityLevel;
              userActivity2.AverageHeartRate = userActivity1.AverageHeartRate;
              userActivity2.CaloriesBurned = userActivity1.CaloriesBurned;
              userActivity2.DayClassification = userActivity1.DayClassification;
              userActivity2.ItCal = userActivity1.ItCal;
              userActivity2.Location = userActivity1.Location;
              userActivity2.LowestHeartRate = userActivity1.LowestHeartRate;
              userActivity2.PeakHeartRate = userActivity1.PeakHeartRate;
              userActivity2.StepsTaken = userActivity1.StepsTaken;
              timeOfDay = userActivity1.TimeOfDay;
              userActivity2.TimeOfDay = timeOfDay.AddMinutes((double) (index2 + 1));
              userActivity2.TotalDistance = userActivity1.TotalDistance;
              userActivity2.UvExposure = userActivity1.UvExposure;
              UserActivity userActivity3 = userActivity2;
              orderedInfo.Insert(index1 + index2, userActivity3);
            }
          }
        }
      }
    }

    internal static void FillInMissingPointMinutes(IList<DateChartPoint> orderedPoints)
    {
      for (int index1 = 0; index1 < orderedPoints.Count; ++index1)
      {
        DateChartPoint dateChartPoint1 = index1 > 0 ? orderedPoints[index1 - 1] : (DateChartPoint) null;
        if (dateChartPoint1 != null)
        {
          int totalMinutes = (int) (orderedPoints[index1].Date - dateChartPoint1.Date).TotalMinutes;
          if (totalMinutes > 1)
          {
            for (int index2 = 0; index2 < totalMinutes - 1; ++index2)
            {
              DateChartPoint dateChartPoint2 = new DateChartPoint();
              dateChartPoint2.Date = dateChartPoint1.Date.AddMinutes((double) (index2 + 1));
              dateChartPoint2.DateWithOffset = dateChartPoint1.DateWithOffset.AddMinutes((double) (index2 + 1));
              dateChartPoint2.Classification = dateChartPoint1.Classification;
              dateChartPoint2.Duration = dateChartPoint1.Duration;
              dateChartPoint2.Value = dateChartPoint1.Value;
              DateChartPoint dateChartPoint3 = dateChartPoint2;
              orderedPoints.Insert(index1 + index2, dateChartPoint3);
            }
          }
        }
      }
    }

    internal static void ShiftInfoCollection(
      IList<UserActivity> orderedInfo,
      TimeSpan startingOffset,
      TimeSpan endingOffset)
    {
      TimeSpan offsetDifference = startingOffset - endingOffset;
      IEnumerable<UserActivity> source1 = orderedInfo.Where<UserActivity>((Func<UserActivity, bool>) (info => info.TimeOfDay.Offset == startingOffset)).Skip<UserActivity>(Math.Max(0, orderedInfo.Count<UserActivity>((Func<UserActivity, bool>) (info => info.TimeOfDay.Offset == startingOffset)) - (int) offsetDifference.TotalMinutes));
      IEnumerable<UserActivity> source2 = orderedInfo.Where<UserActivity>((Func<UserActivity, bool>) (info => info.TimeOfDay.Offset == endingOffset)).Take<UserActivity>(Math.Min(orderedInfo.Count<UserActivity>((Func<UserActivity, bool>) (info => info.TimeOfDay.Offset == endingOffset)), (int) offsetDifference.TotalMinutes));
      if (!(source1 is IList<UserActivity> userActivityList1))
        userActivityList1 = (IList<UserActivity>) source1.ToList<UserActivity>();
      IList<UserActivity> source3 = userActivityList1;
      if (source3.Count<UserActivity>() > (int) (offsetDifference.TotalMinutes / 2.0))
      {
        int index = 0;
        foreach (UserActivity userActivity in (IEnumerable<UserActivity>) source3)
        {
          userActivity.TimeOfDay = ChartExtensions.ShiftTime(userActivity.TimeOfDay, offsetDifference, index, true);
          ++index;
        }
      }
      if (!(source2 is IList<UserActivity> userActivityList2))
        userActivityList2 = (IList<UserActivity>) source2.ToList<UserActivity>();
      IList<UserActivity> source4 = userActivityList2;
      if (source4.Count<UserActivity>() <= (int) (offsetDifference.TotalMinutes / 2.0))
        return;
      int index1 = 0;
      foreach (UserActivity userActivity in (IEnumerable<UserActivity>) source4)
      {
        userActivity.TimeOfDay = ChartExtensions.ShiftTime(userActivity.TimeOfDay, offsetDifference, index1, false);
        ++index1;
      }
    }

    internal static void ShiftPointCollection(
      IList<DateChartPoint> orderedPoints,
      TimeSpan startingOffset,
      TimeSpan endingOffset)
    {
      TimeSpan offsetDifference = startingOffset - endingOffset;
      IEnumerable<DateChartPoint> source1 = orderedPoints.Where<DateChartPoint>((Func<DateChartPoint, bool>) (point => point.DateWithOffset.Offset == startingOffset)).Skip<DateChartPoint>(Math.Max(0, orderedPoints.Count<DateChartPoint>((Func<DateChartPoint, bool>) (point => point.DateWithOffset.Offset == startingOffset)) - (int) offsetDifference.TotalMinutes));
      IEnumerable<DateChartPoint> source2 = orderedPoints.Where<DateChartPoint>((Func<DateChartPoint, bool>) (point => point.DateWithOffset.Offset == endingOffset)).Take<DateChartPoint>(Math.Min(orderedPoints.Count<DateChartPoint>((Func<DateChartPoint, bool>) (point => point.DateWithOffset.Offset == endingOffset)), (int) offsetDifference.TotalMinutes));
      if (!(source1 is IList<DateChartPoint> dateChartPointList1))
        dateChartPointList1 = (IList<DateChartPoint>) source1.ToList<DateChartPoint>();
      IList<DateChartPoint> source3 = dateChartPointList1;
      if (source3.Count<DateChartPoint>() > (int) (offsetDifference.TotalMinutes / 2.0))
      {
        int index = 0;
        foreach (DateChartPoint dateChartPoint in (IEnumerable<DateChartPoint>) source3)
        {
          dateChartPoint.DateWithOffset = ChartExtensions.ShiftTime(dateChartPoint.DateWithOffset, offsetDifference, index, true);
          dateChartPoint.Date = dateChartPoint.DateWithOffset.DateTime;
          ++index;
        }
      }
      if (!(source2 is IList<DateChartPoint> dateChartPointList2))
        dateChartPointList2 = (IList<DateChartPoint>) source2.ToList<DateChartPoint>();
      IList<DateChartPoint> source4 = dateChartPointList2;
      if (source4.Count<DateChartPoint>() <= (int) (offsetDifference.TotalMinutes / 2.0))
        return;
      int index1 = 0;
      foreach (DateChartPoint dateChartPoint in (IEnumerable<DateChartPoint>) source4)
      {
        dateChartPoint.DateWithOffset = ChartExtensions.ShiftTime(dateChartPoint.DateWithOffset, offsetDifference, index1, false);
        dateChartPoint.Date = dateChartPoint.DateWithOffset.DateTime;
        ++index1;
      }
    }
  }
}
