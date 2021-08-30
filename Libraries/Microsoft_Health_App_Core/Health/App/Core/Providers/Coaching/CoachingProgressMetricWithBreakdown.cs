// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.CoachingProgressMetricWithBreakdown
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class CoachingProgressMetricWithBreakdown : CoachingProgressMetric
  {
    private CoachingProgressSubMetric subMetricOne;

    public CoachingProgressMetricWithBreakdown(
      string title,
      int current,
      int total,
      ArgbColor32 metricColor,
      CoachingProgressSubMetric subMetricOne,
      ArgbColor32 subMetricColor,
      bool enableNoGoalState = false,
      string noGoalTitle = null)
      : base(title, current, total, metricColor, enableNoGoalState, noGoalTitle)
    {
      this.subMetricOne = subMetricOne;
      this.SubMetricColor = subMetricColor;
    }

    public CoachingProgressSubMetric SubMetricOne => this.subMetricOne;

    public ArgbColor32 SubMetricColor { get; }
  }
}
