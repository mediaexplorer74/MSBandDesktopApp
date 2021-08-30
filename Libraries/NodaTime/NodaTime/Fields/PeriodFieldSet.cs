// Decompiled with JetBrains decompiler
// Type: NodaTime.Fields.PeriodFieldSet
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;

namespace NodaTime.Fields
{
  internal sealed class PeriodFieldSet
  {
    private readonly IPeriodField ticks;
    private readonly IPeriodField milliseconds;
    private readonly IPeriodField seconds;
    private readonly IPeriodField minutes;
    private readonly IPeriodField hours;
    private readonly IPeriodField days;
    private readonly IPeriodField weeks;
    private readonly IPeriodField months;
    private readonly IPeriodField years;

    internal IPeriodField Ticks => this.ticks;

    internal IPeriodField Milliseconds => this.milliseconds;

    internal IPeriodField Seconds => this.seconds;

    internal IPeriodField Minutes => this.minutes;

    internal IPeriodField Hours => this.hours;

    internal IPeriodField Days => this.days;

    internal IPeriodField Weeks => this.weeks;

    internal IPeriodField Months => this.months;

    internal IPeriodField Years => this.years;

    private PeriodFieldSet(PeriodFieldSet.Builder builder)
    {
      this.ticks = builder.Ticks;
      this.milliseconds = builder.Milliseconds;
      this.seconds = builder.Seconds;
      this.minutes = builder.Minutes;
      this.hours = builder.Hours;
      this.days = builder.Days;
      this.weeks = builder.Weeks;
      this.months = builder.Months;
      this.years = builder.Years;
    }

    internal sealed class Builder
    {
      internal IPeriodField Ticks { get; set; }

      internal IPeriodField Milliseconds { get; set; }

      internal IPeriodField Seconds { get; set; }

      internal IPeriodField Minutes { get; set; }

      internal IPeriodField Hours { get; set; }

      internal IPeriodField HalfDays { get; set; }

      internal IPeriodField Days { get; set; }

      internal IPeriodField Weeks { get; set; }

      internal IPeriodField Months { get; set; }

      internal IPeriodField Years { get; set; }

      internal Builder()
      {
      }

      internal Builder(PeriodFieldSet baseSet)
      {
        Preconditions.CheckNotNull<PeriodFieldSet>(baseSet, nameof (baseSet));
        this.Ticks = baseSet.Ticks;
        this.Milliseconds = baseSet.Milliseconds;
        this.Seconds = baseSet.Seconds;
        this.Minutes = baseSet.Minutes;
        this.Hours = baseSet.Hours;
        this.Days = baseSet.Days;
        this.Weeks = baseSet.Weeks;
        this.Months = baseSet.Months;
        this.Years = baseSet.Years;
      }

      internal PeriodFieldSet Build() => new PeriodFieldSet(this);
    }
  }
}
