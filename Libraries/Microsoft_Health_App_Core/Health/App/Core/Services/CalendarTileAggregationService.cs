// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.CalendarTileAggregationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class CalendarTileAggregationService : ICalendarTileAggregationService
  {
    private static readonly int MaxCalendarEntries = 8;

    public async Task<IList<CalendarEvent>> GetCalendarEventsAsync(
      CancellationToken token)
    {
      List<CalendarEvent> allEvents = new List<CalendarEvent>();
      foreach (ICalendarTileUpdateListener allInstance in ServiceLocator.Current.GetAllInstances<ICalendarTileUpdateListener>())
      {
        List<CalendarEvent> calendarEventList = allEvents;
        CancellationToken none = CancellationToken.None;
        IList<CalendarEvent> calendarEventsAsync = await allInstance.GetCalendarEventsAsync(none);
        calendarEventList.AddRange((IEnumerable<CalendarEvent>) calendarEventsAsync);
        calendarEventList = (List<CalendarEvent>) null;
      }
      return (IList<CalendarEvent>) allEvents.OrderBy<CalendarEvent, DateTime>((Func<CalendarEvent, DateTime>) (x => x.StartTime)).Take<CalendarEvent>(CalendarTileAggregationService.MaxCalendarEntries).ToList<CalendarEvent>();
    }
  }
}
