// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnoisticsEventThrottling
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class DiagnoisticsEventThrottling : IDiagnoisticsEventThrottling
  {
    private readonly int throttleAfterCount;
    private readonly object syncRoot = new object();
    private Dictionary<int, DiagnoisticsEventCounters> counters = new Dictionary<int, DiagnoisticsEventCounters>();

    internal DiagnoisticsEventThrottling(int throttleAfterCount) => this.throttleAfterCount = throttleAfterCount.IsInRangeThrottleAfterCount() ? throttleAfterCount : throw new ArgumentOutOfRangeException(nameof (throttleAfterCount));

    internal int ThrottleAfterCount => this.throttleAfterCount;

    public bool ThrottleEvent(int eventId, long keywords, out bool justExceededThreshold)
    {
      if (!DiagnoisticsEventThrottling.IsExcludedFromThrottling(keywords))
      {
        DiagnoisticsEventCounters eventCounter = this.InternalGetEventCounter(eventId);
        justExceededThreshold = this.ThrottleAfterCount == eventCounter.Increment() - 1;
        return this.ThrottleAfterCount < eventCounter.ExecCount;
      }
      justExceededThreshold = false;
      return false;
    }

    public IDictionary<int, DiagnoisticsEventCounters> CollectSnapshot()
    {
      Dictionary<int, DiagnoisticsEventCounters> counters = this.counters;
      this.syncRoot.ExecuteSpinWaitLock((Action) (() => this.counters = new Dictionary<int, DiagnoisticsEventCounters>()));
      return (IDictionary<int, DiagnoisticsEventCounters>) counters;
    }

    private static bool IsExcludedFromThrottling(long keywords) => 0L != (keywords & 2L);

    private DiagnoisticsEventCounters InternalGetEventCounter(int eventId)
    {
      DiagnoisticsEventCounters result = (DiagnoisticsEventCounters) null;
      this.syncRoot.ExecuteSpinWaitLock((Action) (() =>
      {
        if (this.counters.TryGetValue(eventId, out result))
          return;
        result = new DiagnoisticsEventCounters();
        this.counters.Add(eventId, result);
      }));
      return result;
    }
  }
}
