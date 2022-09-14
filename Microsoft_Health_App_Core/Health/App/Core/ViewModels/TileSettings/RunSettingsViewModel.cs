// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.RunSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class RunSettingsViewModel : 
    SettingsViewModelBase<RunPendingTileSettings>,
    IEventSettingsViewModel
  {
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IBandHardwareService bandHardwareService;
    private HealthCommand cancelCommand;
    private HealthCommand confirmCommand;
    private bool displaySaveChanges;
    private bool isLoaded;
    private BandClass deviceType;
    private IList<LabeledItem<RunDisplayMetricType>> metricChoiceLabels;
    private IList<LabeledItem<RunDisplayMetricType>> metricChoiceLabelsWithNone;
    private IList<RunDisplayMetricType> metricChoices;
    private IList<RunDisplayMetricType> metricChoicesWithNone;
    private LabeledItem<RunDisplayMetricType> selectedMainMetric1;
    private LabeledItem<RunDisplayMetricType> selectedMainMetric2;
    private LabeledItem<RunDisplayMetricType> selectedMainMetric3;
    private LabeledItem<RunDisplayMetricType> selectedDrawerMetric1;
    private LabeledItem<RunDisplayMetricType> selectedDrawerMetric2;
    private LabeledItem<RunDisplayMetricType> selectedDrawerMetric3;
    private LabeledItem<RunDisplayMetricType> selectedDrawerMetric4;
    private string mainMetric1;
    private string mainMetric2;
    private string mainMetric3;
    private string drawerMetric1;
    private string drawerMetric2;
    private string drawerMetric3;
    private string drawerMetric4;

    public RunSettingsViewModel(
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      IErrorHandlingService cargoExceptionUtils,
      INetworkService networkService,
      IBandHardwareService bandHardwareService,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory,
      ITileManagementService tileManagementService)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
      this.smoothNavService = smoothNavService;
      this.messageBoxService = messageBoxService;
      this.bandHardwareService = bandHardwareService;
    }

    public IList<LabeledItem<RunDisplayMetricType>> MetricChoiceLabels
    {
      get => this.metricChoiceLabels;
      set => this.SetProperty<IList<LabeledItem<RunDisplayMetricType>>>(ref this.metricChoiceLabels, value, nameof (MetricChoiceLabels));
    }

    public IList<LabeledItem<RunDisplayMetricType>> MetricChoiceLabelsWithNone
    {
      get => this.metricChoiceLabelsWithNone;
      set => this.SetProperty<IList<LabeledItem<RunDisplayMetricType>>>(ref this.metricChoiceLabelsWithNone, value, nameof (MetricChoiceLabelsWithNone));
    }

    public IList<RunDisplayMetricType> MetricChoices
    {
      get => this.metricChoices;
      private set => this.SetProperty<IList<RunDisplayMetricType>>(ref this.metricChoices, value, nameof (MetricChoices));
    }

    public IList<RunDisplayMetricType> MetricChoicesWithNone
    {
      get => this.metricChoicesWithNone;
      private set => this.SetProperty<IList<RunDisplayMetricType>>(ref this.metricChoicesWithNone, value, nameof (MetricChoicesWithNone));
    }

    public LabeledItem<RunDisplayMetricType> SelectedMainMetric1
    {
      get => this.selectedMainMetric1;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedMainMetric1, value, nameof (SelectedMainMetric1));
      }
    }

    public LabeledItem<RunDisplayMetricType> SelectedMainMetric2
    {
      get => this.selectedMainMetric2;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedMainMetric2, value, nameof (SelectedMainMetric2));
      }
    }

    public LabeledItem<RunDisplayMetricType> SelectedMainMetric3
    {
      get => this.selectedMainMetric3;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedMainMetric3, value, nameof (SelectedMainMetric3));
      }
    }

    public LabeledItem<RunDisplayMetricType> SelectedDrawerMetric1
    {
      get => this.selectedDrawerMetric1;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedDrawerMetric1, value, nameof (SelectedDrawerMetric1));
      }
    }

    public LabeledItem<RunDisplayMetricType> SelectedDrawerMetric2
    {
      get => this.selectedDrawerMetric2;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedDrawerMetric2, value, nameof (SelectedDrawerMetric2));
      }
    }

    public LabeledItem<RunDisplayMetricType> SelectedDrawerMetric3
    {
      get => this.selectedDrawerMetric3;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedDrawerMetric3, value, nameof (SelectedDrawerMetric3));
      }
    }

    public LabeledItem<RunDisplayMetricType> SelectedDrawerMetric4
    {
      get => this.selectedDrawerMetric4;
      set
      {
        this.DisplaySaveChangesIfNeeded();
        this.SetProperty<LabeledItem<RunDisplayMetricType>>(ref this.selectedDrawerMetric4, value, nameof (SelectedDrawerMetric4));
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

    public bool IsLoaded
    {
      get => this.isLoaded;
      set => this.SetProperty<bool>(ref this.isLoaded, value, nameof (IsLoaded));
    }

    public bool IsNeon => this.DeviceType == BandClass.Envoy;

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

    public override string TileGuid => "65bd93db-4293-46af-9a28-bdd6513b4677";

    public override bool CanEdit => true;

    public string Instructions => AppResources.RunSettingsSubheader;

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand(new Action(this.Confirm)));

    private void DisplaySaveChangesIfNeeded()
    {
      if (!this.IsLoaded || this.DisplaySaveChanges)
        return;
      this.DisplaySaveChanges = true;
    }

    private void Confirm()
    {
      this.DisplaySaveChanges = false;
      if (this.DeviceType == BandClass.Envoy)
        this.SetMetricsForEnvoy();
      else
        this.SetMetricsForCargo();
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
        this.MetricChoices = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
        {
          RunDisplayMetricType.Distance,
          RunDisplayMetricType.AveragePace,
          RunDisplayMetricType.Duration,
          RunDisplayMetricType.ElevationGain,
          RunDisplayMetricType.HeartRate,
          RunDisplayMetricType.Time,
          RunDisplayMetricType.Pace,
          RunDisplayMetricType.Calories
        };
        this.MetricChoicesWithNone = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
        {
          RunDisplayMetricType.None,
          RunDisplayMetricType.Distance,
          RunDisplayMetricType.AveragePace,
          RunDisplayMetricType.Duration,
          RunDisplayMetricType.ElevationGain,
          RunDisplayMetricType.HeartRate,
          RunDisplayMetricType.Time,
          RunDisplayMetricType.Pace,
          RunDisplayMetricType.Calories
        };
      }
      else
        this.MetricChoices = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
        {
          RunDisplayMetricType.Distance,
          RunDisplayMetricType.Duration,
          RunDisplayMetricType.HeartRate,
          RunDisplayMetricType.Calories,
          RunDisplayMetricType.Pace
        };
    }

    public async void SetMetricsForCargo()
    {
      if (this.SelectedMainMetric1 != this.SelectedMainMetric2 && this.SelectedMainMetric2 != this.SelectedMainMetric3 && this.SelectedMainMetric3 != this.SelectedMainMetric1)
      {
        this.PendingTileSettings.Metrics = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
        {
          this.SelectedMainMetric1.Value,
          this.SelectedMainMetric2.Value,
          this.SelectedMainMetric3.Value
        };
        this.smoothNavService.GoBack();
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.DataUniqueError, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
        this.DisplaySaveChanges = true;
      }
    }

    public async void SetMetricsForEnvoy()
    {
      if (!this.HasDuplicates((IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
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
        this.PendingTileSettings.Metrics = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
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

    public bool HasDuplicates(IList<RunDisplayMetricType> metrics)
    {
      bool flag = false;
      for (int index1 = 0; index1 < metrics.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < metrics.Count; ++index2)
        {
          if (metrics[index1] != RunDisplayMetricType.None && metrics[index1] == metrics[index2])
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.Cancel)));

    private void Cancel()
    {
      this.DisplaySaveChanges = false;
      this.smoothNavService.GoBack();
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
      this.Header = AppResources.RunSettings;
      this.HasNotifications = false;
      this.SetMetricLabels();
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        this.DeviceType = await this.bandHardwareService.GetDeviceTypeAsync(cancellationTokenSource.Token);
        if (!this.DeviceType.Equals((object) BandClass.Envoy))
        {
          if (!this.DeviceType.Equals((object) BandClass.Cargo))
            this.ValidateDeviceType();
        }
      }
      this.SetMetricChoices();
      this.MetricChoiceLabels = (IList<LabeledItem<RunDisplayMetricType>>) this.MetricChoices.Select<RunDisplayMetricType, LabeledItem<RunDisplayMetricType>>((Func<RunDisplayMetricType, LabeledItem<RunDisplayMetricType>>) (c => LabeledItem<RunDisplayMetricType>.FromEnumValue(c))).ToList<LabeledItem<RunDisplayMetricType>>();
      this.SelectedMainMetric1 = this.FindMetricChoice(this.PendingTileSettings.Metrics[0]);
      this.SelectedMainMetric2 = this.FindMetricChoice(this.PendingTileSettings.Metrics[1]);
      this.SelectedMainMetric3 = this.FindMetricChoice(this.PendingTileSettings.Metrics[2]);
      if (this.IsNeon)
      {
        this.MetricChoiceLabelsWithNone = (IList<LabeledItem<RunDisplayMetricType>>) this.MetricChoicesWithNone.Select<RunDisplayMetricType, LabeledItem<RunDisplayMetricType>>((Func<RunDisplayMetricType, LabeledItem<RunDisplayMetricType>>) (c => LabeledItem<RunDisplayMetricType>.FromEnumValue(c))).ToList<LabeledItem<RunDisplayMetricType>>();
        this.Subheader = AppResources.RunSettingsEditInfoForNeon;
        this.SelectedDrawerMetric1 = this.FindMetricChoice(this.PendingTileSettings.Metrics[3]);
        this.SelectedDrawerMetric2 = this.FindMetricChoiceForNeon(this.PendingTileSettings.Metrics[4]);
        this.SelectedDrawerMetric3 = this.FindMetricChoiceForNeon(this.PendingTileSettings.Metrics[5]);
        this.SelectedDrawerMetric4 = this.FindMetricChoiceForNeon(this.PendingTileSettings.Metrics[6]);
      }
      else
        this.Subheader = AppResources.RunSettingsEditInfo;
      this.IsLoaded = true;
    }

    private LabeledItem<RunDisplayMetricType> FindMetricChoice(
      RunDisplayMetricType metricType)
    {
      return this.MetricChoiceLabels.Single<LabeledItem<RunDisplayMetricType>>((Func<LabeledItem<RunDisplayMetricType>, bool>) (c => c.Value == metricType));
    }

    private LabeledItem<RunDisplayMetricType> FindMetricChoiceForNeon(
      RunDisplayMetricType metricType)
    {
      return this.MetricChoiceLabelsWithNone.Single<LabeledItem<RunDisplayMetricType>>((Func<LabeledItem<RunDisplayMetricType>, bool>) (c => c.Value == metricType));
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
