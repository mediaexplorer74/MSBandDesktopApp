// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Weight
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;

namespace Microsoft.Health.Cloud.Client
{
  public struct Weight : IComparable, IComparable<Weight>, IEquatable<Weight>
  {
    private const double GramsPerPound = 453.592;
    private const double GramsPerKg = 1000.0;
    public static readonly Weight Zero = new Weight();
    public static readonly Weight MaxValue = new Weight(double.MaxValue);
    public static readonly Weight MinValue = new Weight(double.MinValue);
    private readonly double grams;

    private Weight(double grams) => this.grams = grams;

    public double TotalGrams => this.grams;

    public double TotalKilograms => this.grams / 1000.0;

    public double TotalPounds => this.grams / 453.592;

    public static Weight FromGrams(double grams) => new Weight(grams);

    public static Weight FromPounds(double pounds) => new Weight(pounds * 453.592);

    public static Weight FromKilograms(double kilograms) => new Weight(kilograms * 1000.0);

    public Weight Add(Weight weight) => new Weight(this.grams + weight.grams);

    public Weight Subtract(Weight weight) => new Weight(this.grams - weight.grams);

    public override string ToString() => string.Format("{0:0.00}kg", new object[1]
    {
      (object) this.TotalKilograms
    });

    public override int GetHashCode() => this.grams.GetHashCode();

    public override bool Equals(object obj) => obj is Weight other && this.Equals(other);

    public int CompareTo(object obj)
    {
      if (obj is Weight other)
        return this.CompareTo(other);
      throw new ArgumentException("Argument must be a Weight");
    }

    public int CompareTo(Weight other) => this.grams.CompareTo(other.grams);

    public bool Equals(Weight other) => this.grams == other.grams;

    public static bool operator ==(Weight x, Weight y) => x.grams == y.grams;

    public static bool operator !=(Weight x, Weight y) => !(x == y);

    public static bool operator >(Weight x, Weight y) => x.grams > y.grams;

    public static bool operator >=(Weight x, Weight y) => x.grams >= y.grams;

    public static bool operator <(Weight x, Weight y) => x.grams < y.grams;

    public static bool operator <=(Weight x, Weight y) => x.grams <= y.grams;

    public static Weight operator +(Weight x, Weight y) => x.Add(y);

    public static Weight operator -(Weight x, Weight y) => x.Subtract(y);

    public static Weight operator -(Weight x) => new Weight(-x.grams);

    public static Weight operator *(Weight x, int y) => new Weight(x.grams * (double) y);
  }
}
