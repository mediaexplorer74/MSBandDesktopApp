// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.PodAuthorizationHandler
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Authentication;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public sealed class PodAuthorizationHandler : AuthorizationHandlerBase
  {
    private readonly IConnectionInfoProvider connectionInfoProvider;

    public PodAuthorizationHandler(
      HttpMessageHandler innerHandler,
      HealthCloudConnectionInfo connectionInfo)
      : this(innerHandler, (IConnectionInfoProvider) new SimpleConnectionInfoProvider(connectionInfo), false)
    {
    }

    public PodAuthorizationHandler(
      HttpMessageHandler innerHandler,
      IConnectionInfoProvider connectionInfoProvider,
      bool allowUI)
      : base(innerHandler, allowUI)
    {
      this.connectionInfoProvider = connectionInfoProvider != null ? connectionInfoProvider : throw new ArgumentNullException(nameof (connectionInfoProvider));
    }

    protected override async Task<string> GetHeaderValueAsync(
      bool allowUI,
      CancellationToken cancellationToken)
    {
      return AuthUtilities.WrapAccessToken((await this.connectionInfoProvider.GetConnectionInfoAsync(cancellationToken, allowUI).ConfigureAwait(false)).PodSecurityToken);
    }
  }
}
