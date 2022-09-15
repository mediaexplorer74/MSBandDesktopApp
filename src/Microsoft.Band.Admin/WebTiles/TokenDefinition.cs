// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.TokenDefinition
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Text.RegularExpressions;

namespace Microsoft.Band.Admin.WebTiles
{
  public class TokenDefinition
  {
    public RegexMatcher Matcher { get; private set; }

    public CreateTokenDelegate CreateToken { get; private set; }

    public TokenDefinition(string regex, CreateTokenDelegate createToken = null, RegexOptions options = RegexOptions.None)
    {
      this.Matcher = new RegexMatcher(regex, options);
      this.CreateToken = createToken;
    }
  }
}
