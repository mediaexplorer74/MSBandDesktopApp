// Decompiled with JetBrains decompiler
// Type: System.Runtime.CompilerServices.AsyncServices
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System.Threading;

namespace System.Runtime.CompilerServices
{
  internal static class AsyncServices
  {
    internal static void ThrowAsync(Exception exception, SynchronizationContext targetContext)
    {
      if (targetContext != null)
      {
        try
        {
          targetContext.Post((SendOrPostCallback) (state =>
          {
            throw AsyncServices.PrepareExceptionForRethrow((Exception) state);
          }), (object) exception);
          return;
        }
        catch (Exception ex)
        {
          exception = (Exception) new AggregateException(new Exception[2]
          {
            exception,
            ex
          });
        }
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
      {
        throw AsyncServices.PrepareExceptionForRethrow((Exception) state);
      }), (object) exception);
    }

    internal static Exception PrepareExceptionForRethrow(Exception exc) => exc;
  }
}
