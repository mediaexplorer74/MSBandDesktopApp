// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.RemoteDynamicConfigurationFileStore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Http;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.DynamicConfiguration;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  internal sealed class RemoteDynamicConfigurationFileStore : DynamicConfigurationFileStoreBase
  {
    public static readonly ConfigurationValue<bool> IsRemoteFileStoreEnabled = ConfigurationValue.CreateBoolean("DynamicConfigurationService", nameof (IsRemoteFileStoreEnabled), true);
    public static readonly ConfigurationValue<string> LastFetchedTimestamp = ConfigurationValue.Create("DynamicConfigurationService", nameof (LastFetchedTimestamp), DateTimeOffset.MinValue.ToString("o"));
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Configuration\\Dynamic\\RemoteDynamicConfigurationFileStore.cs");
    private readonly IDynamicConfigurationClient dynamicConfigurationClient;
    private readonly IHttpMessageHandlerFactory handlerFactory;
    private readonly IConfigurationService configurationService;
    private readonly IDateTimeService dateTimeService;

    public RemoteDynamicConfigurationFileStore(
      IDynamicConfigurationClient dynamicConfigurationClient,
      IHttpMessageHandlerFactory handlerFactory,
      IConfigurationService configurationService,
      IDateTimeService dateTimeService)
      : base(RemoteDynamicConfigurationFileStore.Logger)
    {
      Assert.ParamIsNotNull((object) dynamicConfigurationClient, nameof (dynamicConfigurationClient));
      Assert.ParamIsNotNull((object) handlerFactory, nameof (handlerFactory));
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      this.dynamicConfigurationClient = dynamicConfigurationClient;
      this.handlerFactory = handlerFactory;
      this.configurationService = configurationService;
      this.dateTimeService = dateTimeService;
    }

    protected override async Task<Stream> GetConfigurationFileStreamAsync(
      RegionInfo region,
      CancellationToken token)
    {
      if (!this.configurationService.GetValue<bool>(RemoteDynamicConfigurationFileStore.IsRemoteFileStoreEnabled))
      {
        RemoteDynamicConfigurationFileStore.Logger.Warn((object) "The remote configuration file store has been disabled.");
        return (Stream) null;
      }
      RemoteDynamicConfigurationFileStore.Logger.Info("Getting configuration file metadata from the Cloud for region '{0}'...", (object) region);
      DateTimeOffset lastFetched = DateTimeOffset.MinValue;
      DateTimeOffset.TryParse(this.configurationService.GetValue<string>(RemoteDynamicConfigurationFileStore.LastFetchedTimestamp), out lastFetched);
      this.configurationService.SetValue<string>((GenericConfigurationValue<string>) RemoteDynamicConfigurationFileStore.LastFetchedTimestamp, this.dateTimeService.Now.ToString("o"));
      DynamicConfigurationFileMetadata metadata = await this.dynamicConfigurationClient.GetConfigurationFileMetadataAsync(region, token).ConfigureAwait(false);
      if (metadata == null || metadata.PrimaryUrl == (Uri) null && metadata.MirrorUrl == (Uri) null)
      {
        RemoteDynamicConfigurationFileStore.Logger.Info((object) "No valid configuration file metadata was returned.");
        return (Stream) null;
      }
      HttpClient httpClient = new HttpClient(this.handlerFactory.CreateHandler());
      httpClient.DefaultRequestHeaders.IfModifiedSince = new DateTimeOffset?(lastFetched);
      Stream stream = (Stream) null;
      try
      {
        token.ThrowIfCancellationRequested();
        RemoteDynamicConfigurationFileStore.Logger.Info("Getting configuration file from primary URL '{0}'...", (object) metadata.PrimaryUrl);
        HttpResponseMessage async = await httpClient.GetAsync(metadata.PrimaryUrl, HttpCompletionOption.ResponseHeadersRead);
        if (async.StatusCode == HttpStatusCode.NotModified)
        {
          RemoteDynamicConfigurationFileStore.Logger.Info((object) "Remote configuration file has not been modified since last checked. Returning null since we already have latest.");
          return (Stream) null;
        }
        stream = await RemoteDynamicConfigurationFileStore.CreateSeekableStreamAsync(await async.Content.ReadAsStreamAsync().ConfigureAwait(false)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteDynamicConfigurationFileStore.Logger.Warn((object) "Unable to get configuration file from primary URL due to exception.", ex);
      }
      if (stream == null || stream.Length != metadata.SizeInBytes)
      {
        token.ThrowIfCancellationRequested();
        if (stream != null)
          this.LogSizeMismatch(metadata.SizeInBytes, stream.Length);
        RemoteDynamicConfigurationFileStore.Logger.Warn("Unable to get configuration file from primary URL; getting configuration file from mirror URL '{0}'...", (object) metadata.MirrorUrl);
        ConfiguredTaskAwaitable<Stream> configuredTaskAwaitable = httpClient.GetStreamAsync(metadata.MirrorUrl).ConfigureAwait(false);
        configuredTaskAwaitable = RemoteDynamicConfigurationFileStore.CreateSeekableStreamAsync(await configuredTaskAwaitable).ConfigureAwait(false);
        stream = await configuredTaskAwaitable;
        if (stream == null || stream.Length != metadata.SizeInBytes)
        {
          if (stream != null)
            this.LogSizeMismatch(metadata.SizeInBytes, stream.Length);
          RemoteDynamicConfigurationFileStore.Logger.Warn((object) "Unable to get configuration file from mirror URL.");
          return (Stream) null;
        }
      }
      return stream;
    }

    private void LogSizeMismatch(long expected, long actual) => RemoteDynamicConfigurationFileStore.Logger.Debug((object) string.Format("Stream length does not match metadata. Expected {0}, got {1}.", new object[2]
    {
      (object) expected,
      (object) actual
    }));

    private static async Task<Stream> CreateSeekableStreamAsync(Stream stream)
    {
      if (stream == null || stream.CanSeek)
        return stream;
      using (stream)
      {
        MemoryStream memoryStream = new MemoryStream();
        try
        {
          await stream.CopyToAsync((Stream) memoryStream).ConfigureAwait(false);
          await memoryStream.FlushAsync().ConfigureAwait(false);
          memoryStream.Seek(0L, SeekOrigin.Begin);
        }
        catch
        {
          memoryStream.Dispose();
          throw;
        }
        return (Stream) memoryStream;
      }
    }
  }
}
