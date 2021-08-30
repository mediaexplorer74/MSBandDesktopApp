// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.Stock
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Band
{
  public class Stock : IEquatable<Stock>
  {
    private readonly string id;
    private readonly string symbol;

    public Stock(string id, string symbol)
    {
      this.id = id;
      this.symbol = symbol;
    }

    public string ID => this.id;

    public double Value { get; set; }

    public double Change { get; set; }

    public string Name { get; set; }

    public string Symbol => this.symbol;

    public string Exchange { get; set; }

    public string Type { get; set; }

    public bool Equals(Stock stock) => stock != null && this.ID.Equals(stock.ID) && this.Symbol.Equals(stock.Symbol);

    public override int GetHashCode() => new
    {
      A = this.ID,
      B = this.Symbol
    }.GetHashCode();

    public override bool Equals(object obj) => this.Equals(obj as Stock);
  }
}
