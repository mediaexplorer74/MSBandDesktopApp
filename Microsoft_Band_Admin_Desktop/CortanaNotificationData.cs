// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CortanaNotificationData
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.InteropServices;

namespace Microsoft.Band.Admin
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  internal struct CortanaNotificationData
  {
    internal CortanaStatus Status;
    internal ushort StringLengthInBytes;
    internal byte RSVD1;
    internal byte RSVD2;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 320)]
    internal string String;
  }
}
