// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.IBandAdminClientManager
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Threading.Tasks;

namespace Microsoft.Band.Admin
{
  public interface IBandAdminClientManager
  {
    Task<IBandInfo[]> GetBandsAsync();

    IBandInfo[] GetBands();

    Task<ICargoClient> ConnectAsync(IBandInfo bandInfo);

    ICargoClient Connect(IBandInfo bandInfo);

    Task<ICargoClient> ConnectAsync(ServiceInfo serviceInfo);

    ICargoClient Connect(ServiceInfo serviceInfo);

    Task<ICargoClient> ConnectAsync(IBandInfo bandInfo, string userId);

    ICargoClient Connect(IBandInfo bandInfo, string userId);

    Task<ICargoClient> ConnectAsync(string bandId, ServiceInfo serviceInfo);

    ICargoClient Connect(string bandId, ServiceInfo serviceInfo);

    Task<ICargoClient> ConnectAsync(IBandInfo bandInfo, ServiceInfo serviceInfo);

    ICargoClient Connect(IBandInfo bandInfo, ServiceInfo serviceInfo);
  }
}
