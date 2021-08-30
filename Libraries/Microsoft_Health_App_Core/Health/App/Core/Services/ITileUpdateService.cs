// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ITileUpdateService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services.Debugging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface ITileUpdateService
  {
    Task UpdateTilesAsync(
      CancellationToken cancellationToken,
      SyncDebugResult syncDebugResult = null,
      ITimedTelemetryEvent timedTelemetryEvent = null);

    Task SyncWebTilesAsync(bool forceSync, CancellationToken cancellationToken);

    Task SyncWebTileAsync(Guid tileId, CancellationToken cancellationToken);

    Task UpdateWeatherAsync(CancellationToken cancellationToken);

    Task UpdateFinanceAsync(CancellationToken cancellationToken);

    Task UpdateCalendarAsync(CancellationToken cancellationToken);

    Task UpdateStarbucksAsync(CancellationToken cancellationToken);
  }
}
