// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsSystemPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Configurations;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities.Logging;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class DebugSettingsSystemPageViewModel : DebugSettingsPageViewModelBase
  {
    private readonly IEnvironmentService environmentService;
    private readonly IDebuggableHttpCacheService cacheService;
    private readonly IConfig config;
    private readonly IConfigProvider configProvider;
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageSender messageSender;
    private int guidedWorkoutsProgressedDays;
    private HealthCommand viewCacheItemsCommand;
    private HealthCommand clearCacheCommand;

    public int GuidedWorkoutsProgressedDays
    {
      get => this.guidedWorkoutsProgressedDays;
      set => this.SetProperty<int>(ref this.guidedWorkoutsProgressedDays, value, nameof (GuidedWorkoutsProgressedDays));
    }

    public bool IsBackgroundLoggingEnabled
    {
      get => this.config.IsBackgroundLoggingEnabled;
      set => this.config.IsBackgroundLoggingEnabled = value;
    }

    public bool IsCachingEnabled
    {
      get => this.config.IsCachingEnabled;
      set => this.config.IsCachingEnabled = value;
    }

    public bool IsMockInsightsEnabled
    {
      get => this.config.IsMockInsightsEnabled;
      set => this.config.IsMockInsightsEnabled = value;
    }

    public bool SuspendApplicationWhenIdle
    {
      get => this.environmentService.SuspendApplicationWhenIdle;
      set => this.environmentService.SuspendApplicationWhenIdle = value;
    }

    public ICommand RestartLoggerCommand { get; private set; }

    public ICommand ApplyGuidedWorkoutsCommand { get; private set; }

    public ICommand ViewCacheItemsCommand => (ICommand) this.viewCacheItemsCommand ?? (ICommand) (this.viewCacheItemsCommand = new HealthCommand(new Action(this.NavigateToCacheItemsPage)));

    public ICommand CrashApplicationCommand { get; private set; }

    public ICommand ClearCacheCommand => (ICommand) this.clearCacheCommand ?? (ICommand) (this.clearCacheCommand = new HealthCommand((Action) (async () =>
    {
      await this.cacheService.RemoveAllAsync();
      this.messageSender.Send<InvalidateAllViewModelsOfTypeInBackCacheMessage>(new InvalidateAllViewModelsOfTypeInBackCacheMessage(typeof (TilesViewModel)));
    })));

    public DebugSettingsSystemPageViewModel(
      IConfig config,
      ISmoothNavService smoothNavService,
      IDebuggableHttpCacheService cacheService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      IEnvironmentService environmentService,
      IConfigProvider configProvider,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.config = config;
      this.smoothNavService = smoothNavService;
      this.cacheService = cacheService;
      this.messageBoxService = messageBoxService;
      this.environmentService = environmentService;
      this.configProvider = configProvider;
      this.messageSender = messageSender;
      this.Header = "System";
      this.SubHeader = "Caching, logging, force crash";
      this.GlyphIcon = "\uE143";
      this.ApplyGuidedWorkoutsCommand = (ICommand) new HealthCommand(new Action(this.ApplyGuidedWorkoutsValues));
      this.CrashApplicationCommand = (ICommand) new HealthCommand(new Action(this.CrashApplication));
      this.RestartLoggerCommand = (ICommand) new HealthCommand((Action) (() =>
      {
        LogManager.Shutdown();
        HealthApplicationLogUtilities.StartLoggingAndInjectListeners(ServiceLocator.Current.GetInstance<ILoggerConfiguration>());
      }));
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.GuidedWorkoutsProgressedDays = this.config.GWTimeProgressedDays;
      return (Task) Task.FromResult<bool>(true);
    }

    private void NavigateToCacheItemsPage() => this.smoothNavService.Navigate(typeof (CacheItemsViewModel));

    private async void ApplyGuidedWorkoutsValues()
    {
      this.config.GWTimeProgressedDays = this.GuidedWorkoutsProgressedDays;
      int num = (int) await this.messageBoxService.ShowAsync(string.Format("Progressed {0} day(s) has been set!", new object[1]
      {
        (object) this.GuidedWorkoutsProgressedDays
      }), (string) null, PortableMessageBoxButton.OK);
    }

    private void CrashApplication() => throw new Exception("Forced exception to force the application to crash from the debug panel.");
  }
}
