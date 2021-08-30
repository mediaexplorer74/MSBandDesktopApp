// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XProcessingInstructionWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XProcessingInstructionWrapper : XObjectWrapper
  {
    private XProcessingInstruction ProcessingInstruction => (XProcessingInstruction) this.WrappedNode;

    public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
      : base((XObject) processingInstruction)
    {
    }

    public override string LocalName => this.ProcessingInstruction.Target;

    public override string Value
    {
      get => this.ProcessingInstruction.Data;
      set => this.ProcessingInstruction.Data = value;
    }
  }
}
