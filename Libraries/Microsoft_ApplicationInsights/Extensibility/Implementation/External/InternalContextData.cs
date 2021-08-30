// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.InternalContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class InternalContextData
  {
    private readonly IDictionary<string, string> tags;

    internal InternalContextData(IDictionary<string, string> tags) => this.tags = tags;

    public string SdkVersion
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.InternalSdkVersion);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.InternalSdkVersion, value);
    }

    public string AgentVersion
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.InternalAgentVersion);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.InternalAgentVersion, value);
    }
  }
}
