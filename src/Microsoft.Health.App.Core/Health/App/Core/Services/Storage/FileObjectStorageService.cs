// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Storage.FileObjectStorageService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Storage
{
  public sealed class FileObjectStorageService : IFileObjectStorageService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Storage\\FileObjectStorageService.cs");
    private readonly IFileSystemService fileSystemService;
    private readonly IMutexService mutexProvider;
    private readonly ICryptographyService cryptographyService;

    public FileObjectStorageService(
      IFileSystemService fileSystemService,
      IMutexService mutexProvider,
      ICryptographyService cryptographyService)
    {
      this.fileSystemService = fileSystemService;
      this.mutexProvider = mutexProvider;
      this.cryptographyService = cryptographyService;
    }

    public Task<T> ReadObjectAsync<T>(
      string fileName,
      CancellationToken token,
      bool decrypt = false,
      bool useMutex = true)
    {
      return useMutex ? this.mutexProvider.GetNamedMutex(false, FileObjectStorageService.CreateMutexName(fileName)).RunSynchronizedAsync<T>((Func<Task<T>>) (() => this.ReadObjectInternalAsync<T>(fileName, token, decrypt)), token) : this.ReadObjectInternalAsync<T>(fileName, token, decrypt);
    }

    private async Task<T> ReadObjectInternalAsync<T>(
      string fileName,
      CancellationToken token,
      bool decrypt)
    {
      T result = default (T);
      try
      {
        IFile file = await (await this.fileSystemService.GetObjectStorageFolderAsync().ConfigureAwait(false)).TryGetFileAsync(fileName, token).ConfigureAwait(false);
        if (file != null)
        {
          ConfiguredTaskAwaitable<byte[]> configuredTaskAwaitable = file.ReadAllBytesAsync().ConfigureAwait(false);
          byte[] numArray = await configuredTaskAwaitable;
          if (decrypt)
          {
            configuredTaskAwaitable = this.cryptographyService.UnprotectAsync(numArray).ConfigureAwait(false);
            numArray = await configuredTaskAwaitable;
          }
          result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(numArray, 0, numArray.Length));
        }
      }
      catch (Exception ex)
      {
        FileObjectStorageService.Logger.Error(ex, "Could not read object from isolated storage");
      }
      return result;
    }

    public Task WriteObjectAsync(
      object item,
      string fileName,
      CancellationToken token,
      bool encrypt = false,
      bool useMutex = true)
    {
      return useMutex ? this.mutexProvider.GetNamedMutex(false, FileObjectStorageService.CreateMutexName(fileName)).RunSynchronizedAsync((Func<Task>) (() => this.WriteObjectInternalAsync(item, fileName, token, encrypt)), token) : this.WriteObjectInternalAsync(item, fileName, token, encrypt);
    }

    private async Task WriteObjectInternalAsync(
      object item,
      string fileName,
      CancellationToken token,
      bool encrypt)
    {
      try
      {
        using (Stream fileStream = await (await (await this.fileSystemService.GetObjectStorageFolderAsync().ConfigureAwait(false)).CreateFileAsync(
            fileName, CreationCollisionOption.ReplaceExisting, token).ConfigureAwait(false))
            .OpenAsync(PCLStorage.FileAccess.ReadAndWrite, token).ConfigureAwait(false))
        {
          byte[] numArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item));
          if (encrypt)
            numArray = await this.cryptographyService.ProtectAsync(numArray).ConfigureAwait(false);
          await fileStream.WriteAsync(numArray, 0, numArray.Length, token).ConfigureAwait(false);
        }
      }
      catch (Exception ex)
      {
        FileObjectStorageService.Logger.Error(ex, "Could not write object to isolated storage");
      }
    }

    public Task DeleteObjectAsync(string fileName, CancellationToken token, bool useMutex = true) => useMutex ? this.mutexProvider.GetNamedMutex(false, FileObjectStorageService.CreateMutexName(fileName)).RunSynchronizedAsync((Func<Task>) (() => this.DeleteObjectInternalAsync(fileName, token)), token) : this.DeleteObjectInternalAsync(fileName, token);

    private async Task DeleteObjectInternalAsync(string fileName, CancellationToken token)
    {
      try
      {
        IFile file = await (await this.fileSystemService.GetObjectStorageFolderAsync().ConfigureAwait(false)).TryGetFileAsync(fileName, token).ConfigureAwait(false);
        if (file == null)
          return;
        await file.DeleteAsync(token).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        FileObjectStorageService.Logger.Error(ex, "Could not delete file from isolated storage");
      }
    }

    private static string CreateMutexName(string fileName) => "IsoMutex_" + fileName;
  }
}
