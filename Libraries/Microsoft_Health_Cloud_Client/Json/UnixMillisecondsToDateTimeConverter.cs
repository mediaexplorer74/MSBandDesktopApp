// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Json.UnixMillisecondsToDateTimeConverter
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Newtonsoft.Json;
using System;

namespace Microsoft.Health.Cloud.Client.Json
{
  public class UnixMillisecondsToDateTimeConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType) => (object) objectType == (object) typeof (DateTime);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      return (object) new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double) (long) reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DateTime dateTime = (DateTime) value;
      writer.WriteValue(dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
    }
  }
}
