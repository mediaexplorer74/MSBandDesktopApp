// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.CortanaSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class CortanaSettingsViewModel : SettingsViewModelBase
  {
    public CortanaSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
    }

    public override string TileGuid => "d7fb5ff5-906a-4f2c-8269-dde6a75138c4";

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      this.Header = AppResources.CortanaSettings;
      this.Subheader = AppResources.CortanaSettingsEnableNotifications;
      this.HasNotifications = true;
      await this.RefreshNotificationToggleAsync();
    }
  }
}
