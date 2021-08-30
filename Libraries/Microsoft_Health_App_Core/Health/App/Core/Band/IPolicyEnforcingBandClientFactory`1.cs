// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.IPolicyEnforcingBandClientFactory`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public interface IPolicyEnforcingBandClientFactory<T> : 
    IPolicyEnforcingBandClientFactory,
    IBandClientFactory<T>
  {
    Task<T> CreateClientAsync(
      BandClientType type,
      CancellationToken cancellationToken,
      IBandInfo bandInfo = null,
      bool forceRenew = false,
      bool allowUI = false,
      bool ignoreCorruptFirmware = false);
  }
}
