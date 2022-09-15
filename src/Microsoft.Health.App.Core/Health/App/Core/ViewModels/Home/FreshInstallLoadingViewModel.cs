// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.FreshInstallLoadingViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class FreshInstallLoadingViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\FreshInstallLoadingViewModel.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageSender messageSender;
    private readonly IFreshInstallService freshInstallService;
    private string loadingText;

    public FreshInstallLoadingViewModel(
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      IErrorHandlingService errorHandlingService,
      IMessageSender messageSender,
      IFreshInstallService freshInstallService)
      : base(networkService)
    {
      this.messageBoxService = messageBoxService;
      this.errorHandlingService = errorHandlingService;
      this.messageSender = messageSender;
      this.freshInstallService = freshInstallService;
    }

    public string LoadingText
    {
      get => this.loadingText;
      set => this.SetProperty<string>(ref this.loadingText, value, nameof (LoadingText));
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.StartAuthorize();
      return (Task) Task.FromResult<object>((object) null);
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, (Action<BackButtonPressedMessage>) (message => message.CancelAction()));
    }

    private async void StartAuthorize() => await this.AuthorizeAsync();

    private async Task AuthorizeAsync()
    {
      bool setupComplete = false;
      while (!setupComplete)
      {
        try
        {
          await this.freshInstallService.SetupAsync((IProgress<InitializationProgress>) new SimpleProgress<InitializationProgress>((Action<InitializationProgress>) (p => this.Report(p))), CancellationToken.None);
          setupComplete = true;
        }
        catch (Exception ex)
        {
          FreshInstallLoadingViewModel.Logger.Error((object) "Error setting up for fresh install.", ex);
          if (ex is InternetException)
          {
            int num = (int) await this.messageBoxService.ShowAsync(AppResources.NeedToCheckProfileMessage, AppResources.NeedToCheckProfileHeader, PortableMessageBoxButton.OK);
          }
          else
            await this.errorHandlingService.HandleExceptionAsync(ex);
        }
      }
    }

    private void Report(InitializationProgress value)
    {
      switch (value)
      {
        case InitializationProgress.LoggingIn:
          this.LoadingText = AppResources.LoggingInLoadingText;
          break;
        case InitializationProgress.GettingReady:
          this.LoadingText = AppResources.GettingReadyLoadingText;
          break;
        case InitializationProgress.AlmostThere:
          this.LoadingText = AppResources.EarlyUpdateSuccessTitle;
          break;
        default:
          this.LoadingText = string.Empty;
          break;
      }
    }
  }
}
