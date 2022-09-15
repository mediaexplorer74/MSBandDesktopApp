// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.NameValueCollection
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Health.Cloud.Client.Http
{
  public class NameValueCollection : Dictionary<string, string>
  {
    public NameValueCollection()
    {
    }

    public NameValueCollection(IDictionary<string, string> original)
      : base(original)
    {
    }

    public string ToQueryString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in (Dictionary<string, string>) this)
      {
        string key = keyValuePair.Key;
        string stringToEscape = keyValuePair.Value;
        if (stringBuilder.Length > 0)
          stringBuilder.Append('&');
        stringBuilder.Append(Uri.EscapeDataString(key));
        stringBuilder.Append('=');
        stringBuilder.Append(Uri.EscapeDataString(stringToEscape));
      }
      return stringBuilder.ToString();
    }
  }
}
