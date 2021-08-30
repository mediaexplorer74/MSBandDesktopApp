// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.LegacyDateTimeZoneReader
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
  internal sealed class LegacyDateTimeZoneReader : IDateTimeZoneReader
  {
    private readonly Stream input;
    private readonly IList<string> stringPool;

    internal LegacyDateTimeZoneReader(Stream input, IList<string> stringPool)
    {
      this.input = input;
      this.stringPool = stringPool;
    }

    public int ReadCount()
    {
      byte num1 = this.ReadByte();
      if (num1 == byte.MaxValue)
        return this.ReadInt32();
      if ((byte) 240 <= num1 && num1 <= (byte) 254)
        return (int) num1 & 15;
      int num2 = 15;
      if (((int) num1 & 128) == 0)
        return (int) num1 + num2;
      int num3 = num2 + 128;
      if (((int) num1 & 192) == 128)
        return (((int) num1 & 63) << 8) + (int) this.ReadByte() + num3;
      int num4 = num3 + 16384;
      if (((int) num1 & 224) == 192)
        return (((int) num1 & 31) << 16) + this.ReadInt16() + num4;
      int num5 = num4 + 2097152;
      return ((((int) num1 & 15) << 8) + (int) this.ReadByte() << 16) + this.ReadInt16() + num5;
    }

    public int ReadSignedCount() => throw new NotSupportedException();

    public Offset ReadOffset()
    {
      byte num = this.ReadByte();
      if (((int) num & 128) == 0)
        return Offset.FromMilliseconds(((int) num - 63) * 1800000);
      return ((int) num & 192) == 128 ? Offset.FromMilliseconds((((((int) num & -193) << 8) + ((int) this.ReadByte() & (int) byte.MaxValue) << 8) + ((int) this.ReadByte() & (int) byte.MaxValue) - 262143) * 1000) : Offset.FromMilliseconds(this.ReadInt32());
    }

    private long ReadTicks()
    {
      byte num = this.ReadByte();
      switch (num)
      {
        case 254:
          return long.MaxValue;
        case byte.MaxValue:
          return long.MinValue;
        default:
          if (((int) num & 192) == 0)
            return ((long) num - 31L) * 18000000000L;
          if (((int) num & 192) == 64)
            return ((long) (((((int) num & -193) << 8) + ((int) this.ReadByte() & (int) byte.MaxValue) << 8) + ((int) this.ReadByte() & (int) byte.MaxValue)) - 2097151L) * 600000000L;
          return ((int) num & 192) == 128 ? (((long) ((int) num & -193) << 32) + ((long) this.ReadInt32() & (long) uint.MaxValue) - 137438953471L) * 10000000L : this.ReadInt64();
      }
    }

    internal bool ReadBoolean() => this.ReadByte() != (byte) 0;

    public IDictionary<string, string> ReadDictionary()
    {
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      int num1 = this.ReadCount();
      int num2 = 0;
      while (num2 < num1)
      {
        string key = this.ReadString();
        string str = this.ReadString();
        dictionary.Add(key, str);
        checked { ++num2; }
      }
      return dictionary;
    }

    public Instant ReadZoneIntervalTransition(Instant? previous) => new Instant(this.ReadTicks());

    public string ReadString()
    {
      if (this.stringPool != null)
        return this.stringPool[this.ReadCount()];
      int count = this.ReadCount();
      byte[] numArray = new byte[count];
      int num1 = 0;
      while (num1 < count)
      {
        int num2 = this.input.Read(numArray, 0, count);
        if (num2 <= 0)
          throw new InvalidNodaDataException("Unexpectedly reached end of data with " + (object) checked (count - num1) + " bytes still to read");
        checked { num1 += num2; }
      }
      return Encoding.UTF8.GetString(numArray, 0, count);
    }

    public DateTimeZone ReadTimeZone(string id)
    {
      int num = (int) this.ReadByte();
      switch (num)
      {
        case 0:
          return CachedDateTimeZone.ReadLegacy(this, id);
        case 1:
          return DaylightSavingsDateTimeZone.ReadLegacy(this, id);
        case 2:
          return FixedDateTimeZone.Read((IDateTimeZoneReader) this, id);
        case 3:
          return (DateTimeZone) null;
        case 4:
          return PrecalculatedDateTimeZone.ReadLegacy(this, id);
        default:
          throw new InvalidNodaDataException("Unknown time zone flag type: " + (object) num);
      }
    }

    private int ReadInt16() => (int) this.ReadByte() << 8 | (int) this.ReadByte();

    internal int ReadInt32() => (this.ReadInt16() & (int) ushort.MaxValue) << 16 | this.ReadInt16() & (int) ushort.MaxValue;

    private long ReadInt64() => ((long) this.ReadInt32() & (long) uint.MaxValue) << 32 | (long) this.ReadInt32() & (long) uint.MaxValue;

    public byte ReadByte()
    {
      int num = this.input.ReadByte();
      return num != -1 ? checked ((byte) num) : throw new InvalidNodaDataException("Unexpected end of data stream");
    }
  }
}
