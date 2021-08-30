// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Storage.IFileSystemService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using PCLStorage;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Storage
{
  public interface IFileSystemService
  {
    Task<IFolder> GetTempFolderAsync();

    Task<IFolder> GetSessionsFolderAsync();

    Task<IFolder> GetTempScreenshotFolderAsync();

    Task<IFolder> GetTempFilePickerImagesFolderAsync();

    Task<IFolder> GetSharingFolderAsync();

    Task<IFolder> GetSocialSharingFolderAsync();

    Task<IFolder> GetCrashesFolderAsync();

    Task<IFolder> GetLogsFolderAsync();

    Task<IFolder> GetObjectStorageFolderAsync();

    Task<IFolder> GetConnectionInfoFolderAsync();

    Task<IFolder> GetDynamicConfigurationFolderAsync();

    Task<IFolder> GetDebugFolderAsync();

    Task<IFolder> GetEncryptionFolderAsync();

    Task<IFolder> GetSensorCoreLogsFolderAsync();

    Task<IFolder> GetCacheFolderAsync();

    Task<IFolder> GetCalendarCacheFolderAsync();

    Task ClearUserFilesAsync();

    Task DeleteSessionFoldersAsync();
  }
}
