// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.DispatcherServiceExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public static class DispatcherServiceExtensions
  {
    public static async Task TryRunOnUIThreadAsync(
      this IDispatchService dispatchService,
      Action action,
      bool runImmediately = true)
    {
      try
      {
        await dispatchService.RunOnUIThreadAsync(action, runImmediately);
      }
      catch (HeadlessException ex)
      {
      }
    }

    public static async Task<T> TryRunOnUIThreadAsync<T>(
      this IDispatchService dispatchService,
      Func<T> func,
      bool runImmediately = true)
    {
      T result = default (T);
      try
      {
        result = await dispatchService.RunOnUIThreadAsync<T>(func, runImmediately);
      }
      catch (HeadlessException ex)
      {
      }
      return result;
    }

    public static async Task TryRunOnUIThreadAsync(
      this IDispatchService dispatchService,
      Func<Task> func,
      bool runImmediately = true)
    {
      try
      {
        await dispatchService.RunOnUIThreadAsync(func, runImmediately);
      }
      catch (HeadlessException ex)
      {
      }
    }

    public static async Task<T> TryRunOnUIThreadAsync<T>(
      this IDispatchService dispatchService,
      Func<Task<T>> func,
      bool runImmediately = true)
    {
      T result = default (T);
      try
      {
        result = await dispatchService.RunOnUIThreadAsync<T>(func, runImmediately);
      }
      catch (HeadlessException ex)
      {
      }
      return result;
    }
  }
}
