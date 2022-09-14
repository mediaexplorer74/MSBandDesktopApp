// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Oobe.OobeMinimumAgeRequirementViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;

namespace Microsoft.Health.App.Core.ViewModels.Oobe
{
  [PageTaxonomy(new string[] {"App", "OOBE", "Minimum age check"})]
  public class OobeMinimumAgeRequirementViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Oobe\\OobeMinimumAgeRequirementViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IOobeService oobeService;

    public OobeMinimumAgeRequirementViewModel(
      INetworkService networkService,
      IErrorHandlingService errorHandlingService,
      IOobeService oobeService)
      : base(networkService)
    {
      this.oobeService = oobeService;
      this.errorHandlingService = errorHandlingService;
    }
  }
}
