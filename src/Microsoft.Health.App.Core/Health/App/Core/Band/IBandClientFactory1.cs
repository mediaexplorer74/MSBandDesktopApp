// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.IBandClientFactory`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public interface IBandClientFactory<TClient>
  {
    Task<TClient> CreateClientAsync(IBandInfo bandInfo, ServiceInfo serviceInfo);

    Task<TClient> CreateBandOnlyClientAsync(IBandInfo bandInfo, string userId);

    Task<TClient> CreateCloudOnlyClientAsync(string bandId, ServiceInfo serviceInfo);
  }
}
