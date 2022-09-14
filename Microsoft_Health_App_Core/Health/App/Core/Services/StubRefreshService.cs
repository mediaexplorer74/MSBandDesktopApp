// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.StubRefreshService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class StubRefreshService : IRefreshService
  {
    public void Refresh(CancellationToken cancellationToken)
    {
    }

    public Task RefreshAsync(CancellationToken cancellationToken) => (Task) Task.FromResult<object>((object) null);

    public void Subscribe(object subscriber, Func<CancellationToken, Task> callOnRefresh)
    {
    }

    public void Unsubscribe(object subscriber)
    {
    }
  }
}
