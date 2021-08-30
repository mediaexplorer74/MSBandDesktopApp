// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.EntityKeyMemberConverter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class EntityKeyMemberConverter : JsonConverter
  {
    private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";
    private const string KeyPropertyName = "Key";
    private const string TypePropertyName = "Type";
    private const string ValuePropertyName = "Value";
    private static ReflectionObject _reflectionObject;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      string str = (string) EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
      object obj = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
      Type type = obj?.GetType();
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key");
      writer.WriteValue(str);
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Type") : "Type");
      writer.WriteValue(type != (Type) null ? type.FullName : (string) null);
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value");
      if (type != (Type) null)
      {
        string s;
        if (JsonSerializerInternalWriter.TryConvertToString(obj, type, out s))
          writer.WriteValue(s);
        else
          writer.WriteValue(obj);
      }
      else
        writer.WriteNull();
      writer.WriteEndObject();
    }

    private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
    {
      EntityKeyMemberConverter.ReadAndAssert(reader);
      if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
        throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
    }

    private static void ReadAndAssert(JsonReader reader)
    {
      if (!reader.Read())
        throw new JsonSerializationException("Unexpected end.");
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      EntityKeyMemberConverter.EnsureReflectionObject(objectType);
      object target = EntityKeyMemberConverter._reflectionObject.Creator(new object[0]);
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
      EntityKeyMemberConverter.ReadAndAssert(reader);
      EntityKeyMemberConverter._reflectionObject.SetValue(target, "Key", (object) reader.Value.ToString());
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
      EntityKeyMemberConverter.ReadAndAssert(reader);
      Type type = Type.GetType(reader.Value.ToString());
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
      EntityKeyMemberConverter.ReadAndAssert(reader);
      EntityKeyMemberConverter._reflectionObject.SetValue(target, "Value", serializer.Deserialize(reader, type));
      EntityKeyMemberConverter.ReadAndAssert(reader);
      return target;
    }

    private static void EnsureReflectionObject(Type objectType)
    {
      if (EntityKeyMemberConverter._reflectionObject != null)
        return;
      EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, "Key", "Value");
    }

    public override bool CanConvert(Type objectType) => objectType.AssignableToTypeName("System.Data.EntityKeyMember");
  }
}
