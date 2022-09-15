// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.DeviceSyncTime
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Devices;
using System;
using System.Diagnostics;

namespace Microsoft.Health.App.Core.Models
{
  public class DeviceSyncTime
  {
    public DeviceSyncTime(DeviceType deviceType, DateTimeOffset? syncTime)
    {
      this.DeviceType = deviceType;
      this.Glyph = DeviceSyncTime.ConvertEnumToGlyph(deviceType);
      this.SyncTime = DeviceSyncTime.ConvertSyncTimeIfNecessary(syncTime);
    }

    public DeviceType DeviceType { get; private set; }

    public string Glyph { get; private set; }

    public DateTimeOffset? SyncTime { get; private set; }

    private static string ConvertEnumToGlyph(DeviceType deviceType)
    {
      if (deviceType == DeviceType.Band)
        return "\uE175";
      if (deviceType == DeviceType.Phone)
        return "\uE143";
      Debugger.Break();
      return string.Empty;
    }

    private static DateTimeOffset? ConvertSyncTimeIfNecessary(DateTimeOffset? syncTime)
    {
      if (syncTime.HasValue)
      {
        DateTimeOffset? nullable = syncTime;
        DateTimeOffset minValue = DateTimeOffset.MinValue;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == minValue ? 1 : 0) : 1) : 0) != 0)
        {
          nullable = new DateTimeOffset?();
          return nullable;
        }
      }
      return syncTime;
    }
  }
}
