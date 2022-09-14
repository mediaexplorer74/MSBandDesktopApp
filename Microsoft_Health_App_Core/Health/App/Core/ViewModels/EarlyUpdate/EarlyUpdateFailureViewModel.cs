// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.EarlyUpdate.EarlyUpdateFailureViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.EarlyUpdate
{
  public class EarlyUpdateFailureViewModel
  {
    private ISmoothNavService navService;
    private HealthCommand doneCommand;

    public EarlyUpdateFailureViewModel(ISmoothNavService navService) => this.navService = navService;

    public ICommand DoneCommand => (ICommand) this.doneCommand ?? (ICommand) (this.doneCommand = new HealthCommand(new Action(this.GoBack)));

    private void GoBack() => this.navService.GoBack();
  }
}
