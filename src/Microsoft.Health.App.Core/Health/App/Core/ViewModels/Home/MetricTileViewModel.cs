// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.MetricTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  [MetricTileType(MetricTileType.Review)]
  public abstract class MetricTileViewModel : TileViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\MetricTileViewModel.cs");
    private readonly INetworkService networkService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageSender messageSender;
    private readonly TileFirstTimeUseViewModel firstTimeUse;
    private readonly ObservableCollection<PivotDefinition> pivots = new ObservableCollection<PivotDefinition>();
    private bool canOpen;
    private TileColorLevel colorLevel;
    private bool hasMetric;
    private StyledSpan header;
    private bool isBest;
    private bool isOpen;
    private FormattedMetricViewModel metric;
    private bool showPivotHeader;
    private string pivotheader;
    private string topText;
    private string subheader;
    private string tileIcon;
    private string headerIcon;

    protected MetricTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(TileViewModel.TileType.Metric)
    {
      this.showPivotHeader = true;
      this.networkService = networkService;
      this.smoothNavService = smoothNavService;
      this.firstTimeUse = firstTimeUse;
      this.messageSender = messageSender;
      this.ColorLevel = TileColorLevel.Medium;
    }

    public bool HasMetric
    {
      get => this.hasMetric;
      private set => this.SetProperty<bool>(ref this.hasMetric, value, nameof (HasMetric));
    }

    public ISmoothNavService NavService => this.smoothNavService;

    public bool IsBest
    {
      get => this.isBest;
      protected set => this.SetProperty<bool>(ref this.isBest, value, nameof (IsBest));
    }

    public bool IsOpen
    {
      get => this.isOpen;
      set
      {
        if (!this.SetProperty<bool>(ref this.isOpen, value, nameof (IsOpen)))
          return;
        if (value && this.Pivots.Count > 0 && !this.Pivots.Any<PivotDefinition>((Func<PivotDefinition, bool>) (p => p.IsSelected)))
          this.Pivots[0].IsSelected = true;
        if (value)
        {
          this.OnOpen();
        }
        else
        {
          this.CleanupPanels();
          this.RefreshColorLevel();
        }
      }
    }

    public FormattedMetricViewModel Metric
    {
      get => this.metric;
      set
      {
        this.SetProperty<FormattedMetricViewModel>(ref this.metric, value, nameof (Metric));
        this.UpdateHasMetric();
      }
    }

    public bool ShowPivotHeader
    {
      get => this.showPivotHeader;
      protected set => this.SetProperty<bool>(ref this.showPivotHeader, value, nameof (ShowPivotHeader));
    }

    public string PivotHeader
    {
      get => this.pivotheader;
      protected set => this.SetProperty<string>(ref this.pivotheader, value, nameof (PivotHeader));
    }

    protected virtual string LoadingMessage => AppResources.UpdatingTileMessage;

    public IList<PivotDefinition> Pivots => (IList<PivotDefinition>) this.pivots;

    public StyledSpan Header
    {
      get => this.header;
      set
      {
        this.SetProperty<StyledSpan>(ref this.header, value, nameof (Header));
        this.UpdateHasMetric();
      }
    }

    public string HeaderIcon
    {
      get => this.headerIcon;
      set
      {
        this.SetProperty<string>(ref this.headerIcon, value, nameof (HeaderIcon));
        this.RaisePropertyChanged("IsHeaderIconVisible");
      }
    }

    public bool IsHeaderIconVisible => !string.IsNullOrEmpty(this.headerIcon);

    public string TopText
    {
      get => this.topText;
      set => this.SetProperty<string>(ref this.topText, value, nameof (TopText));
    }

    public string Subheader
    {
      get => this.subheader;
      set => this.SetProperty<string>(ref this.subheader, value, nameof (Subheader));
    }

    public string TileIcon
    {
      get => this.tileIcon;
      set => this.SetProperty<string>(ref this.tileIcon, value, nameof (TileIcon));
    }

    public TileColorLevel ColorLevel
    {
      get => this.colorLevel;
      set => this.SetProperty<TileColorLevel>(ref this.colorLevel, value, nameof (ColorLevel));
    }

    public bool CanOpen
    {
      get => this.canOpen;
      protected set
      {
        this.SetProperty<bool>(ref this.canOpen, value, nameof (CanOpen));
        this.IsTileCommandEnabled = value;
      }
    }

    public bool OnDetailsPage { get; set; }

    public TileFirstTimeUseViewModel FirstTimeUse => this.firstTimeUse;

    protected override void OnNavigatedFrom() => this.CleanupPanels();

    protected override void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      foreach (PivotDefinition pivot in (IEnumerable<PivotDefinition>) this.Pivots)
      {
        if (pivot.Content is INavigationListener content1)
          content1.OnBackNavigation(this.IsOpen);
      }
    }

    private void CleanupPanels()
    {
      foreach (PivotDefinition pivot in (IEnumerable<PivotDefinition>) this.Pivots)
      {
        try
        {
          if (pivot.Content is HealthViewModelBase content3)
            this.messageSender.Unregister((object) content3);
        }
        catch (Exception ex)
        {
          MetricTileViewModel.Logger.Error((object) ex);
        }
      }
    }

    public virtual void Open() => this.smoothNavService.Navigate(typeof (TilesViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      ["Tile"] = this.GetType().Name
    });

    protected override sealed void OnTileCommand() => this.Open();

    protected virtual void RefreshColorLevel()
    {
    }

    protected virtual void OnOpen()
    {
    }

    protected override async Task OnTransitionToLoadingStateAsync()
    {
      await base.OnTransitionToLoadingStateAsync();
      this.CanOpen = false;
      this.Header = (StyledSpan) null;
      this.HeaderIcon = (string) null;
      this.IsBest = false;
      this.Metric = (FormattedMetricViewModel) null;
      this.Subheader = this.LoadingMessage;
      this.PivotHeader = (string) null;
    }

    protected override async Task OnTransitionToLoadedStateAsync() => await base.OnTransitionToLoadedStateAsync();

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.CanOpen = this.FirstTimeUse.IsSupported;
      this.Header = (StyledSpan) null;
      this.HeaderIcon = (string) null;
      this.IsBest = false;
      this.Metric = (FormattedMetricViewModel) null;
      this.Subheader = (string) null;
      this.PivotHeader = (string) null;
    }

    protected override async Task OnTransitionToErrorStateAsync(Exception ex)
    {
      await base.OnTransitionToErrorStateAsync(ex);
      this.CanOpen = false;
      this.Header = (StyledSpan) null;
      this.HeaderIcon = (string) null;
      this.IsBest = false;
      this.Metric = (FormattedMetricViewModel) null;
      this.Subheader = this.networkService.IsInternetAvailable ? AppResources.DataErrorMessage : AppResources.InternetRequiredMessage;
      this.PivotHeader = (string) null;
    }

    private void UpdateHasMetric() => this.HasMetric = this.Metric != null || this.Header != null;
  }
}
