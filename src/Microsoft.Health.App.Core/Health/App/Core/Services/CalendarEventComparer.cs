// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.CalendarEventComparer
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services
{
  public class CalendarEventComparer : IEqualityComparer<CalendarEvent>
  {
    public bool Equals(CalendarEvent x, CalendarEvent y)
    {
      if (x == null && y == null)
        return true;
      return x != null && y != null && this.CompareCalendarEvents(x, y);
    }

    private bool CompareCalendarEvents(CalendarEvent x, CalendarEvent y) => x.Title == y.Title && x.StartTime == y.StartTime && (int) x.Duration == (int) y.Duration;

    public int GetHashCode(CalendarEvent obj) => (obj.Title + ":" + obj.StartTime.ToString() + ":" + obj.Duration.ToString()).GetHashCode();
  }
}
