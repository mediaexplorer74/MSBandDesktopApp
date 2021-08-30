// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.DetailsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class DetailsViewModel : PanelViewModelBase, IPageTaxonomyTypeProvider
  {
    private readonly ISmoothNavService navigation;
    private readonly IServiceLocator serviceLocator;
    private HealthCommand backCommand;
    private ILoadableParameters contentViewModel;
    private ILoadableParameters headerViewModel;
    private TileViewModel tile;

    public DetailsViewModel(
      ISmoothNavService navigation,
      INetworkService networkService,
      IServiceLocator serviceLocator)
      : base(networkService)
    {
      this.navigation = navigation;
      this.serviceLocator = serviceLocator;
    }

    public TileViewModel Tile
    {
      get => this.tile;
      set => this.SetProperty<TileViewModel>(ref this.tile, value, nameof (Tile));
    }

    public ILoadableParameters ContentViewModel
    {
      get => this.contentViewModel;
      set => this.SetProperty<ILoadableParameters>(ref this.contentViewModel, value, nameof (ContentViewModel));
    }

    public ILoadableParameters HeaderViewModel
    {
      get => this.headerViewModel;
      set => this.SetProperty<ILoadableParameters>(ref this.headerViewModel, value, nameof (HeaderViewModel));
    }

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand(new Action(this.TryGoBack)));

    private void TryGoBack()
    {
      if (!this.navigation.CanGoBack)
        return;
      this.navigation.GoBack();
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      Assert.ParamIsNotNull((object) parameters, nameof (parameters));
      MetricTileViewModel customTile = this.serviceLocator.GetInstance(typeof (MetricTileViewModel), "Custom") as MetricTileViewModel;
      Task task1 = (Task) Task.FromResult<object>((object) null);
      IStatHeaderViewModel headerViewModel = (IStatHeaderViewModel) null;
      string typeName;
      if (parameters.TryGetValue("HeaderViewModelType", out typeName))
      {
        headerViewModel = this.serviceLocator.GetInstance(Type.GetType(typeName)) as IStatHeaderViewModel;
        task1 = headerViewModel.LoadAsync(parameters);
      }
      customTile.ColorLevel = TileColorLevel.Medium;
      this.Tile = (TileViewModel) customTile;
      this.ContentViewModel = this.serviceLocator.GetInstance(Type.GetType(parameters["TargetViewModelType"])) as ILoadableParameters;
      Task task2 = this.ContentViewModel.LoadAsync(parameters);
      await Task.WhenAll(task1, task2);
      if (headerViewModel != null)
      {
        if (!string.IsNullOrEmpty(headerViewModel.Header))
          customTile.Header = StyledSpan.FromSerializedString(headerViewModel.Header);
        customTile.Subheader = headerViewModel.SubHeader ?? string.Empty;
        customTile.TileIcon = headerViewModel.TileIcon;
      }
      else
      {
        customTile.Subheader = parameters["Subheader"] ?? string.Empty;
        customTile.TileIcon = parameters["TileIcon"];
        string serializedString = (string) null;
        if (!parameters.TryGetValue("Header", out serializedString))
          return;
        customTile.Header = StyledSpan.FromSerializedString(serializedString);
      }
    }

    public Type GetPageTaxonomyType(bool isBackNavigation)
    {
      string typeName;
      if (this.Parameters != null && this.Parameters.TryGetValue("TargetViewModelType", out typeName))
        return Type.GetType(typeName);
      DebugUtilities.Fail("Unable to determine a proper taxonomy for the details page.");
      return this.GetType();
    }
  }
}
