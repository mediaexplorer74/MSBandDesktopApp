// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JConstructor
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  public class JConstructor : JContainer
  {
    private string _name;
    private readonly List<JToken> _values = new List<JToken>();

    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._values;

    internal override void MergeItem(object content, JsonMergeSettings settings)
    {
      if (!(content is JConstructor jconstructor))
        return;
      if (jconstructor.Name != null)
        this.Name = jconstructor.Name;
      JContainer.MergeEnumerableContent((JContainer) this, (IEnumerable) jconstructor, settings);
    }

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public override JTokenType Type => JTokenType.Constructor;

    public JConstructor()
    {
    }

    public JConstructor(JConstructor other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    public JConstructor(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    public JConstructor(string name, object content)
      : this(name)
    {
      this.Add(content);
    }

    public JConstructor(string name)
    {
      ValidationUtils.ArgumentNotNullOrEmpty(name, nameof (name));
      this._name = name;
    }

    internal override bool DeepEquals(JToken node) => node is JConstructor jconstructor && this._name == jconstructor.Name && this.ContentsEqual((JContainer) jconstructor);

    internal override JToken CloneToken() => (JToken) new JConstructor(this);

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartConstructor(this._name);
      foreach (JToken child in this.Children())
        child.WriteTo(writer, converters);
      writer.WriteEndConstructor();
    }

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        return key is int index ? this.GetItem(index) : throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is int index))
          throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem(index, value);
      }
    }

    internal override int GetDeepHashCode() => this._name.GetHashCode() ^ this.ContentsHashCode();

    public static JConstructor Load(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
      while (reader.TokenType == JsonToken.Comment)
        reader.Read();
      JConstructor jconstructor = reader.TokenType == JsonToken.StartConstructor ? new JConstructor((string) reader.Value) : throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      jconstructor.SetLineInfo(reader as IJsonLineInfo);
      jconstructor.ReadTokenFrom(reader);
      return jconstructor;
    }
  }
}
