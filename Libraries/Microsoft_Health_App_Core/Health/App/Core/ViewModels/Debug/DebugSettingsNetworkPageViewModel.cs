// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsNetworkPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class DebugSettingsNetworkPageViewModel : DebugSettingsPageViewModelBase
  {
    private readonly IUserService userService;
    private readonly IConnectionInfoStore connectionInfoStore;
    private readonly IOobeService oobeService;
    private readonly IConfig config;
    private readonly IMessageBoxService messageBoxService;
    private bool isEnvironmentButtonEnabled;
    private bool isCustomEnvironmentUrlEnabled;
    private string customEnvironmentUrl;
    private string healthAndFitnessBaseUrl;
    private string selectedEnvironment;
    private HealthCloudConnectionInfo cargoCloudConnectionInfo;

    public ICommand ApplyEnvironmentCommand { get; private set; }

    public ICommand ApplyCloudConnectionCommand { get; private set; }

    public bool IsEnvironmentButtonEnabled
    {
      get => this.isEnvironmentButtonEnabled;
      set => this.SetProperty<bool>(ref this.isEnvironmentButtonEnabled, value, nameof (IsEnvironmentButtonEnabled));
    }

    public IList<string> Environments => CloudEnvironment.EnvironmentNames;

    public string SelectedEnvironment
    {
      get => this.selectedEnvironment;
      set
      {
        if (!this.SetProperty<string>(ref this.selectedEnvironment, value, nameof (SelectedEnvironment)))
          return;
        this.InvalidateCustomEnvironmentUrl();
      }
    }

    public string CustomEnvironmentUrl
    {
      get => this.customEnvironmentUrl;
      set => this.SetProperty<string>(ref this.customEnvironmentUrl, value, nameof (CustomEnvironmentUrl));
    }

    public string HealthAndFitnessBaseUrl
    {
      get => this.healthAndFitnessBaseUrl;
      set => this.SetProperty<string>(ref this.healthAndFitnessBaseUrl, value, nameof (HealthAndFitnessBaseUrl));
    }

    public bool IsCustomEnvironmentUrlEnabled
    {
      get => this.isCustomEnvironmentUrlEnabled;
      private set => this.SetProperty<bool>(ref this.isCustomEnvironmentUrlEnabled, value, nameof (IsCustomEnvironmentUrlEnabled));
    }

    public DebugSettingsNetworkPageViewModel(
      IConfig config,
      IConnectionInfoStore connectionInfoStore,
      IOobeService oobeService,
      INetworkService networkService,
      IUserService userService,
      IMessageBoxService messageBoxService)
      : base(networkService)
    {
      this.IsEnvironmentButtonEnabled = true;
      this.config = config;
      this.messageBoxService = messageBoxService;
      this.connectionInfoStore = connectionInfoStore;
      this.userService = userService;
      this.oobeService = oobeService;
      this.ApplyEnvironmentCommand = (ICommand) new HealthCommand(new Action(this.ApplyEnvironment));
      this.ApplyCloudConnectionCommand = (ICommand) new HealthCommand(new Action(this.ApplyCloudConnectionValues));
      this.Header = "Network";
      this.SubHeader = "Cloud environment";
      this.GlyphIcon = "\uE038";
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.CustomEnvironmentUrl = this.config.CustomEnvironmentUrl;
      this.SelectedEnvironment = this.config.Environment;
      try
      {
        DebugSettingsNetworkPageViewModel networkPageViewModel = this;
        HealthCloudConnectionInfo cloudConnectionInfo = networkPageViewModel.cargoCloudConnectionInfo;
        HealthCloudConnectionInfo async = await this.connectionInfoStore.GetAsync();
        networkPageViewModel.cargoCloudConnectionInfo = async;
        networkPageViewModel = (DebugSettingsNetworkPageViewModel) null;
        this.healthAndFitnessBaseUrl = this.cargoCloudConnectionInfo.HnFEndpoint.ToString();
      }
      catch (Exception ex)
      {
      }
    }

    private async void ApplyEnvironment()
    {
      this.IsEnvironmentButtonEnabled = false;
      if (this.IsCustomEnvironmentUrlEnabled)
      {
        string url = this.TryGetValidUrl(this.customEnvironmentUrl);
        if (url == null)
        {
          int num = (int) await this.messageBoxService.ShowAsync("Please enter a valid Custom Environment URL.", (string) null, PortableMessageBoxButton.OK);
          this.IsEnvironmentButtonEnabled = true;
          return;
        }
        this.config.CustomEnvironmentUrl = url;
        url = (string) null;
      }
      this.config.Environment = this.SelectedEnvironment;
      if (this.userService.SupportsSignOut)
      {
        await this.userService.SignOutAsync();
      }
      else
      {
        int num1 = await this.oobeService.ResetOobeStatusAsync(false) ? 1 : 0;
      }
      this.IsEnvironmentButtonEnabled = true;
    }

    private string TryGetValidUrl(string validateUrl)
    {
      string uriString = (validateUrl ?? string.Empty).Trim();
      return !string.IsNullOrEmpty(uriString) && Uri.IsWellFormedUriString(uriString, UriKind.Absolute) ? uriString : (string) null;
    }

    private async void ApplyCloudConnectionValues()
    {
      if (this.TryGetValidUrl(this.healthAndFitnessBaseUrl) == null)
      {
        int num = (int) await this.messageBoxService.ShowAsync("Please enter a valid HnF base URL.", (string) null, PortableMessageBoxButton.OK);
        this.IsEnvironmentButtonEnabled = true;
      }
      else if (this.cargoCloudConnectionInfo == null)
      {
        int num1 = (int) await this.messageBoxService.ShowAsync("Could not update connection information.", (string) null, PortableMessageBoxButton.OK);
      }
      else
      {
        this.cargoCloudConnectionInfo.HnFEndpoint = new Uri(this.healthAndFitnessBaseUrl);
        await this.connectionInfoStore.SetAsync(this.cargoCloudConnectionInfo);
        int num2 = (int) await this.messageBoxService.ShowAsync("Connection Information updated!", (string) null, PortableMessageBoxButton.OK);
      }
    }

    private void InvalidateCustomEnvironmentUrl() => this.IsCustomEnvironmentUrlEnabled = CloudEnvironment.IsCustom(this.SelectedEnvironment);
  }
}
