// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SendFeedback.HelpAndFeedbackViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.SendFeedback
{
  [PageMetadata(PageContainerType.FullScreen)]
  [PageTaxonomy(new string[] {"App", "Feedback", "Selection"})]
  public class HelpAndFeedbackViewModel : PageViewModelBase
  {
    private readonly ISmoothNavService navigationService;
    private readonly ILauncherService launcherService;
    private readonly IEnvironmentService environmentService;
    private readonly IPagePicker pagePicker;
    private HealthCommand backCommand;
    private HealthCommand reportProblemCommand;
    private HealthCommand helpCommand;
    private HealthCommand suggestCommand;
    private HealthCommand rateUsCommand;

    public HelpAndFeedbackViewModel(
      INetworkService networkService,
      ISmoothNavService navigationService,
      ILauncherService launcherService,
      IEnvironmentService environmentService,
      IPagePicker pagePicker)
      : base(networkService)
    {
      this.launcherService = launcherService;
      this.navigationService = navigationService;
      this.environmentService = environmentService;
      this.pagePicker = pagePicker;
    }

    public bool IsPublicRelease => this.environmentService.IsPublicRelease;

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.navigationService.GoBack())));

    public ICommand ReportProblemCommand => (ICommand) this.reportProblemCommand ?? (ICommand) (this.reportProblemCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogFeedbackSelection("Bugreport");
      this.navigationService.Navigate(typeof (ReportProblemViewModel));
    })));

    public ICommand HelpCommand => (ICommand) this.helpCommand ?? (ICommand) (this.helpCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogFeedbackSelection("Help");
      this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkID=506763"));
    })));

    public ICommand SuggestCommand => (ICommand) this.suggestCommand ?? (ICommand) (this.suggestCommand = new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogFeedbackSelection("UserVoice");
      this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkID=690323"));
    })));

    public ICommand RateUsCommand => (ICommand) this.rateUsCommand ?? (ICommand) (this.rateUsCommand = new HealthCommand((Action) (() => this.navigationService.Navigate(typeof (NetPromoterScoreViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "referrerSource",
        "unprompt"
      }
    }))));
  }
}
