// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DesktopTelemetry
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Health.App.Core.Diagnostics;
using System.Collections.Generic;

namespace DesktopSyncApp
{
  public static class DesktopTelemetry
  {
    private const string Engineering = "885eda77-af95-47e2-8225-86e8d04e2a3e";
    private const string Dogfooding = "90cb1ae5-8070-4e93-bcd6-5c3db36dd2b9";
    private const string Public = "dc6b7b4c-45ff-4e96-a80d-d8855a223fed";

    public static string GetInstrumentationKey()
    {
      string str = "Release_External";
      if (str == "Main" || str == "Stabilization")
        return "885eda77-af95-47e2-8225-86e8d04e2a3e";
      return str == "Release" ? "90cb1ae5-8070-4e93-bcd6-5c3db36dd2b9" : "dc6b7b4c-45ff-4e96-a80d-d8855a223fed";
    }

    public static void LogError(ErrorInfo error) => Telemetry.LogException(error.Exception, (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Description",
        error.Description
      },
      {
        "Function",
        error.Function
      }
    }, (IDictionary<string, double>) null);

    public static class Events
    {
      public const string BandSync = "Utilities/Sync";
      public const string Dashboard = "Visual/Navigation/Dashboard";
      public const string StartOnLogin = "Settings/App/AutoLaunch";
      public const string AutoSync = "Settings/App/AutoSync";
      public const string ProfileUpdate = "ProfileUpdated";
      public const string FWUpdateBegin = "Utilities/FirmwareUpdate/Begin";
      public const string FWUpdateDownload = "Utilities/FirmwareUpdate/Download";
      public const string FWUpdateSendToBand = "Utilities/FirmwareUpdate/Send to band";
      public const string FWUpdateRebootBand = "Utilities/FirmwareUpdate/Reboot band";
      public const string FWUpdateApplyToBand = "Utilities/FirmwareUpdate/Apply Update";
      public const string FWUpdateTwoUp = "2UP";
      public const string InstallUWPLink = "InstallUWPLink";
      public const string UninstallSyncClientLink = "UninstallSyncClientLink";
    }

    public static class PageViews
    {
      public const string Profile = "Settings/User/Profile";
      public const string AppSettings = "Settings/Application";
      public const string About = "Settings/Application/About";
      public const string PersonalizeBand = "Settings/Band/Theme Chooser";
      public const string TileManagement = "Settings/Band/Manage Tiles";
    }
  }
}
