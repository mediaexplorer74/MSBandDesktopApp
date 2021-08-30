// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.PolicyEnforcingBandClientFactoryBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Bluetooth;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public abstract class PolicyEnforcingBandClientFactoryBase<T> : 
    IPolicyEnforcingBandClientFactory<T>,
    IPolicyEnforcingBandClientFactory,
    IBandClientFactory<T>
  {
    private const string SingleDevicePolicyServiceCategory = "SingleDevicePolicyManager";
    private static readonly string IsSingleDevicePolicyEnabledKey = ConfigurationValue.CreateKey("SingleDevicePolicyManager", nameof (IsSingleDevicePolicyEnabled));
    public static readonly string LastKnownUserIdKey = ConfigurationValue.CreateKey("SingleDevicePolicyManager", nameof (LastKnownUserId));
    public static readonly string LastKnownDeviceIdKey = ConfigurationValue.CreateKey("SingleDevicePolicyManager", nameof (LastKnownDeviceId));
    public static readonly string LastKnownDeviceNameKey = ConfigurationValue.CreateKey("SingleDevicePolicyManager", nameof (LastKnownDeviceName));
    private readonly IConfigProvider configProvider;
    private readonly IMsaTokenProvider msaTokenSource;
    private readonly IBluetoothService bluetoothService;
    private readonly IBandInfoService bandInfoService;
    private readonly IBandClientFactory<T> cargoClientFactory;
    private readonly IConnectionInfoProvider connectionInfoProvider;
    private readonly IMicrosoftBandUserAgentService microsoftBandUserAgentService;
    private readonly IDebugReporterService debugReporterService;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Band\\EnforcedBandClientFactoryBase.cs");

    public PolicyEnforcingBandClientFactoryBase(
      IMsaTokenProvider msaTokenSource,
      IConfigProvider configProvider,
      IBluetoothService bluetoothService,
      IBandInfoService bandInfoService,
      IBandClientFactory<T> cargoClientFactory,
      IConnectionInfoProvider connectionInfoProvider,
      IMicrosoftBandUserAgentService microsoftBandUserAgentService,
      IDebugReporterService debugReporterService)
    {
      Assert.ParamIsNotNull((object) msaTokenSource, nameof (msaTokenSource));
      Assert.ParamIsNotNull((object) configProvider, nameof (configProvider));
      Assert.ParamIsNotNull((object) cargoClientFactory, nameof (cargoClientFactory));
      Assert.ParamIsNotNull((object) bandInfoService, nameof (bandInfoService));
      Assert.ParamIsNotNull((object) cargoClientFactory, nameof (cargoClientFactory));
      Assert.ParamIsNotNull((object) connectionInfoProvider, nameof (connectionInfoProvider));
      Assert.ParamIsNotNull((object) microsoftBandUserAgentService, nameof (microsoftBandUserAgentService));
      this.configProvider = configProvider;
      this.msaTokenSource = msaTokenSource;
      this.bandInfoService = bandInfoService;
      this.bluetoothService = bluetoothService;
      this.cargoClientFactory = cargoClientFactory;
      this.connectionInfoProvider = connectionInfoProvider;
      this.microsoftBandUserAgentService = microsoftBandUserAgentService;
      this.debugReporterService = debugReporterService;
    }

    public bool IsSingleDevicePolicyEnabled
    {
      get => this.configProvider.Get<bool>(PolicyEnforcingBandClientFactoryBase<T>.IsSingleDevicePolicyEnabledKey, true);
      set => this.configProvider.Set<bool>(PolicyEnforcingBandClientFactoryBase<T>.IsSingleDevicePolicyEnabledKey, value);
    }

    public Guid LastKnownUserId
    {
      get => this.configProvider.Get<Guid>(PolicyEnforcingBandClientFactoryBase<T>.LastKnownUserIdKey, Guid.Empty);
      set => this.configProvider.Set<Guid>(PolicyEnforcingBandClientFactoryBase<T>.LastKnownUserIdKey, value);
    }

    public Guid LastKnownDeviceId
    {
      get => this.configProvider.Get<Guid>(PolicyEnforcingBandClientFactoryBase<T>.LastKnownDeviceIdKey, Guid.Empty);
      set => this.configProvider.Set<Guid>(PolicyEnforcingBandClientFactoryBase<T>.LastKnownDeviceIdKey, value);
    }

    public string LastKnownDeviceName
    {
      get => this.configProvider.Get<string>(PolicyEnforcingBandClientFactoryBase<T>.LastKnownDeviceNameKey, (string) null);
      set => this.configProvider.Set<string>(PolicyEnforcingBandClientFactoryBase<T>.LastKnownDeviceNameKey, value);
    }

    public void Invalidate() => this.CachePolicyInfo(Guid.Empty, Guid.Empty, (string) null);

    public async Task<T> EnforcePolicyAsync(
      CancellationToken cancellationToken,
      HealthCloudConnectionInfo connectionInfo,
      ServiceInfo serviceInfo,
      bool useCloudClient,
      bool ignoreCorruptFirmware,
      bool forceCloudProfile = false)
    {
      using (ITimedTelemetryEvent sdeTimer = ApplicationTelemetry.TimeSingleDeviceEnforcementCheck())
      {
        Stopwatch enforcePolicy = Stopwatch.StartNew();
        try
        {
          PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<START> Single Device Policy");
          cancellationToken.ThrowIfCancellationRequested();
          IBandInfo[] pairedBands = await this.bluetoothService.GetPairedBandsAsync(cancellationToken);
          if (pairedBands == null || pairedBands.Length == 0)
          {
            PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : No devices are paired.");
            throw new SingleDevicePolicyException(SingleDevicePolicyResult.NoPairedDevices);
          }
          Guid userId = this.LastKnownUserId;
          Guid deviceId = this.LastKnownDeviceId;
          string deviceName = this.LastKnownDeviceName;
          if (((userId == Guid.Empty ? 1 : (deviceId == Guid.Empty ? 1 : 0)) | (forceCloudProfile ? 1 : 0)) != 0)
          {
            IUserProfile userProfileAsync = await this.GetUserProfileAsync(await this.CreateCloudOnlyClientAsync(Guid.Empty.ToString(), serviceInfo), cancellationToken);
            userId = userProfileAsync.UserID;
            deviceId = userProfileAsync.ApplicationSettings.PairedDeviceId;
          }
          cancellationToken.ThrowIfCancellationRequested();
          T linkedClient = default (T);
          if (pairedBands.Length == 1)
            return await this.EnforcePolicyAsync(userId, deviceId, pairedBands[0], cancellationToken, connectionInfo, serviceInfo, useCloudClient, forceCloudProfile, ignoreCorruptFirmware);
          List<IBandInfo> matchedBands = ((IEnumerable<IBandInfo>) pairedBands).Where<IBandInfo>((Func<IBandInfo, bool>) (p => p.Name == deviceName)).ToList<IBandInfo>();
          bool flag = matchedBands.Count != 1;
          if (!flag)
          {
            T clientIfLinkedAsync = await this.GetCargoClientIfLinkedAsync(cancellationToken, userId, deviceId, matchedBands[0], connectionInfo, serviceInfo, useCloudClient, false, ignoreCorruptFirmware);
            flag = (object) (linkedClient = clientIfLinkedAsync) == null;
          }
          if (flag)
          {
            cancellationToken.ThrowIfCancellationRequested();
            if (matchedBands.Count == 1)
              pairedBands = ((IEnumerable<IBandInfo>) pairedBands).Except<IBandInfo>((IEnumerable<IBandInfo>) matchedBands).ToArray<IBandInfo>();
            T obj = await this.EnforcePolicyAllAsync(userId, deviceId, (IEnumerable<IBandInfo>) pairedBands, cancellationToken, connectionInfo, serviceInfo, useCloudClient, forceCloudProfile);
            if ((object) (linkedClient = obj) == null)
            {
              PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : Device that is registered is not available.");
              throw new SingleDevicePolicyException(SingleDevicePolicyResult.DeviceNotAvailable);
            }
          }
          else
            this.CachePolicyInfo(userId, deviceId, matchedBands[0].Name);
          matchedBands = (List<IBandInfo>) null;
          PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : Success!");
          return linkedClient;
        }
        finally
        {
          enforcePolicy.Stop();
          if (sdeTimer != null)
          {
            this.debugReporterService.RecordEntry(new DebugReporterEntry()
            {
              SyntaxColor = "Normal",
              LineEntry = "SDE check: " + (object) enforcePolicy.ElapsedMilliseconds
            }, 2);
            this.debugReporterService.SdeCheckElapsed += enforcePolicy.ElapsedMilliseconds;
          }
        }
      }
    }

    private async Task<T> EnforcePolicyAsync(
      Guid userId,
      Guid deviceId,
      IBandInfo band,
      CancellationToken cancellationToken,
      HealthCloudConnectionInfo connectionInfo,
      ServiceInfo serviceInfo,
      bool useCloudClient,
      bool forceCloudProfile,
      bool ignoreCorruptFirmware)
    {
      T linkedClient = default (T);
      try
      {
        T clientIfLinkedAsync1 = await this.GetCargoClientIfLinkedAsync(cancellationToken, userId, deviceId, band, connectionInfo, serviceInfo, useCloudClient, true, ignoreCorruptFirmware);
        if ((object) (linkedClient = clientIfLinkedAsync1) == null)
        {
          if (!forceCloudProfile)
          {
            IUserProfile userProfileAsync = await this.GetUserProfileAsync(await this.CreateCloudOnlyClientAsync(Guid.Empty.ToString(), serviceInfo), cancellationToken);
            userId = userProfileAsync.UserID;
            deviceId = userProfileAsync.ApplicationSettings.PairedDeviceId;
            T clientIfLinkedAsync2 = await this.GetCargoClientIfLinkedAsync(cancellationToken, userId, deviceId, band, connectionInfo, serviceInfo, useCloudClient, true, ignoreCorruptFirmware);
            if ((object) (linkedClient = clientIfLinkedAsync2) == null)
            {
              PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : Device's registered user does not match user's registered device.");
              throw new SingleDevicePolicyException(SingleDevicePolicyResult.MismatchedDevices);
            }
          }
          else
          {
            PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : Device's registered user does not match user's registered device.");
            throw new SingleDevicePolicyException(SingleDevicePolicyResult.MismatchedDevices);
          }
        }
      }
      catch (BandInOobeException ex)
      {
        PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : User has at least one paired band in OOBE.");
        throw new SingleDevicePolicyException(SingleDevicePolicyResult.DeviceInOobe);
      }
      this.CachePolicyInfo(userId, deviceId, band.Name);
      return linkedClient;
    }

    private async Task<T> EnforcePolicyAllAsync(
      Guid userId,
      Guid deviceId,
      IEnumerable<IBandInfo> bands,
      CancellationToken cancellationToken,
      HealthCloudConnectionInfo connectionInfo,
      ServiceInfo serviceInfo,
      bool useCloudClient,
      bool forceCloudProfile)
    {
      if (!forceCloudProfile)
      {
        IUserProfile userProfileAsync = await this.GetUserProfileAsync(await this.CreateCloudOnlyClientAsync(Guid.Empty.ToString(), serviceInfo), cancellationToken);
        userId = userProfileAsync.UserID;
        deviceId = userProfileAsync.ApplicationSettings.PairedDeviceId;
      }
      T obj = default (T);
      foreach (IBandInfo band in bands)
      {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
          T clientIfLinkedAsync;
          if ((object) (clientIfLinkedAsync = await this.GetCargoClientIfLinkedAsync(cancellationToken, userId, deviceId, band, connectionInfo, serviceInfo, useCloudClient, false, true)) != null)
          {
            this.CachePolicyInfo(userId, deviceId, band.Name);
            return clientIfLinkedAsync;
          }
        }
        catch (BandInOobeException ex)
        {
          PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "<END> Single Device Policy : User has at least one paired band in OOBE.");
          throw new SingleDevicePolicyException(SingleDevicePolicyResult.DeviceInOobe);
        }
      }
      return default (T);
    }

    private void CachePolicyInfo(Guid userId, Guid deviceId, string deviceName)
    {
      this.LastKnownUserId = userId;
      this.LastKnownDeviceId = deviceId;
      this.LastKnownDeviceName = deviceName;
    }

    public Task<T> CreateClientAsync(IBandInfo bandInfo, ServiceInfo serviceInfo) => this.cargoClientFactory.CreateClientAsync(bandInfo, serviceInfo);

    public Task<T> CreateBandOnlyClientAsync(IBandInfo bandInfo, string userId) => this.cargoClientFactory.CreateBandOnlyClientAsync(bandInfo, userId);

    public Task<T> CreateCloudOnlyClientAsync(string bandId, ServiceInfo serviceInfo) => this.cargoClientFactory.CreateCloudOnlyClientAsync(bandId, serviceInfo);

    public async Task<T> CreateClientAsync(
      BandClientType type,
      CancellationToken cancellationToken,
      IBandInfo bandInfo = null,
      bool forceRenew = false,
      bool allowUI = false,
      bool ignoreCorruptFirmware = false)
    {
      cancellationToken.ThrowIfCancellationRequested();
      HealthCloudConnectionInfo connectionInfo = await this.connectionInfoProvider.GetConnectionInfoAsync(cancellationToken, allowUI).ConfigureAwait(false);
      ServiceInfo serviceInfo1 = this.CreateServiceInfo(connectionInfo);
      ServiceInfo serviceInfo2 = serviceInfo1;
      string str = await this.msaTokenSource.GetAsync(allowUI).ConfigureAwait(false);
      serviceInfo2.DiscoveryServiceAccessToken = str;
      serviceInfo2 = (ServiceInfo) null;
      if (type == BandClientType.CloudOnly)
        return await this.cargoClientFactory.CreateCloudOnlyClientAsync(Guid.Empty.ToString(), serviceInfo1);
      if (bandInfo == null)
        return await this.EnforcePolicyAsync(cancellationToken, connectionInfo, serviceInfo1, type == BandClientType.Both, ignoreCorruptFirmware, forceRenew).ConfigureAwait(false);
      T client;
      if (type == BandClientType.Both)
        client = await this.CreateClientAsync(bandInfo, serviceInfo1).ConfigureAwait(false);
      else
        client = await this.CreateBandOnlyClientAsync(bandInfo, connectionInfo.UserId).ConfigureAwait(false);
      return ignoreCorruptFirmware || !this.GetDeviceInTwoUpMode(client) ? client : throw new TwoUpModeException("Band is in 2UP mode indicating that firmware is corrupted.");
    }

    private async Task<T> GetCargoClientIfLinkedAsync(
      CancellationToken cancellationToken,
      Guid userId,
      Guid deviceId,
      IBandInfo band,
      HealthCloudConnectionInfo connectionInfo,
      ServiceInfo serviceInfo,
      bool useCloudClient,
      bool throwExceptions,
      bool ignoreCorruptFirmware,
      bool throwIfInOobe = true)
    {
      Exception exception = (Exception) null;
      T cargoClient = default (T);
      try
      {
        if (useCloudClient)
          cargoClient = await this.CreateClientAsync(band, serviceInfo).ConfigureAwait(false);
        else
          cargoClient = await this.CreateBandOnlyClientAsync(band, connectionInfo.UserId).ConfigureAwait(false);
        if (this.GetDeviceInTwoUpMode(cargoClient))
        {
          if (ignoreCorruptFirmware)
            return cargoClient;
          throw new TwoUpModeException("Band is in 2UP mode indicating that firmware is corrupted.");
        }
        DeviceProfileStatus profileLinkStatusAsync = await this.GetDeviceAndProfileLinkStatusAsync(cargoClient, cancellationToken, userId, deviceId);
        if (profileLinkStatusAsync.DeviceLinkStatus == LinkStatus.Matching && profileLinkStatusAsync.UserLinkStatus == LinkStatus.Matching)
          return cargoClient;
        bool flag = throwIfInOobe;
        if (flag)
          flag = !await this.GetDeviceOobeCompletedAsync(cargoClient, cancellationToken);
        if (flag)
          throw new BandInOobeException();
      }
      catch (Exception ex)
      {
        exception = ex;
        PolicyEnforcingBandClientFactoryBase<T>.Logger.Debug((object) "Failed to get linked status.", exception);
      }
      if ((object) cargoClient != null)
        this.DisposeOfInvalidClient(cargoClient);
      if (throwExceptions && exception != null || throwIfInOobe && exception is BandInOobeException || exception is TwoUpModeException)
        throw exception;
      return default (T);
    }

    protected ServiceInfo CreateServiceInfo(HealthCloudConnectionInfo connectionInfo) => new ServiceInfo()
    {
      DiscoveryServiceAddress = connectionInfo.BaseUri.AbsoluteUri,
      AccessToken = connectionInfo.PodSecurityToken,
      PodAddress = connectionInfo.PodEndpoint.AbsoluteUri,
      UserId = connectionInfo.UserId,
      FileUpdateServiceAddress = connectionInfo.FusEndpoint.AbsoluteUri,
      UserAgent = this.microsoftBandUserAgentService.UserAgent
    };

    protected abstract Task<IUserProfile> GetUserProfileAsync(
      T client,
      CancellationToken cancellationToken);

    protected abstract Task<DeviceProfileStatus> GetDeviceAndProfileLinkStatusAsync(
      T client,
      CancellationToken cancellationToken,
      Guid cloudUserId,
      Guid cloudDeviceId);

    protected abstract Task<bool> GetDeviceOobeCompletedAsync(
      T client,
      CancellationToken cancellationToken);

    protected abstract bool GetDeviceInTwoUpMode(T client);

    protected abstract void DisposeOfInvalidClient(T client);
  }
}
