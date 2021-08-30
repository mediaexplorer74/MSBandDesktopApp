// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XCommentWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XCommentWrapper : XObjectWrapper
  {
    private XComment Text => (XComment) this.WrappedNode;

    public XCommentWrapper(XComment text)
      : base((XObject) text)
    {
    }

    public override string Value
    {
      get => this.Text.Value;
      set => this.Text.Value = value;
    }

    public override IXmlNode ParentNode => this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
  }
}
