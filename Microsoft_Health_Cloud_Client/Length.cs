// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Length
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;

namespace Microsoft.Health.Cloud.Client
{
  public struct Length : IComparable, IComparable<Length>, IEquatable<Length>
  {
    private const int MmPerCm = 10;
    private const int MmPerMeter = 1000;
    private const int MMPerKm = 1000000;
    private const int MMPerMile = 1609344;
    private const double MmPerYard = 914.4;
    private const double MmPerFoot = 304.8;
    private const double MmPerInch = 25.4;
    private const int InchesPerFoot = 12;
    private const int FeetPerYard = 3;
    public static readonly Length Zero = new Length();
    public static readonly Length MaxValue = new Length(double.MaxValue);
    public static readonly Length MinValue = new Length(double.MinValue);
    private readonly double millimeters;

    private Length(double millimeters) => this.millimeters = millimeters;

    public int Feet
    {
      get
      {
        double num1 = this.millimeters / 25.4;
        int num2 = (int) (num1 / 12.0);
        if ((int) Math.Round(num1 % 12.0) == 12)
          ++num2;
        return num2;
      }
    }

    public int Inches
    {
      get
      {
        int num = (int) Math.Round(this.millimeters / 25.4 % 12.0);
        if (num == 12)
          num = 0;
        return num;
      }
    }

    public double TotalKilometers => this.millimeters / 1000000.0;

    public double TotalMeters => this.millimeters / 1000.0;

    public double TotalCentimeters => this.millimeters / 10.0;

    public double TotalMillimeters => this.millimeters;

    public double TotalMiles => this.millimeters / 1609344.0;

    public double TotalYards => this.millimeters / 914.4;

    public double TotalFeet => this.millimeters / 304.8;

    public double TotalInches => this.millimeters / 25.4;

    public static Length FromFeetAndInches(double feet, double inches) => new Length((feet * 12.0 + inches) * 25.4);

    public static Length FromFeet(double feet) => Length.FromFeetAndInches(feet, 0.0);

    public static Length FromYards(double yards) => Length.FromFeet(yards * 3.0);

    public static Length FromMiles(double miles) => new Length(miles * 1609344.0);

    public static Length FromMillimeters(double millimeters) => new Length(millimeters);

    public static Length FromCentimeters(double centimeters) => new Length(centimeters * 10.0);

    public static Length FromMeters(double meters) => new Length(meters * 1000.0);

    public static Length FromKilometers(double kilometers) => new Length(kilometers * 1000000.0);

    public Length Add(Length length) => new Length(this.millimeters + length.millimeters);

    public Length Subtract(Length length) => new Length(this.millimeters - length.millimeters);

    public override string ToString() => string.Format("{0:0.00}m", new object[1]
    {
      (object) this.TotalMeters
    });

    public override int GetHashCode() => this.millimeters.GetHashCode();

    public override bool Equals(object obj) => obj is Length other && this.Equals(other);

    public int CompareTo(object obj)
    {
      if (obj is Length other)
        return this.CompareTo(other);
      throw new ArgumentException("Argument must be a Length");
    }

    public int CompareTo(Length other) => this.millimeters.CompareTo(other.millimeters);

    public bool Equals(Length other) => this.millimeters == other.millimeters;

    public static bool operator ==(Length x, Length y) => x.millimeters == y.millimeters;

    public static bool operator !=(Length x, Length y) => !(x == y);

    public static bool operator >(Length x, Length y) => x.millimeters > y.millimeters;

    public static bool operator >=(Length x, Length y) => x.millimeters >= y.millimeters;

    public static bool operator <(Length x, Length y) => x.millimeters < y.millimeters;

    public static bool operator <=(Length x, Length y) => x.millimeters <= y.millimeters;

    public static Length operator +(Length x, Length y) => x.Add(y);

    public static Length operator -(Length x, Length y) => x.Subtract(y);

    public static Length operator -(Length x) => new Length(-x.millimeters);
  }
}
