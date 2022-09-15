// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Speed
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;

namespace Microsoft.Health.Cloud.Client
{
  public struct Speed : IComparable, IComparable<Speed>, IEquatable<Speed>
  {
    private const int SecondsPerMinute = 60;
    private const int MinutesPerHour = 60;
    private const int SecondsPerHour = 3600;
    private const double CentimetersInFoot = 30.48;
    private static readonly Length Kilometer = Length.FromKilometers(1.0);
    private static readonly Length Mile = Length.FromMiles(1.0);
    private static readonly Length Foot = Length.FromFeet(1.0);
    public static readonly Speed Zero = new Speed();
    public static readonly Speed MaxValue = new Speed(double.MaxValue);
    public static readonly Speed MinValue = new Speed(double.MinValue);
    private readonly double metersPerSecond;

    private Speed(double metersPerSecond) => this.metersPerSecond = metersPerSecond;

    public double MinutesPerMile
    {
      get
      {
        double milesPerHour = this.MilesPerHour;
        return milesPerHour == 0.0 ? 0.0 : 60.0 / milesPerHour;
      }
    }

    public double MinutesPerKilometer
    {
      get
      {
        double kilometersPerHour = this.KilometersPerHour;
        return kilometersPerHour == 0.0 ? 0.0 : 60.0 / kilometersPerHour;
      }
    }

    public double MillisecondsPerKilometer => this.metersPerSecond == 0.0 ? 0.0 : 1000000.0 / this.metersPerSecond;

    public double MilesPerHour => this.metersPerSecond / Speed.Mile.TotalMeters * 3600.0;

    public double KilometersPerHour => this.metersPerSecond / Speed.Kilometer.TotalMeters * 3600.0;

    public double FeetPerSecond => this.metersPerSecond / Speed.Foot.TotalMeters;

    public double MetersPerSecond => this.metersPerSecond;

    public double CentimetersPerSecond => this.metersPerSecond * 100.0;

    public double CentimetersPerHour => this.CentimetersPerSecond * 60.0;

    public double MetersPerHour => this.MetersPerSecond * 60.0;

    public double FeetPerHour => this.CentimetersPerHour / 30.48;

    public static Speed FromDistanceAndTime(Length distance, TimeSpan time) => new Speed(time.TotalSeconds != 0.0 ? distance.TotalMeters / time.TotalSeconds : 0.0);

    public static Speed FromMillisecondsPerKilometer(double millisecondsPerKilometer) => new Speed(millisecondsPerKilometer != 0.0 ? 1000000.0 / millisecondsPerKilometer : 0.0);

    public static Speed FromCentimetersPerSecond(double centimetersPerSecond) => new Speed(centimetersPerSecond / 100.0);

    public static Speed FromCentimetersPerHour(double centimetersPerHour) => new Speed(centimetersPerHour / 100.0 / 60.0);

    public override string ToString() => string.Format("{0:0.00}m/s", new object[1]
    {
      (object) this.MetersPerSecond
    });

    public override int GetHashCode() => this.metersPerSecond.GetHashCode();

    public override bool Equals(object obj) => obj is Speed other && this.Equals(other);

    public int CompareTo(object obj)
    {
      if (obj is Speed other)
        return this.CompareTo(other);
      throw new ArgumentException("Argument must be a Speed");
    }

    public int CompareTo(Speed other) => this.metersPerSecond.CompareTo(other.metersPerSecond);

    public bool Equals(Speed other) => this.metersPerSecond == other.metersPerSecond;

    public static bool operator ==(Speed x, Speed y) => x.metersPerSecond == y.metersPerSecond;

    public static bool operator !=(Speed x, Speed y) => !(x == y);

    public static bool operator >(Speed x, Speed y) => x.metersPerSecond > y.metersPerSecond;

    public static bool operator >=(Speed x, Speed y) => x.metersPerSecond >= y.metersPerSecond;

    public static bool operator <(Speed x, Speed y) => x.metersPerSecond < y.metersPerSecond;

    public static bool operator <=(Speed x, Speed y) => x.metersPerSecond <= y.metersPerSecond;

    public static Speed operator -(Speed x) => new Speed(-x.metersPerSecond);

    public static Speed operator *(Speed x, int y) => new Speed(x.metersPerSecond * (double) y);
  }
}
