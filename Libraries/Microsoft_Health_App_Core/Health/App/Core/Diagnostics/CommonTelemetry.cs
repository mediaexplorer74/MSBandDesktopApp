// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.CommonTelemetry
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Sync;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class CommonTelemetry
  {
    public static ITimedTelemetryEvent TimeSync(SyncType syncType)
    {
      string str;
      switch (syncType)
      {
        case SyncType.Background:
          str = "Background";
          break;
        case SyncType.Manual:
          str = "Manual";
          break;
        default:
          str = "Automatic";
          break;
      }
      return Telemetry.StartTimedEvent("Utilities/Sync", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Type",
          str
        }
      });
    }

    public static ITimedTelemetryEvent TimeSyncPoll() => Telemetry.StartTimedEvent("Utilities/Sync/Poll cloud status");

    public static ITimedTelemetryEvent TimeWorkoutSync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      GuidedWorkoutSyncMode mode = GuidedWorkoutSyncMode.SyncPlan)
    {
      return Telemetry.StartTimedEvent("Fitness/Guided Workouts/Sync", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Workout Plan ID",
          workoutPlanId
        },
        {
          "Workout Index",
          workoutIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "Week ID",
          weekId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "Day ID",
          dayId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "Workout Plan Instance ID",
          workoutPlanInstanceId.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "Mode",
          CommonTelemetry.ToString(mode)
        }
      });
    }

    private static string ToString(GuidedWorkoutSyncMode mode)
    {
      switch (mode)
      {
        case GuidedWorkoutSyncMode.SyncPlan:
          return "Sync Plan";
        case GuidedWorkoutSyncMode.ReplacePlan:
          return "Replace Plan";
        case GuidedWorkoutSyncMode.SyncOutOfPlan:
          return "Sync out of Plan";
        default:
          DebugUtilities.Fail("Unrecognized mode: {0}", (object) mode);
          return mode.ToString();
      }
    }

    public static ITimedTelemetryEvent TimeFirmwareUpdate() => Telemetry.StartTimedEvent("App/Firmware/Update");

    public static ITimedTelemetryEvent TimeFirmwareUpdateDownload(bool inOobe) => Telemetry.StartTimedEvent("App/Firmware/Downloading update", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "In OOBE",
        inOobe.ToString()
      }
    });

    public static ITimedTelemetryEvent TimeFirmwareUpdateSendToBand(bool inOobe) => Telemetry.StartTimedEvent("App/Firmware/Sending to band", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "In OOBE",
        inOobe.ToString()
      }
    });

    public static ITimedTelemetryEvent TimeFirmwareUpdateRebootBand(bool inOobe) => Telemetry.StartTimedEvent("App/Firmware/Rebooting band", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "In OOBE",
        inOobe.ToString()
      }
    });
  }
}
