// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XElementWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
  {
    private XElement Element => (XElement) this.WrappedNode;

    public XElementWrapper(XElement element)
      : base((XContainer) element)
    {
    }

    public void SetAttributeNode(IXmlNode attribute) => this.Element.Add(((XObjectWrapper) attribute).WrappedNode);

    public override IList<IXmlNode> Attributes => (IList<IXmlNode>) this.Element.Attributes().Select<XAttribute, XAttributeWrapper>((Func<XAttribute, XAttributeWrapper>) (a => new XAttributeWrapper(a))).Cast<IXmlNode>().ToList<IXmlNode>();

    public override string Value
    {
      get => this.Element.Value;
      set => this.Element.Value = value;
    }

    public override string LocalName => this.Element.Name.LocalName;

    public override string NamespaceUri => this.Element.Name.NamespaceName;

    public string GetPrefixOfNamespace(string namespaceUri) => this.Element.GetPrefixOfNamespace((XNamespace) namespaceUri);

    public bool IsEmpty => this.Element.IsEmpty;
  }
}
