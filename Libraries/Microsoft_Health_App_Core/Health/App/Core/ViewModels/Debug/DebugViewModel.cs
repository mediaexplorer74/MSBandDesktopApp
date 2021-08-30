// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class DebugViewModel : PageViewModelBase
  {
    private HealthCommand<DebugSettingsPageViewModelBase> gotoDebugSettingCommand;

    protected IServiceLocator ServiceLocator { get; private set; }

    public IList<DebugSettingsPageViewModelBase> DebugSettingsList { get; private set; }

    public ICommand GoToDebugSettingCommand => (ICommand) this.gotoDebugSettingCommand ?? (ICommand) (this.gotoDebugSettingCommand = new HealthCommand<DebugSettingsPageViewModelBase>((Action<DebugSettingsPageViewModelBase>) (debugSettingPageViewModel => this.ServiceLocator.GetInstance<ISmoothNavService>().Navigate(debugSettingPageViewModel.GetType()))));

    public DebugViewModel(IServiceLocator serviceLocator, INetworkService networkService)
      : base(networkService)
    {
      this.ServiceLocator = serviceLocator;
      this.DebugSettingsList = (IList<DebugSettingsPageViewModelBase>) new ObservableCollection<DebugSettingsPageViewModelBase>();
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsSystemPageViewModel>());
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsDevicesPageViewModel>());
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsNetworkPageViewModel>());
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsAccountsPageViewModel>());
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsSyncHistoryPageViewModel>());
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsTimeRegionPageViewModel>());
      this.DebugSettingsList.Add((DebugSettingsPageViewModelBase) serviceLocator.GetInstance<DebugSettingsConfigurationPageViewModel>());
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null) => (Task) Task.FromResult<bool>(true);
  }
}
