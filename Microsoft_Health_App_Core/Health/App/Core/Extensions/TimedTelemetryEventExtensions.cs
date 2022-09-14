// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.TimedTelemetryEventExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class TimedTelemetryEventExtensions
  {
    public static async Task<T> TimeWithStatusAsync<T>(
      this ITimedTelemetryEvent timedEvent,
      Func<Task<T>> func)
    {
      T obj1;
      using (timedEvent)
      {
        try
        {
          T obj2 = await func();
          timedEvent.SetStatus(true);
          obj1 = obj2;
        }
        catch (Exception ex)
        {
          timedEvent.SetStatus(false);
          throw;
        }
      }
      return obj1;
    }

    public static async Task TimeWithStatusAsync(
      this ITimedTelemetryEvent timedEvent,
      Func<Task> func)
    {
      using (timedEvent)
      {
        try
        {
          await func();
          timedEvent.SetStatus(true);
        }
        catch (Exception ex)
        {
          timedEvent.SetStatus(false);
          throw;
        }
      }
    }

    public static void TimeWithStatus(this ITimedTelemetryEvent timedEvent, Action action)
    {
      using (timedEvent)
      {
        try
        {
          action();
          timedEvent.SetStatus(true);
        }
        catch (Exception ex)
        {
          timedEvent.SetStatus(false);
          throw;
        }
      }
    }

    public static void SetStatus(this ITimedTelemetryEvent timedEvent, bool succeeded) => timedEvent.AddProperty("Status", succeeded ? "Success" : "Failure");
  }
}
