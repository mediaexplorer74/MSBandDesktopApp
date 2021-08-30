// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
  {
    internal XDeclaration Declaration { get; private set; }

    public XDeclarationWrapper(XDeclaration declaration)
      : base((XObject) null)
    {
      this.Declaration = declaration;
    }

    public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

    public string Version => this.Declaration.Version;

    public string Encoding
    {
      get => this.Declaration.Encoding;
      set => this.Declaration.Encoding = value;
    }

    public string Standalone
    {
      get => this.Declaration.Standalone;
      set => this.Declaration.Standalone = value;
    }
  }
}
