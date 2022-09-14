// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.QuickResponseSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class QuickResponseSettingsViewModel : 
    SettingsViewModelBase<QuickResponsePendingTileSettings>
  {
    internal const string TileIdKey = "TileId";
    private readonly ISmoothNavService navService;
    private string tileGuidString;
    private string quickResponsesHeader;
    private IList<string> quickResponses;

    public QuickResponseSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
      this.navService = navService;
    }

    public override bool CanEdit => true;

    public override string TileGuid => this.tileGuidString;

    public string QuickResponsesHeader
    {
      get => this.quickResponsesHeader;
      set => this.SetProperty<string>(ref this.quickResponsesHeader, value, nameof (QuickResponsesHeader));
    }

    public IList<string> QuickResponses
    {
      get => this.quickResponses;
      set => this.SetProperty<IList<string>>(ref this.quickResponses, value, nameof (QuickResponses));
    }

    private bool IsMessage => this.TileGuid == "b4edbc35-027b-4d10-a797-1099cd2ad98a";

    protected override void OnBackNavigation()
    {
      this.RefreshQuickResponses();
      base.OnBackNavigation();
    }

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      this.tileGuidString = parameters != null && parameters.ContainsKey("TileId") && !string.IsNullOrEmpty(parameters["TileId"]) ? parameters["TileId"] : throw new ArgumentException("TileId is a required parameter.");
      string str = Guid.TryParse(this.tileGuidString, out Guid _) ? this.tileGuidString : throw new ArgumentException("TileId is not a valid GUID.");
      if (!(str == "b4edbc35-027b-4d10-a797-1099cd2ad98a"))
      {
        if (!(str == "22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"))
          throw new ArgumentException("Unrecognized tile GUID: " + this.tileGuidString);
        this.Header = AppResources.CallsSettings;
        this.Subheader = AppResources.CallsSettingsEnableNotifications;
        this.QuickResponsesHeader = AppResources.CallQuickResponses;
      }
      else
      {
        this.Header = AppResources.MessageSettings;
        this.Subheader = AppResources.MessageSettingsEnableNotifications;
        this.QuickResponsesHeader = AppResources.SMSQuickResponses;
      }
      this.RefreshQuickResponses();
      this.HasNotifications = true;
      await this.RefreshNotificationToggleAsync();
    }

    protected override void OnEdit() => this.navService.Navigate(typeof (EditResponsesViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "TileId",
        this.TileGuid
      }
    });

    private void RefreshQuickResponses() => this.QuickResponses = (IList<string>) (this.IsMessage ? (IEnumerable<string>) this.PendingTileSettings.MessageResponses : (IEnumerable<string>) this.PendingTileSettings.PhoneResponses).Where<string>((Func<string, bool>) (r => !string.IsNullOrEmpty(r))).ToList<string>();
  }
}
