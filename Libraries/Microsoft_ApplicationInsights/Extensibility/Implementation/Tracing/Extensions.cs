// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Extensions
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Globalization;
using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  public static class Extensions
  {
    public static string ToInvariantString(this Exception exception)
    {
      CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
      try
      {
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        return exception.ToString();
      }
      finally
      {
        Thread.CurrentThread.CurrentUICulture = currentUiCulture;
      }
    }
  }
}
