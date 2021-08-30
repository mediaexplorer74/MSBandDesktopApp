// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.BinaryOperator
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.IO;

namespace Microsoft.Band.Admin.WebTiles
{
  internal abstract class BinaryOperator : Token
  {
    protected BinaryOperator(string tokenValue, int position)
      : base(tokenValue, position)
    {
    }

    private static bool TryConvertToNumber(string input, out object result)
    {
      double result1;
      bool flag;
      if (double.TryParse(input, out result1))
      {
        result = (object) Operand.RoundDoubleTo16SignificantDigits(result1);
        flag = true;
      }
      else
      {
        result = (object) input;
        flag = false;
      }
      return flag;
    }

    public abstract bool Compare(object leftOperand, object rightOperand);

    protected bool Compare(
      object leftOperand,
      object rightOperand,
      BinaryOperator.CompareOperation compareOperation)
    {
      if ((object) leftOperand.GetType() != (object) rightOperand.GetType())
      {
        bool flag = false;
        if (leftOperand is string)
          flag = BinaryOperator.TryConvertToNumber((string) leftOperand, out leftOperand);
        else if (rightOperand is string)
          flag = BinaryOperator.TryConvertToNumber((string) rightOperand, out rightOperand);
        if (!flag)
          return !compareOperation(0) && compareOperation(-1) && compareOperation(1);
      }
      int difference;
      switch (leftOperand)
      {
        case double _ when rightOperand is double num2:
          double num1 = (double) leftOperand - num2;
          difference = num1 != 0.0 ? (num1 >= 0.0 ? 1 : -1) : 0;
          break;
        case string _ when rightOperand is string:
          difference = string.Compare((string) leftOperand, (string) rightOperand);
          break;
        default:
          throw new InvalidDataException(CommonSR.WTUnexpectedTypeInCompare);
      }
      return compareOperation(difference);
    }

    public delegate bool CompareOperation(int difference);
  }
}
