// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.JsonWriter
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal class JsonWriter : IJsonWriter
  {
    private readonly JsonWriter.EmptyObjectDetector emptyObjectDetector;
    private readonly TextWriter textWriter;
    private bool currentObjectHasProperties;

    internal JsonWriter(TextWriter textWriter)
    {
      this.emptyObjectDetector = new JsonWriter.EmptyObjectDetector();
      this.textWriter = textWriter;
    }

    public void WriteStartArray() => this.textWriter.Write('[');

    public void WriteStartObject()
    {
      this.textWriter.Write('{');
      this.currentObjectHasProperties = false;
    }

    public void WriteEndArray() => this.textWriter.Write(']');

    public void WriteEndObject() => this.textWriter.Write('}');

    public void WriteComma() => this.textWriter.Write(',');

    public void WriteRawValue(object value) => this.textWriter.Write(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}", value));

    public void WriteProperty(string name, string value)
    {
      if (string.IsNullOrEmpty(value))
        return;
      this.WritePropertyName(name);
      this.WriteString(value);
    }

    public void WriteProperty(string name, bool? value)
    {
      if (!value.HasValue)
        return;
      this.WritePropertyName(name);
      this.textWriter.Write(value.Value ? "true" : "false");
    }

    public void WriteProperty(string name, int? value)
    {
      if (!value.HasValue)
        return;
      this.WritePropertyName(name);
      this.textWriter.Write(value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public void WriteProperty(string name, double? value)
    {
      if (!value.HasValue)
        return;
      this.WritePropertyName(name);
      this.textWriter.Write(value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public void WriteProperty(string name, TimeSpan? value)
    {
      if (!value.HasValue)
        return;
      this.WriteProperty(name, value.Value.ToString(CultureInfo.InvariantCulture, string.Empty));
    }

    public void WriteProperty(string name, DateTimeOffset? value)
    {
      if (!value.HasValue)
        return;
      this.WriteProperty(name, value.Value.ToString("o", (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public void WriteProperty(string name, IJsonSerializable value)
    {
      if (this.IsNullOrEmpty(value))
        return;
      this.WritePropertyName(name);
      value.Serialize((IJsonWriter) this);
    }

    public void WriteProperty(string name, IDictionary<string, double> values)
    {
      if (values == null || values.Count <= 0)
        return;
      this.WritePropertyName(name);
      this.WriteStartObject();
      foreach (KeyValuePair<string, double> keyValuePair in (IEnumerable<KeyValuePair<string, double>>) values)
        this.WriteProperty(keyValuePair.Key, new double?(keyValuePair.Value));
      this.WriteEndObject();
    }

    public void WriteProperty(string name, IDictionary<string, string> values)
    {
      if (values == null || values.Count <= 0)
        return;
      this.WritePropertyName(name);
      this.WriteStartObject();
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) values)
      {
        if (keyValuePair.Value != null)
          this.WriteProperty(keyValuePair.Key, keyValuePair.Value);
      }
      this.WriteEndObject();
    }

    public void WritePropertyName(string name)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException(nameof (name));
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          if (this.currentObjectHasProperties)
            this.textWriter.Write(',');
          else
            this.currentObjectHasProperties = true;
          this.WriteString(name);
          this.textWriter.Write(':');
          break;
      }
    }

    protected bool IsNullOrEmpty(IJsonSerializable instance)
    {
      if (instance == null)
        return true;
      this.emptyObjectDetector.IsEmpty = true;
      instance.Serialize((IJsonWriter) this.emptyObjectDetector);
      return this.emptyObjectDetector.IsEmpty;
    }

    protected void WriteString(string value)
    {
      this.textWriter.Write('"');
      foreach (char ch in value)
      {
        switch (ch)
        {
          case '\b':
            this.textWriter.Write("\\b");
            break;
          case '\t':
            this.textWriter.Write("\\t");
            break;
          case '\n':
            this.textWriter.Write("\\n");
            break;
          case '\f':
            this.textWriter.Write("\\f");
            break;
          case '\r':
            this.textWriter.Write("\\r");
            break;
          case '"':
            this.textWriter.Write("\\\"");
            break;
          case '\\':
            this.textWriter.Write("\\\\");
            break;
          default:
            this.textWriter.Write(ch);
            break;
        }
      }
      this.textWriter.Write('"');
    }

    private sealed class EmptyObjectDetector : IJsonWriter
    {
      public bool IsEmpty { get; set; }

      public void WriteStartArray()
      {
      }

      public void WriteStartObject()
      {
      }

      public void WriteEndArray()
      {
      }

      public void WriteEndObject()
      {
      }

      public void WriteComma()
      {
      }

      public void WriteProperty(string name, string value)
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, bool? value)
      {
        if (!value.HasValue)
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, int? value)
      {
        if (!value.HasValue)
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, double? value)
      {
        if (!value.HasValue)
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, TimeSpan? value)
      {
        if (!value.HasValue)
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, DateTimeOffset? value)
      {
        if (!value.HasValue)
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, IJsonSerializable value) => value?.Serialize((IJsonWriter) this);

      public void WriteProperty(string name, IDictionary<string, double> value)
      {
        if (value == null || value.Count <= 0)
          return;
        this.IsEmpty = false;
      }

      public void WriteProperty(string name, IDictionary<string, string> value)
      {
        if (value == null || value.Count <= 0)
          return;
        this.IsEmpty = false;
      }

      public void WritePropertyName(string name)
      {
      }

      public void WriteRawValue(object value)
      {
        if (value == null)
          return;
        this.IsEmpty = false;
      }
    }
  }
}
