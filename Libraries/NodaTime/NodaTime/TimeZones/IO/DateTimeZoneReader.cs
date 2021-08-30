// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.DateTimeZoneReader
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NodaTime.TimeZones.IO
{
  internal sealed class DateTimeZoneReader : IDateTimeZoneReader
  {
    private readonly Stream input;
    private readonly IList<string> stringPool;

    internal DateTimeZoneReader(Stream input, IList<string> stringPool)
    {
      this.input = input;
      this.stringPool = stringPool;
    }

    public int ReadCount()
    {
      uint num = this.ReadVarint();
      return num <= (uint) int.MaxValue ? checked ((int) num) : throw new InvalidNodaDataException("Count value greater than Int32.MaxValue");
    }

    public int ReadSignedCount()
    {
      uint num = this.ReadVarint();
      return checked ((int) (num >> 1)) ^ checked (-(int) (uint) (unchecked ((int) num) & 1));
    }

    private uint ReadVarint()
    {
      uint num1 = 0;
      int num2 = 0;
      byte num3;
      do
      {
        num3 = this.ReadByte();
        num1 += (uint) (((int) num3 & (int) sbyte.MaxValue) << num2);
        num2 += 7;
      }
      while (num3 >= (byte) 128);
      return num1;
    }

    public Offset ReadOffset()
    {
      byte num1 = this.ReadByte();
      int num2;
      if (((int) num1 & 128) == 0)
      {
        num2 = (int) num1 * 1800000;
      }
      else
      {
        int num3 = (int) num1 & 224;
        int num4 = (int) num1 & 31;
        switch (num3)
        {
          case 128:
            num2 = ((num4 << 8) + (int) this.ReadByte()) * 60000;
            break;
          case 160:
            num2 = ((num4 << 16) + (this.ReadInt16() & (int) ushort.MaxValue)) * 1000;
            break;
          case 192:
            num2 = (num4 << 24) + ((int) this.ReadByte() << 16) + (this.ReadInt16() & (int) ushort.MaxValue);
            break;
          default:
            throw new InvalidNodaDataException("Invalid flag in offset: " + num3.ToString("x2"));
        }
      }
      return Offset.FromMilliseconds(num2 - 86400000);
    }

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

    public Instant ReadZoneIntervalTransition(Instant? previous)
    {
      int num = this.ReadCount();
      if (num < 128)
      {
        switch (num)
        {
          case 0:
            return Instant.MinValue;
          case 1:
            return Instant.MaxValue;
          case 2:
            return new Instant(this.ReadInt64());
          default:
            throw new InvalidNodaDataException("Unrecognised marker value: " + (object) num);
        }
      }
      else
      {
        if (num >= 2097152)
          return DateTimeZoneWriter.ZoneIntervalConstants.EpochForMinutesSinceEpoch + Duration.FromMinutes((long) num);
        if (!previous.HasValue)
          throw new InvalidNodaDataException("No previous value, so can't interpret value encoded as delta-since-previous: " + (object) num);
        return previous.Value + Duration.FromHours((long) num);
      }
    }

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

    private int ReadInt16() => (int) this.ReadByte() << 8 | (int) this.ReadByte();

    private int ReadInt32() => (this.ReadInt16() & (int) ushort.MaxValue) << 16 | this.ReadInt16() & (int) ushort.MaxValue;

    private long ReadInt64() => ((long) this.ReadInt32() & (long) uint.MaxValue) << 32 | (long) this.ReadInt32() & (long) uint.MaxValue;

    public byte ReadByte()
    {
      int num = this.input.ReadByte();
      return num != -1 ? checked ((byte) num) : throw new InvalidNodaDataException("Unexpected end of data stream");
    }
  }
}
