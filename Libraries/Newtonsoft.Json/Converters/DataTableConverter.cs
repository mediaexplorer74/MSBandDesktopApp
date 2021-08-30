// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DataTableConverter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
  public class DataTableConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DataTable dataTable = (DataTable) value;
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartArray();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        writer.WriteStartObject();
        foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
        {
          object obj = row[column];
          if (serializer.NullValueHandling != NullValueHandling.Ignore || obj != null && obj != DBNull.Value)
          {
            writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
            serializer.Serialize(writer, obj);
          }
        }
        writer.WriteEndObject();
      }
      writer.WriteEndArray();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (!(existingValue is DataTable dt))
        dt = objectType == typeof (DataTable) ? new DataTable() : (DataTable) Activator.CreateInstance(objectType);
      if (reader.TokenType == JsonToken.PropertyName)
      {
        dt.TableName = (string) reader.Value;
        DataTableConverter.CheckedRead(reader);
      }
      if (reader.TokenType != JsonToken.StartArray)
        throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      DataTableConverter.CheckedRead(reader);
      while (reader.TokenType != JsonToken.EndArray)
      {
        DataTableConverter.CreateRow(reader, dt, serializer);
        DataTableConverter.CheckedRead(reader);
      }
      return (object) dt;
    }

    private static void CreateRow(JsonReader reader, DataTable dt, JsonSerializer serializer)
    {
      DataRow row = dt.NewRow();
      DataTableConverter.CheckedRead(reader);
      while (reader.TokenType == JsonToken.PropertyName)
      {
        string str = (string) reader.Value;
        DataTableConverter.CheckedRead(reader);
        DataColumn column = dt.Columns[str];
        if (column == null)
        {
          Type columnDataType = DataTableConverter.GetColumnDataType(reader);
          column = new DataColumn(str, columnDataType);
          dt.Columns.Add(column);
        }
        if (column.DataType == typeof (DataTable))
        {
          if (reader.TokenType == JsonToken.StartArray)
            DataTableConverter.CheckedRead(reader);
          DataTable dt1 = new DataTable();
          while (reader.TokenType != JsonToken.EndArray)
          {
            DataTableConverter.CreateRow(reader, dt1, serializer);
            DataTableConverter.CheckedRead(reader);
          }
          row[str] = (object) dt1;
        }
        else if (column.DataType.IsArray && column.DataType != typeof (byte[]))
        {
          if (reader.TokenType == JsonToken.StartArray)
            DataTableConverter.CheckedRead(reader);
          List<object> objectList = new List<object>();
          while (reader.TokenType != JsonToken.EndArray)
          {
            objectList.Add(reader.Value);
            DataTableConverter.CheckedRead(reader);
          }
          Array instance = Array.CreateInstance(column.DataType.GetElementType(), objectList.Count);
          Array.Copy((Array) objectList.ToArray(), instance, objectList.Count);
          row[str] = (object) instance;
        }
        else
          row[str] = reader.Value != null ? serializer.Deserialize(reader, column.DataType) : (object) DBNull.Value;
        DataTableConverter.CheckedRead(reader);
      }
      row.EndEdit();
      dt.Rows.Add(row);
    }

    private static Type GetColumnDataType(JsonReader reader)
    {
      JsonToken tokenType = reader.TokenType;
      switch (tokenType)
      {
        case JsonToken.StartArray:
          DataTableConverter.CheckedRead(reader);
          return reader.TokenType == JsonToken.StartObject ? typeof (DataTable) : DataTableConverter.GetColumnDataType(reader).MakeArrayType();
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Date:
        case JsonToken.Bytes:
          return reader.ValueType;
        case JsonToken.Null:
        case JsonToken.Undefined:
          return typeof (string);
        default:
          throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) tokenType));
      }
    }

    private static void CheckedRead(JsonReader reader)
    {
      if (!reader.Read())
        throw JsonSerializationException.Create(reader, "Unexpected end when reading DataTable.");
    }

    public override bool CanConvert(Type valueType) => typeof (DataTable).IsAssignableFrom(valueType);
  }
}
