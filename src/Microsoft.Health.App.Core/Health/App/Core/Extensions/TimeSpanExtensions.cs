// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.TimeSpanExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class TimeSpanExtensions
  {
    public static string ToStringWithNegative(this TimeSpan timeSpan, string formatString)
    {
      if (!(timeSpan < TimeSpan.Zero))
        return timeSpan.ToString(formatString);
      return timeSpan.ToString(string.Format("\\-{0}", new object[1]
      {
        (object) formatString
      }));
    }

    public static double TotalHoursExceptFractionalComponents(this TimeSpan timeSpan) => timeSpan.TotalHours - (double) timeSpan.Minutes - (double) timeSpan.Seconds - (double) timeSpan.Milliseconds;

    public static double TotalMinutesExceptFractionalComponents(this TimeSpan timeSpan) => timeSpan.TotalMinutes - (double) timeSpan.Seconds - (double) timeSpan.Milliseconds;
  }
}
