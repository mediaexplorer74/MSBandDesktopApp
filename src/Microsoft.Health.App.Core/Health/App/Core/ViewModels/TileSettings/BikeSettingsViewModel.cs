// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.BikeSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  [PageTaxonomy(new string[] {"Settings", "Band", "Manage Tiles", "Fitness", "Bike Data Points"})]
  public class BikeSettingsViewModel : 
    SettingsViewModelBase<BikePendingTileSettings>,
    IEventSettingsViewModel
  {
    private static readonly int[] SplitValues = new int[10]
    {
      1,
      2,
      3,
      4,
      5,
      10,
      15,
      20,
      25,
      50
    };
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IUserProfileService userProfileService;
    private readonly IBandHardwareService bandHardwareService;
    private HealthCommand cancelCommand;
    private HealthCommand confirmCommand;
    private bool displaySaveChanges;
    private bool isLoaded;
    private IList<LabeledItem<int>> splitChoices;
    private LabeledItem<int> selectedSplit;
    private BandClass deviceType;
    private IList<LabeledItem<BikeDisplayMetricType>> metricChoiceLabels;
    private IList<LabeledItem<BikeDisplayMetricType>> metricChoiceLabelsWithNone;
    private IList<BikeDisplayMetricType> metricChoices;
    private IList<BikeDisplayMetricType> metricChoicesWithNone;
    private LabeledItem<BikeDisplayMetricType> selectedMainMetric1;
    private LabeledItem<BikeDisplayMetricType> selectedMainMetric2;
    private LabeledItem<BikeDisplayMetricType> selectedMainMetric3;
    private LabeledItem<BikeDisplayMetricType> selectedDrawerMetric1;
    private LabeledItem<BikeDisplayMetricType> selectedDrawerMetric2;
    private LabeledItem<BikeDisplayMetricType> selectedDrawerMetric3;
    private LabeledItem<BikeDisplayMetricType> selectedDrawerMetric4;
    private string mainMetric1;
    private string mainMetric2;
    private string mainMetric3;
    private string drawerMetric1;
    private string drawerMetric2;
    private string drawerMetric3;
    private string drawerMetric4;

    public BikeSettingsViewModel(
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandHardwareService bandHardwareService,
      INetworkService networkService,
      IBandConnectionFactory cargoConnectionFactory,
      IUserProfileService userProfileService)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
      this.smoothNavService = smoothNavService;
      this.messageBoxService = messageBoxService;
      this.bandHardwareService = bandHardwareService;
      this.userProfileService = userProfileService;
      this.selectedSplit = new LabeledItem<int>(1, "1 miles");
    }

    public IList<LabeledItem<BikeDisplayMetricType>> MetricChoiceLabels
    {
      get => this.metricChoiceLabels;
      set => this.SetProperty<IList<LabeledItem<BikeDisplayMetricType>>>(ref this.metricChoiceLabels, value, nameof (MetricChoiceLabels));
    }

    public IList<LabeledItem<BikeDisplayMetricType>> MetricChoiceLabelsWithNone
    {
      get => this.metricChoiceLabelsWithNone;
      set => this.SetProperty<IList<LabeledItem<BikeDisplayMetricType>>>(ref this.metricChoiceLabelsWithNone, value, nameof (MetricChoiceLabelsWithNone));
    }

    public IList<BikeDisplayMetricType> MetricChoices
    {
      get => this.metricChoices;
      private set => this.SetProperty<IList<BikeDisplayMetricType>>(ref this.metricChoices, value, nameof (MetricChoices));
    }

    public IList<BikeDisplayMetricType> MetricChoicesWithNone
    {
      get => this.metricChoicesWithNone;
      private set => this.SetProperty<IList<BikeDisplayMetricType>>(ref this.metricChoicesWithNone, value, nameof (MetricChoicesWithNone));
    }

    public LabeledItem<BikeDisplayMetricType> SelectedMainMetric1
    {
      get => this.selectedMainMetric1;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedMainMetric1, value, nameof (SelectedMainMetric1));
      }
    }

    public LabeledItem<BikeDisplayMetricType> SelectedMainMetric2
    {
      get => this.selectedMainMetric2;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedMainMetric2, value, nameof (SelectedMainMetric2));
      }
    }

    public LabeledItem<BikeDisplayMetricType> SelectedMainMetric3
    {
      get => this.selectedMainMetric3;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedMainMetric3, value, nameof (SelectedMainMetric3));
      }
    }

    public LabeledItem<BikeDisplayMetricType> SelectedDrawerMetric1
    {
      get => this.selectedDrawerMetric1;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedDrawerMetric1, value, nameof (SelectedDrawerMetric1));
      }
    }

    public LabeledItem<BikeDisplayMetricType> SelectedDrawerMetric2
    {
      get => this.selectedDrawerMetric2;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedDrawerMetric2, value, nameof (SelectedDrawerMetric2));
      }
    }

    public LabeledItem<BikeDisplayMetricType> SelectedDrawerMetric3
    {
      get => this.selectedDrawerMetric3;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedDrawerMetric3, value, nameof (SelectedDrawerMetric3));
      }
    }

    public LabeledItem<BikeDisplayMetricType> SelectedDrawerMetric4
    {
      get => this.selectedDrawerMetric4;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<BikeDisplayMetricType>>(ref this.selectedDrawerMetric4, value, nameof (SelectedDrawerMetric4));
      }
    }

    public BandClass DeviceType
    {
      get => this.deviceType;
      set
      {
        this.SetProperty<BandClass>(ref this.deviceType, value, nameof (DeviceType));
        this.RaisePropertyChanged("IsNeon");
      }
    }

    public string MainMetric1
    {
      get => this.mainMetric1;
      set => this.SetProperty<string>(ref this.mainMetric1, value, nameof (MainMetric1));
    }

    public string MainMetric2
    {
      get => this.mainMetric2;
      set => this.SetProperty<string>(ref this.mainMetric2, value, nameof (MainMetric2));
    }

    public string MainMetric3
    {
      get => this.mainMetric3;
      set => this.SetProperty<string>(ref this.mainMetric3, value, nameof (MainMetric3));
    }

    public string DrawerMetric1
    {
      get => this.drawerMetric1;
      set => this.SetProperty<string>(ref this.drawerMetric1, value, nameof (DrawerMetric1));
    }

    public string DrawerMetric2
    {
      get => this.drawerMetric2;
      set => this.SetProperty<string>(ref this.drawerMetric2, value, nameof (DrawerMetric2));
    }

    public string DrawerMetric3
    {
      get => this.drawerMetric3;
      set => this.SetProperty<string>(ref this.drawerMetric3, value, nameof (DrawerMetric3));
    }

    public string DrawerMetric4
    {
      get => this.drawerMetric4;
      set => this.SetProperty<string>(ref this.drawerMetric4, value, nameof (DrawerMetric4));
    }

    public bool IsNeon => this.DeviceType == BandClass.Envoy;

    public override string TileGuid => "96430fcb-0060-41cb-9de2-e00cac97f85d";

    public override bool CanEdit => true;

    public IList<LabeledItem<int>> SplitChoices
    {
      get => this.splitChoices;
      private set => this.SetProperty<IList<LabeledItem<int>>>(ref this.splitChoices, value, nameof (SplitChoices));
    }

    public LabeledItem<int> SelectedSplit
    {
      get => this.selectedSplit;
      set
      {
        this.SetProperty<LabeledItem<int>>(ref this.selectedSplit, value, nameof (SelectedSplit));
        this.DisplaySaveChangesIfNeeded();
        this.PendingTileSettings.SplitGroupSize = this.SelectedSplit.Value;
      }
    }

    public bool IsLoaded
    {
      get => this.isLoaded;
      set => this.SetProperty<bool>(ref this.isLoaded, value, nameof (IsLoaded));
    }

    public bool DisplaySaveChanges
    {
      get => this.displaySaveChanges;
      set
      {
        this.SetProperty<bool>(ref this.displaySaveChanges, value, nameof (DisplaySaveChanges));
        if (value)
          this.ShowAppBar();
        else
          this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      }
    }

    public string Instructions => AppResources.BikeSettingsSubheader;

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand((Action) (async () =>
    {
      this.DisplaySaveChanges = false;
      if (this.DeviceType == BandClass.Envoy)
        await this.SetMetricsForEnvoyAsync();
      else
        await this.SetMetricsForCargoAsync();
    })));

    public async Task SetMetricsForCargoAsync()
    {
      if (this.SelectedMainMetric1 != this.SelectedMainMetric2 && this.SelectedMainMetric2 != this.SelectedMainMetric3 && this.SelectedMainMetric3 != this.SelectedMainMetric1)
      {
        BikePendingTileSettings pendingTileSettings = this.PendingTileSettings;
        ObservableCollection<BikeDisplayMetricType> observableCollection = new ObservableCollection<BikeDisplayMetricType>();
        observableCollection.Add(this.SelectedMainMetric1.Value);
        observableCollection.Add(this.SelectedMainMetric2.Value);
        observableCollection.Add(this.SelectedMainMetric3.Value);
        pendingTileSettings.Metrics = (IList<BikeDisplayMetricType>) observableCollection;
        this.smoothNavService.GoBack();
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.DataUniqueError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
        this.DisplaySaveChanges = true;
      }
    }

    public async Task SetMetricsForEnvoyAsync()
    {
      if (!this.HasDuplicates((IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
      {
        this.SelectedMainMetric1.Value,
        this.SelectedMainMetric2.Value,
        this.SelectedMainMetric3.Value,
        this.SelectedDrawerMetric1.Value,
        this.SelectedDrawerMetric2.Value,
        this.SelectedDrawerMetric3.Value,
        this.SelectedDrawerMetric4.Value
      }))
      {
        this.PendingTileSettings.Metrics = (IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
        {
          this.SelectedMainMetric1.Value,
          this.SelectedMainMetric2.Value,
          this.SelectedMainMetric3.Value,
          this.SelectedDrawerMetric1.Value,
          this.SelectedDrawerMetric2.Value,
          this.SelectedDrawerMetric3.Value,
          this.SelectedDrawerMetric4.Value
        };
        this.smoothNavService.GoBack();
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.DataUniqueError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
        this.DisplaySaveChanges = true;
      }
    }

    public void SetMetricLabels()
    {
      this.MainMetric1 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 1
      });
      this.MainMetric2 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 2
      });
      this.MainMetric3 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 3
      });
      this.DrawerMetric1 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 1
      });
      this.DrawerMetric2 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 2
      });
      this.DrawerMetric3 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 3
      });
      this.DrawerMetric4 = string.Format(AppResources.MainMetricTitles, new object[1]
      {
        (object) 4
      });
    }

    public void SetMetricChoices()
    {
      if (this.IsNeon)
      {
        this.MetricChoices = (IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
        {
          BikeDisplayMetricType.Distance,
          BikeDisplayMetricType.AverageSpeed,
          BikeDisplayMetricType.Duration,
          BikeDisplayMetricType.ElevationGain,
          BikeDisplayMetricType.HeartRate,
          BikeDisplayMetricType.Time,
          BikeDisplayMetricType.Speed,
          BikeDisplayMetricType.Calories
        };
        this.MetricChoicesWithNone = (IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
        {
          BikeDisplayMetricType.None,
          BikeDisplayMetricType.Distance,
          BikeDisplayMetricType.AverageSpeed,
          BikeDisplayMetricType.Duration,
          BikeDisplayMetricType.ElevationGain,
          BikeDisplayMetricType.HeartRate,
          BikeDisplayMetricType.Time,
          BikeDisplayMetricType.Speed,
          BikeDisplayMetricType.Calories
        };
      }
      else
        this.MetricChoices = (IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
        {
          BikeDisplayMetricType.Distance,
          BikeDisplayMetricType.Duration,
          BikeDisplayMetricType.HeartRate,
          BikeDisplayMetricType.Calories,
          BikeDisplayMetricType.Speed
        };
    }

    public bool HasDuplicates(IList<BikeDisplayMetricType> metrics)
    {
      bool flag = false;
      for (int index1 = 0; index1 < metrics.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < metrics.Count; ++index2)
        {
          if (metrics[index1] != BikeDisplayMetricType.None && metrics[index1] == metrics[index2])
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand((Action) (() =>
    {
      this.DisplaySaveChanges = false;
      this.smoothNavService.GoBack();
    })));

    private void DisplaySaveChangesIfNeeded()
    {
      if (!this.IsLoaded || this.DisplaySaveChanges)
        return;
      this.DisplaySaveChanges = true;
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      if (!this.DisplaySaveChanges)
        this.DisplaySaveChanges = true;
      else
        this.ShowAppBar();
    }

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      this.Header = AppResources.BikeSettings;
      this.HasNotifications = false;
      this.SetMetricLabels();
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        this.DeviceType = await this.bandHardwareService.GetDeviceTypeAsync(cancellationTokenSource.Token);
        if (!this.DeviceType.Equals((object) BandClass.Envoy) && !this.DeviceType.Equals((object) BandClass.Cargo))
          this.ValidateDeviceType();
        this.SplitChoices = (IList<LabeledItem<int>>) ((IEnumerable<int>) BikeSettingsViewModel.SplitValues).Select<int, LabeledItem<int>>((Func<int, LabeledItem<int>>) (p => new LabeledItem<int>(p, string.Format((IFormatProvider) CultureInfo.CurrentCulture, AppResources.DistanceFormat, new object[2]
        {
          (object) p,
          (object) Formatter.GetLongDistanceUnit(this.userProfileService.DistanceUnitType)
        })))).ToList<LabeledItem<int>>();
      }
      this.SetMetricChoices();
      this.MetricChoiceLabels = (IList<LabeledItem<BikeDisplayMetricType>>) this.MetricChoices.Select<BikeDisplayMetricType, LabeledItem<BikeDisplayMetricType>>((Func<BikeDisplayMetricType, LabeledItem<BikeDisplayMetricType>>) (c => LabeledItem<BikeDisplayMetricType>.FromEnumValue(c))).ToList<LabeledItem<BikeDisplayMetricType>>();
      this.SelectedMainMetric1 = this.FindMetricChoice(this.PendingTileSettings.Metrics[0]);
      this.SelectedMainMetric2 = this.FindMetricChoice(this.PendingTileSettings.Metrics[1]);
      this.SelectedMainMetric3 = this.FindMetricChoice(this.PendingTileSettings.Metrics[2]);
      if (this.IsNeon)
      {
        this.MetricChoiceLabelsWithNone = (IList<LabeledItem<BikeDisplayMetricType>>) this.MetricChoicesWithNone.Select<BikeDisplayMetricType, LabeledItem<BikeDisplayMetricType>>((Func<BikeDisplayMetricType, LabeledItem<BikeDisplayMetricType>>) (c => LabeledItem<BikeDisplayMetricType>.FromEnumValue(c))).ToList<LabeledItem<BikeDisplayMetricType>>();
        this.Subheader = AppResources.BikeSettingsEditInfoForNeon;
        this.SelectedDrawerMetric1 = this.FindMetricChoice(this.PendingTileSettings.Metrics[3]);
        this.SelectedDrawerMetric2 = this.FindMetricChoiceForNeon(this.PendingTileSettings.Metrics[4]);
        this.SelectedDrawerMetric3 = this.FindMetricChoiceForNeon(this.PendingTileSettings.Metrics[5]);
        this.SelectedDrawerMetric4 = this.FindMetricChoiceForNeon(this.PendingTileSettings.Metrics[6]);
      }
      else
        this.Subheader = AppResources.BikeSettingsEditInfo;
      int splitGroupSize = this.PendingTileSettings.SplitGroupSize;
      foreach (LabeledItem<int> splitChoice in (IEnumerable<LabeledItem<int>>) this.splitChoices)
      {
        if (splitChoice.Value == splitGroupSize)
          this.SelectedSplit = splitChoice;
      }
      this.IsLoaded = true;
    }

    private LabeledItem<BikeDisplayMetricType> FindMetricChoice(
      BikeDisplayMetricType metricType)
    {
      return this.MetricChoiceLabels.Single<LabeledItem<BikeDisplayMetricType>>((Func<LabeledItem<BikeDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private LabeledItem<BikeDisplayMetricType> FindMetricChoiceForNeon(
      BikeDisplayMetricType metricType)
    {
      return this.MetricChoiceLabelsWithNone.Single<LabeledItem<BikeDisplayMetricType>>((Func<LabeledItem<BikeDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private async void ValidateDeviceType()
    {
      BandClass deviceType = this.DeviceType;
      if (deviceType.Equals((object) BandClass.Envoy))
      {
        deviceType = this.DeviceType;
        if (deviceType.Equals((object) BandClass.Cargo))
          return;
      }
      int num = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorBody, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
    }
  }
}
