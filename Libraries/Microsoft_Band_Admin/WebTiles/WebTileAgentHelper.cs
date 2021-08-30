// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileAgentHelper
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Admin.WebTiles
{
  public class WebTileAgentHelper
  {
    private const string OrganizationMicrosoft = "Microsoft";
    private const string XWebTileAgentHeaderName = "X-WEBTILE-AGENT";
    private static Dictionary<string, HeaderNameValuePair[]> MSUrlToHeadersTable = new Dictionary<string, HeaderNameValuePair[]>()
    {
      {
        "prodcus0dep.blob.core.windows.net",
        new HeaderNameValuePair[1]
        {
          new HeaderNameValuePair("X-WEBTILE-AGENT", "Microsoft")
        }
      },
      {
        "intcus0devweb.blob.core.windows.net",
        new HeaderNameValuePair[1]
        {
          new HeaderNameValuePair("X-WEBTILE-AGENT", "Microsoft")
        }
      },
      {
        "intcus0dep.blob.core.windows.net",
        new HeaderNameValuePair[1]
        {
          new HeaderNameValuePair("X-WEBTILE-AGENT", "Microsoft")
        }
      }
    };

    public static HeaderNameValuePair[] GetAgentHeadersForUrl(
      string url,
      string organization)
    {
      if (organization != null && string.Compare(organization, "Microsoft", StringComparison.OrdinalIgnoreCase) == 0)
      {
        string host = new Uri(url).Host;
        if (host != null && WebTileAgentHelper.MSUrlToHeadersTable.ContainsKey(host))
          return WebTileAgentHelper.MSUrlToHeadersTable[host];
      }
      return (HeaderNameValuePair[]) null;
    }
  }
}
