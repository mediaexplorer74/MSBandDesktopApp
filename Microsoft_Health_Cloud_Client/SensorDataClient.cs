// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.SensorDataClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  internal class SensorDataClient : HealthCloudClientBase, ISensorDataClient
  {
    public SensorDataClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
    }

    public Task UploadSensorDataAsync(
      SensorData sensorData,
      string deviceMetadataString,
      string uploadMetadataJsonString,
      CancellationToken cancellationToken)
    {
      return (Task) this.PostJsonAsync<SensorData>("v2/multidevice/UploadSensorPayload", (NameValueCollection) null, sensorData, deviceMetadataString, uploadMetadataJsonString, cancellationToken);
    }

    public Task<SensorData> ReadTopSensorDataAsync(
      string deviceId,
      DateTimeOffset endTime,
      int count,
      CancellationToken cancellationToken)
    {
      if (count < 1)
        throw new ArgumentOutOfRangeException(nameof (count), "count must be positive.");
      string relativeUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}(deviceId='{1}',endUtcTime='{2}')", new object[3]
      {
        (object) "v2/SensorData",
        (object) deviceId,
        (object) endTime.ToUniversalTime().ToString("s")
      });
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("top", count.ToString());
      NameValueCollection parameters = nameValueCollection;
      return this.GetJsonAsync<SensorData>(relativeUrl, cancellationToken, parameters);
    }
  }
}
