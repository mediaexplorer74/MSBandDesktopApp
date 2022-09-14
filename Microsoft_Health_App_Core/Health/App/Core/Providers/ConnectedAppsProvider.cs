// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.ConnectedAppsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class ConnectedAppsProvider : IConnectedAppsProvider
  {
    public const string GolfPartnerName = "TMag";
    private readonly IHealthCloudClient healthCloudClient;

    public ConnectedAppsProvider(IHealthCloudClient healthCloudClient) => this.healthCloudClient = healthCloudClient;

    public async Task<IReadOnlyList<string>> GetConnectedAppsAsync(
      CancellationToken token)
    {
      return (IReadOnlyList<string>) (await this.healthCloudClient.GetConnectedAppsAsync(token)).Partners.Select<ConnectedApp, string>((Func<ConnectedApp, string>) (p => p.Name)).ToList<string>();
    }
  }
}
