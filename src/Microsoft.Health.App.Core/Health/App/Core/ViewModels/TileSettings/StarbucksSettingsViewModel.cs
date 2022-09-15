// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.StarbucksSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.Cloud.Client.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class StarbucksSettingsViewModel : SettingsViewModelBase<StarbucksPendingTileSettings>
  {
    private const int StarbucksCodeLength = 16;
    private readonly INetworkService networkService;
    private readonly ISmoothNavService navService;
    private readonly IRegionService regionService;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private HealthCommand addCardCommand;
    private HealthCommand removeCardCommand;
    private bool hasValidStarbucksCard;
    private string starbucksCardValue;

    public StarbucksSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory,
      IRegionService regionService,
      IDynamicConfigurationService dynamicConfigurationService)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
      this.networkService = networkService;
      this.navService = navService;
      this.regionService = regionService;
      this.dynamicConfigurationService = dynamicConfigurationService;
    }

    public override string TileGuid => "64a29f65-70bb-4f32-99a2-0f250a05d427";

    public bool HasValidStarbucksCards
    {
      get => this.hasValidStarbucksCard;
      set => this.SetProperty<bool>(ref this.hasValidStarbucksCard, value, nameof (HasValidStarbucksCards));
    }

    public string StarbucksCardDisplayValue
    {
      get
      {
        string str = this.starbucksCardValue;
        if (!string.IsNullOrEmpty(str) && str.Length == 16)
          str = str.Substring(0, 4) + " " + str.Substring(4, 4) + " " + str.Substring(8, 4) + " " + str.Substring(12, 4);
        return str;
      }
      set => this.SetProperty<string>(ref this.starbucksCardValue, value, nameof (StarbucksCardDisplayValue));
    }

    public Uri StarbucksLogin
    {
      get
      {
        if (this.dynamicConfigurationService.Configuration.Features.Starbucks.DisplayUrl != (Uri) null)
          return this.dynamicConfigurationService.Configuration.Features.Starbucks.DisplayUrl;
        return this.regionService.CurrentRegion.TwoLetterISORegionName == "GB" ? new Uri("https://go.microsoft.com/fwlink/?LinkId=615214", UriKind.Absolute) : new Uri("https://go.microsoft.com/fwlink/?LinkID=513161", UriKind.Absolute);
      }
    }

    public string StarbucksHyperlinkText
    {
      get
      {
        if (!string.IsNullOrEmpty(this.dynamicConfigurationService.Configuration.Features.Starbucks.DisplayUrlString))
          return this.dynamicConfigurationService.Configuration.Features.Starbucks.DisplayUrlString;
        return this.regionService.CurrentRegion.TwoLetterISORegionName == "GB" ? AppResources.StarbucksLoginTextUK : AppResources.StarbucksLoginTextUS;
      }
    }

    public string StarbucksCheckBalanceText => this.regionService.CurrentRegion.TwoLetterISORegionName == "GB" ? AppResources.StarbucksHelp2_UK : AppResources.StarbucksHelp2;

    public EmbeddedOrRemoteImageSource StarbucksCardFrontSource
    {
      get
      {
        if (this.dynamicConfigurationService.Configuration.Features.Starbucks.CardFrontUrl != (Uri) null && this.networkService.Connected)
          return new EmbeddedOrRemoteImageSource(this.dynamicConfigurationService.Configuration.Features.Starbucks.CardFrontUrl.ToString());
        return this.regionService.CurrentRegion.TwoLetterISORegionName == "GB" ? new EmbeddedOrRemoteImageSource(EmbeddedAsset.StarbucksCardFrontGb) : new EmbeddedOrRemoteImageSource(EmbeddedAsset.StarbucksCardFrontUs);
      }
    }

    public ICommand AddCardCommand => (ICommand) this.addCardCommand ?? (ICommand) (this.addCardCommand = new HealthCommand((Action) (() => this.navService.Navigate(typeof (EditStarbucksCardViewModel)))));

    public ICommand RemoveCardCommand => (ICommand) this.removeCardCommand ?? (ICommand) (this.removeCardCommand = new HealthCommand((Action) (() =>
    {
      this.PendingTileSettings.CardNumber = string.Empty;
      this.HasValidStarbucksCards = false;
    })));

    protected override Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      if (!string.IsNullOrEmpty(this.PendingTileSettings.CardNumber))
      {
        this.HasValidStarbucksCards = true;
        this.StarbucksCardDisplayValue = this.PendingTileSettings.CardNumber;
      }
      this.Header = AppResources.StarbucksSettings;
      return (Task) Task.FromResult<bool>(true);
    }

    protected override void OnBackNavigation()
    {
      this.Refresh();
      base.OnBackNavigation();
    }
  }
}
