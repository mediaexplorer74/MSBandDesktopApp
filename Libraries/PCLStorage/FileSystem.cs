// Decompiled with JetBrains decompiler
// Type: PCLStorage.FileSystem
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=286fe515a2c35b64
// MVID: 7C72F032-7D19-49C3-B4FA-67DAADE24971
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\PCLStorage.dll

using System;
using System.Threading;

namespace PCLStorage
{
  public static class FileSystem
  {
    private static Lazy<IFileSystem> _fileSystem = new Lazy<IFileSystem>((Func<IFileSystem>) (() => FileSystem.CreateFileSystem()), LazyThreadSafetyMode.PublicationOnly);

    public static IFileSystem Current => FileSystem._fileSystem.Value ?? throw FileSystem.NotImplementedInReferenceAssembly();

    private static IFileSystem CreateFileSystem() => (IFileSystem) null;

    internal static Exception NotImplementedInReferenceAssembly() => (Exception) new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the PCLStorage NuGet package from your main application project in order to reference the platform-specific implementation.");
  }
}
