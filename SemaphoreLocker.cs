// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.SemaphoreLocker
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public class SemaphoreLocker : IDisposable
  {
    private SemaphoreSlim semaphore;

    public static SemaphoreLocker Lock(SemaphoreSlim semaphore)
    {
      SemaphoreLocker semaphoreLocker = new SemaphoreLocker();
      semaphoreLocker.semaphore = semaphore;
      semaphore.Wait();
      return semaphoreLocker;
    }

    public static async Task<SemaphoreLocker> LockAsync(SemaphoreSlim semaphore)
    {
      SemaphoreLocker locker = new SemaphoreLocker();
      locker.semaphore = semaphore;
      await semaphore.WaitAsync();
      return locker;
    }

    public void Dispose()
    {
      if (this.semaphore == null)
        return;
      this.semaphore.Release();
      this.semaphore = (SemaphoreSlim) null;
    }
  }
}
