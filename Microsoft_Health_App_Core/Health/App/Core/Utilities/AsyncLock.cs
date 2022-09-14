// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.AsyncLock
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities
{
  public class AsyncLock
  {
    private readonly AsyncSemaphore semaphore;
    private readonly Task<AsyncLock.Releaser> releaser;

    public AsyncLock()
    {
      this.semaphore = new AsyncSemaphore(1);
      this.releaser = Task.FromResult<AsyncLock.Releaser>(new AsyncLock.Releaser(this));
    }

    public Task<AsyncLock.Releaser> LockAsync()
    {
      Task task = this.semaphore.WaitAsync();
      return !task.IsCompleted ? task.ContinueWith<AsyncLock.Releaser>((Func<Task, object, AsyncLock.Releaser>) ((t, state) => new AsyncLock.Releaser((AsyncLock) state)), (object) this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default) : this.releaser;
    }

    public struct Releaser : IDisposable
    {
      private readonly AsyncLock toRelease;

      internal Releaser(AsyncLock toRelease) => this.toRelease = toRelease;

      public void Dispose()
      {
        if (this.toRelease == null)
          return;
        this.toRelease.semaphore.Release();
      }
    }
  }
}
