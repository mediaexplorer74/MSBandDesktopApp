// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Debugging.DebugReporterService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Debugging
{
  public class DebugReporterService : IDebugReporterService
  {
    private const string DataStoreFileName = "DebugReporter.json";
    private const int CacheSize = 20;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Debugging\\DebugReporterService.cs");
    private readonly JsonSerializer serializer;
    private readonly IFileObjectStorageService fileObjectStorageService;
    private AsyncLock debugReporterFileLock = new AsyncLock();
    private IList<DebugReporterEntry> dataStore;

    public long SdeCheckElapsed { get; set; }

    public DebugReporterService(IFileObjectStorageService fileObjectStorageService)
    {
      this.fileObjectStorageService = fileObjectStorageService;
      this.serializer = JsonSerializer.Create(new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented
      });
      this.LoadDataStore();
    }

    private async void LoadDataStore()
    {
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
        await this.LoadAsync(cancellationTokenSource.Token);
    }

    public IList<DebugReporterEntry> GetReport() => this.dataStore;

    public void RecordEntry(DebugReporterEntry entry, int indentLevel = 0)
    {
      if (indentLevel > 0)
      {
        StringBuilder stringBuilder = new StringBuilder(indentLevel);
        stringBuilder.Insert(0, "    ", indentLevel);
        entry.LineEntry = entry.LineEntry.Insert(0, stringBuilder.ToString());
      }
      this.dataStore.Insert(0, entry);
    }

    public async Task SaveAsync(CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      this.PurgeAboveLimit(20);
      await this.fileObjectStorageService.WriteObjectAsync((object) this.dataStore, "DebugReporter.json", token);
    }

    private void PurgeAboveLimit(int limit)
    {
      int num = 0;
      for (int index = 0; this.dataStore.Count > index; ++index)
      {
        if (!this.dataStore[index].LineEntry.StartsWith(" "))
          ++num;
        if (num > limit)
        {
          this.dataStore = (IList<DebugReporterEntry>) this.dataStore.Take<DebugReporterEntry>(index).ToList<DebugReporterEntry>();
          break;
        }
      }
    }

    public async Task LoadAsync(CancellationToken token)
    {
      this.dataStore = (IList<DebugReporterEntry>) new List<DebugReporterEntry>();
      try
      {
        IList<DebugReporterEntry> debugReporterEntryList = await this.fileObjectStorageService.ReadObjectAsync<IList<DebugReporterEntry>>("DebugReporter.json", token);
        if (debugReporterEntryList == null)
          return;
        this.dataStore = debugReporterEntryList;
      }
      catch (Exception ex)
      {
        DebugReporterService.Logger.Debug((object) "Creating new debug logger");
      }
    }

    public async void RecordSyncResult(SyncDebugResult syncResult)
    {
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Guided Workout: " + (object) syncResult.GuidedWorkout
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Weather: " + (object) syncResult.Weather
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Finance: " + (object) syncResult.Finance
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Calendar: " + (object) syncResult.Calendar
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Goals: " + (object) syncResult.Goals
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Tiles Update Elapsed: " + (object) syncResult.TilesUpdate
      }, 1);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Web tiles(WP): " + (object) syncResult.WebTiles
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Crash dump(WP): " + (object) syncResult.CrashDump
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Ephemeris file check(WP): " + (object) syncResult.EphemerisCheckElapsed
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Ephemeris file update(WP): " + (object) syncResult.EphemerisUpdateElapsed
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Time zone(WP): " + (object) syncResult.TimeZone
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Send phone sensor data to cloud: " + (object) syncResult.SendPhoneSensorToCloud
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Cloud processing: " + (object) syncResult.CloudProcessing
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "KDK Misc Tasks(Android): " + (object) (syncResult.SyncElapsed - (syncResult.FetchLogFromBand + syncResult.SendLogToCloud))
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Send log to cloud: " + (object) syncResult.SendLogToCloud
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Fetch log from Band: " + (object) syncResult.FetchLogFromBand
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "User profile firmware bytes: " + (object) syncResult.UserProfileFirmwareBytes
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "User profile full: " + (object) syncResult.UserProfileFull
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "SDE check: " + (object) this.SdeCheckElapsed
      }, 2);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Sync Elapsed: " + (object) syncResult.SyncElapsed
      }, 1);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = "Start: " + (object) syncResult.StartTime
      }, 1);
      this.RecordEntry(new DebugReporterEntry()
      {
        SyntaxColor = "Normal",
        LineEntry = string.Format("{0} Sync", new object[1]
        {
          (object) syncResult.SyncType
        })
      });
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
        await this.SaveAsync(cancellationTokenSource.Token);
      DebugReporterService.Logger.Debug((object) string.Format("{0} Sync", new object[1]
      {
        (object) syncResult.SyncType
      }));
      DebugReporterService.Logger.Debug((object) ("     Start: " + (object) syncResult.StartTime));
      DebugReporterService.Logger.Debug((object) ("     Sync Elapsed: " + (object) syncResult.SyncElapsed));
      DebugReporterService.Logger.Debug((object) ("          SDE check: " + (object) this.SdeCheckElapsed));
      DebugReporterService.Logger.Debug((object) ("          User profile firmware bytes: " + (object) syncResult.UserProfileFirmwareBytes));
      DebugReporterService.Logger.Debug((object) ("          User profile full: " + (object) syncResult.UserProfileFirmwareBytes));
      DebugReporterService.Logger.Debug((object) ("          Send log to cloud: " + (object) syncResult.SendLogToCloud));
      DebugReporterService.Logger.Debug((object) ("          Fetch log from Band: " + (object) syncResult.FetchLogFromBand));
      DebugReporterService.Logger.Debug((object) ("          Cloud processing: " + (object) syncResult.CloudProcessing));
      DebugReporterService.Logger.Debug((object) ("          Send phone sensor data to cloud: " + (object) syncResult.SendPhoneSensorToCloud));
      DebugReporterService.Logger.Debug((object) ("          Time zone: " + (object) syncResult.TimeZone));
      DebugReporterService.Logger.Debug((object) ("          Ephemeris file check: " + (object) syncResult.EphemerisCheckElapsed));
      DebugReporterService.Logger.Debug((object) ("          Ephemeris file update: " + (object) syncResult.EphemerisUpdateElapsed));
      DebugReporterService.Logger.Debug((object) ("          Crash dump: " + (object) syncResult.CrashDump));
      DebugReporterService.Logger.Debug((object) ("          Web tiles: " + (object) syncResult.WebTiles));
      DebugReporterService.Logger.Debug((object) ("     Tiles Update Elapsed: " + (object) syncResult.TilesUpdate));
      DebugReporterService.Logger.Debug((object) ("          Goals: " + (object) syncResult.Goals));
      DebugReporterService.Logger.Debug((object) ("          Calendar: " + (object) syncResult.Calendar));
      DebugReporterService.Logger.Debug((object) ("          Finance: " + (object) syncResult.Finance));
      DebugReporterService.Logger.Debug((object) ("          Weather: " + (object) syncResult.Weather));
      DebugReporterService.Logger.Debug((object) ("          Guided Workout: " + (object) syncResult.GuidedWorkout));
    }

    public void ResetSdeTotalElapsed() => this.SdeCheckElapsed = 0L;
  }
}
