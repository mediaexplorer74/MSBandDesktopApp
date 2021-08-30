// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.UserAgentHandler
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Services;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public sealed class UserAgentHandler : DelegatingHandler
  {
    private readonly IUserAgentService userAgentService;

    public UserAgentHandler(HttpMessageHandler innerHandler, IUserAgentService userAgentService = null)
      : base(innerHandler)
    {
      this.userAgentService = userAgentService ?? (IUserAgentService) new DefaultUserAgentService();
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      request.Headers.UserAgent.ParseAdd(this.userAgentService.UserAgent);
      return base.SendAsync(request, cancellationToken);
    }
  }
}
