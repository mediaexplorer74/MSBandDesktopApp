// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData]
  [GeneratedCode("gbc", "3.02")]
  internal class StackFrame
  {
    public int level { get; set; }

    public string method { get; set; }

    public string assembly { get; set; }

    public string fileName { get; set; }

    public int line { get; set; }

    public StackFrame()
      : this("AI.StackFrame", nameof (StackFrame))
    {
    }

    protected StackFrame(string fullName, string name)
    {
      this.method = string.Empty;
      this.assembly = string.Empty;
      this.fileName = string.Empty;
    }
  }
}
