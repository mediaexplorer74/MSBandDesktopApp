// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.CurrentDynamicConfigurationFileStore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  internal sealed class CurrentDynamicConfigurationFileStore : ICurrentDynamicConfigurationFileStore
  {
    private const string FileName = "CurrentDynamicConfigurationFile.json";
    public static readonly ConfigurationValue<bool> IsCurrentFileStoreEnabled = ConfigurationValue.CreateBoolean("DynamicConfigurationService", "ICurrentFileStoreEnabled", true);
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Configuration\\Dynamic\\CurrentDynamicConfigurationFileStore.cs");
    private readonly IConfigurationService configurationService;
    private readonly IFileSystemService fileSystemService;
    private readonly SemaphoreSlim fileLock = new SemaphoreSlim(1);

    public CurrentDynamicConfigurationFileStore(
      IFileSystemService fileSystemService,
      IConfigurationService configurationService)
    {
      Assert.ParamIsNotNull((object) fileSystemService, nameof (fileSystemService));
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      this.fileSystemService = fileSystemService;
      this.configurationService = configurationService;
    }

    public Task<CurrentDynamicConfigurationFile> GetConfigurationFileAsync(
      CancellationToken token)
    {
      return this.fileLock.RunSynchronizedAsync<CurrentDynamicConfigurationFile>((Func<Task<CurrentDynamicConfigurationFile>>) (async () =>
      {
        try
        {
          if (!this.configurationService.GetValue<bool>(CurrentDynamicConfigurationFileStore.IsCurrentFileStoreEnabled))
          {
            CurrentDynamicConfigurationFileStore.Logger.Warn((object) "The current configuration file store has been disabled.");
            return (CurrentDynamicConfigurationFile) null;
          }
          IFolder folder = await this.fileSystemService.GetDynamicConfigurationFolderAsync().ConfigureAwait(false);
          if (await folder.CheckExistsAsync("CurrentDynamicConfigurationFile.json", token).ConfigureAwait(false) == ExistenceCheckResult.FileExists)
          {
            IFile file = await folder.GetFileAsync("CurrentDynamicConfigurationFile.json", token).ConfigureAwait(false);
            if (file == null)
            {
              CurrentDynamicConfigurationFileStore.Logger.Warn("Unable to get the current configuration file '{0}'.", (object) "CurrentDynamicConfigurationFile.json");
              return (CurrentDynamicConfigurationFile) null;
            }
            using (Stream fileStream = await file.OpenAsync(FileAccess.Read, token).ConfigureAwait(false))
            {
              if (fileStream == null)
              {
                CurrentDynamicConfigurationFileStore.Logger.Warn("Unable to open the current configuration file '{0}'.", (object) "CurrentDynamicConfigurationFile.json");
                return (CurrentDynamicConfigurationFile) null;
              }
              token.ThrowIfCancellationRequested();
              string str;
              using (StreamReader streamReader = new StreamReader(fileStream))
                str = await streamReader.ReadToEndAsync().ConfigureAwait(false);
              if (string.IsNullOrWhiteSpace(str))
              {
                CurrentDynamicConfigurationFileStore.Logger.Warn((object) "Current configuration file content is invalid (i.e. empty).");
                return (CurrentDynamicConfigurationFile) null;
              }
              CurrentDynamicConfigurationFile configurationFile = JsonConvert.DeserializeObject<CurrentDynamicConfigurationFile>(str);
              if (configurationFile != null && !(configurationFile.Schema != CurrentDynamicConfigurationFile.CurrentDynamicConfigurationFileSchema))
                return configurationFile;
              CurrentDynamicConfigurationFileStore.Logger.Warn((object) "Current configuration file content is invalid (e.g. unrecognized schema).");
              return (CurrentDynamicConfigurationFile) null;
            }
          }
          else
            folder = (IFolder) null;
        }
        catch (Exception ex)
        {
          CurrentDynamicConfigurationFileStore.Logger.Warn((object) "Unable to get the current configuration file due to exception.", ex);
        }
        return (CurrentDynamicConfigurationFile) null;
      }), token);
    }

    public Task<bool> SetConfigurationFileAsync(
      CurrentDynamicConfigurationFile file,
      CancellationToken token)
    {
      return this.fileLock.RunSynchronizedAsync<bool>((Func<Task<bool>>) (async () =>
      {
        try
        {
          if (!this.configurationService.GetValue<bool>(CurrentDynamicConfigurationFileStore.IsCurrentFileStoreEnabled))
          {
            CurrentDynamicConfigurationFileStore.Logger.Warn((object) "The current configuration file store has been disabled.");
            return false;
          }
          string content = JsonConvert.SerializeObject((object) file);
          IFile file1 = await (await this.fileSystemService.GetDynamicConfigurationFolderAsync().ConfigureAwait(false)).CreateFileAsync("CurrentDynamicConfigurationFile.json", CreationCollisionOption.ReplaceExisting, token).ConfigureAwait(false);
          if (file1 == null)
          {
            CurrentDynamicConfigurationFileStore.Logger.Warn("Unable to create/replace the configuration file '{0}'.", (object) "CurrentDynamicConfigurationFile.json");
            return false;
          }
          using (Stream fileStream = await file1.OpenAsync(FileAccess.ReadAndWrite, token).ConfigureAwait(false))
          {
            if (fileStream == null)
            {
              CurrentDynamicConfigurationFileStore.Logger.Warn("Unable to open the configuration file '{0}' for writing.", (object) "CurrentDynamicConfigurationFile.json");
              return false;
            }
            token.ThrowIfCancellationRequested();
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
              await streamWriter.WriteAsync(content).ConfigureAwait(false);
          }
          return true;
        }
        catch (Exception ex)
        {
          CurrentDynamicConfigurationFileStore.Logger.Warn((object) "Unable to set the configuration file due to exception.", ex);
          return false;
        }
      }), token);
    }
  }
}
