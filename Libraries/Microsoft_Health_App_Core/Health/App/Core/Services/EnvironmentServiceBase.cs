// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.EnvironmentServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public abstract class EnvironmentServiceBase : IEnvironmentService, IConfigurationState
  {
    private static readonly Version[] DeveloperBuildVersions = new Version[2]
    {
      new Version("1.0.0.0"),
      new Version("2.0.0.0")
    };
    private string applicationSessionId;

    public virtual bool IsDeveloperBuild => ((IEnumerable<Version>) EnvironmentServiceBase.DeveloperBuildVersions).Any<Version>((Func<Version, bool>) (v => v == this.ApplicationVersion));

    public virtual bool CheckForHockeyAppUpdates => false;

    public virtual bool SupportsSignOut { get; } = true;

    public virtual CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

    public virtual string BuildFlavor => "Release";

    public virtual bool IsPublicRelease => EnvironmentUtilities.IsPublicRelease;

    public virtual bool IsCortanaUpdateRequired => false;

    public virtual bool IsBluetoothBlocked => false;

    public virtual Version UserAgentApplicationVersion => this.ApplicationVersion;

    public virtual string ApplicationNameQualifier => string.Empty;

    public virtual bool SuspendApplicationWhenIdle { get; set; }

    public abstract Version ApplicationVersion { get; }

    public abstract string HardwareVersion { get; }

    public abstract string Manufacturer { get; }

    public abstract string OperatingSystemName { get; }

    public abstract Version OperatingSystemVersion { get; }

    public abstract string ProductName { get; }

    public abstract ulong AppMemoryUsage { get; }

    public abstract ulong AppMemoryUsageLimit { get; }

    public abstract string BatteryStatus { get; }

    public abstract bool IsEmulated { get; }

    public abstract string PhoneId { get; }

    public abstract OperatingSystemType OperatingSystemType { get; }

    public abstract Task ResetStateAsync(CancellationToken token);

    public string ApplicationSessionId
    {
      get
      {
        if (string.IsNullOrEmpty(this.applicationSessionId))
          this.applicationSessionId = Guid.NewGuid().ToString();
        return this.applicationSessionId;
      }
    }

    public abstract LogicalScreenSize LogicalScreenSize { get; }

    public abstract PixelScreenSize PixelScreenSize { get; }

    public abstract bool SupportsBackgroundSyncCancellation { get; }

    public virtual bool IsUwpAppOnDesktop() => false;

    public virtual string DeviceFamilyShort => "Mobile";
  }
}
