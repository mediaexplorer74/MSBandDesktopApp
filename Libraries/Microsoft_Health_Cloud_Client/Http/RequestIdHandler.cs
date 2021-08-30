// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.RequestIdHandler
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public sealed class RequestIdHandler : DelegatingHandler
  {
    private const string RequestIdHeaderName = "AppEx-Activity-Id";

    public RequestIdHandler(HttpMessageHandler innerHandler)
      : base(innerHandler)
    {
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      object obj;
      if (request.Properties.TryGetValue(HealthCloudClientRequestContext.KeyName, out obj) && obj is HealthCloudClientRequestContext clientRequestContext && !string.IsNullOrWhiteSpace(clientRequestContext.RequestId))
        request.Headers.Add("AppEx-Activity-Id", clientRequestContext.RequestId);
      return base.SendAsync(request, cancellationToken);
    }
  }
}
