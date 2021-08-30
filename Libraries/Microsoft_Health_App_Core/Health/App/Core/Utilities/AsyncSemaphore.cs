// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.AsyncSemaphore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities
{
  public class AsyncSemaphore
  {
    private static readonly Task Completed = (Task) Task.FromResult<bool>(true);
    private readonly Queue<TaskCompletionSource<bool>> waiters = new Queue<TaskCompletionSource<bool>>();
    private int currentCount;

    public AsyncSemaphore(int initialCount) => this.currentCount = initialCount >= 0 ? initialCount : throw new ArgumentOutOfRangeException(nameof (initialCount));

    public Task WaitAsync()
    {
      lock (this.waiters)
      {
        if (this.currentCount > 0)
        {
          --this.currentCount;
          return AsyncSemaphore.Completed;
        }
        TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
        this.waiters.Enqueue(completionSource);
        return (Task) completionSource.Task;
      }
    }

    public void Release()
    {
      TaskCompletionSource<bool> completionSource = (TaskCompletionSource<bool>) null;
      lock (this.waiters)
      {
        if (this.waiters.Count > 0)
          completionSource = this.waiters.Dequeue();
        else
          ++this.currentCount;
      }
      completionSource?.SetResult(true);
    }
  }
}
