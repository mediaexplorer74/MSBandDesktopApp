// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.FileSystemExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Newtonsoft.Json;
using PCLStorage;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class FileSystemExtensions
  {
    public static async Task<byte[]> ReadAllBytesAsync(this IFile file, CancellationToken token = default (CancellationToken))
    {
      Assert.ParamIsNotNull((object) file, nameof (file));
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        await file.CopyToAsync((Stream) memoryStream).ConfigureAwait(false);
        array = memoryStream.ToArray();
      }
      return array;
    }

    public static async Task WriteAllBytesAsync(
      this IFile file,
      byte[] bytes,
      CancellationToken token = default (CancellationToken))
    {
      await file.WriteStreamAsync((Stream) new MemoryStream(bytes), token).ConfigureAwait(false);
    }

    public static async Task CopyToAsync(
      this IFile file,
      Stream outputStream,
      CancellationToken token = default (CancellationToken))
    {
      Assert.ParamIsNotNull((object) file, nameof (file));
      Assert.ParamIsNotNull((object) outputStream, nameof (outputStream));
      using (Stream fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read, token).ConfigureAwait(false))
        await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
    }

    public static async Task CopyToAsync(
      this IFile file,
      IFile outputFile,
      CancellationToken token = default (CancellationToken))
    {
      Assert.ParamIsNotNull((object) file, nameof (file));
      Assert.ParamIsNotNull((object) outputFile, nameof (outputFile));
      using (Stream inputStream = await file.OpenAsync(PCLStorage.FileAccess.Read, token).ConfigureAwait(false))
      {
        using (Stream outputStream = await outputFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite, token).ConfigureAwait(false))
          await inputStream.CopyToAsync(outputStream).ConfigureAwait(false);
      }
    }

    public static async Task WriteStreamAsync(
      this IFile file,
      Stream stream,
      CancellationToken token = default (CancellationToken))
    {
      using (Stream fileStream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite, token).ConfigureAwait(false))
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }

    public static async Task WriteJsonAsync(
      this IFile file,
      object value,
      CancellationToken token = default (CancellationToken))
    {
      Assert.ParamIsNotNull((object) file, nameof (file));
      using (MemoryStream stream = new MemoryStream())
      {
        using (StreamWriter writer = new StreamWriter((Stream) stream))
        {
          JsonSerializer.CreateDefault().Serialize((TextWriter) writer, value);
          token.ThrowIfCancellationRequested();
          await writer.FlushAsync().ConfigureAwait(false);
          token.ThrowIfCancellationRequested();
          await stream.FlushAsync().ConfigureAwait(false);
          stream.Seek(0L, SeekOrigin.Begin);
          token.ThrowIfCancellationRequested();
          await file.WriteStreamAsync((Stream) stream, token).ConfigureAwait(false);
        }
      }
    }

    public static async Task<IFile> TryGetFileAsync(
      this IFolder folder,
      string name,
      CancellationToken token = default (CancellationToken))
    {
      Assert.ParamIsNotNull((object) folder, nameof (folder));
      Assert.ParamIsNotNull((object) name, nameof (name));
      try
      {
        return await folder.GetFileAsync(name, token).ConfigureAwait(false);
      }
      catch (PCLStorage.Exceptions.FileNotFoundException ex)
      {
        return (IFile) null;
      }
    }

    public static async Task<IFolder> TryGetFolderAsync(
      this IFolder folder,
      string name,
      CancellationToken token = default (CancellationToken))
    {
      Assert.ParamIsNotNull((object) folder, nameof (folder));
      Assert.ParamIsNotNull((object) name, nameof (name));
      try
      {
        return await folder.GetFolderAsync(name, token).ConfigureAwait(false);
      }
      catch (PCLStorage.Exceptions.DirectoryNotFoundException ex)
      {
        return (IFolder) null;
      }
    }
  }
}
