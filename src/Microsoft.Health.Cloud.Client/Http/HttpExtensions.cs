// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.HttpExtensions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public static class HttpExtensions
  {
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
