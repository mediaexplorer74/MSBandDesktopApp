// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.DeviceContextInitializer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public class DeviceContextInitializer : IContextInitializer
  {
    public void Initialize(TelemetryContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      IDeviceContextReader instance = DeviceContextReader.Instance;
      context.Device.Type = instance.GetDeviceType();
      context.Device.Id = instance.GetDeviceUniqueId();
      instance.GetOperatingSystemAsync().ContinueWith((Action<Task<string>>) (task =>
      {
        if (!task.IsCompleted)
          return;
        context.Device.OperatingSystem = task.Result;
      }));
      context.Device.OemName = instance.GetOemName();
      context.Device.Model = instance.GetDeviceModel();
      context.Device.NetworkType = instance.GetNetworkType().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      instance.GetScreenResolutionAsync().ContinueWith((Action<Task<string>>) (task =>
      {
        if (!task.IsCompleted)
          return;
        context.Device.ScreenResolution = task.Result;
      }));
      context.Device.Language = instance.GetHostSystemLocale();
    }
  }
}
