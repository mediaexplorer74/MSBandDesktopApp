// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.EventTrackingService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Services
{
  public class EventTrackingService : IEventTrackingService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\EventTrackingService.cs");
    private static readonly TimeSpan ScavengeInterval = TimeSpan.FromDays(2.0);
    private static readonly TimeSpan ScavengeAgeLimit = TimeSpan.FromDays(14.0);
    private readonly IConfig config;
    private readonly DateTimeOffset lastScavengeTime;
    private readonly object viewSync = new object();
    private readonly Dictionary<string, DateTimeOffset> viewTimes;

    public EventTrackingService(IConfig config)
    {
      this.config = config;
      this.lastScavengeTime = DateTimeOffset.MinValue;
      if (!string.IsNullOrEmpty(this.config.ViewedEventsJson))
      {
        try
        {
          this.viewTimes = JsonConvert.DeserializeObject<Dictionary<string, DateTimeOffset>>(this.config.ViewedEventsJson);
        }
        catch (JsonException ex)
        {
          EventTrackingService.Logger.ErrorAndDebug((Exception) ex, "Could not read in viewed events dictionary.");
        }
      }
      if (this.viewTimes != null)
        return;
      this.viewTimes = new Dictionary<string, DateTimeOffset>();
    }

    public void ReportView(string eventId)
    {
      lock (this.viewSync)
      {
        if (this.viewTimes.ContainsKey(eventId))
          return;
        this.viewTimes.Add(eventId, DateTimeOffset.Now);
        this.ScavengeIfNeeded();
        this.SaveViewTimes();
      }
    }

    public bool WasViewed(string eventId)
    {
      lock (this.viewSync)
        return this.viewTimes.ContainsKey(eventId);
    }

    private void ScavengeIfNeeded()
    {
      DateTimeOffset now = DateTimeOffset.Now;
      if (!(this.lastScavengeTime + EventTrackingService.ScavengeInterval < now))
        return;
      foreach (KeyValuePair<string, DateTimeOffset> keyValuePair in this.viewTimes.Where<KeyValuePair<string, DateTimeOffset>>((Func<KeyValuePair<string, DateTimeOffset>, bool>) (p => p.Value + EventTrackingService.ScavengeAgeLimit < now)).ToList<KeyValuePair<string, DateTimeOffset>>())
        this.viewTimes.Remove(keyValuePair.Key);
    }

    private void SaveViewTimes()
    {
      try
      {
        this.config.ViewedEventsJson = JsonConvert.SerializeObject((object) this.viewTimes);
      }
      catch (JsonException ex)
      {
        EventTrackingService.Logger.ErrorAndDebug((Exception) ex, "Could not save viewed events dictionary.");
      }
    }
  }
}
