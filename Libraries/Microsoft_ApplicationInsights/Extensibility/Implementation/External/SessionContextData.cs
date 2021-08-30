// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.SessionContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class SessionContextData
  {
    private readonly IDictionary<string, string> tags;

    internal SessionContextData(IDictionary<string, string> tags) => this.tags = tags;

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

    public bool? IsNewSession
    {
      get => this.tags.GetTagBoolValueOrNull(ContextTagKeys.Keys.SessionIsNew);
      set => this.tags.SetTagValueOrRemove<bool?>(ContextTagKeys.Keys.SessionIsNew, value);
    }

    internal void SetDefaults(SessionContextData source)
    {
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.SessionId, source.Id);
      this.tags.InitializeTagValue<bool?>(ContextTagKeys.Keys.SessionIsFirst, source.IsFirst);
      this.tags.InitializeTagValue<bool?>(ContextTagKeys.Keys.SessionIsNew, source.IsNewSession);
    }
  }
}
