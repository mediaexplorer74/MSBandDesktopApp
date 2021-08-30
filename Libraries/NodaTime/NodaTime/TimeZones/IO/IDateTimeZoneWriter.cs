// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.IDateTimeZoneWriter
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using System.Collections.Generic;

namespace NodaTime.TimeZones.IO
{
  internal interface IDateTimeZoneWriter
  {
    void WriteCount(int count);

    void WriteSignedCount(int count);

    void WriteString([NotNull] string value);

    void WriteOffset(Offset offset);

    void WriteZoneIntervalTransition(Instant? previous, Instant value);

    void WriteDictionary([NotNull] IDictionary<string, string> dictionary);

    void WriteByte(byte value);
  }
}
