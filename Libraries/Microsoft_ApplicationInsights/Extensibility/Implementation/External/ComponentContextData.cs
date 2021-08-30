// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.ComponentContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class ComponentContextData
  {
    private readonly IDictionary<string, string> tags;

    internal ComponentContextData(IDictionary<string, string> tags) => this.tags = tags;

    public string Version
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.ApplicationVersion);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.ApplicationVersion, value);
    }

    public string Build
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.ApplicationBuild);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.ApplicationBuild, value);
    }

    internal void SetDefaults(ComponentContextData source)
    {
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.ApplicationVersion, source.Version);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.ApplicationBuild, source.Build);
    }
  }
}
