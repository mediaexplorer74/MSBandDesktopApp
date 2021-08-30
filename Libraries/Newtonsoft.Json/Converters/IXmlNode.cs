// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlNode
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlNode
  {
    XmlNodeType NodeType { get; }

    string LocalName { get; }

    IList<IXmlNode> ChildNodes { get; }

    IList<IXmlNode> Attributes { get; }

    IXmlNode ParentNode { get; }

    string Value { get; set; }

    IXmlNode AppendChild(IXmlNode newChild);

    string NamespaceUri { get; }

    object WrappedNode { get; }
  }
}
