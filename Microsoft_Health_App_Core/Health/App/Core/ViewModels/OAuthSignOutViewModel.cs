// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.OAuthSignOutViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class OAuthSignOutViewModel : PanelViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\OAuthSignOutViewModel.cs");
    private readonly IOAuthMsaTokenStore oAuthTokenStore;
    private readonly ISmoothNavService smoothNavService;
    private bool isLoading;
    private HealthCommand loadCompletedCommand;
    private string logoutUrl;
    private HealthCommand retryCommand;

    public event EventHandler LoginUrlChanged;

    public OAuthSignOutViewModel(
      IConfig config,
      ISmoothNavService smoothNavService,
      IOAuthMsaTokenStore oAuthTokenStore,
      INetworkService networkService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.oAuthTokenStore = oAuthTokenStore;
    }

    public bool IsLoading
    {
      get => this.isLoading;
      set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public string LogoutUrl
    {
      get => this.logoutUrl;
      set
      {
        if (!this.SetProperty<string>(ref this.logoutUrl, value, nameof (LogoutUrl)) || this.LoginUrlChanged == null)
          return;
        this.LoginUrlChanged((object) this, EventArgs.Empty);
      }
    }

    public ICommand RetryCommand => (ICommand) this.retryCommand ?? (ICommand) (this.retryCommand = new HealthCommand(new Action(this.Retry)));

    private void Retry()
    {
      this.IsLoading = true;
      this.LogoutUrl = (string) null;
      this.LoadLogoutPage();
    }

    public ICommand LoadCompletedCommand => (ICommand) this.loadCompletedCommand ?? (ICommand) (this.loadCompletedCommand = new HealthCommand((Action) (async () =>
    {
      await this.oAuthTokenStore.ClearAsync();
      this.IsLoading = false;
      this.smoothNavService.GoBack();
    })));

    private void LoadLogoutPage() => this.LogoutUrl = "https://login.live.com/oauth20_logout.srf";

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.IsLoading = true;
      this.LoadLogoutPage();
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
