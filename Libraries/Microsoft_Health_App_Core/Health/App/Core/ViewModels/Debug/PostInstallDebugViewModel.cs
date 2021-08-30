// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.PostInstallDebugViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class PostInstallDebugViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Debug\\PostInstallDebugViewModel.cs");
    private IConfig config;
    private ISmoothNavService smoothNavService;
    private IPagePicker pagePicker;
    private HealthCommand startCommand;
    private HealthCommand openDebugPageCommand;

    public PostInstallDebugViewModel(
      INetworkService networkService,
      IConfig config,
      ISmoothNavService smoothNavService,
      IPagePicker pagePicker)
      : base(networkService)
    {
      this.config = config;
      this.smoothNavService = smoothNavService;
      this.pagePicker = pagePicker;
    }

    public IList<string> Environments => CloudEnvironment.EnvironmentNames;

    public string SelectedEnvironment
    {
      get => this.config.Environment;
      set => this.config.Environment = value;
    }

    public bool UseOAuth
    {
      get => this.config.UseOAuth;
      set => this.config.UseOAuth = value;
    }

    public ICommand OpenDebugPageCommand => (ICommand) this.openDebugPageCommand ?? (ICommand) (this.openDebugPageCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(this.pagePicker.Debug))));

    public ICommand StartCommand => (ICommand) this.startCommand ?? (ICommand) (this.startCommand = new HealthCommand((Action) (() =>
    {
      PostInstallDebugViewModel.Logger.Debug("Starting with environment {0} and UseOAuth {1}", (object) this.SelectedEnvironment, (object) this.UseOAuth);
      this.smoothNavService.Navigate(typeof (FreshInstallLoadingViewModel));
      this.smoothNavService.ClearBackStack();
    })));

    protected override void OnBackNavigation()
    {
      this.RaisePropertyChanged("SelectedEnvironment");
      this.RaisePropertyChanged("UseOAuth");
      base.OnBackNavigation();
    }
  }
}
