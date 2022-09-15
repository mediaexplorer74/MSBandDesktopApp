// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Json.UserEventConverter
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Events.Golf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.Cloud.Client.Json
{
  public class UserEventConverter : JsonConverter
  {
    private static Dictionary<string, Type> typeMap = new Dictionary<string, Type>()
    {
      {
        "Microsoft.Khronos.Cloud.Ods.Data.Entities.SleepEventDTO",
        typeof (SleepEvent)
      },
      {
        "Microsoft.Khronos.Cloud.Ods.Data.Entities.UserWorkoutEventDTO",
        typeof (ExerciseEvent)
      },
      {
        "Microsoft.Khronos.Cloud.Ods.Data.Entities.UserRunEventDTO",
        typeof (RunEvent)
      },
      {
        "Microsoft.Khronos.Cloud.Ods.Data.Entities.UserBikeEventDTO",
        typeof (BikeEvent)
      },
      {
        "Microsoft.Khronos.Cloud.Ods.Data.Entities.GolfEventDTO",
        typeof (GolfEvent)
      },
      {
        "Microsoft.Khronos.Cloud.Ods.Data.Entities.UserGuidedWorkoutEventDTO",
        typeof (WorkoutEvent)
      },
      {
        "Microsoft.Khronos.Cloud.Ods.Data.TransferObjects.Event.Hike.HikeEventDTO",
        typeof (HikeEvent)
      }
    };

    public override bool CanConvert(Type objectType) => (object) objectType == (object) typeof (UserEvent);

    public override bool CanWrite => false;

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      JObject jObject = JObject.Load(reader);
      UserEvent derivedObject = this.CreateDerivedObject(jObject, objectType);
      serializer.Populate(jObject.CreateReader(), (object) derivedObject);
      return (object) derivedObject;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

    private UserEvent CreateDerivedObject(JObject jObject, Type objectType)
    {
      if (jObject == null)
        return (UserEvent) null;
      if (!objectType.Equals(typeof (UserEvent)))
        return (UserEvent) Activator.CreateInstance(objectType);
      string key = jObject.Value<string>((object) "odata.type");
      Type type = (Type) null;
      return key != null && UserEventConverter.typeMap.TryGetValue(key, out type) ? (UserEvent) Activator.CreateInstance(type) : new UserEvent();
    }
  }
}
