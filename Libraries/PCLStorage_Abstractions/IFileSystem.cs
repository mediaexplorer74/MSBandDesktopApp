// Decompiled with JetBrains decompiler
// Type: PCLStorage.IFileSystem
// Assembly: PCLStorage.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=286fe515a2c35b64
// MVID: 1BC1AC1B-67B7-41CB-A202-40E4FD631624
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\PCLStorage_Abstractions.dll

using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  public interface IFileSystem
  {
    IFolder LocalStorage { get; }

    IFolder RoamingStorage { get; }

    Task<IFile> GetFileFromPathAsync(string path, CancellationToken cancellationToken = default (CancellationToken));

    Task<IFolder> GetFolderFromPathAsync(
      string path,
      CancellationToken cancellationToken = default (CancellationToken));
  }
}
