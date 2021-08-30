// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.IStorageProvider
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.IO;

namespace Microsoft.Band.Admin
{
  public interface IStorageProvider
  {
    Stream OpenFileForWrite(string relativePath, bool append, int bufferSize = 0);

    Stream OpenFileForWrite(
      StorageProviderRoot root,
      string relativePath,
      bool append,
      int bufferSize = 0);

    Stream OpenFileForRead(string relativePath, int bufferSize = 0);

    Stream OpenFileForRead(StorageProviderRoot root, string relativePath, int bufferSize = 0);

    void DeleteFile(string relativePath);

    void DeleteFile(StorageProviderRoot root, string relativePath);

    void RenameFile(string relativeSourcePath, string relativeDestFolder, string destFileName);

    void RenameFile(
      StorageProviderRoot sourceRoot,
      string relativeSourcePath,
      StorageProviderRoot destRoot,
      string relativeDestFolder,
      string destFileName);

    long GetFileSize(string relativePath);

    long GetFileSize(StorageProviderRoot root, string relativePath);

    string[] GetFiles(string folderRelativePath);

    string[] GetFiles(StorageProviderRoot root, string folderRelativePath);

    string[] GetFolders(string folderRelativePath);

    string[] GetFolders(StorageProviderRoot root, string folderRelativePath);

    void CreateFolder(string folderRelativePath);

    void CreateFolder(StorageProviderRoot root, string folderRelativePath);

    void DeleteFolder(string folderRelativePath);

    void DeleteFolder(StorageProviderRoot root, string folderRelativePath);

    DateTime GetFileCreationTimeUtc(string relativePath);

    DateTime GetFileCreationTimeUtc(StorageProviderRoot root, string relativePath);

    bool FileExists(string relativePath);

    bool FileExists(StorageProviderRoot root, string relativePath);

    bool DirectoryExists(string relativePath);

    bool DirectoryExists(StorageProviderRoot root, string relativePath);

    void MoveFolder(
      StorageProviderRoot sourceRoot,
      string relativeSourceFolder,
      StorageProviderRoot destRoot,
      string relativeDestFolder,
      bool overwrite = false);
  }
}
