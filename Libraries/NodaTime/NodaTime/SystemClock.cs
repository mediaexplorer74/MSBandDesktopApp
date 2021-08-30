// Decompiled with JetBrains decompiler
// Type: NodaTime.SystemClock
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using System;

namespace NodaTime
{
  [Immutable]
  public sealed class SystemClock : IClock
  {
    public static readonly SystemClock Instance = new SystemClock();

    private SystemClock()
    {
    }

    public Instant Now => NodaConstants.BclEpoch.PlusTicks(DateTime.UtcNow.Ticks);
  }
}
