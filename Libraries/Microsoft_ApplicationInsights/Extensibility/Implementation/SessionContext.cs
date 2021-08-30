// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.SessionContext
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  public sealed class SessionContext : IJsonSerializable
  {
    private readonly IDictionary<string, string> tags;

    internal SessionContext(IDictionary<string, string> tags) => this.tags = tags;

    public string Id
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.SessionId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.SessionId, value);
    }

    public bool? IsFirst
    {
      get => this.tags.GetTagBoolValueOrNull(ContextTagKeys.Keys.SessionIsFirst);
      set => this.tags.SetTagValueOrRemove<bool?>(ContextTagKeys.Keys.SessionIsFirst, value);
    }

    void IJsonSerializable.Serialize(IJsonWriter writer)
    {
      writer.WriteStartObject();
      writer.WriteProperty("id", this.Id);
      writer.WriteProperty("firstSession", this.IsFirst);
      writer.WriteEndObject();
    }
  }
}
