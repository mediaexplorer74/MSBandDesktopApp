// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.ProgressTracker
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public abstract class ProgressTracker
  {
    internal ProgressTrackerPrimitive[] children;
    private double lastReportedProgress;
    internal double? cachedPercentage;

    internal ProgressTracker(ProgressTrackerPrimitive[] children) => this.children = children;

    internal virtual double PercentageCompletion
    {
      get
      {
        double? nullable = this.cachedPercentage;
        if (!nullable.HasValue)
        {
          double val1 = 0.0;
          foreach (ProgressTrackerPrimitive child in this.children)
            val1 += child.PercentageComplete * child.Weight;
          if (val1 < 0.0 || val1 > 100.0)
            Logger.Log(LogLevel.Warning, "ProgressTracker.PercentageCompletion.get: progress: {0}", (object) val1);
          double num = Math.Min(val1, 100.0);
          if (num > this.lastReportedProgress)
          {
            this.lastReportedProgress = num;
            nullable = new double?(num);
          }
          else
            nullable = new double?(this.lastReportedProgress);
          this.cachedPercentage = nullable;
        }
        return nullable.Value;
      }
    }

    internal abstract void ChildUpdated();
  }
}
