// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.HttpExtensions
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public static class HttpExtensions
  {
    public const string JsonMediaType = "application/json";

    public static void SetJsonContent<T>(this HttpRequestMessage request, T body)
    {
      string content = JsonUtilities.SerializeObject((object) body);
      request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
    }

    public static async Task<T> ReadJsonAsync<T>(this HttpResponseMessage response)
    {
      T obj;
      using (Stream stream = await response.Content.ReadAsStreamAsync())
      {
        using (StreamReader streamReader = new StreamReader(stream))
          obj = JsonUtilities.DeserializeObject<T>((TextReader) streamReader);
      }
      return obj;
    }
  }
}
