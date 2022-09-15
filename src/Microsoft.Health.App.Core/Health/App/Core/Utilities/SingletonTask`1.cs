// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.SingletonTask`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities
{
  public sealed class SingletonTask<TProgress>
  {
    private readonly Func<CancellationToken, IProgress<TProgress>, Task> taskFactory;
    private readonly object taskLock = new object();
    private SingletonTask<TProgress>.ProgressManager<TProgress> progressManager;
    private Task task;

    public SingletonTask(
      Func<CancellationToken, IProgress<TProgress>, Task> taskFactory)
    {
      Assert.ParamIsNotNull((object) taskFactory, nameof (taskFactory));
      this.taskFactory = taskFactory;
    }

    public Task RunAsync(CancellationToken token, IProgress<TProgress> progress = null)
    {
      lock (this.taskLock)
      {
        if (this.task == null || this.task.IsCompleted)
        {
          this.progressManager = new SingletonTask<TProgress>.ProgressManager<TProgress>(progress);
          this.task = this.taskFactory(token, (IProgress<TProgress>) this.progressManager);
        }
        else
          this.progressManager.Add(progress);
        return this.task;
      }
    }

    private sealed class ProgressManager<T> : IProgress<T>
    {
      private readonly IList<IProgress<T>> progressList = (IList<IProgress<T>>) new List<IProgress<T>>();

      public ProgressManager(IProgress<T> progress)
      {
        if (progress == null)
          return;
        this.progressList.Add(progress);
      }

      public void Add(IProgress<T> progress)
      {
        if (progress == null)
          return;
        lock (this.progressList)
          this.progressList.Add(progress);
      }

      public void Report(T value)
      {
        List<IProgress<T>> progressList;
        lock (this.progressList)
          progressList = new List<IProgress<T>>((IEnumerable<IProgress<T>>) this.progressList);
        foreach (IProgress<T> progress in progressList)
          progress.Report(value);
      }
    }
  }
}
