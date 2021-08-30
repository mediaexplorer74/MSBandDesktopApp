// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.UserContext
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  public sealed class UserContext : IJsonSerializable
  {
    private readonly IDictionary<string, string> tags;

    internal UserContext(IDictionary<string, string> tags) => this.tags = tags;

    public string Id
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserId, value);
    }

    public string AccountId
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserAccountId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserAccountId, value);
    }

    public string UserAgent
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserAgent);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserAgent, value);
    }

    public DateTimeOffset? AcquisitionDate
    {
      get => this.tags.GetTagDateTimeOffsetValueOrNull(ContextTagKeys.Keys.UserAccountAcquisitionDate);
      set => this.tags.SetDateTimeOffsetValueOrRemove(ContextTagKeys.Keys.UserAccountAcquisitionDate, value);
    }

    void IJsonSerializable.Serialize(IJsonWriter writer)
    {
      writer.WriteStartObject();
      writer.WriteProperty("id", this.Id);
      writer.WriteProperty("userAgent", this.UserAgent);
      writer.WriteProperty("accountId", this.AccountId);
      writer.WriteProperty("anonUserAcquisitionDate", this.AcquisitionDate);
      writer.WriteEndObject();
    }
  }
}
