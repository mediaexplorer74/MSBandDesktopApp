// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DataSetConverter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Data;

namespace Newtonsoft.Json.Converters
{
  public class DataSetConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DataSet dataSet = (DataSet) value;
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      DataTableConverter dataTableConverter = new DataTableConverter();
      writer.WriteStartObject();
      foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
      {
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(table.TableName) : table.TableName);
        dataTableConverter.WriteJson(writer, (object) table, serializer);
      }
      writer.WriteEndObject();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      DataSet dataSet = objectType == typeof (DataSet) ? new DataSet() : (DataSet) Activator.CreateInstance(objectType);
      DataTableConverter dataTableConverter = new DataTableConverter();
      this.CheckedRead(reader);
      while (reader.TokenType == JsonToken.PropertyName)
      {
        DataTable table1 = dataSet.Tables[(string) reader.Value];
        bool flag = table1 != null;
        DataTable table2 = (DataTable) dataTableConverter.ReadJson(reader, typeof (DataTable), (object) table1, serializer);
        if (!flag)
          dataSet.Tables.Add(table2);
        this.CheckedRead(reader);
      }
      return (object) dataSet;
    }

    public override bool CanConvert(Type valueType) => typeof (DataSet).IsAssignableFrom(valueType);

    private void CheckedRead(JsonReader reader)
    {
      if (!reader.Read())
        throw JsonSerializationException.Create(reader, "Unexpected end when reading DataSet.");
    }
  }
}
