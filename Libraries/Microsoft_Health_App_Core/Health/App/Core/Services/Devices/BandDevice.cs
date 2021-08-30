// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Devices.BandDevice
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Sync;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Devices
{
  public sealed class BandDevice : IDevice
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Devices\\BandDevice.cs");
    private readonly IUserProfileService userProfileService;
    private readonly ISyncService syncService;
    private readonly IBandConnectionFactory cargoConnectionFactory;

    public BandDevice(
      IUserProfileService userProfileService,
      ISyncService syncService,
      IBandConnectionFactory cargoConnectionFactory)
    {
      Assert.ParamIsNotNull((object) userProfileService, nameof (userProfileService));
      Assert.ParamIsNotNull((object) syncService, nameof (syncService));
      Assert.ParamIsNotNull((object) cargoConnectionFactory, nameof (cargoConnectionFactory));
      this.userProfileService = userProfileService;
      this.syncService = syncService;
      this.cargoConnectionFactory = cargoConnectionFactory;
    }

    public DeviceType DeviceType => DeviceType.Band;

    public bool IsBandRegistered => this.userProfileService.IsBandRegistered;

    public Task<DateTimeOffset?> GetLastSyncTimeAsync(CancellationToken token)
    {
      DateTimeOffset lastSyncTime = this.syncService.LastSyncTime;
      return lastSyncTime == DateTimeOffset.MinValue ? Task.FromResult<DateTimeOffset?>(new DateTimeOffset?()) : Task.FromResult<DateTimeOffset?>(new DateTimeOffset?(lastSyncTime));
    }

    public async Task<bool> CanSyncAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      try
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
          return await cargoConnection.GetPrimaryPairedBandAsync(cancellationToken).ConfigureAwait(false) != null || this.IsBandRegistered;
      }
      catch (BluetoothOffException ex)
      {
        return true;
      }
      catch
      {
        throw;
      }
    }

    public Task SyncDeviceAsync(
      SyncType syncType,
      CancellationToken cancellationToken,
      bool ignoreIfUnable = false,
      IProgress<DeviceSyncProgress> progress = null)
    {
      if (!this.IsBandRegistered)
        return (Task) Task.FromResult<bool>(false);
      return ignoreIfUnable && !this.CheckIfAbleToSync(cancellationToken) ? (Task) Task.FromResult<bool>(false) : this.syncService.SyncAsync(syncType, cancellationToken, progress);
    }

    private bool CheckIfAbleToSync(CancellationToken cancellationToken)
    {
      bool isSingleDevice = false;
      try
      {
        Task.Run((Func<Task>) (async () =>
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
          {
            cancellationToken.ThrowIfCancellationRequested();
            // ISSUE: variable of a compiler-generated type
            BandDevice.\u003C\u003Ec__DisplayClass12_0 cDisplayClass120;
            // ISSUE: reference to a compiler-generated field
            int num5 = cDisplayClass120.isSingleDevice ? 1 : 0;
            int num6 = await cargoConnection.TryCheckConnectionWorkingAsync() ? 1 : 0;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass120.isSingleDevice = num6 != 0;
            cDisplayClass120 = (BandDevice.\u003C\u003Ec__DisplayClass12_0) null;
          }
        }), cancellationToken).Wait(cancellationToken);
      }
      catch (Exception ex)
      {
        BandDevice.Logger.Warn(ex, "<ERROR> checking for single device failed, will not start foreground sync");
      }
      return isSingleDevice;
    }
  }
}
