// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IEnvironmentService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Globalization;

namespace Microsoft.Health.App.Core.Services
{
  public interface IEnvironmentService : IConfigurationState
  {
    bool IsEmulated { get; }

    bool IsPublicRelease { get; }

    bool IsDeveloperBuild { get; }

    bool IsCortanaUpdateRequired { get; }

    bool IsBluetoothBlocked { get; }

    bool SuspendApplicationWhenIdle { get; set; }

    string OperatingSystemName { get; }

    Version OperatingSystemVersion { get; }

    string Manufacturer { get; }

    string HardwareVersion { get; }

    string ProductName { get; }

    Version ApplicationVersion { get; }

    string ApplicationNameQualifier { get; }

    Version UserAgentApplicationVersion { get; }

    CultureInfo CurrentCulture { get; }

    string BuildFlavor { get; }

    ulong AppMemoryUsage { get; }

    ulong AppMemoryUsageLimit { get; }

    string BatteryStatus { get; }

    string PhoneId { get; }

    bool CheckForHockeyAppUpdates { get; }

    bool SupportsSignOut { get; }

    OperatingSystemType OperatingSystemType { get; }

    string ApplicationSessionId { get; }

    LogicalScreenSize LogicalScreenSize { get; }

    PixelScreenSize PixelScreenSize { get; }

    string DeviceFamilyShort { get; }

    bool SupportsBackgroundSyncCancellation { get; }

    bool IsUwpAppOnDesktop();
  }
}
