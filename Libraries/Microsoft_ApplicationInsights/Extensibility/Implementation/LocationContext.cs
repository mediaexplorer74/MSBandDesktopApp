// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.LocationContext
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  public sealed class LocationContext : IJsonSerializable
  {
    private readonly IDictionary<string, string> tags;

    internal LocationContext(IDictionary<string, string> tags) => this.tags = tags;

    public string Ip
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.LocationIp);
      set
      {
        if (value == null || !this.IsIpV4(value))
          return;
        this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.LocationIp, value);
      }
    }

    void IJsonSerializable.Serialize(IJsonWriter writer)
    {
      writer.WriteStartObject();
      writer.WriteProperty("ip", this.Ip);
      writer.WriteEndObject();
    }

    private bool IsIpV4(string ip)
    {
      if (ip.Length > 15 || ip.Length < 7 || Enumerable.Cast<char>(ip).Any<char>((Func<char, bool>) (c => (c < '0' || c > '9') && c != '.')))
        return false;
      string[] strArray = ip.Split('.');
      if (strArray.Length != 4)
        return false;
      foreach (string s in strArray)
      {
        if (!byte.TryParse(s, out byte _))
          return false;
      }
      return true;
    }
  }
}
