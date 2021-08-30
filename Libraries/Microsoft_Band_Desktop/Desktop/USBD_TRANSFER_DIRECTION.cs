// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.USBD_TRANSFER_DIRECTION
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;

namespace Microsoft.Band.Desktop
{
  [Flags]
  internal enum USBD_TRANSFER_DIRECTION : uint
  {
    USBD_TRANSFER_DIRECTION_OUT = 0,
    USBD_TRANSFER_DIRECTION_IN = 128, // 0x00000080
  }
}
