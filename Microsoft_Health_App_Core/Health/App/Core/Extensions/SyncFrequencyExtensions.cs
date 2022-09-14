// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.SyncFrequencyExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Sync;
using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class SyncFrequencyExtensions
  {
    public static TimeSpan ToTimeSpan(this SyncFrequency frequency)
    {
      Assert.EnumIsDefined<SyncFrequency>(frequency, nameof (frequency));
      int num = 0;
      switch (frequency)
      {
        case SyncFrequency.FifteenMinutes:
          num = 15;
          break;
        case SyncFrequency.ThirtyMinutes:
          num = 30;
          break;
        case SyncFrequency.OneHour:
          num = 60;
          break;
        case SyncFrequency.FourHours:
          num = 240;
          break;
        default:
          Assert.Fail(string.Format("Unhandled {0} value {1}.", new object[2]
          {
            (object) typeof (SyncFrequency).Name,
            (object) frequency
          }));
          break;
      }
      return TimeSpan.FromMinutes((double) num);
    }
  }
}
