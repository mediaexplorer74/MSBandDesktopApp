// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.RegionHandler
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Services;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public sealed class RegionHandler : DelegatingHandler
  {
    private readonly IRegionService regionService;

    public RegionHandler(HttpMessageHandler innerHandler, IRegionService regionService)
      : base(innerHandler)
    {
      this.regionService = regionService != null ? regionService : throw new ArgumentNullException(nameof (regionService));
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      request.Headers.Add("Region", this.regionService.CurrentRegion.TwoLetterISORegionName);
      return base.SendAsync(request, cancellationToken);
    }
  }
}
