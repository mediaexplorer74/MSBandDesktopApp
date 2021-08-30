// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.TzdbStreamField
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace NodaTime.TimeZones.IO
{
  internal sealed class TzdbStreamField
  {
    private readonly TzdbStreamFieldId id;
    private readonly byte[] data;

    private TzdbStreamField(TzdbStreamFieldId id, byte[] data)
    {
      this.id = id;
      this.data = data;
    }

    internal TzdbStreamFieldId Id => this.id;

    internal Stream CreateStream() => (Stream) new MemoryStream(this.data, false);

    internal T ExtractSingleValue<T>(
      Func<DateTimeZoneReader, T> readerFunction,
      IList<string> stringPool)
    {
      using (Stream stream = this.CreateStream())
        return readerFunction(new DateTimeZoneReader(stream, stringPool));
    }

    internal static IEnumerable<TzdbStreamField> ReadFields(Stream input)
    {
      while (true)
      {
        int fieldId = input.ReadByte();
        if (fieldId != -1)
        {
          TzdbStreamFieldId id = (TzdbStreamFieldId) checked ((byte) fieldId);
          int length = new DateTimeZoneReader(input, (IList<string>) null).ReadCount();
          byte[] data = new byte[length];
          int offset = 0;
          while (offset < data.Length)
          {
            int num = input.Read(data, offset, checked (data.Length - offset));
            if (num == 0)
              throw new InvalidNodaDataException("Stream ended after reading " + (object) offset + " bytes out of " + (object) data.Length);
            checked { offset += num; }
          }
          yield return new TzdbStreamField(id, data);
        }
        else
          break;
      }
    }
  }
}
