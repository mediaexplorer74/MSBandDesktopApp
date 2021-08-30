// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.FixedDateTimeZone
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Text;
using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;
using System.Globalization;

namespace NodaTime.TimeZones
{
  internal sealed class FixedDateTimeZone : DateTimeZone
  {
    private readonly Offset offset;
    private readonly ZoneInterval interval;
    private readonly ZoneIntervalPair intervalPair;

    public FixedDateTimeZone(Offset offset)
      : this(FixedDateTimeZone.MakeId(offset), offset)
    {
    }

    public FixedDateTimeZone(string id, Offset offset)
      : base(id, true, offset, offset)
    {
      this.offset = offset;
      this.interval = new ZoneInterval(id, Instant.MinValue, Instant.MaxValue, offset, Offset.Zero);
      this.intervalPair = ZoneIntervalPair.Unambiguous(this.interval);
    }

    private static string MakeId(Offset offset)
    {
      if (offset == Offset.Zero)
        return "UTC";
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}", new object[2]
      {
        (object) "UTC",
        (object) OffsetPattern.GeneralInvariantPattern.Format(offset)
      });
    }

    internal static DateTimeZone GetFixedZoneOrNull(string id)
    {
      if (!id.StartsWith("UTC"))
        return (DateTimeZone) null;
      if (id == "UTC")
        return DateTimeZone.Utc;
      ParseResult<Offset> parseResult = OffsetPattern.GeneralInvariantPattern.Parse(id.Substring("UTC".Length));
      return !parseResult.Success ? (DateTimeZone) null : DateTimeZone.ForOffset(parseResult.Value);
    }

    public Offset Offset => this.offset;

    public override ZoneInterval GetZoneInterval(Instant instant) => this.interval;

    internal override ZoneIntervalPair GetZoneIntervalPair(LocalInstant localInstant) => this.intervalPair;

    public override Offset GetUtcOffset(Instant instant) => this.offset;

    internal void Write(IDateTimeZoneWriter writer)
    {
      Preconditions.CheckNotNull<IDateTimeZoneWriter>(writer, nameof (writer));
      writer.WriteOffset(this.offset);
    }

    public static DateTimeZone Read(IDateTimeZoneReader reader, string id)
    {
      Preconditions.CheckNotNull<IDateTimeZoneReader>(reader, nameof (reader));
      Preconditions.CheckNotNull<IDateTimeZoneReader>(reader, nameof (id));
      Offset offset = reader.ReadOffset();
      return (DateTimeZone) new FixedDateTimeZone(id, offset);
    }

    protected override bool EqualsImpl(DateTimeZone other) => this.offset == ((FixedDateTimeZone) other).offset && this.Id == other.Id;

    public override int GetHashCode() => HashCodeHelper.Hash<string>(HashCodeHelper.Hash<Offset>(HashCodeHelper.Initialize(), this.offset), this.Id);

    public override string ToString() => this.Id;
  }
}
