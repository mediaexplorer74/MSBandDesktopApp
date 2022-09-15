// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Storage.FileSystemServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Storage
{
  public abstract class FileSystemServiceBase : IFileSystemService
  {
    private readonly IFileSystem fileSystem;
    private readonly IEnvironmentService environmentService;

    protected FileSystemServiceBase(IFileSystem fileSystem, IEnvironmentService environmentService)
    {
      this.fileSystem = fileSystem;
      this.environmentService = environmentService;
    }

    private IFolder LocalStorage => this.fileSystem.LocalStorage;

    public async Task ClearUserFilesAsync()
    {
      HashSet<string> nonUserFolders = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
      {
        "Logs",
        "fiddler",
        "ConnectionInfo"
      };
      IFolder rootFolder = this.LocalStorage;
      foreach (IFolder folder in (IEnumerable<IFolder>) await rootFolder.GetFoldersAsync())
      {
        if (!nonUserFolders.Contains(folder.Name))
          await folder.DeleteAsync();
      }
      foreach (IFile file in (IEnumerable<IFile>) await rootFolder.GetFilesAsync())
      {
        if (!file.Name.EndsWith(".sqlite", StringComparison.OrdinalIgnoreCase) && !file.Name.StartsWith("OAuthMsaToken", StringComparison.OrdinalIgnoreCase))
          await file.DeleteAsync();
      }
    }

    public Task<IFolder> GetCrashesFolderAsync() => Task.FromResult<IFolder>(this.LocalStorage);

    public Task<IFolder> GetLogsFolderAsync() => this.GetOrCreateFolderAsync("Logs");

    public Task<IFolder> GetTempFolderAsync() => this.GetOrCreateFolderAsync("Temp");

    public async Task<IFolder> GetSessionsFolderAsync() => await (await this.GetTempFolderAsync().ConfigureAwait(false)).CreateFolderAsync("Sessions", CreationCollisionOption.OpenIfExists).ConfigureAwait(false);

    public Task<IFolder> GetObjectStorageFolderAsync() => Task.FromResult<IFolder>(this.LocalStorage);

    public Task<IFolder> GetConnectionInfoFolderAsync() => this.GetOrCreateFolderAsync("ConnectionInfo");

    public Task<IFolder> GetDynamicConfigurationFolderAsync() => Task.FromResult<IFolder>(this.LocalStorage);

    public Task<IFolder> GetDebugFolderAsync() => this.GetOrCreateSessionSubfolderAsync("Debug");

    public Task<IFolder> GetEncryptionFolderAsync() => this.GetOrCreateFolderAsync("credentials");

    public async Task<IFolder> GetSensorCoreLogsFolderAsync() => await (await (await this.LocalStorage.CreateFolderAsync("Devices", CreationCollisionOption.OpenIfExists).ConfigureAwait(false)).CreateFolderAsync("SensorCore", CreationCollisionOption.OpenIfExists).ConfigureAwait(false)).CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists).ConfigureAwait(false);

    public virtual Task<IFolder> GetSharingFolderAsync() => this.GetOrCreateSessionSubfolderAsync("Sharing");

    public virtual Task<IFolder> GetSocialSharingFolderAsync() => this.GetSharingFolderAsync();

    public Task<IFolder> GetTempScreenshotFolderAsync() => this.GetOrCreateSessionSubfolderAsync("Screenshots");

    public Task<IFolder> GetTempFilePickerImagesFolderAsync() => this.GetOrCreateSessionSubfolderAsync("Images");

    public async Task DeleteSessionFoldersAsync()
    {
      foreach (IFolder folder in (await (await this.GetSessionsFolderAsync()).GetFoldersAsync()).Where<IFolder>((Func<IFolder, bool>) (folder => folder.Name != this.environmentService.ApplicationSessionId)))
        await folder.DeleteAsync();
    }

    private async Task<IFolder> GetOrCreateSessionSubfolderAsync(string subFolderName)
    {
      ConfiguredTaskAwaitable<IFolder> configuredTaskAwaitable = (await this.GetSessionsFolderAsync().ConfigureAwait(false)).CreateFolderAsync(this.environmentService.ApplicationSessionId, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
      configuredTaskAwaitable = (await configuredTaskAwaitable).CreateFolderAsync(subFolderName, CreationCollisionOption.OpenIfExists).ConfigureAwait(false);
      return await configuredTaskAwaitable;
    }

    private Task<IFolder> GetOrCreateFolderAsync(string folderName) => this.LocalStorage.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

    public Task<IFolder> GetCacheFolderAsync() => this.GetOrCreateFolderAsync("Cache");

    public Task<IFolder> GetCalendarCacheFolderAsync() => this.GetOrCreateFolderAsync("CalendarCache");
  }
}
