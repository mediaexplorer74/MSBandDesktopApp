// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenWriter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Numerics;

namespace Newtonsoft.Json.Linq
{
  public class JTokenWriter : JsonWriter
  {
    private JContainer _token;
    private JContainer _parent;
    private JValue _value;
    private JToken _current;

    public JToken CurrentToken => this._current;

    public JToken Token => this._token != null ? (JToken) this._token : (JToken) this._value;

    public JTokenWriter(JContainer container)
    {
      ValidationUtils.ArgumentNotNull((object) container, nameof (container));
      this._token = container;
      this._parent = container;
    }

    public JTokenWriter()
    {
    }

    public override void Flush()
    {
    }

    public override void Close() => base.Close();

    public override void WriteStartObject()
    {
      base.WriteStartObject();
      this.AddParent((JContainer) new JObject());
    }

    private void AddParent(JContainer container)
    {
      if (this._parent == null)
        this._token = container;
      else
        this._parent.AddAndSkipParentCheck((JToken) container);
      this._parent = container;
      this._current = (JToken) container;
    }

    private void RemoveParent()
    {
      this._current = (JToken) this._parent;
      this._parent = this._parent.Parent;
      if (this._parent == null || this._parent.Type != JTokenType.Property)
        return;
      this._parent = this._parent.Parent;
    }

    public override void WriteStartArray()
    {
      base.WriteStartArray();
      this.AddParent((JContainer) new JArray());
    }

    public override void WriteStartConstructor(string name)
    {
      base.WriteStartConstructor(name);
      this.AddParent((JContainer) new JConstructor(name));
    }

    protected override void WriteEnd(JsonToken token) => this.RemoveParent();

    public override void WritePropertyName(string name)
    {
      this.AddParent((JContainer) new JProperty(name));
      base.WritePropertyName(name);
    }

    private void AddValue(object value, JsonToken token) => this.AddValue(new JValue(value), token);

    internal void AddValue(JValue value, JsonToken token)
    {
      if (this._parent != null)
      {
        this._parent.Add((object) value);
        this._current = this._parent.Last;
        if (this._parent.Type != JTokenType.Property)
          return;
        this._parent = this._parent.Parent;
      }
      else
      {
        this._value = value ?? JValue.CreateNull();
        this._current = (JToken) this._value;
      }
    }

    public override void WriteValue(object value)
    {
      if (value is BigInteger)
      {
        this.InternalWriteValue(JsonToken.Integer);
        this.AddValue(value, JsonToken.Integer);
      }
      else
        base.WriteValue(value);
    }

    public override void WriteNull()
    {
      base.WriteNull();
      this.AddValue((JValue) null, JsonToken.Null);
    }

    public override void WriteUndefined()
    {
      base.WriteUndefined();
      this.AddValue((JValue) null, JsonToken.Undefined);
    }

    public override void WriteRaw(string json)
    {
      base.WriteRaw(json);
      this.AddValue((JValue) new JRaw((object) json), JsonToken.Raw);
    }

    public override void WriteComment(string text)
    {
      base.WriteComment(text);
      this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
    }

    public override void WriteValue(string value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    public override void WriteValue(int value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(uint value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    public override void WriteValue(long value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ulong value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    public override void WriteValue(float value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Float);
    }

    public override void WriteValue(double value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Float);
    }

    public override void WriteValue(bool value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Boolean);
    }

    public override void WriteValue(short value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ushort value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    public override void WriteValue(char value)
    {
      base.WriteValue(value);
      this.AddValue((object) value.ToString((IFormatProvider) CultureInfo.InvariantCulture), JsonToken.String);
    }

    public override void WriteValue(byte value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(sbyte value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Integer);
    }

    public override void WriteValue(Decimal value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Float);
    }

    public override void WriteValue(DateTime value)
    {
      base.WriteValue(value);
      value = DateTimeUtils.EnsureDateTime(value, this.DateTimeZoneHandling);
      this.AddValue((object) value, JsonToken.Date);
    }

    public override void WriteValue(DateTimeOffset value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Date);
    }

    public override void WriteValue(byte[] value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.Bytes);
    }

    public override void WriteValue(TimeSpan value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    public override void WriteValue(Guid value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }

    public override void WriteValue(Uri value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, JsonToken.String);
    }
  }
}
