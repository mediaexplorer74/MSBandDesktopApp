// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.FilePickerUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Practices.ServiceLocation;
using PCLStorage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class FilePickerUtilities
  {
    public static async Task<IList<IFile>> CopyPickerFilesToLocalStorageAsync(
      IEnumerable<IFile> files)
    {
      List<IFile> fileList = new List<IFile>();
      IFolder folder = await ServiceLocator.Current.GetInstance<IFileSystemService>().GetTempFilePickerImagesFolderAsync().ConfigureAwait(false);
      foreach (IFile file in files)
      {
        IFile copiedFile = await folder.CreateFileAsync(file.Name, CreationCollisionOption.GenerateUniqueName, CancellationToken.None).ConfigureAwait(false);
        await file.CopyToAsync(copiedFile).ConfigureAwait(false);
        fileList.Add(copiedFile);
        copiedFile = (IFile) null;
      }
      return (IList<IFile>) fileList;
    }
  }
}
