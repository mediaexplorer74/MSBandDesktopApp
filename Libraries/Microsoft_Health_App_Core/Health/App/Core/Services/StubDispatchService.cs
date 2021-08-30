// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.StubDispatchService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public sealed class StubDispatchService : IDispatchService
  {
    public Task RunOnUIThreadAsync(Action action, bool runImmediately = true) => Task.Run(action);

    public Task<T> RunOnUIThreadAsync<T>(Func<T> func, bool runImmediately = true) => Task.Run<T>(func);

    public Task RunOnUIThreadAsync(Func<Task> func, bool runImmediately = true) => Task.Run(func);

    public Task<T> RunOnUIThreadAsync<T>(Func<Task<T>> func, bool runImmediately = true) => Task.Run<T>(func);

    public Task TryRunOnUIThreadAsync(Action action, bool runImmediately = true) => Task.Run(action);

    public Task<T> TryRunOnUIThreadAsync<T>(Func<T> func, bool runImmediately = true) => Task.Run<T>(func);

    public Task TryRunOnUIThreadAsync(Func<Task> func, bool runImmediately = true) => Task.Run(func);

    public Task<T> TryRunOnUIThreadAsync<T>(Func<Task<T>> func, bool runImmediately = true) => Task.Run<T>(func);
  }
}
