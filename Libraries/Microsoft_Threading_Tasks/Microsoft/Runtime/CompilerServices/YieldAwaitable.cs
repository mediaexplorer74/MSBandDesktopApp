// Decompiled with JetBrains decompiler
// Type: Microsoft.Runtime.CompilerServices.YieldAwaitable
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct YieldAwaitable
  {
    public YieldAwaitable.YieldAwaiter GetAwaiter() => new YieldAwaitable.YieldAwaiter();

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct YieldAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
      private static readonly Task s_completed = (Task) TaskEx.FromResult<int>(0);

      public bool IsCompleted => false;

      public void OnCompleted(Action continuation) => AwaitExtensions.GetAwaiter(YieldAwaitable.YieldAwaiter.s_completed).OnCompleted(continuation);

      public void UnsafeOnCompleted(Action continuation) => AwaitExtensions.GetAwaiter(YieldAwaitable.YieldAwaiter.s_completed).UnsafeOnCompleted(continuation);

      public void GetResult()
      {
      }
    }
  }
}
