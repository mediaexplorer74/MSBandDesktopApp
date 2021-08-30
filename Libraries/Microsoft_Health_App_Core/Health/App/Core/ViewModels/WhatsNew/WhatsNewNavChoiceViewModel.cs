// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WhatsNew.WhatsNewNavChoiceViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;

namespace Microsoft.Health.App.Core.ViewModels.WhatsNew
{
  public class WhatsNewNavChoiceViewModel : NavChoiceViewModel
  {
    private readonly IWhatsNewService whatsNewService;
    private bool showIndicator;

    public bool ShowIndicator
    {
      get => this.showIndicator;
      private set => this.SetProperty<bool>(ref this.showIndicator, value, nameof (ShowIndicator));
    }

    public WhatsNewNavChoiceViewModel(
      IMessageBoxService messageBoxService,
      ILauncherService launcherService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IWhatsNewService whatsNewService)
      : base(messageBoxService, launcherService, smoothNavService, messageSender)
    {
      this.whatsNewService = whatsNewService;
      this.ShowIndicator = !this.whatsNewService.HasBeenViewed;
      this.whatsNewService.WasViewedChanged += new EventHandler(this.OnWasViewedChanged);
    }

    private void OnWasViewedChanged(object sender, EventArgs eventArgs) => this.ShowIndicator = !this.whatsNewService.HasBeenViewed;
  }
}
