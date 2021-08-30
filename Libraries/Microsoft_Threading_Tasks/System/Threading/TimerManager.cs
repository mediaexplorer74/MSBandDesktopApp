// Decompiled with JetBrains decompiler
// Type: System.Threading.TimerManager
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System.Collections.Generic;

namespace System.Threading
{
  internal static class TimerManager
  {
    private static Dictionary<Timer, object> s_rootedTimers = new Dictionary<Timer, object>();

    public static void Add(Timer timer)
    {
      lock (TimerManager.s_rootedTimers)
        TimerManager.s_rootedTimers.Add(timer, (object) null);
    }

    public static void Remove(Timer timer)
    {
      lock (TimerManager.s_rootedTimers)
        TimerManager.s_rootedTimers.Remove(timer);
    }
  }
}
