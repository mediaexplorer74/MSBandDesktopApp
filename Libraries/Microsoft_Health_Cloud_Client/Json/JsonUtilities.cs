// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Json.JsonUtilities
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Newtonsoft.Json;
using System;
using System.IO;

namespace Microsoft.Health.Cloud.Client.Json
{
  internal static class JsonUtilities
  {
    private static readonly Lazy<JsonSerializerSettings> LazySerializerSettings = new Lazy<JsonSerializerSettings>(new Func<JsonSerializerSettings>(JsonUtilities.CreateSerializerSettings));

    private static JsonSerializerSettings CreateSerializerSettings()
    {
      JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
      serializerSettings.Converters.Add((JsonConverter) new UnknownEnumConverter());
      return serializerSettings;
    }

    public static string SerializeObject(object item) => JsonConvert.SerializeObject(item, JsonUtilities.LazySerializerSettings.Value);

    public static T DeserializeObject<T>(string value) => JsonConvert.DeserializeObject<T>(value, JsonUtilities.LazySerializerSettings.Value);

    public static void SerializeObject(object item, TextWriter writer) => JsonSerializer.Create(JsonUtilities.LazySerializerSettings.Value).Serialize(writer, item);

    public static T DeserializeObject<T>(TextReader reader)
    {
      JsonTextReader jsonTextReader = new JsonTextReader(reader);
      return JsonSerializer.Create(JsonUtilities.LazySerializerSettings.Value).Deserialize<T>((JsonReader) jsonTextReader);
    }
  }
}
