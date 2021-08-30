// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Desktop.StorageProvider
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Band.Admin.Desktop
{
  internal sealed class StorageProvider : IStorageProvider
  {
    private static readonly string rootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "CargoDevice");
    private DirectoryInfo appRootFolder;
    private DirectoryInfo deviceRootFolder;

    private StorageProvider(DirectoryInfo appRootFolder, DirectoryInfo deviceRootFolder)
    {
      this.appRootFolder = appRootFolder;
      this.deviceRootFolder = deviceRootFolder;
    }

    internal static void CleanDeprecated()
    {
      string rootFolderPath = StorageProvider.rootFolderPath;
      string path2 = "userId";
      DirectoryInfo directory = Directory.CreateDirectory(rootFolderPath);
      string path = Path.Combine(directory.FullName, path2);
      List<string> stringList = new List<string>(Directory.EnumerateDirectories(directory.FullName));
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (stringList[index].Equals(path, StringComparison.OrdinalIgnoreCase))
        {
          Directory.Delete(path, true);
          break;
        }
      }
    }

    internal static StorageProvider Create() => new StorageProvider(Directory.CreateDirectory(StorageProvider.rootFolderPath), (DirectoryInfo) null);

    internal static StorageProvider Create(string userId, string deviceId)
    {
      string path1 = !string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(deviceId) ? string.Format("{0}{1}", (object) "u_", (object) userId) : throw new ArgumentException();
      string path2 = string.Format("{0}{1}", (object) "d_", (object) deviceId);
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
      if (path1.IndexOfAny(invalidFileNameChars) != -1)
        throw new ArgumentException(string.Format(CommonSR.InvalidCharactersInFolderName, (object) path1));
      if (path2.IndexOfAny(invalidFileNameChars) != -1)
        throw new ArgumentException(string.Format(CommonSR.InvalidCharactersInFolderName, (object) path2));
      DirectoryInfo directory = Directory.CreateDirectory(StorageProvider.rootFolderPath);
      return new StorageProvider(directory, directory.CreateSubdirectory(Path.Combine(path1, path2)));
    }

    private string GetPath(StorageProviderRoot root, string relativePath)
    {
      if (root == StorageProviderRoot.App)
        return Path.Combine(this.appRootFolder.FullName, relativePath);
      if (root != StorageProviderRoot.Device)
        throw new ArgumentException("StorageProviderRoot value unknown", nameof (root));
      if (this.deviceRootFolder == null)
        throw new ArgumentException("StorageProviderRoot.Device not supported", nameof (root));
      return Path.Combine(this.deviceRootFolder.FullName, relativePath);
    }

    public Stream OpenFileForWrite(string relativePath, bool append, int bufferSize = 0) => this.OpenFileForWrite(StorageProviderRoot.Device, relativePath, append, bufferSize);

    public Stream OpenFileForWrite(
      StorageProviderRoot root,
      string relativePath,
      bool append,
      int bufferSize = 0)
    {
      if (bufferSize == 0)
        bufferSize = 1;
      return (Stream) new FileStream(this.GetPath(root, relativePath), append ? FileMode.Append : FileMode.Create, FileAccess.ReadWrite, FileShare.Read, bufferSize);
    }

    public Stream OpenFileForRead(string relativePath, int bufferSize = 0) => this.OpenFileForRead(StorageProviderRoot.Device, relativePath, bufferSize);

    public Stream OpenFileForRead(
      StorageProviderRoot root,
      string relativePath,
      int bufferSize = 0)
    {
      string path = this.GetPath(root, relativePath);
      if (bufferSize == -1)
        bufferSize = (int) Math.Min(new FileInfo(path).Length, 8192L);
      if (bufferSize == 0)
        bufferSize = 1;
      return (Stream) new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
    }

    public void DeleteFile(string relativePath) => this.DeleteFile(StorageProviderRoot.Device, relativePath);

    public void DeleteFile(StorageProviderRoot root, string relativePath) => File.Delete(this.GetPath(root, relativePath));

    public void RenameFile(
      string relativeSourcePath,
      string relativeDestFolder,
      string destFileName)
    {
      this.RenameFile(StorageProviderRoot.Device, relativeSourcePath, StorageProviderRoot.Device, relativeDestFolder, destFileName);
    }

    public void RenameFile(
      StorageProviderRoot sourceRoot,
      string relativeSourcePath,
      StorageProviderRoot destRoot,
      string relativeDestFolder,
      string destFileName)
    {
      string path = this.GetPath(sourceRoot, relativeSourcePath);
      if (Path.Combine(this.GetPath(destRoot, relativeDestFolder), destFileName) == path)
        return;
      File.Copy(path, Path.Combine(this.GetPath(destRoot, relativeDestFolder), destFileName), true);
      File.Delete(path);
    }

    public long GetFileSize(string relativePath) => this.GetFileSize(StorageProviderRoot.Device, relativePath);

    public long GetFileSize(StorageProviderRoot root, string relativePath) => new FileInfo(this.GetPath(root, relativePath)).Length;

    public DateTime GetFileCreationTimeUtc(string relativePath) => this.GetFileCreationTimeUtc(StorageProviderRoot.Device, relativePath);

    public DateTime GetFileCreationTimeUtc(StorageProviderRoot root, string relativePath)
    {
      FileInfo fileInfo = new FileInfo(this.GetPath(root, relativePath));
      return fileInfo.Exists ? fileInfo.CreationTimeUtc : throw new FileNotFoundException(string.Format(CommonSR.FileNotFound, (object) relativePath));
    }

    public string[] GetFiles(string relativePath) => this.GetFiles(StorageProviderRoot.Device, relativePath);

    public string[] GetFiles(StorageProviderRoot root, string relativePath)
    {
      List<string> stringList = new List<string>(Directory.EnumerateFiles(new DirectoryInfo(this.GetPath(root, relativePath)).FullName));
      string[] strArray = new string[stringList.Count];
      for (int index = 0; index < strArray.Length; ++index)
      {
        string fileName = Path.GetFileName(stringList[index]);
        strArray[index] = fileName;
      }
      return strArray;
    }

    public string[] GetFolders(string relativePath) => this.GetFolders(StorageProviderRoot.Device, relativePath);

    public string[] GetFolders(StorageProviderRoot root, string relativePath)
    {
      List<string> stringList = new List<string>(Directory.EnumerateDirectories(new DirectoryInfo(this.GetPath(root, relativePath)).FullName));
      string[] strArray = new string[stringList.Count];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = stringList[index];
      return strArray;
    }

    public void CreateFolder(string relativePath) => this.CreateFolder(StorageProviderRoot.Device, relativePath);

    public void CreateFolder(StorageProviderRoot root, string relativePath) => Directory.CreateDirectory(this.GetPath(root, relativePath));

    public void DeleteFolder(string relativePath) => this.DeleteFolder(StorageProviderRoot.Device, relativePath);

    public void DeleteFolder(StorageProviderRoot root, string relativePath)
    {
      string path = this.GetPath(root, relativePath);
      if (!Directory.Exists(path))
        return;
      Directory.Delete(path, true);
    }

    public bool FileExists(string relativePath) => this.FileExists(StorageProviderRoot.Device, relativePath);

    public bool FileExists(StorageProviderRoot root, string relativePath) => File.Exists(this.GetPath(root, relativePath));

    public bool DirectoryExists(string relativePath) => this.DirectoryExists(StorageProviderRoot.Device, relativePath);

    public bool DirectoryExists(StorageProviderRoot root, string relativePath) => Directory.Exists(this.GetPath(root, relativePath));

    public void MoveFolder(
      StorageProviderRoot sourceRoot,
      string relativeSourceFolder,
      StorageProviderRoot destRoot,
      string relativeDestFolder,
      bool overwrite = false)
    {
      if (string.IsNullOrWhiteSpace(relativeSourceFolder))
      {
        ArgumentException argumentException = new ArgumentException();
        Logger.LogException(Microsoft.Band.Admin.LogLevel.Error, (Exception) argumentException);
        throw argumentException;
      }
      string path1 = this.GetPath(sourceRoot, relativeSourceFolder);
      string path2 = this.GetPath(destRoot, relativeDestFolder);
      if (!Directory.Exists(path1))
        throw new DirectoryNotFoundException("Source folder not found");
      if (overwrite)
        this.DeleteFolder(destRoot, relativeDestFolder);
      Directory.Move(path1, path2);
    }
  }
}
