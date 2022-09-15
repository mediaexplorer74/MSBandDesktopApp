// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.DefaultHttpResponseContent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public class DefaultHttpResponseContent : IHttpResponseContent
  {
    private DefaultHttpResponseContent()
    {
    }

    public byte[] ResponseBytes { get; private set; }

    public string CharacterSet { get; private set; }

    public string ContentEncoding { get; private set; }

    public static async Task<DefaultHttpResponseContent> CreateAsync(
      HttpResponseMessage response)
    {
      DefaultHttpResponseContent result = new DefaultHttpResponseContent();
      int size = (int) (response.Content.Headers.ContentLength ?? 0L);
      MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
      if (contentType != null)
        result.CharacterSet = contentType.CharSet;
      string str = response.Content.Headers.ContentEncoding.FirstOrDefault<string>();
      if (str != null)
        result.ContentEncoding = str;
      using (Stream responseStream = await response.Content.ReadAsStreamAsync())
      {
        using (MemoryStream memoryStream = size > 0 ? new MemoryStream(size) : new MemoryStream())
        {
          await responseStream.CopyToAsync((Stream) memoryStream);
          result.ResponseBytes = memoryStream.ToArray();
        }
      }
      return result;
    }
  }
}
