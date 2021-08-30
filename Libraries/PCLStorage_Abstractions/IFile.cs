// Decompiled with JetBrains decompiler
// Type: PCLStorage.IFile
// Assembly: PCLStorage.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=286fe515a2c35b64
// MVID: 1BC1AC1B-67B7-41CB-A202-40E4FD631624
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\PCLStorage_Abstractions.dll

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  public interface IFile
  {
    string Name { get; }

    string Path { get; }

    Task<Stream> OpenAsync(FileAccess fileAccess, CancellationToken cancellationToken = default (CancellationToken));

    Task DeleteAsync(CancellationToken cancellationToken = default (CancellationToken));

    Task RenameAsync(
      string newName,
      NameCollisionOption collisionOption = NameCollisionOption.FailIfExists,
      CancellationToken cancellationToken = default (CancellationToken));

    Task MoveAsync(
      string newPath,
      NameCollisionOption collisionOption = NameCollisionOption.ReplaceExisting,
      CancellationToken cancellationToken = default (CancellationToken));
  }
}
