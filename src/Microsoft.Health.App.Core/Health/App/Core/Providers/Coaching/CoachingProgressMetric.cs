// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.CoachingProgressMetric
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using System;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class CoachingProgressMetric
  {
    private readonly int current;
    private readonly int total;
    private readonly string title;
    private readonly bool showNoGoalState;

    public CoachingProgressMetric(
      string title,
      int current,
      int total,
      ArgbColor32 metricColor,
      bool enableNoGoalState = false,
      string noGoalTitle = null)
    {
      bool flag = enableNoGoalState && total == 0;
      this.title = flag ? noGoalTitle : title;
      this.current = current;
      this.total = total;
      this.MetricColor = metricColor;
      this.showNoGoalState = flag;
    }

    public int Current => this.current;

    public int ProgressBarCurrent => this.total != 0 ? this.current : 0;

    public int Total => this.total;

    public int ProgressBarTotal => this.total != 0 ? this.total : 1;

    public ArgbColor32 MetricColor { get; }

    public string Title => this.title;

    public bool ShowNoGoalState => this.showNoGoalState;

    public string TotalString => string.Format(AppResources.ActionPlanMetricTotalFormat, new object[1]
    {
      (object) this.Total
    });

    public double Percentage => this.total != 0 ? Math.Min(100.0, 100.0 * (double) this.current / (double) this.total) : 0.0;
  }
}
