// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.NavChoiceViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public class NavChoiceViewModel : HealthViewModelBase, INavChoiceViewModel, INavListItem
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\NavChoiceViewModel.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private readonly ILauncherService launcherService;
    private bool isNavigationEnabled;
    private HealthCommand actionCommand;
    private string icon;
    private string name;
    private bool isEnabled;

    public NavChoiceViewModel(
      IMessageBoxService messageBoxService,
      ILauncherService launcherService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender)
    {
      this.messageBoxService = messageBoxService;
      this.launcherService = launcherService;
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.IsEnabled = true;
      this.isNavigationEnabled = true;
      this.messageSender.Register<NavChoiceMessage>((object) this, new Action<NavChoiceMessage>(this.OnNavChoiceMessage));
    }

    public string Destination { get; set; }

    public Type Page { get; set; }

    public ICommand ActionCommand => (ICommand) this.actionCommand ?? (ICommand) (this.actionCommand = new HealthCommand(new Action(this.Navigate)));

    private async void Navigate()
    {
      bool flag = true;
      if (!this.isNavigationEnabled)
        flag = await this.UserChoicePostAdvisementAsync();
      if (!flag)
        return;
      if (this.Destination != null)
      {
        if (this.Destination.ToLower().StartsWith("http"))
          this.launcherService.ShowUserWebBrowser(new Uri(this.Destination, UriKind.Absolute));
        else
          NavChoiceViewModel.Logger.Warn((object) ("Unrecognized destination: " + this.Destination));
      }
      if ((object) this.Page == null)
        return;
      this.smoothNavService.Navigate(this.Page);
    }

    public string Name
    {
      get => this.name;
      set => this.SetProperty<string>(ref this.name, value, nameof (Name));
    }

    public string GlyphIcon
    {
      get => this.icon;
      set => this.SetProperty<string>(ref this.icon, value, nameof (GlyphIcon));
    }

    public bool IsEnabled
    {
      get => this.isEnabled;
      set => this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled));
    }

    private async Task<bool> UserChoicePostAdvisementAsync()
    {
      if (await this.messageBoxService.ShowAsync(AppResources.ManageTilesConfirmationBody, AppResources.ManageTilesConfirmationTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
        return false;
      this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
      {
        Enabled = true
      });
      this.messageSender.Send<PanelRefreshMessage>(new PanelRefreshMessage());
      return true;
    }

    private void OnNavChoiceMessage(NavChoiceMessage message) => this.isNavigationEnabled = message.Enabled;
  }
}
