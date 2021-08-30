// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mocks.MockAddBandService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.WindowsPhone.ViewModels.AddBand;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Mocks
{
  public class MockAddBandService : IAddBandService
  {
    public MockAddBandService(ISmoothNavService smoothNavService) => this.SmoothNavService = smoothNavService;

    protected ISmoothNavService SmoothNavService { get; private set; }

    public virtual Task ExitAsync(CancellationToken cancellationToken)
    {
      this.SmoothNavService.GoHome();
      this.SmoothNavService.ClearBackStack();
      return (Task) Task.FromResult<object>((object) null);
    }

    public virtual Task NextAsync(
      CancellationToken cancellationToken,
      IProgress<InitializationProgress> progressListener = null)
    {
      return (Task) Task.FromResult<object>((object) null);
    }

    public Task SetBandAsync(IBandInfo bandInfo, CancellationToken cancellationToken) => (Task) Task.FromResult<object>((object) null);

    public Task SetBandScreenAsync(BandScreen screen, CancellationToken cancellationToken) => (Task) Task.FromResult<object>((object) null);

    public Task<bool> SkipAsync(CancellationToken cancellationToken) => Task.FromResult<bool>(true);

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
      this.SmoothNavService.Navigate(typeof (AddBandPairingViewModel), action: NavigationStackAction.RemovePrevious);
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
