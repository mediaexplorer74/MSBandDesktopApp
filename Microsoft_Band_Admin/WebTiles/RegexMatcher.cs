// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.RegexMatcher
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Text.RegularExpressions;

namespace Microsoft.Band.Admin.WebTiles
{
  public class RegexMatcher
  {
    private readonly Regex regex;

    public RegexMatcher(string regex, RegexOptions options) => this.regex = new Regex(string.Format("\\G{0}", new object[1]
    {
      (object) regex
    }), options);

    public int Match(string input, int startat = 0)
    {
      System.Text.RegularExpressions.Match match = this.regex.Match(input, startat);
      return !match.Success ? 0 : match.Length;
    }

    public override string ToString() => this.regex.ToString();
  }
}
