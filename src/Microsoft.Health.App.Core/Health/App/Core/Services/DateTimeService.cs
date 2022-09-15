// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.DateTimeService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using NodaTime;
using System;

namespace Microsoft.Health.App.Core.Services
{
  public class DateTimeService : IDateTimeService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\DateTimeService.cs");
    private readonly IConfigProvider configProvider;
    private DateTimeOffset startTime;

    public DateTimeService(IConfigProvider configProvider)
    {
      DateTimeService.Logger.Debug((object) "Instantiating the [DateTimeService]...");
      this.configProvider = configProvider != null ? configProvider : throw new ArgumentNullException(nameof (configProvider));
      if (this.IsDebugSelectedDateTimeEnabled)
        this.RestartDebugTimePassedCounter();
      DateTimeService.Logger.Debug((object) "Instantiating the [DateTimeService]... [complete]");
    }

    public void RestartDebugTimePassedCounter()
    {
      this.startTime = DateTimeOffset.Now;
      DateTimeService.Logger.Debug("Restarted Date and Time at [{0}]...", (object) this.Now.ToString("MM/dd/yyyy h:mm tt zz"));
    }

    public bool IsDebugSelectedDateTimeEnabled
    {
      get => this.configProvider.Get<bool>("DateTimeService.IsDebugSelectedDateTimeEnabled", false, ConfigDomain.System);
      set
      {
        DateTimeService.Logger.Debug("Setting Override Operating System Time Enabled to [{0}]...", (object) value);
        this.configProvider.Set<bool>("DateTimeService.IsDebugSelectedDateTimeEnabled", value, ConfigDomain.System);
        if (!this.IsDebugSelectedDateTimeEnabled)
          return;
        this.RestartDebugTimePassedCounter();
      }
    }

    public DateTimeOffset DebugStartDate
    {
      get => (DateTimeOffset) this.configProvider.Get<DateTime>("DateTimeService.DebugCurrentDate", DateTimeOffset.Now.Date, ConfigDomain.System);
      set
      {
        DateTimeService.Logger.Debug("Setting Start Date to [{0}]...", (object) value.Date.ToString("MM/dd/yyyy"));
        this.configProvider.Set<DateTime>("DateTimeService.DebugCurrentDate", value.Date, ConfigDomain.System);
        if (!this.IsDebugSelectedDateTimeEnabled)
          return;
        this.RestartDebugTimePassedCounter();
      }
    }

    public TimeSpan DebugStartTime
    {
      get => this.configProvider.Get<TimeSpan>("DateTimeService.DebugCurrentTime", DateTime.Now.TimeOfDay, ConfigDomain.System);
      set
      {
        DateTimeService.Logger.Debug("Setting Start Time to [{0}]...", (object) value.ToString("h\\:mm"));
        this.configProvider.Set<TimeSpan>("DateTimeService.DebugCurrentTime", value, ConfigDomain.System);
        if (!this.IsDebugSelectedDateTimeEnabled)
          return;
        this.RestartDebugTimePassedCounter();
      }
    }

    public DateTimeZone DebugTimeZone
    {
      get => DateTimeZoneProviders.Tzdb[this.configProvider.Get<string>("DateTimeService.DebugTimeZone", DateTimeZoneProviders.Tzdb.GetSystemDefault().Id, ConfigDomain.System)];
      set
      {
        DateTimeService.Logger.Debug("Setting Time Zone to [{0}]...", (object) value.Id);
        this.configProvider.Set<string>("DateTimeService.DebugTimeZone", value.Id, ConfigDomain.System);
        if (!this.IsDebugSelectedDateTimeEnabled)
          return;
        this.RestartDebugTimePassedCounter();
      }
    }

    public DateTimeZone TimeZone => this.IsDebugSelectedDateTimeEnabled ? this.DebugTimeZone : DateTimeZoneProviders.Tzdb.GetSystemDefault();

    public DateTimeOffset Now
    {
      get
      {
        if (!this.IsDebugSelectedDateTimeEnabled)
          return DateTimeOffset.Now;
        TimeSpan timeSpan = DateTimeOffset.Now - this.startTime;
        DateTimeOffset debugStartDate = this.DebugStartDate;
        TimeSpan debugStartTime = this.DebugStartTime;
        DateTimeZone debugTimeZone = this.DebugTimeZone;
        DateTimeOffset dateTimeOffset = new LocalDateTime(debugStartDate.Year, debugStartDate.Month, debugStartDate.Day, debugStartTime.Hours, debugStartTime.Minutes, debugStartTime.Seconds).PlusTicks(timeSpan.Ticks).InZoneLeniently(this.TimeZone).ToDateTimeOffset();
        DateTimeService.Logger.Debug("Reporting Current Date and Time As [{0}]...", (object) dateTimeOffset.ToString("MM/dd/yyyy h:mm tt zz"));
        return dateTimeOffset;
      }
    }
  }
}
