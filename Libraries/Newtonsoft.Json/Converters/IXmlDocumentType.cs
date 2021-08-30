// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlDocumentType
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlDocumentType : IXmlNode
  {
    string Name { get; }

    string System { get; }

    string Public { get; }

    string InternalSubset { get; }
  }
}
