// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.EarlyUpdate.EarlyUpdateEulaViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.EarlyUpdate
{
  public class EarlyUpdateEulaViewModel : HealthViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\EarlyUpdate\\EarlyUpdateEulaViewModel.cs");
    private ISmoothNavService navService;
    private IPhoneOsUpdateService devProgram;
    private IMessageBoxService messageBoxService;
    private HealthCommand acceptCommand;
    private HealthCommand rejectCommand;

    public EarlyUpdateEulaViewModel(
      ISmoothNavService navService,
      IPhoneOsUpdateService devProgram,
      IMessageBoxService messageBoxService)
    {
      this.navService = navService;
      this.devProgram = devProgram;
      this.messageBoxService = messageBoxService;
    }

    public ICommand AcceptCommand => (ICommand) this.acceptCommand ?? (ICommand) (this.acceptCommand = new HealthCommand(new Action(this.EnableDevProgram)));

    private async void EnableDevProgram()
    {
      if (this.devProgram.EnableDevProgram())
      {
        EarlyUpdateEulaViewModel.Logger.Debug((object) "Enabled dev program.");
        ApplicationTelemetry.LogEarlyUpdateProvision();
        this.navService.Navigate(typeof (EarlyUpdateSuccessViewModel), action: NavigationStackAction.RemovePrevious);
      }
      else
      {
        EarlyUpdateEulaViewModel.Logger.Warn((object) "Could not enable dev program.");
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.EarlyUpdateError, (string) null, PortableMessageBoxButton.OK);
      }
    }

    public ICommand RejectCommand => (ICommand) this.rejectCommand ?? (ICommand) (this.rejectCommand = new HealthCommand(new Action(this.Reject)));

    private void Reject() => this.navService.Navigate(typeof (EarlyUpdateFailureViewModel), action: NavigationStackAction.RemovePrevious);
  }
}
