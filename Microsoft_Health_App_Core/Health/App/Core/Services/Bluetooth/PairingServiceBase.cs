// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Bluetooth.PairingServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Bluetooth
{
  public abstract class PairingServiceBase : IPairingService
  {
    private const int PairedDeviceWaitTime = 20000;
    private const int PairedDeviceDelayTime = 500;
    private const int ConnectDeviceWaitTime = 20000;
    private const int ConnectDeviceDelayTime = 2000;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Bluetooth\\PairingServiceBase.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IAddBandService addBandService;

    public PairingServiceBase(
      IBandConnectionFactory cargoConnectionFactory,
      IAddBandService addBandService)
    {
      Assert.ParamIsNotNull((object) cargoConnectionFactory, nameof (cargoConnectionFactory));
      Assert.ParamIsNotNull((object) addBandService, nameof (addBandService));
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.addBandService = addBandService;
    }

    public async Task<bool> PairAsync(
      IBandDevice device,
      BluetoothConnectionState state,
      CancellationToken cancellationToken)
    {
      Assert.ParamIsNotNull((object) device, nameof (device));
      try
      {
        if (state == BluetoothConnectionState.Paired || state == BluetoothConnectionState.Connected)
        {
          try
          {
            await this.CompletePairingAsync(device, cancellationToken);
            return true;
          }
          catch (Exception ex) when (!(ex is BandNotInOobeException))
          {
            await this.DisconnectAsync(device, state, cancellationToken);
          }
        }
        if (await this.RequestPairingAsync(device, state, cancellationToken))
        {
          await this.CompletePairingAsync(device, cancellationToken);
          return true;
        }
      }
      catch (Exception ex)
      {
        PairingServiceBase.Logger.Error((object) "Error occured when attempting to pair.", ex);
        throw;
      }
      return false;
    }

    protected abstract Task<bool> RequestPairingAsync(
      IBandDevice device,
      BluetoothConnectionState state,
      CancellationToken cancellationToken);

    protected virtual Task DisconnectAsync(
      IBandDevice device,
      BluetoothConnectionState state,
      CancellationToken cancellationToken)
    {
      return (Task) Task.FromResult<object>((object) null);
    }

    protected virtual async Task CompletePairingAsync(
      IBandDevice device,
      CancellationToken cancellationToken)
    {
      Assert.ParamIsNotNull((object) device, nameof (device));
      IBandInfo bandInfo = await this.GetBandInfoFromDeviceAsync(device, cancellationToken);
      if (!await this.IsBandInOobeModeAsync(bandInfo, cancellationToken))
        throw new BandNotInOobeException();
      await this.addBandService.SetBandAsync(bandInfo, cancellationToken);
    }

    protected async Task LogAndExecuteTaskAsync(string message, Task task)
    {
      Assert.ParamIsNotNullOrEmpty(message, nameof (message));
      Assert.ParamIsNotNull((object) task, nameof (task));
      PairingServiceBase.Logger.Info((object) string.Format("<BEGIN> {0}", new object[1]
      {
        (object) message
      }));
      await task;
      PairingServiceBase.Logger.Info((object) string.Format("<END> {0}", new object[1]
      {
        (object) message
      }));
    }

    protected async Task<T> LogAndExecuteTaskAsync<T>(string message, Task<T> task)
    {
      Assert.ParamIsNotNullOrEmpty(message, nameof (message));
      Assert.ParamIsNotNull((object) task, nameof (task));
      PairingServiceBase.Logger.Info((object) string.Format("<BEGIN> {0}", new object[1]
      {
        (object) message
      }));
      T obj;
      try
      {
        obj = await task;
      }
      finally
      {
        PairingServiceBase.Logger.Info((object) string.Format("<END> {0}", new object[1]
        {
          (object) message
        }));
      }
      return obj;
    }

    private async Task<IBandInfo> GetBandInfoFromDeviceAsync(
      IBandDevice device,
      CancellationToken cancellationToken)
    {
      Assert.ParamIsNotNull((object) device, nameof (device));
      IBandInfo bandInfo1;
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        Stopwatch stopwatch = Stopwatch.StartNew();
        IBandInfo bandInfo2;
        while (true)
        {
          if (stopwatch.ElapsedMilliseconds < 20000L)
          {
            bandInfo2 = ((IEnumerable<IBandInfo>) await cargoConnection.GetPairedBandsAsync(cancellationToken)).FirstOrDefault<IBandInfo>((Func<IBandInfo, bool>) (b => b.Name == device.Name));
            if (bandInfo2 == null)
              await Task.Delay(500);
            else
              break;
          }
          else
            goto label_7;
        }
        bandInfo1 = bandInfo2;
        goto label_11;
label_7:
        throw new BluetoothException("The selected device is not a supported band or has not been cached by the system yet.");
      }
label_11:
      return bandInfo1;
    }

    private async Task<bool> IsBandInOobeModeAsync(IBandInfo bandInfo, CancellationToken token)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        await this.addBandService.SetBandAsync(bandInfo, token);
        object obj = (object) null;
        int num = 0;
        bool flag;
        try
        {
          Stopwatch stopwatch = Stopwatch.StartNew();
          while (true)
          {
            try
            {
              flag = !await cargoConnection.GetBandOobeCompletedAsync(token);
              break;
            }
            catch
            {
              if (stopwatch.ElapsedMilliseconds > 20000L)
                throw;
            }
            await Task.Delay(2000);
          }
          num = 1;
        }
        catch (object ex)
        {
          obj = ex;
        }
        await this.addBandService.SetBandAsync((IBandInfo) null, token);
        object obj1 = obj;
        if (obj1 != null)
        {
          if (!(obj1 is Exception source4))
            throw obj1;
          ExceptionDispatchInfo.Capture(source4).Throw();
        }
        if (num == 1)
          return flag;
        obj = (object) null;
      }
      bool flag1;
      return flag1;
    }
  }
}
