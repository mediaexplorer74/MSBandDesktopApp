// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.FirmwareUpdateProgress
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using System;

namespace DesktopSyncApp
{
  public class FirmwareUpdateProgress : KDKProgress<FirmwareUpdateProgress>
  {
    private FirmwareUpdateState? latestState;
    private double? latestProgressPercentage;
    private ITimedTelemetryEvent stateTimedEvent;
    private ITimedTelemetryEvent updateTimedEvent;
    private bool isOobeUpdate;

    public double? LatestProgressPercentage
    {
      get => this.latestProgressPercentage;
      set
      {
        double? progressPercentage = this.latestProgressPercentage;
        double? nullable = value;
        if ((progressPercentage.GetValueOrDefault() == nullable.GetValueOrDefault() ? (progressPercentage.HasValue != nullable.HasValue ? 1 : 0) : 1) == 0)
          return;
        this.latestProgressPercentage = value;
        this.OnPropertyChanged(nameof (LatestProgressPercentage));
      }
    }

    public FirmwareUpdateState? LatestState
    {
      get => this.latestState;
      set
      {
        FirmwareUpdateState? latestState = this.latestState;
        FirmwareUpdateState? nullable = value;
        if ((latestState.GetValueOrDefault() == nullable.GetValueOrDefault() ? (latestState.HasValue != nullable.HasValue ? 1 : 0) : 1) == 0)
          return;
        this.latestState = value;
        this.OnPropertyChanged(nameof (LatestState));
        this.HandleStateChange(this.latestState.Value);
      }
    }

    public override void Report(FirmwareUpdateProgress progress)
    {
      this.LatestProgressPercentage = new double?(progress.PercentageCompletion);
      this.LatestState = new FirmwareUpdateState?(progress.State);
    }

    private void HandleStateChange(FirmwareUpdateState state)
    {
      if (this.updateTimedEvent == null && !this.isOobeUpdate)
        this.updateTimedEvent = CommonTelemetry.TimeFirmwareUpdate();
      if (this.stateTimedEvent != null)
      {
        this.stateTimedEvent.End();
        ((IDisposable) this.stateTimedEvent).Dispose();
        this.stateTimedEvent = (ITimedTelemetryEvent) null;
      }
      switch ((int) state)
      {
        case 0:
          break;
        case 1:
          this.stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateDownload(this.isOobeUpdate);
          break;
        case 2:
          break;
        case 3:
          break;
        case 4:
          this.stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateSendToBand(this.isOobeUpdate);
          break;
        case 5:
          this.stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateRebootBand(this.isOobeUpdate);
          break;
        case 6:
          if (this.updateTimedEvent == null)
            break;
          this.updateTimedEvent.End();
          ((IDisposable) this.updateTimedEvent).Dispose();
          this.updateTimedEvent = (ITimedTelemetryEvent) null;
          break;
        default:
          DebugUtilities.Fail("Received unrecognized FW update state: {0}", new object[1]
          {
            (object) state
          });
          break;
      }
    }
  }
}
