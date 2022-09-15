// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CargoRunDisplayMetrics
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Band.Admin
{
  public class CargoRunDisplayMetrics
  {
    private const int MaximumDisplaySlots = 7;
    private static readonly HashSet<RunDisplayMetricType> CargoValidMetrics = new HashSet<RunDisplayMetricType>()
    {
      RunDisplayMetricType.Duration,
      RunDisplayMetricType.HeartRate,
      RunDisplayMetricType.Calories,
      RunDisplayMetricType.Distance,
      RunDisplayMetricType.Pace
    };
    private RunDisplayMetricType[] metrics;

    public CargoRunDisplayMetrics() => this.metrics = Enumerable.Repeat<RunDisplayMetricType>(RunDisplayMetricType.None, 7).ToArray<RunDisplayMetricType>();

    public CargoRunDisplayMetrics(RunDisplayMetricType[] metrics)
    {
      if (metrics == null)
        throw new ArgumentNullException(nameof (metrics));
      if (metrics.Length > 7)
        throw new ArgumentOutOfRangeException(nameof (metrics));
      if (metrics.Length < 7)
        this.metrics = ((IEnumerable<RunDisplayMetricType>) metrics).Concat<RunDisplayMetricType>(Enumerable.Repeat<RunDisplayMetricType>(RunDisplayMetricType.None, 7 - metrics.Length)).ToArray<RunDisplayMetricType>();
      else
        this.metrics = metrics;
    }

    public int GetSize() => this.metrics.Length;

    public RunDisplayMetricType this[int index]
    {
      get => this.metrics[index];
      set => this.metrics[index] = value;
    }

    public RunDisplayMetricType PrimaryMetric
    {
      get => this[0];
      set => this[0] = value;
    }

    public RunDisplayMetricType TopLeftMetric
    {
      get => this[1];
      set => this[1] = value;
    }

    public RunDisplayMetricType TopRightMetric
    {
      get => this[2];
      set => this[2] = value;
    }

    public RunDisplayMetricType LeftDrawerMetric
    {
      get => this[3];
      set => this[3] = value;
    }

    public RunDisplayMetricType RightDrawerMetric
    {
      get => this[4];
      set => this[4] = value;
    }

    public RunDisplayMetricType Metric06
    {
      get => this[5];
      set => this[5] = value;
    }

    public RunDisplayMetricType Metric07
    {
      get => this[6];
      set => this[6] = value;
    }

    public CargoRunDisplayMetrics Clone() => new CargoRunDisplayMetrics((RunDisplayMetricType[]) this.metrics.Clone());

    internal bool IsValid(DynamicAdminBandConstants constants)
    {
      IOrderedEnumerable<RunDisplayMetricType> orderedEnumerable = ((IEnumerable<RunDisplayMetricType>) this.metrics).Take<RunDisplayMetricType>(constants.RunMetricsDisplaySlotCount).Where<RunDisplayMetricType>((Func<RunDisplayMetricType, bool>) (metric => metric != RunDisplayMetricType.None)).OrderBy<RunDisplayMetricType, RunDisplayMetricType>((Func<RunDisplayMetricType, RunDisplayMetricType>) (metric => metric));
      RunDisplayMetricType displayMetricType1 = RunDisplayMetricType.None;
      foreach (RunDisplayMetricType displayMetricType2 in (IEnumerable<RunDisplayMetricType>) orderedEnumerable)
      {
        if (constants.BandClass == BandClass.Cargo && !CargoRunDisplayMetrics.CargoValidMetrics.Contains(displayMetricType2) || displayMetricType2 == displayMetricType1)
          return false;
        displayMetricType1 = displayMetricType2;
      }
      return true;
    }

    internal static int GetSerializedByteCount(DynamicAdminBandConstants constants) => constants.RunMetricsDisplaySlotCount * 2;

    internal static CargoRunDisplayMetrics DeserializeFromBand(
      ICargoReader reader,
      DynamicAdminBandConstants constants)
    {
      RunDisplayMetricType[] metrics = new RunDisplayMetricType[7];
      int index;
      for (index = 0; index < constants.RunMetricsDisplaySlotCount; ++index)
        metrics[index] = (RunDisplayMetricType) reader.ReadUInt16();
      for (; index < metrics.Length; ++index)
        metrics[index] = RunDisplayMetricType.None;
      return new CargoRunDisplayMetrics(metrics);
    }

    internal void SerializeToBand(ICargoWriter writer, DynamicAdminBandConstants constants)
    {
      foreach (RunDisplayMetricType displayMetricType in ((IEnumerable<RunDisplayMetricType>) this.metrics).Take<RunDisplayMetricType>(constants.RunMetricsDisplaySlotCount))
        writer.WriteUInt16((ushort) displayMetricType);
    }
  }
}
