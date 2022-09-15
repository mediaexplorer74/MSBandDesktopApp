// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.CalendarServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Utilities;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public abstract class CalendarServiceBase : ICalendarService, ICalendarTileUpdateListener
  {
    public static readonly TimeSpan CalendarEventWindow = TimeSpan.FromDays(14.0);
    private const string CalendarEventsFileName = "CalendarEvents";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\CalendarServiceBase.cs");
    private IFileSystemService fileSystemService;
    private IDateTimeService dateTimeService;
    private AsyncLock calendarFileLock = new AsyncLock();
    private CalendarEventComparer calendarEventComparer = new CalendarEventComparer();

    protected CalendarServiceBase(
      IFileSystemService fileSystemService,
      IDateTimeService dateTimeService)
    {
      this.fileSystemService = fileSystemService;
      this.dateTimeService = dateTimeService;
    }

    public abstract Task<IList<CalendarEvent>> GetCalendarEventsAsync(
      CancellationToken cancellationToken);

    protected IList<CalendarEvent> OrderAndTrim(
      IEnumerable<CalendarEvent> calendarEvents)
    {
      return (IList<CalendarEvent>) calendarEvents.Distinct<CalendarEvent>((IEqualityComparer<CalendarEvent>) this.calendarEventComparer).OrderBy<CalendarEvent, DateTime>((Func<CalendarEvent, DateTime>) (s => s.StartTime)).Take<CalendarEvent>(8).ToList<CalendarEvent>();
    }

    private static string SerializeCalendarEvents(IList<CalendarEvent> calendarEvents)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (CalendarEvent calendarEvent in (IEnumerable<CalendarEvent>) calendarEvents)
      {
        stringBuilder.AppendFormat("CalendarEvent: Title=['{0}'], Location=['{1}'], StartDate=[{2}], Duration[{3} mins], Availability=[{4}], IsAllDay=[{5}], Reminder=[{6} mins], EventCategory=[{7}]", (object) calendarEvent.Title, (object) calendarEvent.Location, (object) calendarEvent.StartTime, (object) calendarEvent.Duration, (object) calendarEvent.AcceptedState, (object) calendarEvent.AllDay, (object) calendarEvent.NotificationTime, (object) calendarEvent.EventCategory);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Environment.NewLine);
      }
      return stringBuilder.ToString();
    }

    public async Task<bool> GetCalendarEventsHasChangedAsync(IList<CalendarEvent> calendarEvents)
    {
      string newEvents = this.TrimAllNewlines(CalendarServiceBase.SerializeCalendarEvents(calendarEvents));
      LastSentCalendarEventsState calendarEventsAsync = await this.GetLastCalendarEventsAsync();
      return calendarEventsAsync == null || newEvents != this.TrimAllNewlines(calendarEventsAsync.SerializedCalendarEvents);
    }

    private string TrimAllNewlines(string inputString)
    {
      if (string.IsNullOrEmpty(inputString))
        return string.Empty;
      string newLine = Environment.NewLine;
      if (inputString.Contains(newLine))
      {
        while (inputString.Substring(inputString.Length - newLine.Length, newLine.Length) == newLine)
        {
          inputString = inputString.Substring(0, inputString.Length - newLine.Length);
          if (inputString.Length == 0)
            break;
        }
      }
      return inputString;
    }

    public async void SaveLastSentCalendarEvents(IList<CalendarEvent> calendarEvents)
    {
      AsyncLock.Releaser releaser = await this.calendarFileLock.LockAsync().ConfigureAwait(false);
      try
      {
        string serializedCalendarEvents = CalendarServiceBase.SerializeCalendarEvents(calendarEvents);
        using (Stream fileStream = await (await (await this.fileSystemService.GetCalendarCacheFolderAsync().ConfigureAwait(false)).CreateFileAsync(
            "CalendarEvents", CreationCollisionOption.ReplaceExisting).ConfigureAwait(false))
            .OpenAsync(PCLStorage.FileAccess.ReadAndWrite).ConfigureAwait(false))
        {
          StreamWriter writer = default;
          int num = 0;
          if (num != 4 && num != 5)
            writer = new StreamWriter(fileStream);
          try
          {
            ConfiguredTaskAwaitable configuredTaskAwaitable = writer.WriteLineAsync(DateTimeOffset.Now.ToString()).ConfigureAwait(false);
            await configuredTaskAwaitable;
            configuredTaskAwaitable = writer.WriteLineAsync(serializedCalendarEvents).ConfigureAwait(false);
            await configuredTaskAwaitable;
          }
          finally
          {
            writer?.Dispose();
          }
          writer = (StreamWriter) null;
        }
        serializedCalendarEvents = (string) null;
      }
      catch (Exception ex)
      {
        CalendarServiceBase.Logger.Error(ex, "SaveLastSentCalendarEvents failed");
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
    }

    public async Task<LastSentCalendarEventsState> GetLastCalendarEventsAsync()
    {
      using (await this.calendarFileLock.LockAsync().ConfigureAwait(false))
      {
        try
        {
          IFile file = await (await this.fileSystemService.GetCalendarCacheFolderAsync().ConfigureAwait(false)).TryGetFileAsync("CalendarEvents").ConfigureAwait(false);
          if (file != null)
          {
            using (Stream fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read).ConfigureAwait(false))
            {
              StreamReader reader = default;
              int num = 0;
              if (num != 4 && num != 5)
                reader = new StreamReader(fileStream);
              try
              {
                string timestamp = await reader.ReadLineAsync().ConfigureAwait(false);
                string str = await reader.ReadToEndAsync().ConfigureAwait(false);
                return new LastSentCalendarEventsState()
                {
                  Timestamp = timestamp,
                  SerializedCalendarEvents = str
                };
              }
              finally
              {
                reader?.Dispose();
              }
            }
          }
        }
        catch (Exception ex)
        {
          CalendarServiceBase.Logger.Error(ex, "GetLastCalendarEventsAsync failed");
        }
        return (LastSentCalendarEventsState) null;
      }
    }
  }
}
