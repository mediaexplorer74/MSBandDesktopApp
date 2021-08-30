// Decompiled with JetBrains decompiler
// Type: System.TimeSpanEx
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Globalization;

namespace System
{
  internal static class TimeSpanEx
  {
    public static TimeSpan Parse(string value, CultureInfo info) => TimeSpan.Parse(value, (IFormatProvider) info);

    public static bool TryParse(string value, CultureInfo info, out TimeSpan output) => TimeSpan.TryParse(value, (IFormatProvider) info, out output);

    public static string ToString(this TimeSpan timeSpan, CultureInfo info, string format) => timeSpan.ToString(format, (IFormatProvider) info);
  }
}
