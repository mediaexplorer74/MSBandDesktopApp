// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PivotDetailsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class PivotDetailsViewModel : PageViewModelBase, IPageTaxonomyTypeProvider
  {
    private readonly IEventTrackingService eventViewTracker;
    private readonly ISmoothNavService navigation;
    private readonly IServiceLocator serviceLocator;
    private readonly IMessageSender messageSender;
    private HealthCommand backCommand;
    private MetricTileViewModel tile;
    private bool isBlocked;

    public PivotDetailsViewModel(
      IEventTrackingService eventViewTracker,
      ISmoothNavService navigation,
      INetworkService networkService,
      IServiceLocator serviceLocator,
      IMessageSender messageSender)
      : base(networkService)
    {
      this.SupportsChanges = false;
      this.eventViewTracker = eventViewTracker;
      this.navigation = navigation;
      this.serviceLocator = serviceLocator;
      this.messageSender = messageSender;
    }

    public MetricTileViewModel Tile
    {
      get => this.tile;
      set => this.SetProperty<MetricTileViewModel>(ref this.tile, value, nameof (Tile));
    }

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand(new Action(this.TryGoBack)));

    private void TryGoBack()
    {
      if (!this.navigation.CanGoBack)
        return;
      this.navigation.GoBack();
    }

    public bool IsBlocked
    {
      get => this.isBlocked;
      set => this.SetProperty<bool>(ref this.isBlocked, value, nameof (IsBlocked));
    }

    private void OnBlockUserInteractionMessageChanged(BlockUserInteractionMessage message) => this.IsBlocked = message.IsBlocked;

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      string key;
      if (parameters == null || !parameters.TryGetValue("Type", out key))
        throw new NoDataException();
      string eventId;
      if (parameters.TryGetValue("ID", out eventId))
        this.eventViewTracker.ReportView(eventId);
      this.Tile = this.serviceLocator.GetInstance<MetricTileViewModel>(key);
      this.Tile.OnDetailsPage = true;
      this.Tile.NavigateTo();
      await this.Tile.LoadAsync(parameters);
      this.Tile.ColorLevel = TileColorLevel.Medium;
      foreach (PivotDefinition pivot in (IEnumerable<PivotDefinition>) this.tile.Pivots)
        await ((PanelViewModelBase) pivot.Content).LoadAsync(parameters);
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BlockUserInteractionMessage>((object) this, new Action<BlockUserInteractionMessage>(this.OnBlockUserInteractionMessageChanged));
    }

    protected override void OnBackNavigation()
    {
      base.OnBackNavigation();
      this.Tile.NavigateTo();
    }

    protected override void OnNavigatedFrom()
    {
      base.OnNavigatedFrom();
      this.Tile.NavigateFrom();
    }

    public Type GetPageTaxonomyType(bool isBackNavigation)
    {
      if (this.Tile != null)
      {
        if (!isBackNavigation)
          return (Type) null;
        PivotDefinition pivotDefinition = this.Tile.Pivots.FirstOrDefault<PivotDefinition>((Func<PivotDefinition, bool>) (pivot => pivot.IsSelected));
        if (pivotDefinition != null && pivotDefinition.Content != null)
          return pivotDefinition.Content.GetType();
        DebugUtilities.Fail("Unable to retrieve a proper taxonomy for the selected pivot.");
      }
      DebugUtilities.Fail("Unable to retrieve a more specific taxonomy for the pivot details page.");
      return this.GetType();
    }
  }
}
