// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.OAuthSignInViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class OAuthSignInViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\OAuthSignInViewModel.cs");
    private readonly IConfig config;
    private readonly IOAuthMsaTokenStore oAuthTokenStore;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private bool isBrowserVisible;
    private bool isLoading;
    private HealthCommand<WebViewNavigationCompletedEventArgsWrapper> loadCompletedCommand;
    private string loginUrl;
    private HealthCommand retryCommand;

    public event EventHandler LoginUrlChanged;

    public OAuthSignInViewModel(
      IConfig config,
      ISmoothNavService smoothNavService,
      IOAuthMsaTokenStore oAuthTokenStore,
      INetworkService networkService,
      IErrorHandlingService errorHandlingService,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.config = config;
      this.smoothNavService = smoothNavService;
      this.oAuthTokenStore = oAuthTokenStore;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
    }

    public bool IsBrowserVisible
    {
      get => this.isBrowserVisible;
      set => this.SetProperty<bool>(ref this.isBrowserVisible, value, nameof (IsBrowserVisible));
    }

    public bool IsLoading
    {
      get => this.isLoading;
      set => this.SetProperty<bool>(ref this.isLoading, value, nameof (IsLoading));
    }

    public string LoginUrl
    {
      get => this.loginUrl;
      set
      {
        if (!this.SetProperty<string>(ref this.loginUrl, value, nameof (LoginUrl)))
          return;
        EventHandler loginUrlChanged = this.LoginUrlChanged;
        if (loginUrlChanged == null)
          return;
        loginUrlChanged((object) this, EventArgs.Empty);
      }
    }

    public ICommand RetryCommand => (ICommand) this.retryCommand ?? (ICommand) (this.retryCommand = new HealthCommand(new Action(this.Retry)));

    private void Retry()
    {
      this.IsBrowserVisible = false;
      this.IsLoading = true;
      this.LoginUrl = (string) null;
      this.LoadLoginPage();
    }

    public ICommand LoadCompletedCommand => (ICommand) this.loadCompletedCommand ?? (ICommand) (this.loadCompletedCommand = new HealthCommand<WebViewNavigationCompletedEventArgsWrapper>((Action<WebViewNavigationCompletedEventArgsWrapper>) (async e =>
    {
      this.IsLoading = false;
      Exception ex = (Exception) null;
      if (e.IsSuccess && e.Uri.AbsoluteUri.Contains("access_token"))
      {
        try
        {
          OAuthSignInViewModel.Logger.Debug((object) "Got access token response.");
          await this.oAuthTokenStore.SetAsync(OAuthSignInViewModel.CreateStoredMsaTokenFromUri(e.Uri));
          if (this.NavigationState != PageNavigationState.Current)
          {
            OAuthSignInViewModel.Logger.Debug((object) string.Format("Extracted and stored MSA token. Navigation state {0} != Current, returning without navigating back.", new object[1]
            {
              (object) this.NavigationState
            }));
            return;
          }
          OAuthSignInViewModel.Logger.Debug((object) "Extracted and stored MSA token. Navigating back.");
          this.smoothNavService.GoBack();
        }
        catch (Exception ex1)
        {
          ex = ex1;
          OAuthSignInViewModel.Logger.ErrorAndDebug(ex1, "Unexpected error parsing token from OAuth page.");
        }
      }
      else if (!e.IsSuccess)
      {
        string message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "OAuth navigation failed. URL: {0} , WebErrorStatus: {1}", new object[2]
        {
          (object) e.Uri,
          (object) e.WebErrorStatus
        });
        ex = (Exception) new HttpRequestException(message);
        OAuthSignInViewModel.Logger.Error((object) message);
      }
      if (ex != null)
      {
        this.IsBrowserVisible = false;
        if (await this.errorHandlingService.HandleExceptionWithRetryAsync(ex))
        {
          this.Retry();
        }
        else
        {
          OAuthSignInViewModel.Logger.Debug((object) "Failed to get MSA token. Navigating back.");
          this.smoothNavService.GoBack();
        }
      }
      else
        this.IsBrowserVisible = true;
    })));

    private static StoredMsaToken CreateStoredMsaTokenFromUri(Uri uri)
    {
      IDictionary<string, string> fragmentAsQuery = uri.ParseFragmentAsQuery();
      string str1 = fragmentAsQuery["access_token"];
      if (string.IsNullOrEmpty(str1))
        throw new InvalidOperationException("Could not find access token in auth response.");
      string str2;
      if (!fragmentAsQuery.TryGetValue("refresh_token", out str2) || string.IsNullOrEmpty(str2))
        throw new InvalidOperationException("Could not find refresh token in auth response.");
      string s;
      if (!fragmentAsQuery.TryGetValue("expires_in", out s))
        throw new InvalidOperationException("Could not find expires value in auth response.");
      int result;
      if (!int.TryParse(s, out result))
        throw new InvalidOperationException("Invalid expiry value in auth response.");
      return new StoredMsaToken()
      {
        MsaToken = str1,
        RefreshToken = str2,
        Expiry = DateTimeOffset.UtcNow.AddSeconds((double) result)
      };
    }

    private void LoadLoginPage() => this.LoginUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=service::{1}::MBI_SSL&response_type=token&redirect_uri=https://login.live.com/oauth20_desktop.srf", new object[2]
    {
      (object) "000000004811DB42",
      (object) new Uri(this.config.AuthBaseUrl).Host
    });

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.IsBrowserVisible = false;
      this.IsLoading = true;
      this.LoadLoginPage();
      return (Task) Task.FromResult<object>((object) null);
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message =>
      {
        if (this.IsBrowserVisible)
          return;
        message.CancelAction();
      }));
    }
  }
}
