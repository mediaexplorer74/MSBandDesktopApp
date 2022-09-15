// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Appenders.StorageFileAppender
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Logging.Appenders
{
  public class StorageFileAppender : AppenderBase, IDisposable, IStorageFileAppender
  {
    private const int WriterBufferSize = 1;
    private readonly IFileSystemService fileSystemService;
    private readonly SemaphoreSlim appenderLock = new SemaphoreSlim(1);
    private string logName;
    private IFile logFile;
    private StreamWriter writer;
    private bool initialized;
    private long? maxFileSize;
    private int maxLogFiles = 1;
    private bool isSuspending;

    public StorageFileAppender(IFileSystemService fileSystemService, string logName)
    {
      this.fileSystemService = fileSystemService;
      this.logName = logName;
      this.AutoFlushOverride = Level.Error;
    }

    public StorageFileAppender(
      IApplicationLifecycleService application,
      IFileSystemService fileSystemService,
      string logName)
      : this(fileSystemService, logName)
    {
      if (logName == "App")
        application.RegisterSuspendingLogFlush(new Func<CancellationToken, Task>(this.ApplicationOnSuspendingAsync));
      else
        application.RegisterSuspending(new Func<CancellationToken, Task>(this.ApplicationOnSuspendingAsync));
      application.Resuming += new EventHandler<object>(this.ApplicationOnResuming);
    }

    private Task ApplicationOnSuspendingAsync(CancellationToken token)
    {
      this.isSuspending = true;
      return this.appenderLock.RunSynchronizedAsync((Func<Task>) (() => this.writer == null ? (Task) Task.FromResult<bool>(true) : this.writer.FlushAsync()), token);
    }

    private void ApplicationOnResuming(object sender, object e) => this.isSuspending = false;

    public bool AppendToExisting { get; set; }

    public string LogName => this.logName;

    public bool AutoFlush { get; set; }

    public Level AutoFlushOverride { get; set; }

    public long? MaxFileSize
    {
      get => this.maxFileSize;
      set
      {
        if (value.HasValue)
        {
          long? nullable = value;
          long num = 0;
          if ((nullable.GetValueOrDefault() <= num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            throw new ArgumentOutOfRangeException("MaxFileSize must be greater than 0");
        }
        this.maxFileSize = value;
      }
    }

    public int MaxLogFiles
    {
      get => this.maxLogFiles;
      set => this.maxLogFiles = value > 0 ? value : throw new ArgumentOutOfRangeException("MaxLogFiles must be greater than 0");
    }

    protected override async void Append(LoggingEvent loggingEvent)
    {
      if (this.isSuspending)
        return;
      await this.appenderLock.RunSynchronizedAsync((Func<Task>) (async () =>
      {
        ConfiguredTaskAwaitable configuredTaskAwaitable = this.EnsureInitializedAsync().ConfigureAwait(false);
        await configuredTaskAwaitable;
        if (this.writer == null)
          return;
        try
        {
          configuredTaskAwaitable = this.RollOverIfNeededAsync().ConfigureAwait(false);
          await configuredTaskAwaitable;
          this.Layout.Format((TextWriter) this.writer, loggingEvent);
          this.FlushIfNeeded(loggingEvent.Level);
        }
        catch (Exception ex)
        {
          Debugger.Break();
        }
      })).ConfigureAwait(false);
    }

    private void FlushIfNeeded(Level eventLevel)
    {
      if (!this.AutoFlush && eventLevel < this.AutoFlushOverride)
        return;
      this.writer.Flush();
    }

    public override void Close() => this.appenderLock.RunSynchronized((Action) (() => this.CloseInternal()));

    private void CloseInternal()
    {
      if (this.writer == null)
        return;
      this.writer.Flush();
      this.writer.Dispose();
      this.writer = (StreamWriter) null;
    }

    public void Dispose() => this.Close();

    public Task<IFile> CopyCurrentFileToTempFileAsync() => this.appenderLock.RunSynchronizedAsync<IFile>((Func<Task<IFile>>) (async () =>
    {
      await this.EnsureInitializedAsync().ConfigureAwait(false);
      this.CloseInternal();
      IFile targetFile = await (await this.fileSystemService.GetTempFolderAsync().ConfigureAwait(false)).CreateFileAsync(this.logFile.Name, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
      using (Stream outputStream = await targetFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite).ConfigureAwait(false))
      {
        using (Stream stream = await this.logFile.OpenAsync(PCLStorage.FileAccess.Read).ConfigureAwait(false))
          stream.CopyTo(outputStream);
      }
      await this.CreateWriterAsync(true).ConfigureAwait(false);
      return targetFile;
    }));

    public Task ForceRolloverAsync() => this.appenderLock.RunSynchronizedAsync((Func<Task>) (async () =>
    {
      await this.EnsureInitializedAsync().ConfigureAwait(false);
      await this.RollOverAsync().ConfigureAwait(false);
    }));

    private async Task EnsureInitializedAsync()
    {
      if (this.initialized)
        return;
      this.initialized = true;
      StorageFileAppender storageFileAppender = this;
      IFile logFile = storageFileAppender.logFile;
      IFile file = await this.GetLatestFileAsync().ConfigureAwait(false);
      storageFileAppender.logFile = file;
      storageFileAppender = (StorageFileAppender) null;
      if (this.logFile == null)
        this.logFile = this.CreateNewLogFile();
      await this.CreateWriterAsync(this.AppendToExisting).ConfigureAwait(false);
    }

    private async Task CreateWriterAsync(bool append)
    {
      Stream stream = (Stream) null;
      try
      {
        stream = await this.logFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite).ConfigureAwait(false);
        if (append)
          stream.Seek(0L, SeekOrigin.End);
        this.writer = new StreamWriter(stream, Encoding.UTF8, 1);
      }
      catch (Exception ex)
      {
        Debugger.Break();
        stream?.Dispose();
      }
    }

    private async Task RollOverIfNeededAsync()
    {
      long? maxFileSize = this.MaxFileSize;
      if (!maxFileSize.HasValue)
        return;
      long length = this.writer.BaseStream.Length;
      maxFileSize = this.MaxFileSize;
      long valueOrDefault = maxFileSize.GetValueOrDefault();
      if ((length > valueOrDefault ? (maxFileSize.HasValue ? 1 : 0) : 0) == 0)
        return;
      await this.RollOverAsync().ConfigureAwait(false);
    }

    private async Task RollOverAsync()
    {
      this.logFile = this.CreateNewLogFile();
      this.CloseInternal();
      await this.DeleteFilesIfNeededAsync().ConfigureAwait(false);
      await this.CreateWriterAsync(false).ConfigureAwait(false);
    }

    private async Task DeleteFilesIfNeededAsync()
    {
      IFolder folder = await this.GetFolderForLogNameAsync().ConfigureAwait(false);
      if (folder == null)
        return;
      IList<IFile> result = folder.GetFilesAsync().Result;
      if (result.Count <= this.MaxLogFiles)
        return;
      int num = result.Count - this.MaxLogFiles;
      for (int index = 0; index < num; ++index)
        result[index].DeleteAsync().Wait();
    }

    private IFile CreateNewLogFile()
    {
      string desiredName = DateTimeOffset.UtcNow.ToString("u").Replace(":", string.Empty).Replace(' ', 'T') + ".jsonl";
      return this.GetFolderForLogNameAsync().Result.CreateFileAsync(desiredName, CreationCollisionOption.ReplaceExisting, CancellationToken.None).Result;
    }

    private async Task<IFile> GetLatestFileAsync()
    {
      IFolder folder = await this.GetFolderForLogNameAsync().ConfigureAwait(false);
      if (folder == null)
        return (IFile) null;
      IList<IFile> fileList = await folder.GetFilesAsync().ConfigureAwait(false);
      return fileList.Count != 0 ? fileList[fileList.Count - 1] : (IFile) null;
    }

    private async Task<IFolder> GetFolderForLogNameAsync()
    {
      ConfiguredTaskAwaitable<IFolder> configuredTaskAwaitable = this.fileSystemService.GetLogsFolderAsync().ConfigureAwait(false);
      configuredTaskAwaitable = (await configuredTaskAwaitable).CreateFolderAsync(this.logName, CreationCollisionOption.OpenIfExists, CancellationToken.None).ConfigureAwait(false);
      return await configuredTaskAwaitable;
    }
  }
}
