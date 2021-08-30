// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.CoachingProgressSection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class CoachingProgressSection
  {
    private readonly string title;
    private readonly DateTimeOffset weekStart;
    private readonly DateTimeOffset weekEnd;
    private readonly IReadOnlyList<CoachingProgressMetric> metrics;
    private readonly WellnessPlanType planType;

    public CoachingProgressSection(
      string title,
      DateTimeOffset weekStart,
      DateTimeOffset weekEnd,
      IEnumerable<CoachingProgressMetric> metrics,
      WellnessPlanType planType)
    {
      Assert.ParamIsNotNull((object) metrics, nameof (metrics));
      this.title = title;
      this.weekStart = weekStart;
      this.weekEnd = weekEnd;
      this.metrics = (IReadOnlyList<CoachingProgressMetric>) new List<CoachingProgressMetric>(metrics);
      this.planType = planType;
    }

    public string Title => this.title;

    public DateTimeOffset WeekStart => this.weekStart;

    public DateTimeOffset WeekEnd => this.weekEnd;

    public IReadOnlyList<CoachingProgressMetric> Metrics => this.metrics;

    public WellnessPlanType PlanType => this.planType;

    public string WeekStartToEnd => string.Format(AppResources.ActionPlanWeekDisplayFormat, new object[2]
    {
      (object) Formatter.GetMonthNameShortDayString(this.weekStart),
      (object) Formatter.GetMonthNameShortDayString(this.weekEnd)
    });

    public double Percentage => this.Metrics.Where<CoachingProgressMetric>((Func<CoachingProgressMetric, bool>) (m => !m.ShowNoGoalState)).Average<CoachingProgressMetric>((Func<CoachingProgressMetric, double>) (m => m.Percentage));

    public EmbeddedAsset Image
    {
      get
      {
        double percentage = this.Percentage;
        switch (this.PlanType)
        {
          case WellnessPlanType.Sleep:
            if (percentage == 100.0)
              return EmbeddedAsset.SleepBadge5;
            if (percentage >= 75.0)
              return EmbeddedAsset.SleepBadge4;
            if (percentage >= 50.0)
              return EmbeddedAsset.SleepBadge3;
            return percentage >= 25.0 ? EmbeddedAsset.SleepBadge2 : EmbeddedAsset.SleepBadge1;
          case WellnessPlanType.Activity:
            if (percentage == 100.0)
              return EmbeddedAsset.MoveBadge5;
            if (percentage >= 75.0)
              return EmbeddedAsset.MoveBadge4;
            if (percentage >= 50.0)
              return EmbeddedAsset.MoveBadge3;
            return percentage >= 25.0 ? EmbeddedAsset.MoveBadge2 : EmbeddedAsset.MoveBadge1;
          default:
            Assert.Fail(string.Format("Unhandled WellnessPlanType {0}", new object[1]
            {
              (object) this.PlanType
            }));
            return EmbeddedAsset.MoveBadge1;
        }
      }
    }

    public ArgbColor32 ProgressColor
    {
      get
      {
        switch (this.PlanType)
        {
          case WellnessPlanType.Sleep:
            return CoreColors.SleepBetterMedium;
          case WellnessPlanType.Activity:
            return CoreColors.MoveMoreMedium;
          default:
            Assert.Fail(string.Format("Unhandled WellnessPlanType {0}", new object[1]
            {
              (object) this.PlanType
            }));
            return new ArgbColor32(0U);
        }
      }
    }

    public static CoachingProgressSection FromCloudSleepProgress(
      WeeklySleepProgressSummary summary)
    {
      if (summary == null)
        return (CoachingProgressSection) null;
      WellnessPlanType planType = WellnessPlanType.Sleep;
      List<CoachingProgressMetric> coachingProgressMetricList1 = new List<CoachingProgressMetric>();
      int num = summary.DailyProgressSummaries.Count<DailySleepProgressSummary>((Func<DailySleepProgressSummary, bool>) (sum => sum.HasReportedSleep));
      ArgbColor32 sleepBetterMedium = CoreColors.SleepBetterMedium;
      List<CoachingProgressMetric> coachingProgressMetricList2 = coachingProgressMetricList1;
      string sleepMetricOneTitle = AppResources.ActionPlanSleepMetricOneTitle;
      int? nullable = summary.ActualSleepHoursGoalMet;
      int current1 = nullable ?? 0;
      int total1 = num;
      ArgbColor32 metricColor1 = sleepBetterMedium;
      CoachingProgressMetric coachingProgressMetric1 = new CoachingProgressMetric(sleepMetricOneTitle, current1, total1, metricColor1);
      coachingProgressMetricList2.Add(coachingProgressMetric1);
      List<CoachingProgressMetric> coachingProgressMetricList3 = coachingProgressMetricList1;
      string sleepMetricTwoTitle = AppResources.ActionPlanSleepMetricTwoTitle;
      nullable = summary.BedTimeGoalMet;
      int current2 = nullable ?? 0;
      int total2 = num;
      ArgbColor32 metricColor2 = sleepBetterMedium;
      CoachingProgressMetric coachingProgressMetric2 = new CoachingProgressMetric(sleepMetricTwoTitle, current2, total2, metricColor2);
      coachingProgressMetricList3.Add(coachingProgressMetric2);
      List<CoachingProgressMetric> coachingProgressMetricList4 = coachingProgressMetricList1;
      string metricThreeTitle = AppResources.ActionPlanSleepMetricThreeTitle;
      nullable = summary.WakeupTimeGoalMet;
      int current3 = nullable ?? 0;
      int total3 = num;
      ArgbColor32 metricColor3 = sleepBetterMedium;
      CoachingProgressMetric coachingProgressMetric3 = new CoachingProgressMetric(metricThreeTitle, current3, total3, metricColor3);
      coachingProgressMetricList4.Add(coachingProgressMetric3);
      return new CoachingProgressSection(AppResources.ActionPlanSleepProgressHeader, summary.TimeOfWeek, summary.NextTimeOfWeek.AddDays(-1.0), (IEnumerable<CoachingProgressMetric>) coachingProgressMetricList1, planType);
    }

    public static CoachingProgressSection FromCloudActivityProgress(
      WeeklyActivityProgressSummary summary)
    {
      WellnessPlanType planType = WellnessPlanType.Activity;
      List<CoachingProgressMetric> coachingProgressMetricList1 = new List<CoachingProgressMetric>();
      int current1 = (summary.CardioScore ?? 0) / 60;
      int? nullable = summary.CardioScoreTarget;
      int total1 = (nullable ?? 0) / 60;
      nullable = summary.IntenseCardio;
      int num1 = (nullable ?? 0) / 60;
      nullable = summary.MediumCardio;
      int num2 = nullable ?? 0;
      nullable = summary.IntenseCardio;
      int num3 = nullable ?? 0;
      int num4 = (num2 + num3) / 60;
      nullable = summary.StrengthWorkoutTarget;
      int num5 = nullable ?? 0;
      ArgbColor32 moveMoreMedium = CoreColors.MoveMoreMedium;
      coachingProgressMetricList1.Add((CoachingProgressMetric) new CoachingProgressMetricWithBreakdown(AppResources.CoachingPlanActivityMetricOneTitle, current1, total1, moveMoreMedium, new CoachingProgressSubMetric(AppResources.CoachingPlanActivitySubmetricOneTitle, string.Format(AppResources.CoachingPlanActivityMinutesFormat, new object[1]
      {
        (object) num4
      }), moveMoreMedium), new ArgbColor32(4278229324U), (total1 == 0 ? 1 : 0) != 0, AppResources.CoachingPlanActivityMetricTwoMissingTitle));
      List<CoachingProgressMetric> coachingProgressMetricList2 = coachingProgressMetricList1;
      string activityMetricTwoTitle = AppResources.CoachingPlanActivityMetricTwoTitle;
      nullable = summary.AverageDailySteps;
      int current2 = nullable ?? 0;
      nullable = summary.StepsTarget;
      int total2 = nullable ?? 0;
      ArgbColor32 metricColor1 = moveMoreMedium;
      CoachingProgressMetric coachingProgressMetric1 = new CoachingProgressMetric(activityMetricTwoTitle, current2, total2, metricColor1);
      coachingProgressMetricList2.Add(coachingProgressMetric1);
      List<CoachingProgressMetric> coachingProgressMetricList3 = coachingProgressMetricList1;
      string metricThreeTitle = AppResources.CoachingPlanActivityMetricThreeTitle;
      nullable = summary.StrengthWorkout;
      int current3 = nullable ?? 0;
      int total3 = num5;
      ArgbColor32 metricColor2 = moveMoreMedium;
      int num6 = num5 == 0 ? 1 : 0;
      string threeMissingTitle = AppResources.CoachingPlanActivityMetricThreeMissingTitle;
      CoachingProgressMetric coachingProgressMetric2 = new CoachingProgressMetric(metricThreeTitle, current3, total3, metricColor2, num6 != 0, threeMissingTitle);
      coachingProgressMetricList3.Add(coachingProgressMetric2);
      return new CoachingProgressSection(AppResources.CoachingPlanActivityProgressHeader, summary.TimeOfWeek, summary.NextTimeOfWeek.AddDays(-1.0), (IEnumerable<CoachingProgressMetric>) coachingProgressMetricList1, planType);
    }
  }
}
