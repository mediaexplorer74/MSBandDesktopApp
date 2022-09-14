// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Diagnostics.FiddlerLogService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client.Http;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Health.App.Core.Services.Diagnostics
{
  public class FiddlerLogService : IFiddlerLogService
  {
    private const string Protocol = "HTTP";
    private const string ProtocolVersion = "1.1";
    private const string FiddlerFileName = "app.saz";
    private const string UserAgentHeaderName = "User-Agent";
    private const string ContentEncodingHeaderName = "Content-Encoding";
    private const long MaxFileSizeBytes = 524288;
    private const int TransactionFlushInterval = 10;
    private const int TransactionScavengeInterval = 100;
    private static readonly Regex TransactionMetadataFilenameRegex = new Regex("^raw(?:/|\\\\)(\\d+)_m.xml$");
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Diagnostics\\FiddlerLogService.cs");
    private static readonly TimeSpan SessionAgeLimit = TimeSpan.FromDays(7.0);
    private static readonly TimeSpan DefaultStartTimeout = TimeSpan.FromSeconds(10.0);
    private readonly IFileSystemService fileSystemService;
    private readonly IDateTimeService dateTimeService;
    private readonly IApplicationLifecycleService applicationLifecycleService;
    private IFile zipArchiveFile;
    private Stream zipArchiveStream;
    private ZipArchive zipArchive;
    private bool started;
    private bool suspending;
    private int transactionCount;
    private AsyncLock writeLock = new AsyncLock();

    public FiddlerLogService(
      IFileSystemService fileSystemService,
      IDateTimeService dateTimeService,
      IApplicationLifecycleService applicationLifecycleService)
    {
      this.fileSystemService = fileSystemService;
      this.dateTimeService = dateTimeService;
      this.applicationLifecycleService = applicationLifecycleService;
      this.applicationLifecycleService.RegisterSuspending(new Func<CancellationToken, Task>(this.OnSuspendingAsync));
      this.applicationLifecycleService.Resuming += new EventHandler<object>(this.OnResuming);
    }

    private async Task OnSuspendingAsync(CancellationToken token)
    {
      this.suspending = true;
      try
      {
        token.ThrowIfCancellationRequested();
        await this.FlushAsync(token);
      }
      catch (Exception ex)
      {
        FiddlerLogService.Logger.Error((object) "Fiddler log service failed to flush on suspension.", ex);
        throw;
      }
    }

    private async void OnResuming(object sender, object e)
    {
      this.suspending = false;
      try
      {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(FiddlerLogService.DefaultStartTimeout))
        {
          if (!this.started)
            await this.StartAsync(cancellationTokenSource.Token);
        }
      }
      catch (Exception ex)
      {
        FiddlerLogService.Logger.Warn((object) "Unable to resume fiddler logging.", ex);
      }
    }

    public async Task StartAsync(CancellationToken token)
    {
      AsyncLock.Releaser releaser = await this.writeLock.LockAsync().ConfigureAwait(false);
      try
      {
        if (this.started)
        {
          FiddlerLogService.Logger.Warn((object) "Start attempted when already started.");
          return;
        }
        IFolder folder = await this.fileSystemService.GetLogsFolderAsync().ConfigureAwait(false);
        FiddlerLogService fiddlerLogService = this;
        IFile zipArchiveFile1 = fiddlerLogService.zipArchiveFile;
        IFile file1 = await folder.CreateFileAsync("app.saz", CreationCollisionOption.OpenIfExists, token).ConfigureAwait(false);
        fiddlerLogService.zipArchiveFile = file1;
        fiddlerLogService = (FiddlerLogService) null;
        bool logFileExists;
        using (Stream scanStream = await this.zipArchiveFile.OpenAsync(FileAccess.Read, token).ConfigureAwait(false))
        {
          if (scanStream.Length > 0L)
          {
            try
            {
              using (ZipArchive zipArchive = new ZipArchive(scanStream, ZipArchiveMode.Read, true))
              {
                int? maxSessionId = FiddlerLogService.GetMaxSessionId(zipArchive);
                this.transactionCount = !maxSessionId.HasValue ? 0 : maxSessionId.Value;
              }
              logFileExists = true;
            }
            catch (Exception ex)
            {
              logFileExists = false;
              FiddlerLogService.Logger.Error((object) "Could not open old fiddler archive. Discarding old entries.", ex);
              fiddlerLogService = this;
              IFile zipArchiveFile2 = fiddlerLogService.zipArchiveFile;
              IFile file2 = await folder.CreateFileAsync("app.saz", CreationCollisionOption.ReplaceExisting, token).ConfigureAwait(false);
              fiddlerLogService.zipArchiveFile = file2;
              fiddlerLogService = (FiddlerLogService) null;
              this.transactionCount = 0;
            }
          }
          else
          {
            this.transactionCount = 0;
            logFileExists = false;
          }
        }
        this.HasEntries = logFileExists;
        await this.OpenArchiveStreamAsync(token).ConfigureAwait(false);
        this.OpenArchive(logFileExists);
        this.started = true;
        folder = (IFolder) null;
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
    }

    public async Task StopAsync(CancellationToken token)
    {
      using (await this.writeLock.LockAsync().ConfigureAwait(false))
      {
        if (!this.started)
        {
          FiddlerLogService.Logger.Warn((object) "Stop attempted when already stopped.");
        }
        else
        {
          if (this.zipArchive != null)
          {
            this.zipArchive.Dispose();
            this.zipArchive = (ZipArchive) null;
          }
          if (this.zipArchiveStream != null)
          {
            this.zipArchiveStream.Dispose();
            this.zipArchiveStream = (Stream) null;
          }
          this.zipArchiveFile = (IFile) null;
          this.started = false;
        }
      }
    }

    public async Task FlushAsync(CancellationToken token)
    {
      AsyncLock.Releaser releaser = await this.writeLock.LockAsync().ConfigureAwait(false);
      try
      {
        if (this.zipArchive != null)
        {
          if (this.zipArchiveStream != null)
            await this.FlushInternalAsync(token).ConfigureAwait(false);
        }
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
    }

    public bool HasEntries { get; private set; }

    private async Task FlushInternalAsync(CancellationToken token)
    {
      if (this.suspending)
      {
        Stopwatch sw = Stopwatch.StartNew();
        await this.zipArchiveStream.FlushAsync(token);
        sw.Stop();
        sw = (Stopwatch) null;
      }
      else
      {
        await this.FlushAndDisposeArchiveAsync(token);
        this.OpenArchive(true);
      }
    }

    private async Task FlushAndDisposeArchiveAsync(CancellationToken token)
    {
      Stopwatch sw = Stopwatch.StartNew();
      this.zipArchive.Dispose();
      await this.zipArchiveStream.FlushAsync(token);
      this.zipArchive = (ZipArchive) null;
      sw.Stop();
    }

    private void OpenArchive(bool reopen) => this.zipArchive = new ZipArchive(this.zipArchiveStream, reopen ? ZipArchiveMode.Update : ZipArchiveMode.Create, true);

    private async Task OpenArchiveStreamAsync(CancellationToken token)
    {
      FiddlerLogService fiddlerLogService = this;
      Stream zipArchiveStream = fiddlerLogService.zipArchiveStream;
      Stream stream = await this.zipArchiveFile.OpenAsync(FileAccess.ReadAndWrite, token);
      fiddlerLogService.zipArchiveStream = stream;
      fiddlerLogService = (FiddlerLogService) null;
    }

    private void CleanEntries(long bytesToRemove)
    {
      long num1 = bytesToRemove;
      int? minSessionId = FiddlerLogService.GetMinSessionId(this.zipArchive);
      if (!minSessionId.HasValue)
        return;
      Stopwatch stopwatch = Stopwatch.StartNew();
      int num2 = 0;
      int sessionId = minSessionId.Value;
      while (true)
      {
        IList<ZipArchiveEntry> entriesForSessionId = FiddlerLogService.GetEntriesForSessionId(this.zipArchive, sessionId);
        if (entriesForSessionId.Count != 0 && (!(this.dateTimeService.Now - entriesForSessionId[0].LastWriteTime < FiddlerLogService.SessionAgeLimit) || num1 > 0L))
        {
          foreach (ZipArchiveEntry zipArchiveEntry in (IEnumerable<ZipArchiveEntry>) entriesForSessionId)
          {
            num1 -= zipArchiveEntry.CompressedLength;
            zipArchiveEntry.Delete();
            ++num2;
          }
          ++sessionId;
        }
        else
          break;
      }
      stopwatch.Stop();
    }

    public async Task PauseCaptureAndCopyToAsync(Stream stream, CancellationToken token)
    {
      AsyncLock.Releaser releaser = await this.writeLock.LockAsync().ConfigureAwait(false);
      try
      {
        if (!this.started)
          throw new InvalidOperationException("Must call StartAsync before capturing.");
        await this.FlushAndDisposeArchiveAsync(token);
        try
        {
          this.zipArchiveStream.Seek(0L, SeekOrigin.Begin);
          this.zipArchiveStream.CopyTo(stream);
        }
        finally
        {
          this.OpenArchive(true);
        }
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
    }

    public async Task WriteTransactionAsync(
      IHttpTransaction transaction,
      CancellationToken token)
    {
      AsyncLock.Releaser releaser = await this.writeLock.LockAsync().ConfigureAwait(false);
      try
      {
        if (!this.started)
          throw new InvalidOperationException("Must call StartAsync first.");
        int transactionId = this.transactionCount + 1;
        string transactionIdString = FiddlerLogService.FormatSessionId(transactionId);
        HttpRequestMessage request = transaction.Request;
        HttpResponseMessage response = transaction.Response;
        using (Stream requestEntryStream = this.zipArchive.CreateEntry("raw\\" + transactionIdString + "_c.txt").Open())
        {
          using (Stream requestStream = transaction.OpenRequestStream())
            await this.WriteRequestFileAsync(request, requestStream, requestEntryStream);
        }
        if (response != null)
        {
          using (Stream responseEntryStream = this.zipArchive.CreateEntry("raw\\" + transactionIdString + "_s.txt").Open())
          {
            using (Stream responseStream = transaction.OpenResponseStream())
              await this.WriteResponseFileAsync(response, responseStream, responseEntryStream);
          }
        }
        using (Stream outputStream = this.zipArchive.CreateEntry("raw\\" + transactionIdString + "_m.xml").Open())
          this.WriteSessionMetadataFile(transaction, transactionId, outputStream);
        this.HasEntries = true;
        ++this.transactionCount;
        if (this.transactionCount % 10 == 0)
        {
          await this.FlushAndDisposeArchiveAsync(token);
          long length = this.zipArchiveStream.Length;
          this.OpenArchive(true);
          if (length > 524288L || this.transactionCount % 100 == 0)
            this.CleanEntries(length - 524288L);
        }
        transactionIdString = (string) null;
        response = (HttpResponseMessage) null;
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
    }

    private async Task WriteHeadersToStreamAsync(StreamWriter writer, HttpHeaders headers)
    {
      try
      {
        foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in headers.ToList<KeyValuePair<string, IEnumerable<string>>>())
        {
          if (!string.Equals(keyValuePair.Key, "Content-Encoding", StringComparison.OrdinalIgnoreCase))
          {
            string separator = string.Equals(keyValuePair.Key, "User-Agent", StringComparison.OrdinalIgnoreCase) ? " " : ",";
            await writer.WriteLineAsync(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}: {1}", new object[2]
            {
              (object) keyValuePair.Key,
              (object) string.Join(separator, keyValuePair.Value)
            }));
          }
        }
      }
      catch (InvalidOperationException ex)
      {
        FiddlerLogService.Logger.Warn((object) "Could not write HTTP headers to fiddler archive.");
      }
    }

    private async Task WriteRequestFileAsync(
      HttpRequestMessage request,
      Stream requestStream,
      Stream outputStream)
    {
      StreamWriter writer = new StreamWriter(outputStream);
      await writer.WriteLineAsync(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1} {2}/{3}", (object) request.Method, (object) request.RequestUri, (object) "HTTP", (object) "1.1"));
      if (request.Content != null && request.Content.Headers != null)
        await this.WriteHeadersToStreamAsync(writer, (HttpHeaders) request.Content.Headers);
      await this.WriteHeadersToStreamAsync(writer, (HttpHeaders) request.Headers);
      if (!request.Headers.Contains("Host"))
        await writer.WriteLineAsync(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}: {1}", new object[2]
        {
          (object) "Host",
          (object) request.RequestUri.Host
        }));
      await writer.WriteLineAsync();
      await writer.FlushAsync();
      if (requestStream == null)
        return;
      await requestStream.CopyToAsync(outputStream);
    }

    private async Task WriteResponseFileAsync(
      HttpResponseMessage response,
      Stream responseStream,
      Stream outputStream)
    {
      StreamWriter writer = new StreamWriter(outputStream);
      await writer.WriteLineAsync(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1} {2} {3}", (object) "HTTP", (object) "1.1", (object) (int) response.StatusCode, (object) response.ReasonPhrase));
      if (response.Content != null && response.Content.Headers != null)
        await this.WriteHeadersToStreamAsync(writer, (HttpHeaders) response.Content.Headers);
      await this.WriteHeadersToStreamAsync(writer, (HttpHeaders) response.Headers);
      await writer.WriteLineAsync();
      await writer.FlushAsync();
      if (responseStream == null)
        return;
      await responseStream.CopyToAsync(outputStream);
    }

    private void WriteSessionMetadataFile(
      IHttpTransaction transaction,
      int transactionId,
      Stream outputStream)
    {
      XElement xelement1 = new XElement((XName) "Session", (object) new XAttribute((XName) "SID", (object) transactionId));
      XElement xelement2 = new XElement((XName) "SessionTimers");
      if (transaction.BeginRequestTime.HasValue)
      {
        xelement2.Add((object) new XAttribute((XName) "ClientConnected", (object) transaction.BeginRequestTime.Value));
        xelement2.Add((object) new XAttribute((XName) "ClientBeginRequest", (object) transaction.BeginRequestTime.Value));
        xelement2.Add((object) new XAttribute((XName) "GotRequestHeaders", (object) transaction.BeginRequestTime.Value));
        xelement2.Add((object) new XAttribute((XName) "ClientDoneRequest", (object) transaction.BeginRequestTime.Value));
        xelement2.Add((object) new XAttribute((XName) "ServerConnected", (object) transaction.BeginRequestTime.Value));
        xelement2.Add((object) new XAttribute((XName) "FiddlerBeginRequest", (object) transaction.BeginRequestTime.Value));
        xelement2.Add((object) new XAttribute((XName) "ServerGotRequest", (object) transaction.BeginRequestTime.Value));
      }
      DateTimeOffset? nullable = transaction.GotResponseHeadersTime;
      if (nullable.HasValue)
      {
        XElement xelement3 = xelement2;
        XName name1 = (XName) "ServerBeginResponse";
        nullable = transaction.GotResponseHeadersTime;
        // ISSUE: variable of a boxed type
        __Boxed<DateTimeOffset> local1 = (ValueType) nullable.Value;
        XAttribute xattribute1 = new XAttribute(name1, (object) local1);
        xelement3.Add((object) xattribute1);
        XElement xelement4 = xelement2;
        XName name2 = (XName) "GotResponseHeaders";
        nullable = transaction.GotResponseHeadersTime;
        // ISSUE: variable of a boxed type
        __Boxed<DateTimeOffset> local2 = (ValueType) nullable.Value;
        XAttribute xattribute2 = new XAttribute(name2, (object) local2);
        xelement4.Add((object) xattribute2);
        XElement xelement5 = xelement2;
        XName name3 = (XName) "ServerDoneResponse";
        nullable = transaction.GotResponseHeadersTime;
        // ISSUE: variable of a boxed type
        __Boxed<DateTimeOffset> local3 = (ValueType) nullable.Value;
        XAttribute xattribute3 = new XAttribute(name3, (object) local3);
        xelement5.Add((object) xattribute3);
        XElement xelement6 = xelement2;
        XName name4 = (XName) "ClientBeginResponse";
        nullable = transaction.GotResponseHeadersTime;
        // ISSUE: variable of a boxed type
        __Boxed<DateTimeOffset> local4 = (ValueType) nullable.Value;
        XAttribute xattribute4 = new XAttribute(name4, (object) local4);
        xelement6.Add((object) xattribute4);
      }
      nullable = transaction.DoneResponseTime;
      if (nullable.HasValue)
      {
        XElement xelement7 = xelement2;
        XName name = (XName) "ClientDoneResponse";
        nullable = transaction.DoneResponseTime;
        // ISSUE: variable of a boxed type
        __Boxed<DateTimeOffset> local = (ValueType) nullable.Value;
        XAttribute xattribute = new XAttribute(name, (object) local);
        xelement7.Add((object) xattribute);
      }
      xelement1.Add((object) xelement2);
      XElement xelement8 = new XElement((XName) "SessionFlags", (object) new XElement((XName) "SessionFlag", new object[2]
      {
        (object) new XAttribute((XName) "N", (object) "x-processinfo"),
        (object) new XAttribute((XName) "V", (object) "fiddler-on-dotnet")
      }));
      xelement1.Add((object) xelement8);
      new XDocument(new object[1]{ (object) xelement1 }).Save(outputStream);
    }

    private static int? GetMaxSessionId(ZipArchive zipArchive)
    {
      List<int> list = zipArchive.Entries.Select<ZipArchiveEntry, int?>((Func<ZipArchiveEntry, int?>) (e => FiddlerLogService.TryGetSessionIdFromEntryPath(e.FullName))).Where<int?>((Func<int?, bool>) (e => e.HasValue)).Select<int?, int>((Func<int?, int>) (e => e.Value)).ToList<int>();
      return !list.Any<int>() ? new int?() : new int?(list.Max());
    }

    private static int? GetMinSessionId(ZipArchive zipArchive)
    {
      List<int> list = zipArchive.Entries.Select<ZipArchiveEntry, int?>((Func<ZipArchiveEntry, int?>) (e => FiddlerLogService.TryGetSessionIdFromEntryPath(e.FullName))).Where<int?>((Func<int?, bool>) (e => e.HasValue)).Select<int?, int>((Func<int?, int>) (e => e.Value)).ToList<int>();
      return !list.Any<int>() ? new int?() : new int?(list.Min());
    }

    private static int? TryGetSessionIdFromEntryPath(string path)
    {
      Match match = FiddlerLogService.TransactionMetadataFilenameRegex.Match(path);
      int result;
      return match.Success && int.TryParse(match.Groups[1].Value, out result) ? new int?(result) : new int?();
    }

    private static string FormatSessionId(int sessionId) => sessionId.ToString("D6");

    private static IList<ZipArchiveEntry> GetEntriesForSessionId(
      ZipArchive zipArchive,
      int sessionId)
    {
      List<ZipArchiveEntry> zipArchiveEntryList = new List<ZipArchiveEntry>();
      string str = FiddlerLogService.FormatSessionId(sessionId);
      ZipArchiveEntry entry1 = zipArchive.GetEntry("raw\\" + str + "_c.txt");
      if (entry1 != null)
        zipArchiveEntryList.Add(entry1);
      ZipArchiveEntry entry2 = zipArchive.GetEntry("raw\\" + str + "_s.txt");
      if (entry2 != null)
        zipArchiveEntryList.Add(entry2);
      ZipArchiveEntry entry3 = zipArchive.GetEntry("raw\\" + str + "_m.xml");
      if (entry3 != null)
        zipArchiveEntryList.Add(entry3);
      return (IList<ZipArchiveEntry>) zipArchiveEntryList;
    }
  }
}
