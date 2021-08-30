// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.EditMailListViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles", "Email VIP List"})]
  public class EditMailListViewModel : PageViewModelBase
  {
    private const int MaxVips = 10;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\TileSettings\\EditMailListViewModel.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly ITileManagementService tileManagementService;
    private readonly ISmoothNavService navService;
    private readonly ILauncherService launcherService;
    private HealthCommand addCommand;
    private HealthCommand cancelCommand;
    private HealthCommand confirmCommand;
    private HealthCommand<EmailAddress> removeCommand;
    private MailPendingTileSettings pendingTileSettings;
    private IList<EmailAddress> vips;

    public EditMailListViewModel(
      IConfig config,
      ISmoothNavService navService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      ITileManagementService tileManagementService,
      ILauncherService launcherService)
      : base(networkService)
    {
      this.navService = navService;
      this.messageBoxService = messageBoxService;
      this.tileManagementService = tileManagementService;
      this.launcherService = launcherService;
    }

    public IList<EmailAddress> Vips
    {
      get => this.vips;
      set => this.SetProperty<IList<EmailAddress>>(ref this.vips, value, nameof (Vips));
    }

    public ICommand AddCommand => (ICommand) this.addCommand ?? (ICommand) (this.addCommand = new HealthCommand(new Action(this.AddVip)));

    private async void AddVip()
    {
      if (this.Vips.Count < 10)
      {
        await this.AddEmailsAsync(await this.launcherService.PromptEmailAsync());
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorMailMaxVIPsBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
      }
    }

    public ICommand RemoveCommand => (ICommand) this.removeCommand ?? (ICommand) (this.removeCommand = new HealthCommand<EmailAddress>(new Action<EmailAddress>(this.RemoveVip)));

    private void RemoveVip(EmailAddress vip)
    {
      if (this.Vips == null)
        return;
      this.Vips.Remove(vip);
      this.ShowAppBar();
    }

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand(new Action(this.SaveVips)));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.GoBack)));

    private void GoBack() => this.navService.GoBack();

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EditMailListViewModel mailListViewModel = this;
      MailPendingTileSettings pendingTileSettings = mailListViewModel.pendingTileSettings;
      MailPendingTileSettings pendingSettingsAsync = await this.tileManagementService.GetPendingSettingsAsync<MailPendingTileSettings>();
      mailListViewModel.pendingTileSettings = pendingSettingsAsync;
      mailListViewModel = (EditMailListViewModel) null;
      this.Vips = (IList<EmailAddress>) new ObservableCollection<EmailAddress>((IEnumerable<EmailAddress>) this.pendingTileSettings.Vips);
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.ShowAppBar();
    }

    public void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private async Task AddEmailsAsync(IList<EmailAddress> emails)
    {
      foreach (EmailAddress email1 in (IEnumerable<EmailAddress>) emails)
      {
        EmailAddress email = email1;
        if (this.Vips.Count < 10)
        {
          if (!this.Vips.Any<EmailAddress>((Func<EmailAddress, bool>) (item => item.Name == email.Name && item.Email == email.Email)))
          {
            this.Vips.Add(email);
          }
          else
          {
            int num1 = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorMailDuplicateBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
          }
        }
        else
        {
          int num2 = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorMailMaxVIPsBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
          break;
        }
      }
    }

    private void SaveVips()
    {
      if (this.Vips == null)
        return;
      try
      {
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        this.LoadState = LoadState.Loading;
        this.pendingTileSettings.Vips = (IList<EmailAddress>) this.Vips.ToList<EmailAddress>();
        this.navService.GoBack();
      }
      catch (Exception ex)
      {
        EditMailListViewModel.Logger.Error(ex, "Exception encountered while trying to save VIP list");
        this.ShowAppBar();
      }
      finally
      {
        this.LoadState = LoadState.Loaded;
      }
    }
  }
}
