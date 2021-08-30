// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.BandInfoService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public class BandInfoService : IBandInfoService
  {
    private IBandInfo bandInfo;

    public Task<IBandInfo> GetBandInfoAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return Task.FromResult<IBandInfo>(this.bandInfo);
    }

    public Task SetBandInfoAsync(IBandInfo newBandInfo, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      this.bandInfo = newBandInfo;
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
