// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XContainerWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XContainerWrapper : XObjectWrapper
  {
    private IList<IXmlNode> _childNodes;

    private XContainer Container => (XContainer) this.WrappedNode;

    public XContainerWrapper(XContainer container)
      : base((XObject) container)
    {
    }

    public override IList<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
          this._childNodes = (IList<IXmlNode>) this.Container.Nodes().Select<XNode, IXmlNode>(new Func<XNode, IXmlNode>(XContainerWrapper.WrapNode)).ToList<IXmlNode>();
        return this._childNodes;
      }
    }

    public override IXmlNode ParentNode => this.Container.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Container.Parent);

    internal static IXmlNode WrapNode(XObject node)
    {
      switch (node)
      {
        case XDocument _:
          return (IXmlNode) new XDocumentWrapper((XDocument) node);
        case XElement _:
          return (IXmlNode) new XElementWrapper((XElement) node);
        case XContainer _:
          return (IXmlNode) new XContainerWrapper((XContainer) node);
        case XProcessingInstruction _:
          return (IXmlNode) new XProcessingInstructionWrapper((XProcessingInstruction) node);
        case XText _:
          return (IXmlNode) new XTextWrapper((XText) node);
        case XComment _:
          return (IXmlNode) new XCommentWrapper((XComment) node);
        case XAttribute _:
          return (IXmlNode) new XAttributeWrapper((XAttribute) node);
        case XDocumentType _:
          return (IXmlNode) new XDocumentTypeWrapper((XDocumentType) node);
        default:
          return (IXmlNode) new XObjectWrapper(node);
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      this.Container.Add(newChild.WrappedNode);
      this._childNodes = (IList<IXmlNode>) null;
      return newChild;
    }
  }
}
