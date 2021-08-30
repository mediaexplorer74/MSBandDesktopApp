// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.SemaphoreQueue
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities
{
  public class SemaphoreQueue
  {
    private readonly Queue<TaskCompletionSource<bool>> queue = new Queue<TaskCompletionSource<bool>>();
    private readonly object queueLock = new object();
    private readonly SemaphoreSlim semaphore;

    public SemaphoreQueue(int initialCount) => this.semaphore = new SemaphoreSlim(initialCount);

    public SemaphoreQueue(int initialCount, int maxCount) => this.semaphore = new SemaphoreSlim(initialCount, maxCount);

    public Task WaitAsync()
    {
      TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
      this.queue.Enqueue(completionSource);
      this.semaphore.WaitAsync().ContinueWith((Action<Task>) (t =>
      {
        lock (this.queueLock)
        {
          if (this.queue.Count <= 0)
            return;
          this.queue.Dequeue().SetResult(true);
        }
      }));
      return (Task) completionSource.Task;
    }

    public void Release() => this.semaphore.Release();
  }
}
