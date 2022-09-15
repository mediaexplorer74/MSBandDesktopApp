// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.RunPendingTileSettings
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
  public class RunPendingTileSettings : PendingTileSettings
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\TileSettings\\RunPendingTileSettings.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IBandHardwareService bandHardwareService;
    private CargoRunDisplayMetrics bandMetrics;
    private BandClass deviceType;
    private IList<RunDisplayMetricType> metrics;

    public RunPendingTileSettings(
      IBandConnectionFactory cargoConnectionFactory,
      IBandHardwareService bandHardwareService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.bandHardwareService = bandHardwareService;
    }

    public IList<RunDisplayMetricType> Metrics
    {
      get => this.metrics;
      set
      {
        this.metrics = value;
        this.IsChanged = true;
      }
    }

    public override async Task LoadSettingsAsync(CancellationToken token)
    {
      RunPendingTileSettings pendingTileSettings = this;
      int deviceType = (int) pendingTileSettings.deviceType;
      int deviceTypeAsync = (int) await this.bandHardwareService.GetDeviceTypeAsync(token);
      pendingTileSettings.deviceType = (BandClass) deviceTypeAsync;
      pendingTileSettings = (RunPendingTileSettings) null;
      if (this.deviceType == BandClass.Envoy)
        await this.GetEnvoyRunSettingMetricsAsync();
      else
        await this.GetCargoRunSettingMetricsAsync(token);
    }

    public override async Task ApplyChangesAsync()
    {
      try
      {
        List<RunDisplayMetricType> list = new List<RunDisplayMetricType>((IEnumerable<RunDisplayMetricType>) Enum.GetValues(typeof (RunDisplayMetricType))).Except<RunDisplayMetricType>((IEnumerable<RunDisplayMetricType>) this.Metrics).ToList<RunDisplayMetricType>();
        this.bandMetrics.PrimaryMetric = this.Metrics[0];
        this.bandMetrics.TopLeftMetric = this.Metrics[1];
        this.bandMetrics.TopRightMetric = this.Metrics[2];
        if (this.deviceType == BandClass.Cargo)
        {
          list.Remove(RunDisplayMetricType.None);
          this.bandMetrics.LeftDrawerMetric = list[0];
          this.bandMetrics.RightDrawerMetric = list[1];
        }
        else if (this.deviceType == BandClass.Envoy)
        {
          this.bandMetrics.LeftDrawerMetric = this.Metrics[3];
          this.bandMetrics.RightDrawerMetric = this.Metrics[4];
          this.bandMetrics.Metric06 = this.Metrics[5];
          this.bandMetrics.Metric07 = this.Metrics[6];
        }
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
          await cargoConnection.SetRunDisplayMetricsAsync(this.bandMetrics);
        ApplicationTelemetry.LogRunDataPointsChange();
      }
      catch (Exception ex)
      {
        RunPendingTileSettings.Logger.Error(ex, "Failed to set display metrics");
        throw;
      }
    }

    public async Task GetEnvoyRunSettingMetricsAsync()
    {
      IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None);
      try
      {
        if (this.Metrics != null)
        {
          if (this.Metrics.Count != 0)
            goto label_9;
        }
        RunPendingTileSettings pendingTileSettings = this;
        CargoRunDisplayMetrics bandMetrics = pendingTileSettings.bandMetrics;
        CargoRunDisplayMetrics displayMetricsAsync = await cargoConnection.GetRunDisplayMetricsAsync();
        pendingTileSettings.bandMetrics = displayMetricsAsync;
        pendingTileSettings = (RunPendingTileSettings) null;
        this.metrics = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
        {
          this.bandMetrics.PrimaryMetric,
          this.bandMetrics.TopLeftMetric,
          this.bandMetrics.TopRightMetric,
          this.bandMetrics.LeftDrawerMetric,
          this.bandMetrics.RightDrawerMetric,
          this.bandMetrics.Metric06,
          this.bandMetrics.Metric07
        };
      }
      finally
      {
        cargoConnection?.Dispose();
      }
label_9:
      cargoConnection = (IBandConnection) null;
    }

    public async Task GetCargoRunSettingMetricsAsync(CancellationToken token)
    {
      IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token);
      try
      {
        if (this.Metrics != null)
        {
          if (this.Metrics.Count != 0)
            goto label_9;
        }
        RunPendingTileSettings pendingTileSettings = this;
        CargoRunDisplayMetrics bandMetrics = pendingTileSettings.bandMetrics;
        CargoRunDisplayMetrics displayMetricsAsync = await cargoConnection.GetRunDisplayMetricsAsync();
        pendingTileSettings.bandMetrics = displayMetricsAsync;
        pendingTileSettings = (RunPendingTileSettings) null;
        this.metrics = (IList<RunDisplayMetricType>) new List<RunDisplayMetricType>()
        {
          this.bandMetrics.PrimaryMetric,
          this.bandMetrics.TopLeftMetric,
          this.bandMetrics.TopRightMetric
        };
      }
      finally
      {
        cargoConnection?.Dispose();
      }
label_9:
      cargoConnection = (IBandConnection) null;
    }
  }
}
