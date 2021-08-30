// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnoisticsEventThrottlingDefaults
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal static class DiagnoisticsEventThrottlingDefaults
  {
    internal const int MinimalThrottleAfterCount = 1;
    internal const int DefaultThrottleAfterCount = 5;
    internal const int MaxThrottleAfterCount = 10;
    internal const uint MinimalThrottlingRecycleIntervalInMinutes = 1;
    internal const uint DefaultThrottlingRecycleIntervalInMinutes = 5;
    internal const uint MaxThrottlingRecycleIntervalInMinutes = 60;
    internal const int KeywordsExcludedFromEventThrottling = 2;

    internal static bool IsInRangeThrottleAfterCount(this int throttleAfterCount) => throttleAfterCount >= 1 && throttleAfterCount <= 10;

    internal static bool IsInRangeThrottlingRecycleInterval(
      this uint throttlingRecycleIntervalInMinutes)
    {
      return throttlingRecycleIntervalInMinutes >= 1U && throttlingRecycleIntervalInMinutes <= 60U;
    }
  }
}
