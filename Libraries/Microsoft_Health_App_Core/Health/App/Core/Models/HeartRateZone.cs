// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.HeartRateZone
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class HeartRateZone
  {
    public HeartRateZone(int max) => this.Max = max;

    public static HeartRateZone CreateHeartRateZoneWithAge(int age) => new HeartRateZone(220 - age);

    public int Max { get; }

    public double MaximumThreshold => 1.1 * (double) this.Max;

    public double VeryHardThreshold => 0.9 * (double) this.Max;

    public double HardThreshold => 0.8 * (double) this.Max;

    public double ModerateThreshold => 0.7 * (double) this.Max;

    public double LightThreshold => 0.6 * (double) this.Max;

    public double VeryLightThreshold => 0.5 * (double) this.Max;

    public double MinimumThreshold => 0.25 * (double) this.Max;
  }
}
