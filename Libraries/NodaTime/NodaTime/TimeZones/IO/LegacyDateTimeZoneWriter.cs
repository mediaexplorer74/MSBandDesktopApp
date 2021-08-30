// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.LegacyDateTimeZoneWriter
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NodaTime.TimeZones.IO
{
  internal sealed class LegacyDateTimeZoneWriter : IDateTimeZoneWriter
  {
    internal const byte FlagTimeZoneCached = 0;
    internal const byte FlagTimeZoneDst = 1;
    internal const byte FlagTimeZoneFixed = 2;
    internal const byte FlagTimeZoneNull = 3;
    internal const byte FlagTimeZonePrecalculated = 4;
    internal const long HalfHoursMask = 63;
    internal const byte FlagMaxValue = 254;
    internal const byte FlagMinValue = 255;
    internal const byte FlagHalfHour = 0;
    internal const byte FlagMinutes = 64;
    internal const byte FlagSeconds = 128;
    private const byte FlagTicks = 192;
    internal const long MaxHalfHours = 31;
    private const long MinHalfHours = -31;
    internal const long MaxMinutes = 2097151;
    private const long MinMinutes = -2097151;
    internal const long MaxSeconds = 137438953471;
    private const long MinSeconds = -137438953471;
    internal const byte FlagOffsetSeconds = 128;
    internal const byte FlagOffsetMilliseconds = 253;
    internal const int MaxOffsetHalfHours = 63;
    internal const int MinOffsetHalfHours = -63;
    internal const int MaxOffsetSeconds = 262143;
    internal const int MinOffsetSeconds = -262143;
    private readonly Stream output;
    private readonly IList<string> stringPool;

    internal LegacyDateTimeZoneWriter(Stream output, IList<string> stringPool)
    {
      this.output = output;
      this.stringPool = stringPool;
    }

    public void WriteCount(int value)
    {
      Preconditions.CheckArgumentRange(nameof (value), value, 0, int.MaxValue);
      if (value <= 14)
      {
        this.WriteByte((byte) (240 + value));
      }
      else
      {
        value -= 15;
        if (value <= (int) sbyte.MaxValue)
        {
          this.WriteByte((byte) value);
        }
        else
        {
          value -= 128;
          if (value <= 16383)
          {
            this.WriteByte((byte) (128 + (value >> 8)));
            this.WriteByte((byte) (value & (int) byte.MaxValue));
          }
          else
          {
            value -= 16384;
            if (value <= 2097151)
            {
              this.WriteByte((byte) (192 + (value >> 16)));
              this.WriteInt16((short) (value & (int) ushort.MaxValue));
            }
            else
            {
              value -= 2097152;
              if (value <= 268435455)
              {
                this.WriteByte((byte) (224 + (value >> 24)));
                this.WriteByte((byte) (value >> 16 & (int) byte.MaxValue));
                this.WriteInt16((short) (value & (int) ushort.MaxValue));
              }
              else
              {
                this.WriteByte(byte.MaxValue);
                this.WriteInt32(value + 2097152 + 16384 + 128 + 15);
              }
            }
          }
        }
      }
    }

    public void WriteSignedCount(int count) => throw new NotSupportedException();

    public void WriteOffset(Offset offset)
    {
      int milliseconds = offset.Milliseconds;
      if (milliseconds % 1800000 == 0)
      {
        int num = milliseconds / 1800000;
        if (-63 <= num && num <= 63)
        {
          this.WriteByte((byte) (num + 63 & (int) sbyte.MaxValue));
          return;
        }
      }
      if (milliseconds % 1000 == 0)
      {
        int num1 = milliseconds / 1000;
        if (-262143 <= num1 && num1 <= 262143)
        {
          int num2 = num1 + 262143;
          this.WriteByte((byte) (128U | (uint) (byte) (num2 >> 16 & 63)));
          this.WriteInt16((short) (num2 & (int) ushort.MaxValue));
          return;
        }
      }
      this.WriteByte((byte) 253);
      this.WriteInt32(milliseconds);
    }

    private void WriteTicks(long value)
    {
      if (value == long.MinValue)
        this.WriteByte(byte.MaxValue);
      else if (value == long.MaxValue)
      {
        this.WriteByte((byte) 254);
      }
      else
      {
        if (value % 18000000000L == 0L)
        {
          long num = value / 18000000000L;
          if (-31L <= num && num <= 31L)
          {
            this.WriteByte((byte) ((ulong) (num + 31L) & 63UL));
            return;
          }
        }
        if (value % 600000000L == 0L)
        {
          long num1 = value / 600000000L;
          if (-2097151L <= num1 && num1 <= 2097151L)
          {
            long num2 = num1 + 2097151L;
            this.WriteByte((byte) (64U | (uint) (byte) ((ulong) (num2 >> 16) & 63UL)));
            this.WriteInt16((short) (num2 & (long) ushort.MaxValue));
            return;
          }
        }
        if (value % 10000000L == 0L)
        {
          long num3 = value / 10000000L;
          if (-137438953471L <= num3 && num3 <= 137438953471L)
          {
            long num4 = num3 + 137438953471L;
            this.WriteByte((byte) (128U | (uint) (byte) ((ulong) (num4 >> 32) & 63UL)));
            this.WriteInt32((int) (num4 & (long) uint.MaxValue));
            return;
          }
        }
        this.WriteByte((byte) 192);
        this.WriteInt64(value);
      }
    }

    internal void WriteBoolean(bool value) => this.WriteByte(value ? (byte) 1 : (byte) 0);

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

    public void WriteZoneIntervalTransition(Instant? previous, Instant value) => this.WriteTicks(value.Ticks);

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

    public void WriteTimeZone(DateTimeZone value)
    {
      switch (value)
      {
        case null:
          this.WriteByte((byte) 3);
          break;
        case FixedDateTimeZone _:
          this.WriteByte((byte) 2);
          ((FixedDateTimeZone) value).Write((IDateTimeZoneWriter) this);
          break;
        case PrecalculatedDateTimeZone _:
          this.WriteByte((byte) 4);
          ((PrecalculatedDateTimeZone) value).WriteLegacy(this);
          break;
        case CachedDateTimeZone _:
          this.WriteByte((byte) 0);
          ((CachedDateTimeZone) value).WriteLegacy(this);
          break;
        case DaylightSavingsDateTimeZone _:
          this.WriteByte((byte) 1);
          ((DaylightSavingsDateTimeZone) value).WriteLegacy(this);
          break;
        default:
          throw new ArgumentException("Unknown DateTimeZone type " + (object) value.GetType());
      }
    }

    private void WriteInt16(short value)
    {
      this.WriteByte((byte) ((int) value >> 8 & (int) byte.MaxValue));
      this.WriteByte((byte) ((uint) value & (uint) byte.MaxValue));
    }

    internal void WriteInt32(int value)
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
  }
}
