// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.MicrosoftBandUserAgentService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.Services;
using System;

namespace Microsoft.Health.App.Core.Services
{
  public class MicrosoftBandUserAgentService : IMicrosoftBandUserAgentService, IUserAgentService
  {
    public const string ApplicationName = "KApp";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\MicrosoftBandUserAgentService.cs");
    private readonly IConfigProvider configProvider;
    private readonly string hardwareVersion;
    private readonly IEnvironmentService applicationEnvironmentService;
    private readonly string manufacturer;
    private readonly Version operatingSystemVersion;
    private readonly Version packageVersion;
    private readonly string productName;

    public MicrosoftBandUserAgentService(
      IConfigProvider configProvider,
      IEnvironmentService applicationEnvironmentService)
    {
      MicrosoftBandUserAgentService.Logger.Debug((object) "Instantiating the Microsoft Band User Agent Service...");
      this.configProvider = configProvider;
      this.applicationEnvironmentService = applicationEnvironmentService;
      this.operatingSystemVersion = applicationEnvironmentService.OperatingSystemVersion;
      this.manufacturer = applicationEnvironmentService.Manufacturer;
      this.hardwareVersion = applicationEnvironmentService.HardwareVersion;
      this.productName = applicationEnvironmentService.ProductName;
      this.packageVersion = applicationEnvironmentService.UserAgentApplicationVersion;
      MicrosoftBandUserAgentService.Logger.Debug((object) "Instantiating the Microsoft Band User Agent Service... (complete)");
    }

    private Version MicrosoftBandFirmwareVersion
    {
      get => this.configProvider.GetVersion("CargoUserAgentService.MicrosoftBandFirmwareVersion", (Version) null);
      set => this.configProvider.SetVersion("CargoUserAgentService.MicrosoftBandFirmwareVersion", value);
    }

    public string UserAgent
    {
      get
      {
        string str = string.Empty;
        if (this.MicrosoftBandFirmwareVersion != (Version) null)
          str = string.Format(" Cargo/{0}", new object[1]
          {
            (object) this.MicrosoftBandFirmwareVersion
          });
        return string.Format("{0}/{1} ({2}/{3}; {4} {5}/{6}; {7}){8}", (object) "KApp", (object) this.packageVersion, (object) this.applicationEnvironmentService.OperatingSystemName, (object) this.operatingSystemVersion, (object) this.manufacturer, (object) this.productName, (object) this.hardwareVersion, (object) this.applicationEnvironmentService.CurrentCulture.Name, (object) str);
      }
    }

    public void SetFirmwareVersion(Version firmwareVersion) => this.MicrosoftBandFirmwareVersion = firmwareVersion;
  }
}
