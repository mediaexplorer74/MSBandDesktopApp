// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Extensions.UriExtensions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.Cloud.Client.Extensions
{
  public static class UriExtensions
  {
    public static IDictionary<string, string> ParseQueryWithKeysLowerCase(this Uri uri) => (IDictionary<string, string>) UriExtensions.KeyValueStringToDictionary(uri.Query.Length > 0 ? uri.Query.Substring(1) : string.Empty).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key == null ? (string) null : pair.Key.ToLowerInvariant()), (Func<KeyValuePair<string, string>, string>) (pair => pair.Value));

    public static IDictionary<string, string> ParseQuery(this Uri uri) => UriExtensions.KeyValueStringToDictionary(uri.Query.Length > 0 ? uri.Query.Substring(1) : string.Empty);

    public static IDictionary<string, string> ParseFragmentAsQuery(this Uri uri) => UriExtensions.KeyValueStringToDictionary(uri.Fragment.Substring(1));

    private static IDictionary<string, string> KeyValueStringToDictionary(
      string keyValueString)
    {
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      int length = keyValueString.Length;
      for (int index = 0; index < length; ++index)
      {
        int startIndex = index;
        int num = -1;
        for (; index < length; ++index)
        {
          switch (keyValueString[index])
          {
            case '&':
              goto label_7;
            case '=':
              if (num < 0)
              {
                num = index;
                break;
              }
              break;
          }
        }
label_7:
        string stringToUnescape1 = (string) null;
        string stringToUnescape2;
        if (num >= 0)
        {
          stringToUnescape1 = keyValueString.Substring(startIndex, num - startIndex);
          stringToUnescape2 = keyValueString.Substring(num + 1, index - num - 1);
        }
        else
          stringToUnescape2 = keyValueString.Substring(startIndex, index - startIndex);
        dictionary.Add(Uri.UnescapeDataString(stringToUnescape1), Uri.UnescapeDataString(stringToUnescape2));
        if (index == length - 1 && keyValueString[index] == '&')
          dictionary.Add((string) null, Uri.UnescapeDataString(stringToUnescape2));
      }
      return dictionary;
    }
  }
}
