// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.HttpDebugLogger
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client.Http;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class HttpDebugLogger
  {
    private const string Protocol = "HTTP";
    private const string ProtocolVersion = "1.1";

    public static void LogTransaction(IHttpTransaction transaction)
    {
    }

    private static string FormatTransaction(IHttpTransaction transaction)
    {
      HttpRequestMessage request = transaction.Request;
      HttpResponseMessage response = transaction.Response;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0} {1} {2}/{3}", (object) request.Method, (object) request.RequestUri, (object) "HTTP", (object) "1.1");
      stringBuilder.AppendLine();
      foreach (KeyValuePair<string, IEnumerable<string>> header in (HttpHeaders) request.Headers)
      {
        stringBuilder.AppendFormat("{0}: {1}", new object[2]
        {
          (object) header.Key,
          (object) header.Value
        });
        stringBuilder.AppendLine();
      }
      using (TextReader textReader = transaction.OpenRequestReader())
      {
        if (textReader != null)
        {
          string end = textReader.ReadToEnd();
          if (end.Length > 0)
          {
            stringBuilder.AppendLine();
            stringBuilder.Append(end);
            stringBuilder.AppendLine();
          }
        }
      }
      if (response != null)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("{0}/{1} {2} {3}", (object) "HTTP", (object) "1.1", (object) (int) response.StatusCode, (object) response.ReasonPhrase);
        stringBuilder.AppendLine();
        foreach (KeyValuePair<string, IEnumerable<string>> header in (HttpHeaders) response.Headers)
        {
          stringBuilder.AppendFormat("{0}: {1}", new object[2]
          {
            (object) header.Key,
            (object) header.Value
          });
          stringBuilder.AppendLine();
        }
        using (TextReader textReader = transaction.OpenResponseReader())
        {
          if (textReader != null)
          {
            string end = textReader.ReadToEnd();
            if (end.Length > 0)
            {
              stringBuilder.AppendLine();
              stringBuilder.Append(end);
              stringBuilder.AppendLine();
            }
          }
        }
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("--------------------------------------------");
      stringBuilder.AppendLine();
      return stringBuilder.ToString();
    }
  }
}
