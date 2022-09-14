// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.NavShellViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services;
using System;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class NavShellViewModel : HealthViewModelBase
  {
    private readonly IWhatsNewService whatsNewService;
    private readonly MenuViewModel mainMenu = new MenuViewModel();

    public MenuViewModel MainMenu => this.mainMenu;

    public NavShellViewModel(IWhatsNewService whatsNewService)
    {
      this.whatsNewService = whatsNewService;
      this.MainMenu.ShowWhatsNewIndicator = !this.whatsNewService.HasBeenViewed;
      this.whatsNewService.WasViewedChanged += new EventHandler(this.OnWasViewedChanged);
    }

    private void OnWasViewedChanged(object sender, EventArgs eventArgs) => this.MainMenu.ShowWhatsNewIndicator = !this.whatsNewService.HasBeenViewed;
  }
}
