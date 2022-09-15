// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.IHealthDiscoveryClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public interface IHealthDiscoveryClient
  {
    IHttpTracingConfiguration Configuration { get; }

    Task<HttpResponseMessage> CreateUserAsync(
      CancellationToken cancellationToken);

    Task<MsaUserProfile> GetMsaUserProfileAsync(
      CancellationToken cancellationToken);

    Task<KdsResponse> AuthenticateAsync(CancellationToken cancellationToken);

    Task<UserInfo> GetUserAsync(CancellationToken cancellationToken);
  }
}
