// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.WaitHandleExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class WaitHandleExtensions
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Extensions\\WaitHandleExtensions.cs");
    private static readonly TimeSpan PingInterval = TimeSpan.FromMilliseconds(100.0);

    public static async Task WaitAsync(
      this WaitHandle waitHandle,
      CancellationToken cancellationToken)
    {
      while (true)
      {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
          if (waitHandle.WaitOne(TimeSpan.Zero))
            break;
        }
        catch (AbandonedMutexException ex)
        {
          WaitHandleExtensions.Logger.Warn((object) "<FLAG> caught and handled abandoned mutex", (Exception) ex);
          break;
        }
        await Task.Delay(WaitHandleExtensions.PingInterval, cancellationToken).ConfigureAwait(false);
      }
    }

    public static void Wait(this WaitHandle waitHandle, CancellationToken cancellationToken)
    {
label_0:
      cancellationToken.ThrowIfCancellationRequested();
      try
      {
        if (!waitHandle.WaitOne(WaitHandleExtensions.PingInterval))
          goto label_0;
      }
      catch (AbandonedMutexException ex)
      {
        WaitHandleExtensions.Logger.Warn((Exception) ex, "<FLAG> caught and handled abandoned mutex");
      }
    }
  }
}
