// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.FitnessPlanWorkout
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.InteropServices;

namespace Microsoft.Band.Admin
{
  internal struct FitnessPlanWorkout
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    internal byte[] Id;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
    internal byte[] Name;
    internal byte NameLength;
    internal ushort CircuitCount;
    internal CompletionType CompletionType;
    internal uint CompletionValue;
  }
}
