// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Clock
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Diagnostics;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal class Clock : IClock
  {
    private static readonly DateTimeOffset InitialTimeStamp = DateTimeOffset.Now;
    private static readonly Stopwatch OffsetStopwatch = Stopwatch.StartNew();
    private static IClock instance = (IClock) new Clock();

    protected Clock()
    {
    }

    public static IClock Instance
    {
      get => Clock.instance;
      protected set => Clock.instance = value ?? (IClock) new Clock();
    }

    public DateTimeOffset Time => Clock.InitialTimeStamp + Clock.OffsetStopwatch.Elapsed;
  }
}
