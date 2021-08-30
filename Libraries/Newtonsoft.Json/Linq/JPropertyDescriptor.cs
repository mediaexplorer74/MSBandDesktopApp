// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JPropertyDescriptor
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.ComponentModel;

namespace Newtonsoft.Json.Linq
{
  public class JPropertyDescriptor : PropertyDescriptor
  {
    public JPropertyDescriptor(string name)
      : base(name, (Attribute[]) null)
    {
    }

    private static JObject CastInstance(object instance) => (JObject) instance;

    public override bool CanResetValue(object component) => false;

    public override object GetValue(object component) => (object) JPropertyDescriptor.CastInstance(component)[this.Name];

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value)
    {
      JToken jtoken = value is JToken ? (JToken) value : (JToken) new JValue(value);
      JPropertyDescriptor.CastInstance(component)[this.Name] = jtoken;
    }

    public override bool ShouldSerializeValue(object component) => false;

    public override Type ComponentType => typeof (JObject);

    public override bool IsReadOnly => false;

    public override Type PropertyType => typeof (object);

    protected override int NameHashCode => base.NameHashCode;
  }
}
