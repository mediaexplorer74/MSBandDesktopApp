// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.BikePendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class BikePendingTileSettings : PendingTileSettings
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\TileSettings\\BikePendingTileSettings.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IBandHardwareService bandHardwareService;
    private CargoBikeDisplayMetrics bandMetrics;
    private BandClass deviceType;
    private IList<BikeDisplayMetricType> metrics;
    private int splitGroupSize;

    public BikePendingTileSettings(
      IBandConnectionFactory cargoConnectionFactory,
      IBandHardwareService bandHardwareService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.bandHardwareService = bandHardwareService;
    }

    public IList<BikeDisplayMetricType> Metrics
    {
      get => this.metrics;
      set
      {
        this.metrics = value;
        this.MetricsChanged = true;
        this.IsChanged = true;
      }
    }

    public int SplitGroupSize
    {
      get => this.splitGroupSize;
      set
      {
        int splitGroupSize = this.splitGroupSize;
        this.splitGroupSize = value;
        if (splitGroupSize <= 0 || splitGroupSize == value)
          return;
        this.SplitGroupSizeChanged = true;
        this.IsChanged = true;
      }
    }

    public bool MetricsChanged { get; private set; }

    public bool SplitGroupSizeChanged { get; private set; }

    public override async Task LoadSettingsAsync(CancellationToken token)
    {
      BikePendingTileSettings pendingTileSettings = this;
      int deviceType = (int) pendingTileSettings.deviceType;
      int deviceTypeAsync = (int) await this.bandHardwareService.GetDeviceTypeAsync(token);
      pendingTileSettings.deviceType = (BandClass) deviceTypeAsync;
      pendingTileSettings = (BikePendingTileSettings) null;
      if (this.deviceType == BandClass.Envoy)
        await this.GetEnvoyBikeSettingMetricsAsync(token);
      else
        await this.GetCargoBikeSettingMetricsAsync(token);
    }

    public override async Task ApplyChangesAsync()
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
      {
        if (this.Metrics != null && this.MetricsChanged)
        {
          try
          {
            List<BikeDisplayMetricType> list = new List<BikeDisplayMetricType>((IEnumerable<BikeDisplayMetricType>) Enum.GetValues(typeof (BikeDisplayMetricType))).Except<BikeDisplayMetricType>((IEnumerable<BikeDisplayMetricType>) this.Metrics).ToList<BikeDisplayMetricType>();
            this.bandMetrics.PrimaryMetric = this.Metrics[0];
            this.bandMetrics.TopLeftMetric = this.Metrics[1];
            this.bandMetrics.TopRightMetric = this.Metrics[2];
            if (this.deviceType == BandClass.Cargo)
            {
              list.Remove(BikeDisplayMetricType.None);
              this.bandMetrics.DrawerTopLeftMetric = list[0];
              this.bandMetrics.DrawerBottomLeftMetric = list[1];
            }
            else if (this.deviceType == BandClass.Envoy)
            {
              this.bandMetrics.DrawerTopLeftMetric = this.Metrics[3];
              this.bandMetrics.DrawerBottomLeftMetric = this.Metrics[4];
              this.bandMetrics.DrawerBottomRightMetric = this.Metrics[5];
              this.bandMetrics.Metric07 = this.Metrics[6];
            }
            await cargoConnection.SetBikeDisplayMetricsAsync(this.bandMetrics).ConfigureAwait(false);
            this.MetricsChanged = false;
            ApplicationTelemetry.LogBikeDataPointsChange();
          }
          catch (Exception ex)
          {
            BikePendingTileSettings.Logger.Error(ex, "Failed to set display metrics");
            throw;
          }
        }
        if (this.SplitGroupSize > 0)
        {
          if (this.SplitGroupSizeChanged)
          {
            try
            {
              await cargoConnection.SetBikeSplitDistanceAsync(this.SplitGroupSize).ConfigureAwait(false);
              this.SplitGroupSizeChanged = false;
            }
            catch (Exception ex)
            {
              BikePendingTileSettings.Logger.Error(ex, "Failed to set split distance");
              throw;
            }
          }
        }
      }
    }

    public async Task GetEnvoyBikeSettingMetricsAsync(CancellationToken token)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        BikePendingTileSettings pendingTileSettings;
        if (this.Metrics == null || this.Metrics.Count == 0)
        {
          pendingTileSettings = this;
          CargoBikeDisplayMetrics bandMetrics = pendingTileSettings.bandMetrics;
          CargoBikeDisplayMetrics bikeDisplayMetrics = await cargoConnection.GetBikeDisplayMetricsAsync().ConfigureAwait(false);
          pendingTileSettings.bandMetrics = bikeDisplayMetrics;
          pendingTileSettings = (BikePendingTileSettings) null;
          this.metrics = (IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
          {
            this.bandMetrics.PrimaryMetric,
            this.bandMetrics.TopLeftMetric,
            this.bandMetrics.TopRightMetric,
            this.bandMetrics.DrawerTopLeftMetric,
            this.bandMetrics.DrawerBottomLeftMetric,
            this.bandMetrics.DrawerBottomRightMetric,
            this.bandMetrics.Metric07
          };
        }
        if (this.splitGroupSize == 0)
        {
          pendingTileSettings = this;
          int splitGroupSize = pendingTileSettings.splitGroupSize;
          int num = await cargoConnection.GetBikeSplitDistanceAsync().ConfigureAwait(false);
          pendingTileSettings.splitGroupSize = num;
          pendingTileSettings = (BikePendingTileSettings) null;
        }
      }
    }

    public async Task GetCargoBikeSettingMetricsAsync(CancellationToken token)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        BikePendingTileSettings pendingTileSettings;
        if (this.Metrics == null || this.Metrics.Count == 0)
        {
          pendingTileSettings = this;
          CargoBikeDisplayMetrics bandMetrics = pendingTileSettings.bandMetrics;
          CargoBikeDisplayMetrics displayMetricsAsync = await cargoConnection.GetBikeDisplayMetricsAsync();
          pendingTileSettings.bandMetrics = displayMetricsAsync;
          pendingTileSettings = (BikePendingTileSettings) null;
          this.metrics = (IList<BikeDisplayMetricType>) new List<BikeDisplayMetricType>()
          {
            this.bandMetrics.PrimaryMetric,
            this.bandMetrics.TopLeftMetric,
            this.bandMetrics.TopRightMetric
          };
        }
        if (this.splitGroupSize == 0)
        {
          pendingTileSettings = this;
          int splitGroupSize = pendingTileSettings.splitGroupSize;
          int num = await cargoConnection.GetBikeSplitDistanceAsync().ConfigureAwait(false);
          pendingTileSettings.splitGroupSize = num;
          pendingTileSettings = (BikePendingTileSettings) null;
        }
      }
    }
  }
}
