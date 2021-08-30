// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.DiGetClassFlags
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;

namespace Microsoft.Band.Desktop
{
  [Flags]
  internal enum DiGetClassFlags : uint
  {
    DIGCF_DEFAULT = 1,
    DIGCF_PRESENT = 2,
    DIGCF_ALLCLASSES = 4,
    DIGCF_PROFILE = 8,
    DIGCF_DEVICEINTERFACE = 16, // 0x00000010
  }
}
