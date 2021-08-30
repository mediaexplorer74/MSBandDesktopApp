// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.ComponentContext
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  public sealed class ComponentContext : IJsonSerializable
  {
    private readonly IDictionary<string, string> tags;

    internal ComponentContext(IDictionary<string, string> tags) => this.tags = tags;

    public string Version
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.ApplicationVersion);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.ApplicationVersion, value);
    }

    void IJsonSerializable.Serialize(IJsonWriter writer)
    {
      writer.WriteStartObject();
      writer.WriteProperty("version", this.Version);
      writer.WriteEndObject();
    }
  }
}
