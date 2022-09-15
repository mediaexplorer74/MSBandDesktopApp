// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.FacebookSendMessageDialogViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class FacebookSendMessageDialogViewModel : PageViewModelBase
  {
    private const uint UrlmonOptionUserAgent = 268435457;
    private const uint UrlmonOptionUserAgentReset = 268435458;
    private const string DesktopUserAgent = "Mozilla / 5.0(Windows NT 6.3; WOW64; Trident / 7.0; rv: 11.0) like Gecko";
    private readonly ISmoothNavService smoothNavService;
    private readonly IShareService shareService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private Uri initialUri;
    private bool isBrowserPageLoaded;
    private HealthCommand<WebViewNavigationStartingEventArgsWrapper> browserNavigationStartingCommand;
    private HealthCommand<WebViewNavigationCompletedEventArgsWrapper> browserNavigationCompletedCommand;

    public event EventHandler CancelBrowser;

    public FacebookSendMessageDialogViewModel(
      ISmoothNavService smoothNavService,
      IShareService shareService,
      IMessageBoxService messageBoxService,
      IMessageSender messageSender,
      INetworkService networkService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.shareService = shareService;
      this.messageBoxService = messageBoxService;
      this.messageSender = messageSender;
    }

    public Uri InitialUri
    {
      get => this.initialUri;
      private set => this.SetProperty<Uri>(ref this.initialUri, value, nameof (InitialUri));
    }

    public bool IsBrowserPageLoaded
    {
      get => this.isBrowserPageLoaded;
      private set => this.SetProperty<bool>(ref this.isBrowserPageLoaded, value, nameof (IsBrowserPageLoaded));
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.ChangeUserAgent("Mozilla / 5.0(Windows NT 6.3; WOW64; Trident / 7.0; rv: 11.0) like Gecko");
      this.InitialUri = FacebookUtilities.GenerateFacebookSendMessageWithLinkDialogUrl(new Uri(parameters["MessageLink"]));
      return (Task) Task.FromResult<object>((object) null);
    }

    public ICommand BrowserNavigationStartingCommand => (ICommand) this.browserNavigationStartingCommand ?? (ICommand) (this.browserNavigationStartingCommand = new HealthCommand<WebViewNavigationStartingEventArgsWrapper>((Action<WebViewNavigationStartingEventArgsWrapper>) (args =>
    {
      if (!FacebookUtilities.IsFacebookSendMessageCompletedUrl(args.Uri))
        return;
      ApplicationTelemetry.LogSocialInviteFriend(SocialInviteType.Message);
      if (this.CancelBrowser != null)
        this.CancelBrowser((object) this, EventArgs.Empty);
      this.GoBack();
    })));

    public ICommand BrowserNavigationCompletedCommand => (ICommand) this.browserNavigationCompletedCommand ?? (ICommand) (this.browserNavigationCompletedCommand = new HealthCommand<WebViewNavigationCompletedEventArgsWrapper>((Action<WebViewNavigationCompletedEventArgsWrapper>) (args => this.IsBrowserPageLoaded = true)));

    private void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      message.CancelAction();
      this.GoBack();
    }

    private void GoBack()
    {
      this.messageSender.Unregister<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.ResetUserAgent();
      this.smoothNavService.GoBack();
    }

    private void ChangeUserAgent(string userAgent) => FacebookSendMessageDialogViewModel.UrlMkSetSessionOption(268435457U, userAgent, (uint) userAgent.Length, 0U);

    private void ResetUserAgent() => FacebookSendMessageDialogViewModel.UrlMkSetSessionOption(268435458U, string.Empty, 0U, 0U);

    [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
    private static extern int UrlMkSetSessionOption(
      uint dwOption,
      string pBuffer,
      uint dwBufferLength,
      uint dwReserved);
  }
}
