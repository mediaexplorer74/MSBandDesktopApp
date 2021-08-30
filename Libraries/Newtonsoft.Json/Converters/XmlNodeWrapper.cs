// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlNodeWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlNodeWrapper : IXmlNode
  {
    private readonly XmlNode _node;
    private IList<IXmlNode> _childNodes;

    public XmlNodeWrapper(XmlNode node) => this._node = node;

    public object WrappedNode => (object) this._node;

    public XmlNodeType NodeType => this._node.NodeType;

    public virtual string LocalName => this._node.LocalName;

    public IList<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
          this._childNodes = (IList<IXmlNode>) this._node.ChildNodes.Cast<XmlNode>().Select<XmlNode, IXmlNode>(new Func<XmlNode, IXmlNode>(XmlNodeWrapper.WrapNode)).ToList<IXmlNode>();
        return this._childNodes;
      }
    }

    internal static IXmlNode WrapNode(XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return (IXmlNode) new XmlElementWrapper((XmlElement) node);
        case XmlNodeType.DocumentType:
          return (IXmlNode) new XmlDocumentTypeWrapper((XmlDocumentType) node);
        case XmlNodeType.XmlDeclaration:
          return (IXmlNode) new XmlDeclarationWrapper((XmlDeclaration) node);
        default:
          return (IXmlNode) new XmlNodeWrapper(node);
      }
    }

    public IList<IXmlNode> Attributes => this._node.Attributes == null ? (IList<IXmlNode>) null : (IList<IXmlNode>) this._node.Attributes.Cast<XmlAttribute>().Select<XmlAttribute, IXmlNode>(new Func<XmlAttribute, IXmlNode>(XmlNodeWrapper.WrapNode)).ToList<IXmlNode>();

    public IXmlNode ParentNode
    {
      get
      {
        XmlNode node = this._node is XmlAttribute ? (XmlNode) ((XmlAttribute) this._node).OwnerElement : this._node.ParentNode;
        return node == null ? (IXmlNode) null : XmlNodeWrapper.WrapNode(node);
      }
    }

    public string Value
    {
      get => this._node.Value;
      set => this._node.Value = value;
    }

    public IXmlNode AppendChild(IXmlNode newChild)
    {
      this._node.AppendChild(((XmlNodeWrapper) newChild)._node);
      this._childNodes = (IList<IXmlNode>) null;
      return newChild;
    }

    public string NamespaceUri => this._node.NamespaceURI;
  }
}
