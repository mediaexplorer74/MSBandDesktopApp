// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.UnixTimeExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class UnixTimeExtensions
  {
    private static readonly DateTimeOffset UnixOriginTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

    public static long ToUnixTimestamp(this DateTimeOffset dateTime) => (long) (dateTime - UnixTimeExtensions.UnixOriginTime).TotalMilliseconds;

    public static DateTimeOffset ToUnixTimestamp(this long millisecondsSinceEpoch) => UnixTimeExtensions.UnixOriginTime.AddMilliseconds((double) millisecondsSinceEpoch);
  }
}
