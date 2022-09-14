// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.CancellationTokenUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class CancellationTokenUtilities
  {
    public static TimeSpan DefaultCancellationTokenTimespan => TimeSpan.FromSeconds(60.0);

    public static TimeSpan LongRunningCancellationTokenTimespan => TimeSpan.FromMinutes(10.0);

    public static CancellationToken FromTimeout(TimeSpan delay) => new CancellationTokenSource(delay).Token;
  }
}
