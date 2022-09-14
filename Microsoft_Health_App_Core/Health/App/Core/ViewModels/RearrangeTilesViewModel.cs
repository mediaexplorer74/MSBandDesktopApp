// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.RearrangeTilesViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles", "Re-Order"})]
  [PageMetadata(PageContainerType.FullScreenLandscape)]
  public class RearrangeTilesViewModel : PageViewModelBase
  {
    private readonly ISmoothNavService navigationService;
    private readonly ITileManagementService tileManager;
    private HealthCommand cancelCommand;
    private IList<AppBandTile> enabledTiles;
    private string header;
    private bool rearranged;
    private HealthCommand saveCommand;
    private string subheader;
    private bool reorderEnabled;

    public RearrangeTilesViewModel(
      ISmoothNavService navSvc,
      ITileManagementService tileManager,
      INetworkService networkService)
      : base(networkService)
    {
      this.navigationService = navSvc;
      this.tileManager = tileManager;
      this.reorderEnabled = true;
    }

    public IList<AppBandTile> EnabledTiles
    {
      get => this.enabledTiles;
      private set => this.SetProperty<IList<AppBandTile>>(ref this.enabledTiles, value, nameof (EnabledTiles));
    }

    public ICommand SaveCommand
    {
      get
      {
        this.saveCommand = this.saveCommand ?? new HealthCommand(new Action(this.SaveTileNewOrder));
        return (ICommand) this.saveCommand;
      }
    }

    public ICommand CancelCommand
    {
      get
      {
        this.cancelCommand = this.cancelCommand ?? new HealthCommand(new Action(this.CancelRearrange));
        return (ICommand) this.cancelCommand;
      }
    }

    public string Header
    {
      get => this.header;
      set => this.SetProperty<string>(ref this.header, value, nameof (Header));
    }

    public string Subheader
    {
      get => this.subheader;
      set => this.SetProperty<string>(ref this.subheader, value, nameof (Subheader));
    }

    public bool ShowSaveCancelButtons
    {
      get => this.rearranged;
      set => this.SetProperty<bool>(ref this.rearranged, value, nameof (ShowSaveCancelButtons));
    }

    public bool ReorderEnabled
    {
      get => this.reorderEnabled;
      set
      {
        this.SetProperty<bool>(ref this.reorderEnabled, value, nameof (ReorderEnabled));
        if (value)
          return;
        this.navigationService.GoBack();
      }
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.EnabledTiles = (IList<AppBandTile>) new ObservableCollection<AppBandTile>(this.tileManager.PendingTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.ShowTile)));
      this.ShowSaveCancelButtons = this.EnabledTiles.Any<AppBandTile>();
      return (Task) Task.FromResult<bool>(true);
    }

    protected override void OnNavigatedTo()
    {
      this.navigationService.DisableNavPanel(this.GetType());
      this.ShowAppBar();
      base.OnNavigatedTo();
    }

    protected override void OnNavigatedFrom()
    {
      this.navigationService.EnableNavPanel(this.GetType());
      base.OnNavigatedFrom();
    }

    private void SaveTileNewOrder()
    {
      ObservableCollection<AppBandTile> observableCollection = new ObservableCollection<AppBandTile>((IEnumerable<AppBandTile>) this.EnabledTiles);
      foreach (AppBandTile pendingTile in (IEnumerable<AppBandTile>) this.tileManager.PendingTiles)
      {
        if (!observableCollection.Contains(pendingTile))
          observableCollection.Add(pendingTile.Copy());
      }
      this.tileManager.PendingTiles = (IList<AppBandTile>) new List<AppBandTile>((IEnumerable<AppBandTile>) observableCollection);
      this.tileManager.HaveTilesChanged = true;
      this.tileManager.HaveTilesBeenReordered = true;
      this.navigationService.GoBack();
    }

    private void CancelRearrange() => this.navigationService.GoBack();

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.SaveCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });
  }
}
