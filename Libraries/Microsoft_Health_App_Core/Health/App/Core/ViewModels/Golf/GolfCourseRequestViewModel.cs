// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseRequestViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class GolfCourseRequestViewModel : PageViewModelBase
  {
    private readonly ISmoothNavService smoothNavService;
    private readonly ILauncherService launcherService;
    private HealthCommand backCommand;
    private HealthCommand requestCourseCommand;

    public string SearchTitle => AppResources.GolfCourseRequestPageHeaderLocked;

    public GolfCourseRequestViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      ILauncherService launcherService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.launcherService = launcherService;
    }

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    public ICommand RequestCourseCommand => (ICommand) this.requestCourseCommand ?? (ICommand) (this.requestCourseCommand = new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkId=625095")))));
  }
}
