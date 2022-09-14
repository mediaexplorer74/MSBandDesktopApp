// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.NumberOperand
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;
using System.IO;

namespace Microsoft.Band.Admin.WebTiles
{
  public class NumberOperand : Operand
  {
    private NumberOperand(string tokenValue, int position)
      : base(tokenValue, position)
    {
    }

    public static NumberOperand Create(string tokenValue, int position) => new NumberOperand(tokenValue, position);

    public override object GetValue(Dictionary<string, string> variableValues, bool stringRequired)
    {
      if (stringRequired)
        throw new InvalidDataException(CommonSR.WTContainsOperatorOnNumeric);
      return (object) Operand.RoundDoubleTo16SignificantDigits(double.Parse(this.MatchedString));
    }
  }
}
