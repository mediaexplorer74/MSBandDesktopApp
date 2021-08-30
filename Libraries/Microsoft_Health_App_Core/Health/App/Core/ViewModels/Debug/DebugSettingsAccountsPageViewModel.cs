// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsAccountsPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class DebugSettingsAccountsPageViewModel : DebugSettingsPageViewModelBase
  {
    private readonly IConnectionInfoProvider connectionInfoProvider;
    private readonly IEmailService emailService;
    private readonly IMessageSender messageSender;
    private readonly IOobeService oobeService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IOAuthMsaTokenStore oAuthMsaTokenStore;
    private readonly HealthCommand applyAuthenticationCommand;
    private bool clearingData;
    private string debugInfo;
    private IConfig config;
    private bool useOAuth;
    private bool signOut;
    private TaskCompletionSource<object> navigationTaskCompletionSource;

    public bool UseOAuth
    {
      get => this.useOAuth;
      set
      {
        this.SetProperty<bool>(ref this.useOAuth, value, nameof (UseOAuth));
        this.applyAuthenticationCommand.RaiseCanExecuteChanged();
      }
    }

    public bool SignOut
    {
      get => this.signOut;
      set => this.SetProperty<bool>(ref this.signOut, value, nameof (SignOut));
    }

    public ICommand ApplyAuthenticationCommand => (ICommand) this.applyAuthenticationCommand;

    public string DebugInfo
    {
      get => this.debugInfo;
      set => this.SetProperty<string>(ref this.debugInfo, value, nameof (DebugInfo));
    }

    public ICommand MailCommand { get; set; }

    public DebugSettingsAccountsPageViewModel(
      IConfig config,
      ISmoothNavService smoothNavService,
      IConnectionInfoProvider connectionInfoProvider,
      INetworkService networkService,
      IOAuthMsaTokenStore oAuthMsaTokenStore,
      IOobeService oobeService,
      IEmailService emailService,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.config = config;
      this.connectionInfoProvider = connectionInfoProvider;
      this.emailService = emailService;
      this.messageSender = messageSender;
      this.oAuthMsaTokenStore = oAuthMsaTokenStore;
      this.oobeService = oobeService;
      this.smoothNavService = smoothNavService;
      this.MailCommand = (ICommand) new HealthCommand(new Action(this.MailCredentials_Click));
      this.applyAuthenticationCommand = new HealthCommand(new Action(this.ApplyAuthentication), new Func<bool>(this.CanApplyAuthentication));
      this.Header = "Accounts";
      this.SubHeader = "SDE, OAuth, Credentials";
      this.GlyphIcon = "\uE173";
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      HealthCloudConnectionInfo connectionInfoAsync = await this.connectionInfoProvider.GetConnectionInfoAsync(CancellationToken.None);
      SimpleWebToken simpleWebToken = new SimpleWebToken(connectionInfoAsync.PodSecurityToken);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(DebugSettingsAccountsPageViewModel.FormatFieldLabelValue("KAccessToken", connectionInfoAsync.PodSecurityToken));
      stringBuilder.Append(DebugSettingsAccountsPageViewModel.FormatFieldLabelValue("KAccessToken expires on", simpleWebToken.ExpiresOn.ToString()));
      stringBuilder.Append(DebugSettingsAccountsPageViewModel.FormatFieldLabelValue("MSA Token", connectionInfoAsync.SecurityToken));
      stringBuilder.Append(DebugSettingsAccountsPageViewModel.FormatFieldLabelValue("UserId", connectionInfoAsync.UserId));
      stringBuilder.Append(DebugSettingsAccountsPageViewModel.FormatFieldLabelValue("Kds Address", connectionInfoAsync.BaseUri.AbsoluteUri));
      stringBuilder.Append(DebugSettingsAccountsPageViewModel.FormatFieldLabelValue("Pod Address", connectionInfoAsync.PodEndpoint.AbsoluteUri));
      this.DebugInfo = stringBuilder.ToString();
      this.useOAuth = this.config.UseOAuth;
      this.signOut = false;
    }

    private void MailCredentials_Click() => this.emailService.ComposeNewEmailAsync(new EmailMessage()
    {
      Subject = "Credentials Debug Info",
      Body = this.DebugInfo
    });

    private static string FormatFieldLabelValue(string fieldLabel, string value) => string.Format("{0}:{2}{1}{2}{2}", new object[3]
    {
      (object) fieldLabel,
      (object) value,
      (object) Environment.NewLine
    });

    private bool CanApplyAuthentication()
    {
      if (this.clearingData)
        return false;
      return this.UseOAuth != this.config.UseOAuth || this.UseOAuth;
    }

    private async void ApplyAuthentication()
    {
      int num1 = this.config.UseOAuth ? 1 : 0;
      this.config.UseOAuth = this.UseOAuth;
      this.clearingData = true;
      this.applyAuthenticationCommand.RaiseCanExecuteChanged();
      if (this.SignOut)
      {
        this.navigationTaskCompletionSource = new TaskCompletionSource<object>();
        EventHandler<NavigationEventArguments> navigatingEvent = (EventHandler<NavigationEventArguments>) null;
        navigatingEvent = (EventHandler<NavigationEventArguments>) ((s, e) =>
        {
          if (e.NavigationType != NavigationType.Backward)
            return;
          this.smoothNavService.Navigating -= navigatingEvent;
          this.navigationTaskCompletionSource.SetResult((object) null);
        });
        this.smoothNavService.Navigating += navigatingEvent;
        this.smoothNavService.Navigate(typeof (OAuthSignOutViewModel));
        object obj = await this.navigationTaskCompletionSource.Task.ConfigureAwait(false);
      }
      await this.oAuthMsaTokenStore.ClearAsync();
      int num2 = await this.oobeService.ResetOobeStatusAsync(false, false, false) ? 1 : 0;
      this.smoothNavService.Navigate(typeof (FreshInstallLoadingViewModel));
      this.clearingData = false;
    }
  }
}
