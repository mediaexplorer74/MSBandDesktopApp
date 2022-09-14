// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.WINUSB_PIPE_INFORMATION
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

namespace Microsoft.Band.Desktop
{
  internal struct WINUSB_PIPE_INFORMATION
  {
    public USBD_PIPE_TYPE PipeType;
    public byte PipeId;
    public ushort MaximumPacketSize;
    public byte Interval;
  }
}
