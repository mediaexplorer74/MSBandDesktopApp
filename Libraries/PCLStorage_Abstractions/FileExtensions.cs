// Decompiled with JetBrains decompiler
// Type: PCLStorage.FileExtensions
// Assembly: PCLStorage.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=286fe515a2c35b64
// MVID: 1BC1AC1B-67B7-41CB-A202-40E4FD631624
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\PCLStorage_Abstractions.dll

using System.IO;
using System.Threading.Tasks;

namespace PCLStorage
{
  public static class FileExtensions
  {
    public static async Task<string> ReadAllTextAsync(this IFile file)
    {
      string str;
      using (Stream stream = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false))
      {
        using (StreamReader sr = new StreamReader(stream))
          str = await sr.ReadToEndAsync().ConfigureAwait(false);
      }
      return str;
    }

    public static async Task WriteAllTextAsync(this IFile file, string contents)
    {
      using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
      {
        stream.SetLength(0L);
        using (StreamWriter sw = new StreamWriter(stream))
          await sw.WriteAsync(contents).ConfigureAwait(false);
      }
    }
  }
}
