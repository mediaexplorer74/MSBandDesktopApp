// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
  public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
  {
    public CamelCasePropertyNamesContractResolver()
      : base(true)
    {
    }

    protected override string ResolvePropertyName(string propertyName) => StringUtils.ToCamelCase(propertyName);
  }
}
