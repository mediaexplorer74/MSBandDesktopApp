// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.CultureHandler
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
  public sealed class CultureHandler : DelegatingHandler
  {
    private readonly ICultureService cultureService;

    public CultureHandler(HttpMessageHandler innerHandler, ICultureService cultureService)
      : base(innerHandler)
    {
      this.cultureService = cultureService != null ? cultureService : throw new ArgumentNullException(nameof (cultureService));
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      request.Headers.AcceptLanguage.ParseAdd(this.cultureService.CurrentSupportedUICulture.Name);
      return base.SendAsync(request, cancellationToken);
    }
  }
}
