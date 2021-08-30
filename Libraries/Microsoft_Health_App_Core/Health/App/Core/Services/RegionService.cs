// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.RegionService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.Cloud.Client.Services;
using System.Globalization;

namespace Microsoft.Health.App.Core.Services
{
  public sealed class RegionService : IRegionService
  {
    public static readonly ConfigurationValue<string> CurrentRegionValue = ConfigurationValue.Create(nameof (RegionService), nameof (CurrentRegion), string.Empty);
    private readonly IConfigurationService configurationService;

    public RegionService(IConfigurationService configurationService)
    {
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      this.configurationService = configurationService;
    }

    public RegionInfo CurrentRegion
    {
      get
      {
        string actualValue;
        return this.configurationService.TryGetValue<string>((GenericConfigurationValue<string>) RegionService.CurrentRegionValue, out actualValue) && !string.IsNullOrWhiteSpace(actualValue) ? new RegionInfo(actualValue) : RegionInfo.CurrentRegion;
      }
    }
  }
}
