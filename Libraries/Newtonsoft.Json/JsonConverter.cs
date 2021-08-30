// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Schema;
using System;

namespace Newtonsoft.Json
{
  public abstract class JsonConverter
  {
    public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

    public abstract object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer);

    public abstract bool CanConvert(Type objectType);

    [Obsolete("JSON Schema validation has been moved to its own package. It is strongly recommended that you do not override GetSchema() in your own converter. It is not used by Json.NET and will be removed at some point in the future. Converter's that override GetSchema() will stop working when it is removed.")]
    public virtual JsonSchema GetSchema() => (JsonSchema) null;

    public virtual bool CanRead => true;

    public virtual bool CanWrite => true;
  }
}
