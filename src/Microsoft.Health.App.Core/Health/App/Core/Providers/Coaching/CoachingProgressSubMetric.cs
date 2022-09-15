// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.CoachingProgressSubMetric
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class CoachingProgressSubMetric
  {
    public CoachingProgressSubMetric(string title, string value, ArgbColor32 metricColor)
    {
      this.Title = title;
      this.Value = value;
      this.MetricColor = metricColor;
    }

    public string Title { get; }

    public string Value { get; }

    public ArgbColor32 MetricColor { get; }
  }
}
