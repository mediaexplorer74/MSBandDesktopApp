// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.VariableOperand
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;
using System.IO;

namespace Microsoft.Band.Admin.WebTiles
{
  public class VariableOperand : Operand
  {
    private VariableOperand(string tokenValue, int position)
      : base(tokenValue, position)
    {
    }

    public static VariableOperand Create(string tokenValue, int position) => new VariableOperand(tokenValue, position);

    public override object GetValue(Dictionary<string, string> variableValues, bool stringRequired)
    {
      if (variableValues == null)
        return (object) "0";
      string key = this.MatchedString.Substring(2, this.MatchedString.Length - 4);
      string s = variableValues.ContainsKey(key) ? variableValues[key] : throw new InvalidDataException(string.Format(CommonSR.WTUndefinedVariable, new object[1]
      {
        (object) key
      }));
      double result;
      return !stringRequired && double.TryParse(s, out result) ? (object) Operand.RoundDoubleTo16SignificantDigits(result) : (object) s;
    }
  }
}
