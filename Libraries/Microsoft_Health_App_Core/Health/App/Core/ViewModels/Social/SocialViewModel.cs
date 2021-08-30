// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Social.SocialViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models.Social;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Social
{
  [PageTaxonomy(new string[] {"Social"})]
  public class SocialViewModel : PanelViewModelBase
  {
    private const string FallbackUrlSuffix = "#/friend";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Social\\SocialViewModel.cs");
    private readonly IUserProfileService userProfileService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageSender messageSender;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly ISocialEngagementService socialEngagementService;
    private readonly IFacebookService facebookService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IWebViewInteractionService webViewInteractionService;
    private readonly ITimer webViewLoadTimeoutTimer;
    private ICommand loginCommand;
    private ICommand removeTileCommand;
    private FacebookCredentials facebookCredentials;
    private bool isFirstTimeUse;
    private SocialTileViewModel socialTileViewModel;
    private string socialWebViewUrlSuffix;
    private Uri initialUri;
    private bool isWebViewVisible;
    private bool showErrorPage;
    private bool showProgressBar = true;
    private bool isSocialReady;
    private ICommand browserContentLoadingCommand;
    private ICommand browserNavigationFailedCommand;
    private ICommand browserScriptNotifyCommand;
    private HealthCommand socialTileTappedCommand;
    private string[] webViewInitializationArgs;

    public SocialViewModel(
      INetworkService networkService,
      IUserProfileService userProfileService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IHealthCloudClient healthCloudClient,
      ISocialEngagementService socialEngagementService,
      IFacebookService facebookService,
      IMessageBoxService messageBoxService,
      IWebViewInteractionService webViewInteractionService,
      ITimerService timerService)
      : base(networkService)
    {
      this.userProfileService = userProfileService;
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.healthCloudClient = healthCloudClient;
      this.socialEngagementService = socialEngagementService;
      this.facebookService = facebookService;
      this.messageBoxService = messageBoxService;
      this.webViewInteractionService = webViewInteractionService;
      this.webViewLoadTimeoutTimer = timerService.CreateTimer();
      this.webViewLoadTimeoutTimer.Interval = TimeSpan.FromSeconds(60.0);
      this.webViewLoadTimeoutTimer.Tick += new EventHandler<object>(this.WebViewLoadTimeoutTimer_Tick);
    }

    public override bool UseLoadPanel => false;

    public ICommand LoginCommand => this.loginCommand ?? (this.loginCommand = (ICommand) new HealthCommand((Action) (async () =>
    {
      FacebookCredentials credentials = this.facebookCredentials;
      if (credentials == null)
        await this.LoginAsync();
      else
        await this.SignupAsync(credentials);
    })));

    public ICommand RemoveTileCommand => this.removeTileCommand ?? (this.removeTileCommand = (ICommand) new HealthCommand((Action) (async () =>
    {
      if (await this.messageBoxService.ShowAsync(AppResources.SocialRemoveTileConfirmation, AppResources.SocialLabelConfirm, PortableMessageBoxButton.CasualBooleanChoice) != PortableMessageBoxResult.OK)
        return;
      await this.socialEngagementService.ToggleSocialAsync(SocialRemoveType.Tile);
      this.smoothNavService.GoBack();
    })));

    public bool IsFirstTimeUse
    {
      get => this.isFirstTimeUse;
      private set => this.SetProperty<bool>(ref this.isFirstTimeUse, value, nameof (IsFirstTimeUse));
    }

    public SocialTileViewModel SocialTileViewModel
    {
      get => this.socialTileViewModel;
      set => this.SetProperty<SocialTileViewModel>(ref this.socialTileViewModel, value, nameof (SocialTileViewModel));
    }

    public bool IsSocialReady
    {
      get => this.isSocialReady;
      set => this.SetProperty<bool>(ref this.isSocialReady, value, nameof (IsSocialReady));
    }

    public Uri InitialUri
    {
      get => this.initialUri;
      private set => this.SetProperty<Uri>(ref this.initialUri, value, nameof (InitialUri));
    }

    public string TileIcon => this.SocialTileViewModel?.TileIcon;

    public string Subheader => this.SocialTileViewModel.Subheader;

    public TileColorLevel ColorLevel
    {
      get
      {
        SocialTileViewModel socialTileViewModel = this.SocialTileViewModel;
        return socialTileViewModel == null ? TileColorLevel.Medium : socialTileViewModel.ColorLevel;
      }
    }

    public StyledSpan Header => this.SocialTileViewModel?.Header;

    public bool HasMetric => this.SocialTileViewModel?.Header != null;

    public bool IsWebViewVisible
    {
      get => this.isWebViewVisible;
      set => this.SetProperty<bool>(ref this.isWebViewVisible, value, nameof (IsWebViewVisible));
    }

    public bool ShowErrorPage
    {
      get => this.showErrorPage;
      set => this.SetProperty<bool>(ref this.showErrorPage, value, nameof (ShowErrorPage));
    }

    public bool ShowProgressBar
    {
      get => this.showProgressBar;
      set => this.SetProperty<bool>(ref this.showProgressBar, value, nameof (ShowProgressBar));
    }

    public ISocialWebViewTarget SocialWebViewTarget { get; set; }

    public ICommand BrowserContentLoadingCommand => this.browserContentLoadingCommand ?? (this.browserContentLoadingCommand = (ICommand) new HealthCommand<WebViewContentLoadingEventArgsWrapper>((Action<WebViewContentLoadingEventArgsWrapper>) (args => this.NotifyContentLoading())));

    public ICommand BrowserNavigationFailedCommand => this.browserNavigationFailedCommand ?? (this.browserNavigationFailedCommand = (ICommand) new HealthCommand<WebViewNavigationFailedEventArgsWrapper>((Action<WebViewNavigationFailedEventArgsWrapper>) (args => this.NotifyBrowserNavigationFailure(args.Uri, args.WebErrorStatus))));

    public ICommand BrowserScriptNotifyCommand => this.browserScriptNotifyCommand ?? (this.browserScriptNotifyCommand = (ICommand) new HealthCommand<NotifyEventArgsWrapper>((Action<NotifyEventArgsWrapper>) (args => this.NotifyBrowserScript(args.Value))));

    public ICommand SocialTileTappedCommand => (ICommand) this.socialTileTappedCommand ?? (ICommand) (this.socialTileTappedCommand = new HealthCommand((Action) (() =>
    {
      this.webViewLoadTimeoutTimer.Stop();
      this.GoBackOrHome();
    })));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.UpdateFakeTile();
      if (this.IsSocialReady)
        return;
      SocialTileStatusResponse statusResponse = (SocialTileStatusResponse) null;
      this.socialWebViewUrlSuffix = (string) null;
      await base.LoadDataAsync(parameters);
      FacebookCredentials credentials = await this.facebookService.GetCachedFacebookCredentialsAsync();
      if (credentials != null)
      {
        FacebookPermissions permissionsAsync = await this.facebookService.GetUserPermissionsAsync(credentials.Token);
        if (permissionsAsync == null || !permissionsAsync.CanViewFriends)
        {
          await this.facebookService.DeleteCachedFacebookCredentialsAsync();
          credentials = (FacebookCredentials) null;
        }
      }
      try
      {
        statusResponse = await this.socialEngagementService.GetSocialTileDisplayAsync(credentials, CancellationToken.None, true);
      }
      catch (Exception ex)
      {
        ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, ex.ToString());
        throw;
      }
      finally
      {
        this.SocialTileViewModel.SetTileContent(statusResponse);
        this.UpdateFakeTile();
      }
      switch (statusResponse.Status)
      {
        case SocialTileStatus.Activate:
          this.IsFirstTimeUse = true;
          this.ShowProgressBar = false;
          this.facebookCredentials = credentials;
          break;
        default:
          this.IsFirstTimeUse = false;
          this.ShowProgressBar = true;
          this.socialWebViewUrlSuffix = statusResponse.RelativeUrl;
          this.NavigateToSocialWebView();
          break;
      }
      statusResponse = (SocialTileStatusResponse) null;
      credentials = (FacebookCredentials) null;
    }

    private void UpdateFakeTile()
    {
      this.RaisePropertyChanged("TileIcon");
      this.RaisePropertyChanged("Subheader");
      this.RaisePropertyChanged("ColorLevel");
      this.RaisePropertyChanged("Header");
      this.RaisePropertyChanged("HasMetric");
    }

    private async Task LoginAsync()
    {
      int num = 0;
      object obj;
      try
      {
        FacebookAccessToken token = await this.facebookService.LoginAsync();
        if (token == null)
          return;
        FacebookPermissions permissionsAsync = await this.facebookService.GetUserPermissionsAsync(token);
        if (permissionsAsync == null || !permissionsAsync.CanViewFriends)
        {
          int num1 = (int) await this.messageBoxService.ShowAsync(AppResources.FacebookFriendsListPermissionError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
          return;
        }
        this.IsFirstTimeUse = false;
        this.ShowProgressBar = true;
        this.LoadState = LoadState.Loading;
        DateTime? expires = token.Expires;
        DateTime dateTime = DateTime.UtcNow.AddDays(5.0);
        if ((expires.HasValue ? (expires.GetValueOrDefault() < dateTime ? 1 : 0) : 0) != 0)
          token = await this.facebookService.GetLongLivedAccessTokenAsync(token);
        FacebookUserProfileFragment userProfileAsync = await this.facebookService.GetUserProfileAsync(token);
        FacebookCredentials credentials = new FacebookCredentials()
        {
          Token = token,
          Profile = userProfileAsync
        };
        await this.facebookService.SetCachedFacebookCredentialsAsync(credentials);
        if (this.socialTileViewModel.IsOpen)
          await this.SignupAsync(credentials);
        token = (FacebookAccessToken) null;
        credentials = (FacebookCredentials) null;
      }
      catch (Exception ex)
      {
        obj = (object) ex;
        num = 1;
      }
      if (num == 1)
      {
        Exception exception = (Exception) obj;
        this.IsFirstTimeUse = true;
        this.ShowProgressBar = false;
        this.LoadState = LoadState.Loaded;
        ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, exception.ToString());
        SocialViewModel.Logger.Error((object) exception);
        await this.messageBoxService.ShowUnexpectedErrorAsync(AppResources.GenericErrorMessage);
      }
      else
        obj = (object) null;
    }

    private async Task SignupAsync(FacebookCredentials credentials)
    {
      this.LoadState = LoadState.Loading;
      int num = 0;
      object obj;
      try
      {
        await this.socialEngagementService.SignUpForSocialEngagementAsync(credentials, CancellationToken.None);
        SocialTileStatusResponse tileDisplayAsync = await this.socialEngagementService.GetSocialTileDisplayAsync(credentials, CancellationToken.None, true);
        this.socialTileViewModel.SetTileContent(tileDisplayAsync);
        if (tileDisplayAsync.Status != SocialTileStatus.Activate)
        {
          ApplicationTelemetry.LogSocialFacebookRegistration(true);
          this.IsFirstTimeUse = false;
          this.ShowProgressBar = true;
          this.socialWebViewUrlSuffix = tileDisplayAsync.RelativeUrl;
          this.NavigateToSocialWebView();
        }
        else
        {
          this.LoadState = LoadState.Error;
          ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, "SocialTileStatus was Activate after calling bind.");
          return;
        }
      }
      catch (Exception ex)
      {
        obj = (object) ex;
        num = 1;
      }
      if (num == 1)
      {
        Exception exception = (Exception) obj;
        this.IsFirstTimeUse = true;
        this.ShowProgressBar = false;
        this.LoadState = LoadState.Loaded;
        ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, exception.ToString());
        SocialViewModel.Logger.Error((object) exception);
        if (exception is ConflictException)
        {
          int num1 = (int) await this.messageBoxService.ShowAsync(AppResources.FacebookDuplicateBindingError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
        }
        else
        {
          this.socialTileViewModel.SetTileContent((SocialTileStatusResponse) null);
          await this.messageBoxService.ShowUnexpectedErrorAsync(AppResources.GenericErrorMessage);
        }
      }
      else
      {
        obj = (object) null;
        this.LoadState = LoadState.Loaded;
      }
    }

    private async void NavigateToSocialWebView()
    {
      if (!this.socialTileViewModel.IsOpen)
        return;
      await this.LoadWebViewAsync();
    }

    protected async Task LoadWebViewAsync()
    {
      try
      {
        SocialViewModel socialViewModel = this;
        string[] initializationArgs = socialViewModel.webViewInitializationArgs;
        string[] initializationArgsAsync = await this.webViewInteractionService.GetSocialWebViewInitializationArgsAsync();
        socialViewModel.webViewInitializationArgs = initializationArgsAsync;
        socialViewModel = (SocialViewModel) null;
        this.webViewLoadTimeoutTimer.Start();
        this.socialEngagementService.IsSocialTileUpdatePending = true;
        this.InitialUri = await this.socialEngagementService.GetSocialSiteUrlAsync(this.socialWebViewUrlSuffix, CancellationToken.None);
      }
      catch (Exception ex)
      {
        ApplicationTelemetry.LogSocialFailure(SocialFailureType.App, ex.ToString());
        this.ShowFailureUI();
      }
    }

    public async void NotifyContentLoading()
    {
      string str = await this.InvokeEvalScriptAsync(this.webViewInitializationArgs);
    }

    public void NotifyBrowserNavigationFailure(Uri uri, int webErrorStatus)
    {
      this.webViewLoadTimeoutTimer.Stop();
      string failureDetail;
      if (!(uri == (Uri) null))
        failureDetail = string.Format("Failed to load page {0}. Error code {1}.", new object[2]
        {
          (object) uri,
          (object) webErrorStatus
        });
      else
        failureDetail = "Failed to load page.";
      ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, failureDetail);
      this.ShowFailureUI();
    }

    public void NotifyBrowserScript(string command) => this.HandleWebViewScriptNotify(command);

    private Task<string> InvokeEvalScriptAsync(params string[] arguments)
    {
      ISocialWebViewTarget socialWebViewTarget = this.SocialWebViewTarget;
      return socialWebViewTarget != null ? socialWebViewTarget.InvokeEvalScriptAsync(arguments) : Task.FromResult<string>(string.Empty);
    }

    private async void HandleWebViewScriptNotify(string action)
    {
      bool isEnabled = this.webViewLoadTimeoutTimer.IsEnabled;
      this.webViewLoadTimeoutTimer.Stop();
      string str = action;
      if (!(str == "SocialReady"))
      {
        if (!(str == "ExitSocial"))
        {
          if (!(str == "InviteFriend"))
          {
            if (str == "Logout")
            {
              ApplicationTelemetry.LogSocialFacebookRegistration(false);
              await this.facebookService.DeleteCachedFacebookCredentialsAsync();
              this.GoBackOrHome();
            }
            else if (action.Contains("Popup="))
              this.socialEngagementService.ShowFacebookCommentPopup(action);
            else
              ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, string.Format("Unknown WebView action {0}.", new object[1]
              {
                (object) action
              }));
          }
          else
            await this.facebookService.InviteFriendsAsync();
        }
        else
          this.GoBackOrHome();
      }
      else
      {
        this.IsWebViewVisible = true;
        if (!isEnabled)
          return;
        this.IsSocialReady = true;
        this.ShowErrorPage = false;
        this.ShowProgressBar = false;
      }
    }

    private async void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      this.webViewLoadTimeoutTimer.Stop();
      message.CancelAction();
      if (this.IsSocialReady)
      {
        string str = await this.InvokeEvalScriptAsync(this.webViewInteractionService.SocialBackButtonJavaScriptCode);
      }
      else
        this.GoBackOrHome();
    }

    private void GoBackOrHome()
    {
      this.ResetUI();
      this.smoothNavService.GoBack();
    }

    private void ResetUI()
    {
      this.IsSocialReady = false;
      this.IsWebViewVisible = false;
      this.ShowErrorPage = false;
      this.ShowProgressBar = true;
      this.InitialUri = (Uri) null;
    }

    private void WebViewLoadTimeoutTimer_Tick(object sender, object e)
    {
      this.webViewLoadTimeoutTimer.Stop();
      if (this.IsSocialReady)
        return;
      this.ShowFailureUI();
      ApplicationTelemetry.LogSocialFailure(SocialFailureType.Web, "Social site timeout");
    }

    private void ShowFailureUI()
    {
      this.ShowErrorPage = true;
      this.ShowProgressBar = false;
      this.IsWebViewVisible = false;
    }
  }
}
