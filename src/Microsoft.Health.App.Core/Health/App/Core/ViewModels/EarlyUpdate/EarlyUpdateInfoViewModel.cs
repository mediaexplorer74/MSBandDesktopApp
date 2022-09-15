// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.EarlyUpdate.EarlyUpdateInfoViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.EarlyUpdate
{
  public class EarlyUpdateInfoViewModel : HealthViewModelBase
  {
    private ISmoothNavService navService;
    private HealthCommand nextCommand;

    public EarlyUpdateInfoViewModel(ISmoothNavService navService) => this.navService = navService;

    public ICommand NextCommand => (ICommand) this.nextCommand ?? (ICommand) (this.nextCommand = new HealthCommand(new Action(this.Next)));

    private void Next() => this.navService.Navigate(typeof (EarlyUpdateEulaViewModel), action: NavigationStackAction.RemovePrevious);
  }
}
