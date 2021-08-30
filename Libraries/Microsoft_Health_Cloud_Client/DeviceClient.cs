// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.DeviceClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  internal sealed class DeviceClient : HealthCloudClientBase, IDeviceClient
  {
    public DeviceClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
    }

    public Task AddDevicesAsync(
      IEnumerable<RegisteredDeviceSettings> devices,
      CancellationToken cancellationToken)
    {
      return (Task) this.PostJsonAsync<IEnumerable<RegisteredDeviceSettings>>("v2/Devices", (NameValueCollection) null, devices, cancellationToken);
    }

    public async Task<IEnumerable<RegisteredDeviceSettings>> GetDevicesAsync(
      CancellationToken cancellationToken)
    {
      return (IEnumerable<RegisteredDeviceSettings>) await this.GetJsonAsync<List<RegisteredDeviceSettings>>("v2/Devices", cancellationToken).ConfigureAwait(false);
    }

    public async Task RemoveDeviceAsync(string deviceId, CancellationToken cancellationToken)
    {
      StringBuilder stringBuilder = new StringBuilder("v2/Devices");
      stringBuilder.AppendFormat("?DeviceId={0}", new object[1]
      {
        (object) deviceId
      });
      await this.DeleteAsync(stringBuilder.ToString(), cancellationToken);
    }
  }
}
