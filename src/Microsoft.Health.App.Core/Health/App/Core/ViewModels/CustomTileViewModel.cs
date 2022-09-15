// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.CustomTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class CustomTileViewModel : MetricTileViewModel
  {
    public CustomTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
    }
  }
}
