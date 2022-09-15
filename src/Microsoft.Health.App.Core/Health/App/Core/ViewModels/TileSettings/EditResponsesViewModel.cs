// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.EditResponsesViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class EditResponsesViewModel : PageViewModelBase, IPageTaxonomyProvider
  {
    internal const string TileIdKey = "TileId";
    private readonly IMessageBoxService messageBoxService;
    private readonly ITileManagementService tileManagementService;
    private readonly ISmoothNavService smoothNavService;
    private HealthCommand cancelCommand;
    private HealthCommand confirmCommand;
    private QuickResponsePendingTileSettings pendingTileSettings;
    private bool displaySaveChanges;
    private bool isSms;
    private bool isTextLoaded;
    private string quickResponsesHeader;
    private string response1;
    private string response2;
    private string response3;
    private string response4;

    public EditResponsesViewModel(
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      ITileManagementService tileManagementService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.messageBoxService = messageBoxService;
      this.tileManagementService = tileManagementService;
    }

    public string QuickResponsesHeader
    {
      get => this.quickResponsesHeader;
      private set => this.SetProperty<string>(ref this.quickResponsesHeader, value, nameof (QuickResponsesHeader));
    }

    public string Response1
    {
      get => this.response1;
      set
      {
        if (this.IsTextLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<string>(ref this.response1, value, nameof (Response1));
      }
    }

    public string Response2
    {
      get => this.response2;
      set
      {
        if (this.IsTextLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<string>(ref this.response2, value, nameof (Response2));
      }
    }

    public string Response3
    {
      get => this.response3;
      set
      {
        if (this.IsTextLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<string>(ref this.response3, value, nameof (Response3));
      }
    }

    public string Response4
    {
      get => this.response4;
      set
      {
        if (this.IsTextLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<string>(ref this.response4, value, nameof (Response4));
      }
    }

    public bool IsTextLoaded
    {
      get => this.isTextLoaded;
      set => this.SetProperty<bool>(ref this.isTextLoaded, value, nameof (IsTextLoaded));
    }

    public bool DisplaySaveChanges
    {
      get => this.displaySaveChanges;
      set
      {
        if (!this.SetProperty<bool>(ref this.displaySaveChanges, value, nameof (DisplaySaveChanges)))
          return;
        if (this.displaySaveChanges)
          this.ShowAppBar();
        else
          this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      }
    }

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand(new Action(this.Confirm)));

    private async void Confirm()
    {
      this.DisplaySaveChanges = false;
      if (!string.IsNullOrWhiteSpace(this.Response1) || !string.IsNullOrWhiteSpace(this.Response2) || !string.IsNullOrWhiteSpace(this.Response3) || !string.IsNullOrWhiteSpace(this.Response4))
      {
        if (this.isSms)
          this.pendingTileSettings.MessageResponses = new string[4]
          {
            this.Response1,
            this.Response2,
            this.Response3,
            this.Response4
          };
        else
          this.pendingTileSettings.PhoneResponses = new string[4]
          {
            this.Response1,
            this.Response2,
            this.Response3,
            this.Response4
          };
        this.IsTextLoaded = false;
        this.smoothNavService.GoBack();
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.SMSEmptyResponseError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
        this.DisplaySaveChanges = true;
      }
    }

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.Cancel)));

    private void Cancel()
    {
      this.DisplaySaveChanges = false;
      this.IsTextLoaded = false;
      this.smoothNavService.GoBack();
    }

    public IReadOnlyList<string> GetPageTaxonomy() => (IReadOnlyList<string>) new List<string>()
    {
      "Settings",
      "Manage Tiles",
      this.isSms ? "Messaging" : "Calls",
      "Edit Custom Responses"
    };

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EditResponsesViewModel responsesViewModel = this;
      QuickResponsePendingTileSettings pendingTileSettings = responsesViewModel.pendingTileSettings;
      QuickResponsePendingTileSettings pendingSettingsAsync = await this.tileManagementService.GetPendingSettingsAsync<QuickResponsePendingTileSettings>();
      responsesViewModel.pendingTileSettings = pendingSettingsAsync;
      responsesViewModel = (EditResponsesViewModel) null;
      this.isSms = parameters["TileId"] == "b4edbc35-027b-4d10-a797-1099cd2ad98a";
      if (this.isSms)
      {
        this.Response1 = this.pendingTileSettings.MessageResponses[0];
        this.Response2 = this.pendingTileSettings.MessageResponses[1];
        this.Response3 = this.pendingTileSettings.MessageResponses[2];
        this.Response4 = this.pendingTileSettings.MessageResponses[3];
        this.QuickResponsesHeader = AppResources.SMSQuickResponses;
      }
      else
      {
        this.Response1 = this.pendingTileSettings.PhoneResponses[0];
        this.Response2 = this.pendingTileSettings.PhoneResponses[1];
        this.Response3 = this.pendingTileSettings.PhoneResponses[2];
        this.Response4 = this.pendingTileSettings.PhoneResponses[3];
        this.QuickResponsesHeader = AppResources.CallQuickResponses;
      }
      if (!this.DisplaySaveChanges)
        this.DisplaySaveChanges = true;
      else
        this.ShowAppBar();
      this.IsTextLoaded = true;
    }
  }
}
