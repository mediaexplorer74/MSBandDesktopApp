// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.HttpResponseExtensions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.Health.Cloud.Client.Http
{
  public static class HttpResponseExtensions
  {
    public static string ReadString(this IHttpResponseContent response)
    {
      string characterSet = response.CharacterSet;
      try
      {
        Encoding encoding = Encoding.GetEncoding(characterSet);
        using (StreamReader streamReader = new StreamReader((Stream) new MemoryStream(response.ResponseBytes), encoding))
          return streamReader.ReadToEnd();
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException("Character set is missing or invalid: " + characterSet, (Exception) ex);
      }
    }

    public static T ReadJson<T>(this IHttpResponseContent response) => JsonUtilities.DeserializeObject<T>(response.ReadString());

    public static T ReadXml<T>(this IHttpResponseContent response)
    {
      string s = response.ReadString();
      using (StringReader stringReader = new StringReader(s))
        return (T) new XmlSerializer(typeof (T)).Deserialize((TextReader) stringReader);
    }
  }
}
