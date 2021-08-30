// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Desktop.DesktopProvider
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Microsoft.Band.Admin.Desktop
{
  internal class DesktopProvider : IPlatformProvider
  {
    public int MaxChunkRange => 128;

    public void Sleep(int milliseconds) => Thread.Sleep(milliseconds);

    public string GetAssemblyVersion() => typeof (CargoClient).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

    public Version GetHostOSVersion() => Environment.OSVersion.Version;

    public string GetHostOS() => "Windows Desktop";

    public string GetDefaultUserAgent(FirmwareVersions firmwareVersions)
    {
      string assemblyVersion = this.GetAssemblyVersion();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("KDK/{0} (.NET CLR/{1}; {2}/{3}; {4})", (object) assemblyVersion, (object) Environment.Version, (object) this.GetHostOS(), (object) this.GetHostOSVersion(), (object) CultureInfo.CurrentCulture.Name);
      if (firmwareVersions != null)
        stringBuilder.AppendFormat(" Cargo/{0} (PcbId/{1})", (object) firmwareVersions.ApplicationVersion, (object) firmwareVersions.PcbId);
      return stringBuilder.ToString();
    }

    public byte[] ComputeHashMd5(byte[] data)
    {
      using (MD5 md5 = MD5.Create())
        return md5.ComputeHash(data);
    }

    public byte[] ComputeHashMd5(Stream data)
    {
      using (MD5 md5 = MD5.Create())
        return md5.ComputeHash(data);
    }
  }
}
