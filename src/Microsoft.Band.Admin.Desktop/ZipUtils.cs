﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.ZipUtils
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Microsoft.Band.Admin
{
  internal static class ZipUtils
  {
    internal static void Unzip(
      IStorageProvider storageProvider,
      StorageProviderRoot root,
      Stream zipStream,
      string extractFolder)
    {
      if (storageProvider == null)
        throw new ArgumentNullException(nameof (storageProvider));
      if (zipStream == null)
        throw new ArgumentNullException(nameof (zipStream));
      if (extractFolder == null)
        throw new ArgumentNullException(nameof (extractFolder));
      Logger.Log(LogLevel.Info, "Unzip file to folder: {0}", (object) extractFolder);
      Stopwatch stopwatch = Stopwatch.StartNew();
      storageProvider.CreateFolder(root, extractFolder);
      using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, true))
      {
        foreach (ZipArchiveEntry entry in zipArchive.Entries)
          ZipUtils.UnzipZipArchiveEntry(storageProvider, root, entry, entry.FullName, extractFolder);
      }
      stopwatch.Stop();
      Logger.Log(LogLevel.Info, "Time to unzip: {0}", (object) stopwatch.Elapsed);
    }

    private static void UnzipZipArchiveEntry(
      IStorageProvider storageProvider,
      StorageProviderRoot root,
      ZipArchiveEntry entry,
      string entryRelativePath,
      string extractFolder)
    {
      Logger.Log(LogLevel.Info, "Unzip file: {0}", (object) entry.FullName);
      string directoryName = Path.GetDirectoryName(entryRelativePath);
      if (!string.IsNullOrEmpty(directoryName))
      {
        extractFolder = Path.Combine(extractFolder, directoryName);
        storageProvider.CreateFolder(root, extractFolder);
        entryRelativePath = Path.GetFileName(entryRelativePath);
      }
      if (string.IsNullOrEmpty(entryRelativePath))
        return;
      using (Stream stream = entry.Open())
      {
        string relativePath = Path.Combine(extractFolder, entryRelativePath);
        using (Stream destination = storageProvider.OpenFileForWrite(root, relativePath, false, 8192))
          stream.CopyTo(destination);
      }
    }
  }
}
