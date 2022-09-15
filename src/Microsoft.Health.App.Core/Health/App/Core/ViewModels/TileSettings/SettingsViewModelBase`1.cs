// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.SettingsViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public abstract class SettingsViewModelBase<T> : SettingsViewModelBase where T : Microsoft.Health.App.Core.Services.TileSettings.PendingTileSettings
  {
    protected SettingsViewModelBase(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService errorHandlingService,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManagementService, errorHandlingService, navService, cargoConnectionFactory)
    {
    }

    protected T PendingTileSettings { get; private set; }

    protected override async Task LoadPendingSettingsAsync() => this.PendingTileSettings = await this.TileManagementService.GetPendingSettingsAsync<T>();
  }
}
