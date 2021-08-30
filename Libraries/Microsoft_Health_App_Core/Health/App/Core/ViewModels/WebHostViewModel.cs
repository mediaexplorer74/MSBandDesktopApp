// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WebHostViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class WebHostViewModel : PageViewModelBase
  {
    public const string UrlKey = "Url";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WebHostViewModel.cs");
    private readonly INetworkService networkService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageBoxService messageBoxService;
    private HealthCommand doneCommand;
    private HealthCommand navigationStartingCommand;
    private HealthCommand<WebViewNavigationCompletedEventArgsWrapper> navigationCompletedCommand;
    private Uri pageUrl;
    private bool isNavigating;

    public WebHostViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService)
      : base(networkService)
    {
      this.networkService = networkService;
      this.messageBoxService = messageBoxService;
      this.smoothNavService = smoothNavService;
    }

    public ICommand NavigationStartingCommand => (ICommand) this.navigationStartingCommand ?? (ICommand) (this.navigationStartingCommand = new HealthCommand((Action) (() => this.IsNavigating = true)));

    public ICommand NavigationCompletedCommand => (ICommand) this.navigationCompletedCommand ?? (ICommand) (this.navigationCompletedCommand = new HealthCommand<WebViewNavigationCompletedEventArgsWrapper>(new Action<WebViewNavigationCompletedEventArgsWrapper>(this.OnNavigationCompleted)));

    public ICommand DoneCommand => (ICommand) this.doneCommand ?? (ICommand) (this.doneCommand = new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    public Uri PageUrl
    {
      get => this.pageUrl;
      set => this.SetProperty<Uri>(ref this.pageUrl, value, nameof (PageUrl));
    }

    public bool IsNavigating
    {
      get => this.isNavigating;
      set => this.SetProperty<bool>(ref this.isNavigating, value, nameof (IsNavigating));
    }

    protected virtual async void OnNavigationCompleted(
      WebViewNavigationCompletedEventArgsWrapper eventArgs)
    {
      this.IsNavigating = false;
      if (eventArgs.IsSuccess)
        return;
      WebHostViewModel.Logger.Error((object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Web page {0} failed to load. Error code: {1}", new object[2]
      {
        (object) eventArgs.Uri,
        (object) eventArgs.WebErrorStatus
      }));
      if (!this.networkService.IsInternetAvailable)
      {
        int num1 = (int) await this.messageBoxService.ShowAsync(AppResources.InternetRequiredMessage, AppResources.NetworkErrorTitle, PortableMessageBoxButton.OK);
      }
      else if (eventArgs.WebErrorStatus >= 400)
      {
        int num2 = (int) await this.messageBoxService.ShowAsync(AppResources.SystemErrorBody, AppResources.SystemErrorTitle, PortableMessageBoxButton.OK);
      }
      else
      {
        int num3 = (int) await this.messageBoxService.ShowAsync(AppResources.NetworkErrorBody, AppResources.NetworkErrorTitle, PortableMessageBoxButton.OK);
      }
      this.smoothNavService.GoBack();
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.PageUrl = new Uri(this.GetStringParameter("Url"));
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
