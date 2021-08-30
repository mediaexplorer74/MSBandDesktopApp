// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.IWebTileResource
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Band.Admin.WebTiles
{
  public interface IWebTileResource
  {
    bool AllowInvalidValues { get; set; }

    Dictionary<string, string> PropertyErrors { get; }

    string Url { get; set; }

    ResourceStyle Style { get; set; }

    Dictionary<string, string> Content { get; set; }

    Task<List<Dictionary<string, string>>> ResolveFeedContentMappingsAsync();

    string Username { get; set; }

    string Password { get; set; }

    Task<bool> AuthenticateAsync();

    IWebTileResourceCacheInfo CacheInfo { get; set; }

    HeaderNameValuePair[] RequestHeaders { get; set; }
  }
}
