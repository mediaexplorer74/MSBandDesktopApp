// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.ZonedDateTimePattern
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Globalization;
using NodaTime.Text.Patterns;
using NodaTime.TimeZones;
using NodaTime.Utility;
using System;
using System.Globalization;

namespace NodaTime.Text
{
  [Immutable]
  public sealed class ZonedDateTimePattern : IPattern<ZonedDateTime>
  {
    internal static readonly ZonedDateTime DefaultTemplateValue = new LocalDateTime(2000, 1, 1, 0, 0).InUtc();
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<ZonedDateTime> pattern;
    private readonly ZonedDateTime templateValue;
    private readonly ZoneLocalMappingResolver resolver;
    private readonly IDateTimeZoneProvider zoneProvider;

    public static ZonedDateTimePattern GeneralFormatOnlyIsoPattern => ZonedDateTimePattern.Patterns.GeneralFormatOnlyPatternImpl;

    public static ZonedDateTimePattern ExtendedFormatOnlyIsoPattern => ZonedDateTimePattern.Patterns.ExtendedFormatOnlyPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    public ZonedDateTime TemplateValue => this.templateValue;

    public ZoneLocalMappingResolver Resolver => this.resolver;

    public IDateTimeZoneProvider ZoneProvider => this.zoneProvider;

    private ZonedDateTimePattern(
      string patternText,
      NodaFormatInfo formatInfo,
      ZonedDateTime templateValue,
      ZoneLocalMappingResolver resolver,
      IDateTimeZoneProvider zoneProvider,
      IPattern<ZonedDateTime> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.templateValue = templateValue;
      this.resolver = resolver;
      this.zoneProvider = zoneProvider;
      this.pattern = pattern;
    }

    public ParseResult<ZonedDateTime> Parse(string text) => this.pattern.Parse(text);

    public string Format(ZonedDateTime value) => this.pattern.Format(value);

    private static ZonedDateTimePattern Create(
      string patternText,
      NodaFormatInfo formatInfo,
      ZoneLocalMappingResolver resolver,
      IDateTimeZoneProvider zoneProvider,
      ZonedDateTime templateValue)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      Preconditions.CheckNotNull<ZoneLocalMappingResolver>(resolver, nameof (resolver));
      IPattern<ZonedDateTime> pattern = new ZonedDateTimePatternParser(templateValue, resolver, zoneProvider).ParsePattern(patternText, formatInfo);
      return new ZonedDateTimePattern(patternText, formatInfo, templateValue, resolver, zoneProvider, pattern);
    }

    public static ZonedDateTimePattern Create(
      string patternText,
      CultureInfo cultureInfo,
      ZoneLocalMappingResolver resolver,
      IDateTimeZoneProvider zoneProvider,
      ZonedDateTime templateValue)
    {
      return ZonedDateTimePattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo), resolver, zoneProvider, templateValue);
    }

    public static ZonedDateTimePattern CreateWithInvariantCulture(
      string patternText,
      IDateTimeZoneProvider zoneProvider)
    {
      return ZonedDateTimePattern.Create(patternText, NodaFormatInfo.InvariantInfo, Resolvers.StrictResolver, zoneProvider, ZonedDateTimePattern.DefaultTemplateValue);
    }

    public static ZonedDateTimePattern CreateWithCurrentCulture(
      string patternText,
      IDateTimeZoneProvider zoneProvider)
    {
      return ZonedDateTimePattern.Create(patternText, NodaFormatInfo.CurrentInfo, Resolvers.StrictResolver, zoneProvider, ZonedDateTimePattern.DefaultTemplateValue);
    }

    public ZonedDateTimePattern WithPatternText(string newPatternText) => ZonedDateTimePattern.Create(newPatternText, this.formatInfo, this.resolver, this.zoneProvider, this.templateValue);

    private ZonedDateTimePattern WithFormatInfo(NodaFormatInfo newFormatInfo) => ZonedDateTimePattern.Create(this.patternText, newFormatInfo, this.resolver, this.zoneProvider, this.templateValue);

    public ZonedDateTimePattern WithCulture(CultureInfo cultureInfo) => this.WithFormatInfo(NodaFormatInfo.GetFormatInfo(cultureInfo));

    public ZonedDateTimePattern WithResolver(
      ZoneLocalMappingResolver newResolver)
    {
      return !(this.resolver == newResolver) ? ZonedDateTimePattern.Create(this.patternText, this.formatInfo, newResolver, this.zoneProvider, this.templateValue) : this;
    }

    public ZonedDateTimePattern WithZoneProvider(
      IDateTimeZoneProvider newZoneProvider)
    {
      return newZoneProvider != this.zoneProvider ? ZonedDateTimePattern.Create(this.patternText, this.formatInfo, this.resolver, newZoneProvider, this.templateValue) : this;
    }

    public ZonedDateTimePattern WithTemplateValue(ZonedDateTime newTemplateValue) => !(newTemplateValue == this.templateValue) ? ZonedDateTimePattern.Create(this.patternText, this.formatInfo, this.resolver, this.zoneProvider, newTemplateValue) : this;

    internal static class Patterns
    {
      internal static readonly ZonedDateTimePattern GeneralFormatOnlyPatternImpl = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss z '('o<g>')'", (IDateTimeZoneProvider) null);
      internal static readonly ZonedDateTimePattern ExtendedFormatOnlyPatternImpl = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFF z '('o<g>')'", (IDateTimeZoneProvider) null);
      internal static readonly PatternBclSupport<ZonedDateTime> BclSupport = new PatternBclSupport<ZonedDateTime>("G", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<ZonedDateTime>>) (fi => fi.ZonedDateTimePatternParser));
    }
  }
}
