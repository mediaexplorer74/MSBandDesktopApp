// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.MailSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class MailSettingsViewModel : SettingsViewModelBase<MailPendingTileSettings>
  {
    private readonly ISmoothNavService navService;
    private IList<EmailAddress> vips;
    private bool enableVipNotifications;
    private string mailListText;

    public MailSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
      this.navService = navService;
      this.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "EnableNotifications") || this.EnableNotifications || !this.EnableVipNotifications)
        return;
      this.EnableVipNotifications = false;
    }

    public override string TileGuid => "823ba55a-7c98-4261-ad5e-929031289c6e";

    public override bool CanEdit => true;

    public IList<EmailAddress> Vips
    {
      get => this.vips;
      set => this.SetProperty<IList<EmailAddress>>(ref this.vips, value, nameof (Vips));
    }

    public bool EnableVipNotifications
    {
      get => this.enableVipNotifications;
      set
      {
        this.SetProperty<bool>(ref this.enableVipNotifications, value, nameof (EnableVipNotifications));
        if (this.SuppressNotificationChange)
          return;
        if (!this.EnableNotifications & value)
          this.EnableNotifications = this.EnableVipNotifications;
        this.PendingTileSettings.AreVipsEnabled = this.EnableVipNotifications;
      }
    }

    public string MailListText
    {
      get => this.mailListText;
      set => this.SetProperty<string>(ref this.mailListText, value, nameof (MailListText));
    }

    protected override void OnBackNavigation()
    {
      this.RefreshVips();
      base.OnBackNavigation();
    }

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      this.Header = AppResources.MailSettings;
      this.Subheader = AppResources.MailSettingsEnableNotifications;
      this.HasNotifications = true;
      await this.RefreshNotificationToggleAsync();
      this.RefreshVips();
    }

    private void RefreshVips()
    {
      this.EnableVipNotifications = this.EnableNotifications && this.PendingTileSettings.AreVipsEnabled;
      this.Vips = (IList<EmailAddress>) new ObservableCollection<EmailAddress>((IEnumerable<EmailAddress>) this.PendingTileSettings.Vips);
      if (this.Vips.Count == 0)
      {
        this.MailListText = AppResources.MailSettingsListEmpty;
      }
      else
      {
        string str;
        if (this.Vips.Count != 1)
          str = string.Format(AppResources.MailSettingsListMultiple, new object[1]
          {
            (object) this.Vips.Count
          });
        else
          str = AppResources.MailSettingsListSingle;
        this.MailListText = str;
      }
    }

    protected override void OnEdit() => this.navService.Navigate(typeof (EditMailListViewModel));
  }
}
