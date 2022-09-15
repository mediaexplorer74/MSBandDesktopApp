// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.CentimetersPerSecondToSpeedConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Health.Cloud.Client;
using Newtonsoft.Json;
using System;

namespace DesktopSyncApp
{
    /*
  internal class CentimetersPerSecondToSpeedConverter : JsonConverter
  {

      public virtual bool CanConvert(Type objectType)
      {
            return objectType == typeof(Speed);
      }

      public virtual object ReadJson
      (
          JsonReader reader,
          Type objectType,
          object existingValue,
          JsonSerializer serializer
        )
        {
          return (object) Speed.FromCentimetersPerSecond(Convert.ToDouble(reader.Value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
          Speed speed = (Speed) value;
          writer.WriteValue((int) ((Speed) speed).CentimetersPerSecond);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    */
}
