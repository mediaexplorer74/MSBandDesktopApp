// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.StatHistoryEntry
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Utilities;
using System;

namespace Microsoft.Health.App.Core.Models
{
  public class StatHistoryEntry
  {
    private readonly bool isWeek;

    public StatHistoryEntry(DateTimeOffset date, int count, bool isWeek)
    {
      this.Date = date;
      this.Count = count;
      this.isWeek = isWeek;
    }

    public DateTimeOffset Date { get; private set; }

    public string DateText => this.isWeek ? Formatter.GetMonthDayString(this.Date, false) + " - " + Formatter.GetMonthDayString(this.Date.AddDays(6.0), false) : Formatter.FormatTileTime(this.Date, false);

    public string StartDateText => Formatter.FormatStatDateParameter(this.Date);

    public int Count { get; private set; }

    public string CountText => this.isWeek ? string.Empty : this.Count.ToString("n0");
  }
}
