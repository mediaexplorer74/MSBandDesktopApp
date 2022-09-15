// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.BandHardwareService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class BandHardwareService : IBandHardwareService
  {
    private const string HardwareTypeConfigKey = "BandHardwareService.Type";
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IConfigProvider configProvider;

    public BandHardwareService(
      IBandConnectionFactory cargoConnectionFactory,
      IConfigProvider configProvider)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.configProvider = configProvider;
    }

    public async Task<BandClass> GetDeviceTypeAsync(CancellationToken token)
    {
      BandClass bandClass;
      if (this.configProvider.TryGetEnum<BandClass>("BandHardwareService.Type", out bandClass))
        return bandClass;
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        IDynamicBandConstants bandConstantsAsync = await cargoConnection.GetAppBandConstantsAsync(token);
        this.configProvider.SetEnum<BandClass>("BandHardwareService.Type", bandConstantsAsync.BandClass);
        return bandConstantsAsync.BandClass;
      }
    }

    public Task ClearDeviceTypeAsync(CancellationToken token)
    {
      if (this.configProvider.TryGetEnum<BandClass>("BandHardwareService.Type", out BandClass _))
        this.configProvider.Remove("BandHardwareService.Type");
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
