// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Oobe.OobeLegalTermsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Threading;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Oobe
{
  [PageTaxonomy(new string[] {"App", "OOBE", "Legal TOS"})]
  public class OobeLegalTermsViewModel : OobePageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Oobe\\OobeLegalTermsViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IOobeService oobeService;
    private readonly ILauncherService launcherService;
    private ActionViewModel agreeAction;
    private ActionViewModel cancelAction;
    private ISmoothNavService navService;
    private IUserService userService;
    private ICommand navigateToTermsOfUseCommand;
    private ICommand navigateToPrivacyPolicyCommand;
    private bool agreePressed;

    public OobeLegalTermsViewModel(
      INetworkService networkService,
      IErrorHandlingService errorHandlingService,
      IOobeService oobeService,
      ISmoothNavService navService,
      ILauncherService launcherService,
      IUserService userService)
      : base(networkService)
    {
      this.oobeService = oobeService;
      this.errorHandlingService = errorHandlingService;
      this.navService = navService;
      this.launcherService = launcherService;
      this.userService = userService;
    }

    public override ActionViewModel PositiveAction => this.agreeAction ?? (this.agreeAction = new ActionViewModel(AppResources.OobeLegalTermsIAgreeButton, (ICommand) new HealthCommand((Action) (async () =>
    {
      HealthCommand command = this.agreeAction.Command as HealthCommand;
      this.agreePressed = true;
      command.RaiseCanExecuteChanged();
      try
      {
        await this.oobeService.CompleteStepAsync(OobeStep.LegalTerms, CancellationToken.None);
      }
      catch (Exception ex)
      {
        OobeLegalTermsViewModel.Logger.Error(ex, "Failed to complete OOBE Legal Terms Page.");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.agreePressed = false;
        command.RaiseCanExecuteChanged();
      }
    }), (Func<bool>) (() => !this.agreePressed))));

    public override ActionViewModel NegativeAction => this.cancelAction ?? (this.cancelAction = new ActionViewModel(AppResources.OobeLegalTermsCancelButton, (ICommand) new HealthCommand((Action) (async () => await this.userService.SignOutAsync()))));

    public ICommand NavigateToTermsOfUseCommand => this.navigateToTermsOfUseCommand ?? (this.navigateToTermsOfUseCommand = (ICommand) new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser("https://go.microsoft.com/fwlink/?LinkId=507589"))));

    public ICommand NavigateToPrivacyPolicyCommand => this.navigateToPrivacyPolicyCommand ?? (this.navigateToPrivacyPolicyCommand = (ICommand) new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser("https://go.microsoft.com/fwlink/?LinkId=507593"))));

    public override string PageTitle { get; } = AppResources.OobeLegalTermsPageHeader;

    public override string PageSubtitle { get; } = AppResources.OobeLegalTermsText1;
  }
}
