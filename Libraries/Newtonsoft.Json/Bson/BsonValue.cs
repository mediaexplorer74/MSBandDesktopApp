// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonValue
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonValue : BsonToken
  {
    private readonly object _value;
    private readonly BsonType _type;

    public BsonValue(object value, BsonType type)
    {
      this._value = value;
      this._type = type;
    }

    public object Value => this._value;

    public override BsonType Type => this._type;
  }
}
