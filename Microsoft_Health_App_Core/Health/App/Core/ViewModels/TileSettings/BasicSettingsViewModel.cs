// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.BasicSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class BasicSettingsViewModel : SettingsViewModelBase
  {
    internal const string TileIdKey = "TileId";
    internal const string TileTitleKey = "TileTitle";
    private string tileGuidString;

    public BasicSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
    }

    public override string TileGuid => this.tileGuidString;

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters == null || !parameters.TryGetValue("TileId", out this.tileGuidString))
        throw new ArgumentException("TileId is a required parameter.");
      string str1 = Guid.TryParse(this.tileGuidString, out Guid _) ? this.tileGuidString : throw new ArgumentException("TileId is not a valid GUID.");
      string header;
      string subheader;
      if (!(str1 == "ec149021-ce45-40e9-aeee-08f86e4746a7"))
      {
        if (!(str1 == "76b08699-2f2e-9041-96c2-1f4bfc7eab10"))
        {
          if (!(str1 == "fd06b486-bbda-4da5-9014-124936386237"))
          {
            if (str1 == "2e76a806-f509-4110-9c03-43dd2359d2ad")
            {
              header = AppResources.TwitterSettings;
              subheader = AppResources.TwitterSettingsEnableNotifications;
            }
            else
            {
              string str2;
              if (!parameters.TryGetValue("TileTitle", out str2))
                throw new ArgumentException("TileTitle is a required parameter.");
              header = string.Format(AppResources.TileSettingsHeaderFormat, new object[1]
              {
                (object) str2
              });
              subheader = string.Format(AppResources.TileSettingsSubheaderFormat, new object[1]
              {
                (object) str2
              });
            }
          }
          else
          {
            header = AppResources.FacebookSettings;
            subheader = AppResources.FacebookSettingsEnableNotifications;
          }
        }
        else
        {
          header = AppResources.FacebookMessengerSettings;
          subheader = AppResources.FacebookMessengerSettingsEnableNotifications;
        }
      }
      else
      {
        header = AppResources.CalendarSettings;
        subheader = AppResources.CalendarSettingsEnableNotifications;
      }
      this.Header = header;
      this.Subheader = subheader;
      this.HasNotifications = true;
      await this.RefreshNotificationToggleAsync();
    }
  }
}
