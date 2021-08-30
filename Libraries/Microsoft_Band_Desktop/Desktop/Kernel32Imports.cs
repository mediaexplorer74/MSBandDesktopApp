// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.Kernel32Imports
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Band.Desktop
{
  internal class Kernel32Imports
  {
    public const uint ERROR_INSUFFICIENT_BUFFER = 122;
    public const uint ERROR_NO_MORE_ITEMS = 259;
    public const uint ERROR_IO_PENDING = 997;

    public static class NativeMethods
    {
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      public static extern IntPtr CreateFile(
        [MarshalAs(UnmanagedType.LPTStr)] string filename,
        [MarshalAs(UnmanagedType.U4)] EFileAccess access,
        [MarshalAs(UnmanagedType.U4)] EFileShare share,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] ECreationDisposition creationDisposition,
        [MarshalAs(UnmanagedType.U4)] EFileAttributes flagsAndAttributes,
        IntPtr templateFile);

      [DllImport("kernel32.dll", SetLastError = true)]
      public static extern bool GetOverlappedResult(
        IntPtr fileHandle,
        IntPtr overlapped,
        ref uint bytesTransferred,
        bool wait);

      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool CloseHandle(IntPtr hObject);
    }
  }
}
