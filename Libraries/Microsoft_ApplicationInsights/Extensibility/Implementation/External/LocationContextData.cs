// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.LocationContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class LocationContextData
  {
    private readonly IDictionary<string, string> tags;

    internal LocationContextData(IDictionary<string, string> tags) => this.tags = tags;

    public string Ip
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.LocationIp);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.LocationIp, value);
    }
  }
}
