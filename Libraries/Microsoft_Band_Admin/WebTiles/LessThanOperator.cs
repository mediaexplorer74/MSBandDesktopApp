﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.LessThanOperator
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin.WebTiles
{
  internal class LessThanOperator : BinaryOperator
  {
    private LessThanOperator(string tokenValue, int position)
      : base(tokenValue, position)
    {
    }

    public static LessThanOperator Create(string tokenValue, int position) => new LessThanOperator(tokenValue, position);

    public override bool Compare(object leftOperand, object rightOperand) => this.Compare(leftOperand, rightOperand, (BinaryOperator.CompareOperation) (diff => diff < 0));
  }
}