// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.MsaAuthorizationHandler
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
  public sealed class MsaAuthorizationHandler : AuthorizationHandlerBase
  {
    private readonly IMsaTokenProvider tokenProvider;

    public MsaAuthorizationHandler(HttpMessageHandler innerHandler, string msaToken)
      : this(innerHandler, (IMsaTokenProvider) new SimpleMsaTokenProvider(msaToken), false)
    {
    }

    public MsaAuthorizationHandler(
      HttpMessageHandler innerHandler,
      IMsaTokenProvider tokenProvider,
      bool allowUI)
      : base(innerHandler, allowUI)
    {
      this.tokenProvider = tokenProvider != null ? tokenProvider : throw new ArgumentNullException(nameof (tokenProvider));
    }

    protected override Task<string> GetHeaderValueAsync(
      bool allowUI,
      CancellationToken cancellationToken)
    {
      return this.tokenProvider.GetAsync(allowUI);
    }
  }
}
