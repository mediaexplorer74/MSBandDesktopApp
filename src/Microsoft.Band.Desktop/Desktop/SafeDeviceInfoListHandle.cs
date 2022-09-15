// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.SafeDeviceInfoListHandle
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using Microsoft.Win32.SafeHandles;
using System;

namespace Microsoft.Band.Desktop
{
  internal class SafeDeviceInfoListHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public SafeDeviceInfoListHandle(IntPtr existingHandle)
      : base(true)
    {
      this.handle = existingHandle;
    }

    public IntPtr Handle => this.handle;

    protected override bool ReleaseHandle() => SetupApiImports.NativeMethods.SetupDiDestroyDeviceInfoList(this.handle);
  }
}
