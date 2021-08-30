// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnoisticsEventThrottlingManager`1
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class DiagnoisticsEventThrottlingManager<T> : IDiagnoisticsEventThrottlingManager
    where T : IDiagnoisticsEventThrottling
  {
    private readonly T snapshotContainer;

    internal DiagnoisticsEventThrottlingManager(
      T snapshotContainer,
      IDiagnoisticsEventThrottlingScheduler scheduler,
      uint throttlingRecycleIntervalInMinutes)
    {
      if ((object) snapshotContainer == null)
        throw new ArgumentNullException(nameof (snapshotContainer));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (!throttlingRecycleIntervalInMinutes.IsInRangeThrottlingRecycleInterval())
        throw new ArgumentOutOfRangeException(nameof (throttlingRecycleIntervalInMinutes));
      this.snapshotContainer = snapshotContainer;
      int interval = (int) throttlingRecycleIntervalInMinutes * 60 * 1000;
      scheduler.ScheduleToRunEveryTimeIntervalInMilliseconds(interval, new Action(this.ResetThrottling));
    }

    public bool ThrottleEvent(int eventId, long keywords)
    {
      bool justExceededThreshold;
      bool flag = this.snapshotContainer.ThrottleEvent(eventId, keywords, out justExceededThreshold);
      if (justExceededThreshold)
        CoreEventSource.Log.DiagnosticsEventThrottlingHasBeenStartedForTheEvent(eventId);
      return flag;
    }

    private void ResetThrottling()
    {
      foreach (KeyValuePair<int, DiagnoisticsEventCounters> keyValuePair in (IEnumerable<KeyValuePair<int, DiagnoisticsEventCounters>>) this.snapshotContainer.CollectSnapshot())
        CoreEventSource.Log.DiagnosticsEventThrottlingHasBeenResetForTheEvent(keyValuePair.Key, keyValuePair.Value.ExecCount);
    }
  }
}
