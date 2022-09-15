// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.RefreshablePageViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class RefreshablePageViewModelBase : PageViewModelBase
  {
    private readonly IDispatchService dispatchService;
    private readonly IRefreshService refreshService;

    protected RefreshablePageViewModelBase(
      IRefreshService refreshService,
      IDispatchService dispatchService,
      INetworkService networkService)
      : base(networkService)
    {
      this.refreshService = refreshService;
      this.dispatchService = dispatchService;
      this.refreshService.Subscribe((object) this, (Func<CancellationToken, Task>) (async cancellationToken =>
      {
        if (this.IsActive)
        {
          cancellationToken.ThrowIfCancellationRequested();
          await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () => await this.RefreshAsync()));
        }
        else
        {
          cancellationToken.ThrowIfCancellationRequested();
          this.RefreshPending = true;
        }
      }));
    }

    protected bool RefreshPending { get; set; }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      if (!this.RefreshPending)
        return;
      this.Refresh();
      this.RefreshPending = false;
    }
  }
}
