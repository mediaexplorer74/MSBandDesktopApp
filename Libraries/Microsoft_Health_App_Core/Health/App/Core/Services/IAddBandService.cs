// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IAddBandService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Health.App.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IAddBandService
  {
    Task StartAsync(CancellationToken cancellationToken);

    Task NextAsync(
      CancellationToken cancellationToken,
      IProgress<InitializationProgress> progressListener = null);

    Task<bool> SkipAsync(CancellationToken cancellationToken);

    Task ExitAsync(CancellationToken cancellationToken);

    Task SetBandAsync(IBandInfo bandInfo, CancellationToken cancellationToken);

    Task SetBandScreenAsync(BandScreen screen, CancellationToken cancellationToken);
  }
}
