// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.VersionExtensions
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin
{
  internal static class VersionExtensions
  {
    public static FirmwareVersions ToFirmwareVersions(this CargoVersions versions) => new FirmwareVersions()
    {
      BootloaderVersion = new FirmwareVersion(versions.BootloaderVersion),
      UpdaterVersion = new FirmwareVersion(versions.UpdaterVersion),
      ApplicationVersion = new FirmwareVersion(versions.ApplicationVersion),
      PcbId = versions.ApplicationVersion.PCBId
    };
  }
}
