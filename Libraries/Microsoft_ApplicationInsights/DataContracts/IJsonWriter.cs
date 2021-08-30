// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.IJsonWriter
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.DataContracts
{
  public interface IJsonWriter
  {
    void WriteStartArray();

    void WriteStartObject();

    void WriteEndArray();

    void WriteEndObject();

    void WriteComma();

    void WriteProperty(string name, string value);

    void WriteProperty(string name, bool? value);

    void WriteProperty(string name, int? value);

    void WriteProperty(string name, double? value);

    void WriteProperty(string name, TimeSpan? value);

    void WriteProperty(string name, DateTimeOffset? value);

    void WriteProperty(string name, IDictionary<string, double> values);

    void WriteProperty(string name, IDictionary<string, string> values);

    void WriteProperty(string name, IJsonSerializable value);

    void WritePropertyName(string name);

    void WriteRawValue(object value);
  }
}
