// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ShareDestinationViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Sharing;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class ShareDestinationViewModel : PageViewModelBase
  {
    private const string SharingAssetsBase = "ms-appx:///Assets/Images/Sharing/";
    private readonly IRouteBasedExerciseEventProvider<RunEvent> service;
    private readonly ISmoothNavService smoothNavService;
    private readonly IShareService shareService;
    private readonly ILauncherService launcherService;
    private readonly IMessageSender messageSender;
    private HealthCommand exitCommand;
    private HealthCommand shareToFacebookCommand;
    private HealthCommand shareOtherCommand;
    private HealthCommand learnMoreCommand;

    public ShareDestinationViewModel(
      IRouteBasedExerciseEventProvider<RunEvent> service,
      ISmoothNavService smoothNavService,
      IShareService shareService,
      ILauncherService launcherService,
      IMessageSender messageSender,
      INetworkService networkService)
      : base(networkService)
    {
      this.service = service;
      this.smoothNavService = smoothNavService;
      this.shareService = shareService;
      this.launcherService = launcherService;
      this.messageSender = messageSender;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      return (Task) Task.FromResult<object>((object) null);
    }

    public Uri ImageContentPath => new Uri(string.Format("{0}{1}", new object[2]
    {
      (object) "ms-appx:///Assets/Images/Sharing/",
      (object) this.shareService.Request.SharePreviewImageFileName
    }), UriKind.Absolute);

    public ICommand LearnMoreCommand => (ICommand) this.learnMoreCommand ?? (ICommand) (this.learnMoreCommand = new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkId=718055")))));

    public ICommand ShareToFacebookCommand => (ICommand) this.shareToFacebookCommand ?? (ICommand) (this.shareToFacebookCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogShareButtonTap(this.shareService.Request.EventType, ShareType.Enhanced, ShareButtonType.IntermediateFacebook);
      this.messageSender.Unregister<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.smoothNavService.Navigate<FacebookShareDialogViewModel>(action: NavigationStackAction.RemovePrevious);
    })));

    public ICommand ShareOtherCommand => (ICommand) this.shareOtherCommand ?? (ICommand) (this.shareOtherCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogShareButtonTap(this.shareService.Request.EventType, ShareType.Enhanced, ShareButtonType.IntermediateOther);
      this.GoBack();
      this.shareService.ShareViaOsFacility();
    })));

    public ICommand ExitCommand => (ICommand) this.exitCommand ?? (ICommand) (this.exitCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogShareCancellation(this.shareService.Request.EventType, ShareType.Enhanced, this.shareService.Request.ButtonType, new ShareCancellationType?(ShareCancellationType.IntermediateCancel));
      this.GoBack();
    })));

    private void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      ApplicationTelemetry.LogShareCancellation(this.shareService.Request.EventType, ShareType.Enhanced, this.shareService.Request.ButtonType, new ShareCancellationType?(ShareCancellationType.IntermediateBack));
      message.CancelAction();
      this.GoBack();
    }

    private void GoBack()
    {
      this.messageSender.Unregister<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.smoothNavService.GoBack();
    }
  }
}
