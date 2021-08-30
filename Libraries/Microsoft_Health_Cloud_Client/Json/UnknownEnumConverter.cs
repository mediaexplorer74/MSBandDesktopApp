// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Json.UnknownEnumConverter
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Health.Cloud.Client.Json
{
  public class UnknownEnumConverter : JsonConverter
  {
    private JsonConverter delegateEnumConverter;
    private IDictionary<object, object> unknownEnumValueCache = (IDictionary<object, object>) new Dictionary<object, object>();

    public UnknownEnumConverter()
      : this((JsonConverter) new StringEnumConverter())
    {
    }

    public UnknownEnumConverter(JsonConverter delegateEnumConverter) => this.delegateEnumConverter = delegateEnumConverter;

    public override bool CanConvert(Type objectType) => this.delegateEnumConverter.CanConvert(objectType);

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => this.delegateEnumConverter.WriteJson(writer, value, serializer);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      try
      {
        object obj = this.delegateEnumConverter.ReadJson(reader, objectType, existingValue, serializer);
        if (obj is Enum)
        {
          if (Enum.IsDefined(obj.GetType(), obj))
            return obj;
        }
      }
      catch (Exception ex)
      {
        if (reader.TokenType != JsonToken.String)
        {
          if (reader.TokenType != JsonToken.Integer)
          {
            if (reader.TokenType != JsonToken.Null)
            {
              if (reader.TokenType != JsonToken.Float)
                throw;
            }
          }
        }
      }
      try
      {
        return this.GetUnknownEnumValue(objectType);
      }
      catch (Exception ex)
      {
        int num;
        string str1;
        if (!(reader is JsonTextReader jsonTextReader1))
        {
          str1 = "?";
        }
        else
        {
          num = jsonTextReader1.LineNumber;
          str1 = num.ToString();
        }
        string str2 = str1;
        string str3;
        if (jsonTextReader1 == null)
        {
          str3 = "?";
        }
        else
        {
          num = jsonTextReader1.LinePosition;
          str3 = num.ToString();
        }
        string str4 = str3;
        throw new JsonSerializationException(string.Format("Error converting value \"{0}\" to type '{1}'. Path '{2}', line {3}, position {4}.", reader.Value, (object) objectType.FullName, (object) reader.Path, (object) str2, (object) str4), ex);
      }
    }

    private object GetUnknownEnumValue(Type enumType)
    {
      object obj;
      if (!this.unknownEnumValueCache.TryGetValue((object) enumType, out obj))
      {
        obj = UnknownEnumConverter.ReflectUnknownEnumValue(enumType);
        this.unknownEnumValueCache[(object) enumType] = obj;
      }
      return obj;
    }

    private static object ReflectUnknownEnumValue(Type enumType)
    {
      FieldInfo[] array = enumType.GetTypeInfo().DeclaredFields.Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.CustomAttributes.Any<CustomAttributeData>((Func<CustomAttributeData, bool>) (a => (object) a.AttributeType == (object) typeof (UnknownEnumValueAttribute))))).ToArray<FieldInfo>();
      if (array.Length == 1)
        return ((IEnumerable<FieldInfo>) array).First<FieldInfo>().GetValue((object) null);
      if (array.Length > 1)
        throw new InvalidOperationException(string.Format("Found multiple enum values marked as [UnknownValue] for enum type '{0}'", new object[1]
        {
          (object) enumType.FullName
        }));
      throw new InvalidOperationException(string.Format("There was no enum value marked as [UnknownValue] for enum type '{0}'", new object[1]
      {
        (object) enumType.FullName
      }));
    }
  }
}
