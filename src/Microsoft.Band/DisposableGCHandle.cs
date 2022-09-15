// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.DisposableGCHandle
// Assembly: Microsoft.Band, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: AFCBFE03-E2CF-481D-86F4-92C60C36D26A
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Band
{
  internal sealed class DisposableGCHandle : IDisposable
  {
    private GCHandle handle;

    internal static DisposableGCHandle Alloc(
      object target,
      GCHandleType handleType = GCHandleType.Normal)
    {
      return target != null ? new DisposableGCHandle()
      {
        handle = GCHandle.Alloc(target, handleType)
      } : throw new ArgumentNullException(nameof (target));
    }

    internal object Target => this.handle.Target;

    internal bool IsAllocated => this.handle.IsAllocated;

    internal IntPtr AddrOfPinnedObject() => this.handle.AddrOfPinnedObject();

    internal void Free() => this.handle.Free();

    public void Dispose()
    {
      if (!this.IsAllocated)
        return;
      this.Free();
    }
  }
}
