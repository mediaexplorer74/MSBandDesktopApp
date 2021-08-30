// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonString
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonString : BsonValue
  {
    public int ByteCount { get; set; }

    public bool IncludeLength { get; set; }

    public BsonString(object value, bool includeLength)
      : base(value, BsonType.String)
    {
      this.IncludeLength = includeLength;
    }
  }
}
