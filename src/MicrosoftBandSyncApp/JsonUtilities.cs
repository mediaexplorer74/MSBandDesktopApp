// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.JsonUtilities
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DesktopSyncApp
{
  internal static class JsonUtilities
  {
    private static readonly Lazy<JsonSerializerSettings> LazySerializerSettings = new Lazy<JsonSerializerSettings>(new Func<JsonSerializerSettings>(JsonUtilities.CreateSerializerSettings));

    private static JsonSerializerSettings CreateSerializerSettings()
    {
      JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
      ((ICollection<JsonConverter>) serializerSettings.Converters).Add((JsonConverter) new UnknownEnumConverter());
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
