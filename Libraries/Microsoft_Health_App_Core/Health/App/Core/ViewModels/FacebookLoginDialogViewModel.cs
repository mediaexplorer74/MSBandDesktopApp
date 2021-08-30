// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.FacebookLoginDialogViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class FacebookLoginDialogViewModel : PageViewModelBase
  {
    private readonly ISmoothNavService smoothNavService;
    private readonly IShareService shareService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly IFacebookService facebookService;
    private Uri initialUri;
    private bool isBrowserPageLoaded;
    private HealthCommand<WebViewNavigationStartingEventArgsWrapper> browserNavigationStartingCommand;
    private HealthCommand<WebViewNavigationCompletedEventArgsWrapper> browserNavigationCompletedCommand;

    public event EventHandler CancelBrowser;

    public FacebookLoginDialogViewModel(
      ISmoothNavService smoothNavService,
      IShareService shareService,
      IMessageBoxService messageBoxService,
      IMessageSender messageSender,
      INetworkService networkService,
      IFacebookService facebookService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.shareService = shareService;
      this.messageBoxService = messageBoxService;
      this.messageSender = messageSender;
      this.facebookService = facebookService;
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
      this.InitialUri = FacebookUtilities.GenerateFacebookLoginDialogUrl(true);
      return (Task) Task.FromResult<object>((object) null);
    }

    public ICommand BrowserNavigationStartingCommand => (ICommand) this.browserNavigationStartingCommand ?? (ICommand) (this.browserNavigationStartingCommand = new HealthCommand<WebViewNavigationStartingEventArgsWrapper>((Action<WebViewNavigationStartingEventArgsWrapper>) (async args =>
    {
      if (!FacebookUtilities.IsFacebookLoginCompletedUrl(args.Uri))
        return;
      if (this.CancelBrowser != null)
        this.CancelBrowser((object) this, EventArgs.Empty);
      await this.GoBackAsync(await FacebookUtilities.ParseFacebookLoginFinishedUriAsync(args.Uri));
    })));

    public ICommand BrowserNavigationCompletedCommand => (ICommand) this.browserNavigationCompletedCommand ?? (ICommand) (this.browserNavigationCompletedCommand = new HealthCommand<WebViewNavigationCompletedEventArgsWrapper>((Action<WebViewNavigationCompletedEventArgsWrapper>) (args => this.IsBrowserPageLoaded = true)));

    private async void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      message.CancelAction();
      await this.GoBackAsync((FacebookAccessToken) null);
    }

    private async Task GoBackAsync(FacebookAccessToken token)
    {
      this.messageSender.Unregister<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      await this.facebookService.SetLoginResultAsync(token);
      this.smoothNavService.GoBack();
    }
  }
}
