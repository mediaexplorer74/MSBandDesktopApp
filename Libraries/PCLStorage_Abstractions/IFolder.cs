// Decompiled with JetBrains decompiler
// Type: PCLStorage.IFolder
// Assembly: PCLStorage.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=286fe515a2c35b64
// MVID: 1BC1AC1B-67B7-41CB-A202-40E4FD631624
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\PCLStorage_Abstractions.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  public interface IFolder
  {
    string Name { get; }

    string Path { get; }

    Task<IFile> CreateFileAsync(
      string desiredName,
      CreationCollisionOption option,
      CancellationToken cancellationToken = default (CancellationToken));

    Task<IFile> GetFileAsync(string name, CancellationToken cancellationToken = default (CancellationToken));

    Task<IList<IFile>> GetFilesAsync(CancellationToken cancellationToken = default (CancellationToken));

    Task<IFolder> CreateFolderAsync(
      string desiredName,
      CreationCollisionOption option,
      CancellationToken cancellationToken = default (CancellationToken));

    Task<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default (CancellationToken));

    Task<IList<IFolder>> GetFoldersAsync(CancellationToken cancellationToken = default (CancellationToken));

    Task<ExistenceCheckResult> CheckExistsAsync(
      string name,
      CancellationToken cancellationToken = default (CancellationToken));

    Task DeleteAsync(CancellationToken cancellationToken = default (CancellationToken));
  }
}
