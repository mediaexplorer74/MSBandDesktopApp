// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.DateTimeZoneWriter
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NodaTime.TimeZones.IO
{
  internal sealed class DateTimeZoneWriter : IDateTimeZoneWriter
  {
    private readonly Stream output;
    private readonly IList<string> stringPool;

    internal DateTimeZoneWriter(Stream output, IList<string> stringPool)
    {
      this.output = output;
      this.stringPool = stringPool;
    }

    public void WriteCount(int value)
    {
      Preconditions.CheckArgumentRange(nameof (value), value, 0, int.MaxValue);
      this.WriteVarint(checked ((uint) value));
    }

    public void WriteSignedCount(int count) => this.WriteVarint((uint) (count >> 31 ^ count << 1));

    private void WriteVarint(uint value)
    {
      for (; value > (uint) sbyte.MaxValue; value >>= 7)
        this.output.WriteByte((byte) (128 | (int) value & (int) sbyte.MaxValue));
      this.output.WriteByte((byte) (value & (uint) sbyte.MaxValue));
    }

    public void WriteOffset(Offset offset)
    {
      int num1 = checked (offset.Milliseconds + 86400000);
      if (num1 % 1800000 == 0)
        this.WriteByte((byte) (num1 / 1800000));
      else if (num1 % 60000 == 0)
      {
        int num2 = num1 / 60000;
        this.WriteByte((byte) (128 | num2 >> 8));
        this.WriteByte((byte) (num2 & (int) byte.MaxValue));
      }
      else if (num1 % 1000 == 0)
      {
        int num3 = num1 / 1000;
        this.WriteByte((byte) (160U | (uint) (byte) (num3 >> 16)));
        this.WriteInt16((short) (num3 & (int) ushort.MaxValue));
      }
      else
        this.WriteInt32(-1073741824 | num1);
    }

    public void WriteDictionary(IDictionary<string, string> dictionary)
    {
      Preconditions.CheckNotNull<IDictionary<string, string>>(dictionary, nameof (dictionary));
      this.WriteCount(dictionary.Count);
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) dictionary)
      {
        this.WriteString(keyValuePair.Key);
        this.WriteString(keyValuePair.Value);
      }
    }

    public void WriteZoneIntervalTransition(Instant? previous, Instant value)
    {
      if (previous.HasValue)
        Preconditions.CheckArgumentRange(nameof (value), value.Ticks, previous.Value.Ticks, long.MaxValue);
      if (value == Instant.MinValue)
        this.WriteCount(0);
      else if (value == Instant.MaxValue)
      {
        this.WriteCount(1);
      }
      else
      {
        if (previous.HasValue)
        {
          ulong num1 = (ulong) (value.Ticks - previous.Value.Ticks);
          if (num1 % 36000000000UL == 0UL)
          {
            ulong num2 = num1 / 36000000000UL;
            if (128UL <= num2 && num2 < 2097152UL)
            {
              this.WriteCount((int) num2);
              return;
            }
          }
        }
        if (value >= DateTimeZoneWriter.ZoneIntervalConstants.EpochForMinutesSinceEpoch)
        {
          ulong num3 = (ulong) (value.Ticks - DateTimeZoneWriter.ZoneIntervalConstants.EpochForMinutesSinceEpoch.Ticks);
          if (num3 % 600000000UL == 0UL)
          {
            ulong num4 = num3 / 600000000UL;
            if (2097152UL < num4 && num4 <= (ulong) int.MaxValue)
            {
              this.WriteCount((int) num4);
              return;
            }
          }
        }
        this.WriteCount(2);
        this.WriteInt64(value.Ticks);
      }
    }

    public void WriteString(string value)
    {
      if (this.stringPool == null)
      {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        this.WriteCount(bytes.Length);
        this.output.Write(bytes, 0, bytes.Length);
      }
      else
      {
        int num = this.stringPool.IndexOf(value);
        if (num == -1)
        {
          num = this.stringPool.Count;
          this.stringPool.Add(value);
        }
        this.WriteCount(num);
      }
    }

    private void WriteInt16(short value)
    {
      this.WriteByte((byte) ((int) value >> 8 & (int) byte.MaxValue));
      this.WriteByte((byte) ((uint) value & (uint) byte.MaxValue));
    }

    private void WriteInt32(int value)
    {
      this.WriteInt16((short) (value >> 16));
      this.WriteInt16((short) value);
    }

    private void WriteInt64(long value)
    {
      this.WriteInt32((int) (value >> 32));
      this.WriteInt32((int) value);
    }

    public void WriteByte(byte value) => this.output.WriteByte(value);

    internal enum DateTimeZoneType : byte
    {
      Fixed = 1,
      Precalculated = 2,
    }

    internal static class ZoneIntervalConstants
    {
      internal const int MarkerMinValue = 0;
      internal const int MarkerMaxValue = 1;
      internal const int MarkerRaw = 2;
      internal const int MinValueForHoursSincePrevious = 128;
      internal const int MinValueForMinutesSinceEpoch = 2097152;
      internal static readonly Instant EpochForMinutesSinceEpoch = Instant.FromUtc(1800, 1, 1, 0, 0);
    }
  }
}
