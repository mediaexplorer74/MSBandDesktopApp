// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XObjectWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XObjectWrapper : IXmlNode
  {
    private readonly XObject _xmlObject;

    public XObjectWrapper(XObject xmlObject) => this._xmlObject = xmlObject;

    public object WrappedNode => (object) this._xmlObject;

    public virtual XmlNodeType NodeType => this._xmlObject.NodeType;

    public virtual string LocalName => (string) null;

    public virtual IList<IXmlNode> ChildNodes => (IList<IXmlNode>) new List<IXmlNode>();

    public virtual IList<IXmlNode> Attributes => (IList<IXmlNode>) null;

    public virtual IXmlNode ParentNode => (IXmlNode) null;

    public virtual string Value
    {
      get => (string) null;
      set => throw new InvalidOperationException();
    }

    public virtual IXmlNode AppendChild(IXmlNode newChild) => throw new InvalidOperationException();

    public virtual string NamespaceUri => (string) null;
  }
}
