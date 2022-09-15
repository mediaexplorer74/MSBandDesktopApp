// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Run.RunFullMapViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.ViewModels.Run
{
  [PageTaxonomy(new string[] {"Fitness", "Run", "Expanded Map"})]
  public class RunFullMapViewModel : FullMapViewModelBase<RunEvent>
  {
    public RunFullMapViewModel(
      ISmoothNavService smoothNavService,
      INetworkService networkService,
      IRouteBasedExerciseEventProvider<RunEvent> eventProvider)
      : base(networkService, smoothNavService, eventProvider)
    {
    }
  }
}
