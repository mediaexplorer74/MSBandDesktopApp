// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Storage.IFileObjectStorageService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Storage
{
  public interface IFileObjectStorageService
  {
    Task<T> ReadObjectAsync<T>(
      string fileName,
      CancellationToken token,
      bool decrypt = false,
      bool useMutex = true);

    Task WriteObjectAsync(
      object item,
      string fileName,
      CancellationToken token,
      bool encrypt = false,
      bool useMutex = true);

    Task DeleteObjectAsync(string fileName, CancellationToken token, bool useMutex = true);
  }
}
