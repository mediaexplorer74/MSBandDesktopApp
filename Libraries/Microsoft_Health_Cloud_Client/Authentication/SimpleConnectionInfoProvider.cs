// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Authentication.SimpleConnectionInfoProvider
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Authentication
{
  internal class SimpleConnectionInfoProvider : IConnectionInfoProvider
  {
    private HealthCloudConnectionInfo connectionInfo;

    public SimpleConnectionInfoProvider(HealthCloudConnectionInfo connectionInfo) => this.connectionInfo = connectionInfo;

    public Task<HealthCloudConnectionInfo> GetConnectionInfoAsync(
      CancellationToken cancellationToken,
      bool allowUI = false)
    {
      return Task.FromResult<HealthCloudConnectionInfo>(this.connectionInfo);
    }
  }
}
