// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Pedometer.PedometerLogger
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Diagnostics;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Newtonsoft.Json.Serialization;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Pedometer
{
  public sealed class PedometerLogger : 
    IPedometerLogger,
    IConfigurationState,
    IDiagnosticsDataProvider
  {
    private const string LogExtension = ".log";
    private const string SensorCoreLoggerCategory = "SensorCoreLogger";
    private static readonly string LastLoggedReadingKey = ConfigurationValue.CreateKey("SensorCoreLogger", "LastLoggedReading");
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Pedometer\\PedometerLogger.cs");
    public static readonly ConfigurationValue<bool> LogCollectedDataValue = ConfigurationValue.CreateBoolean("SensorCoreLogger", "LogCollectedData", false);
    private readonly IConfigProvider configProvider;
    private readonly IConfigurationService configurationService;
    private readonly IFileSystemService fileSystemService;
    private readonly Mutex sensorCoreLoggerMutex;

    public PedometerLogger(
      IConfigProvider configProvider,
      IConfigurationService configurationService,
      IFileSystemService fileSystemService,
      IMutexService mutexService)
    {
      Assert.ParamIsNotNull((object) configProvider, nameof (configProvider));
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      Assert.ParamIsNotNull((object) fileSystemService, nameof (fileSystemService));
      this.configProvider = configProvider;
      this.configurationService = configurationService;
      this.fileSystemService = fileSystemService;
      this.sensorCoreLoggerMutex = mutexService.GetNamedMutex(false, "KApp.Pedometer.Logger");
    }

    public Task<DateTimeOffset> GetLastLoggedReadingAsync(CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      return Task.FromResult<DateTimeOffset>(this.GetLastLoggedReading());
    }

    public Task LogReadingsAsync(IEnumerable<SensorReading> readings, CancellationToken token)
    {
      Assert.ParamIsNotNull((object) readings, nameof (readings));
      if (this.configurationService.GetValue<bool>(PedometerLogger.LogCollectedDataValue))
        return this.sensorCoreLoggerMutex.RunSynchronizedAsync((Func<Task>) (async () =>
        {
          PedometerLogger.Logger.Info((object) "Starting logging...");
          List<SensorReading> orderedReadings = readings.OrderBy<SensorReading, DateTimeOffset>((Func<SensorReading, DateTimeOffset>) (reading => reading.StartTime + reading.Duration)).ToList<SensorReading>();
          if (orderedReadings.Any<SensorReading>())
          {
            SensorReading sensorReading = orderedReadings.Last<SensorReading>();
            DateTimeOffset lastReadingTime = sensorReading.StartTime + sensorReading.Duration;
            string fileName = PedometerLogger.CreateLogFileName(lastReadingTime);
            PedometerLogger.Logger.Info("Writing readings to '{0}'...", (object) fileName);
            await (await (await this.fileSystemService.GetSensorCoreLogsFolderAsync().ConfigureAwait(false)).CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting, token).ConfigureAwait(false)).WriteJsonAsync((object) new SensorReadingLog((IEnumerable<SensorReading>) orderedReadings), token).ConfigureAwait(false);
            if (this.GetLastLoggedReading() < lastReadingTime)
              this.SetLastLoggedReading(lastReadingTime);
            lastReadingTime = new DateTimeOffset();
            fileName = (string) null;
          }
          PedometerLogger.Logger.Info((object) "Logging complete.");
        }), token);
      PedometerLogger.Logger.Info((object) "SensorCore logging is disabled; log request ignored.");
      return (Task) Task.FromResult<bool>(true);
    }

    public Task RemoveReadingsAsync(DateTimeOffset endTime, CancellationToken token)
    {
        //TODO
        /*
        Func<<IFile, DateTimeOffset?>, bool> func;
        return this.sensorCoreLoggerMutex.RunSynchronizedAsync((Func<Task>) (async () =>
        {
            PedometerLogger.Logger.Debug("Removing logs with all entries older than '{0}'...", (object) endTime);
            foreach (IFile log in (await (await this.fileSystemService.GetSensorCoreLogsFolderAsync().ConfigureAwait(false)).GetFilesAsync(token).ConfigureAwait(false)).Select(file => new
            {
            File = file,
            EndTime = PedometerLogger.ParseDateTimeFromFileName(file.Name)
            }).Where(func ?? (func = file =>
            {
            if (!file.EndTime.HasValue)
                return false;
            DateTimeOffset? endTime2 = file.EndTime;
            DateTimeOffset dateTimeOffset = endTime;
            return endTime2.HasValue && endTime2.GetValueOrDefault() < dateTimeOffset;
            })).Select(file => file.File).ToList<IFile>())
            {
            PedometerLogger.Logger.Debug("Removing log '{0}'...", (object) log.Name);
            try
            {
                await log.DeleteAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                PedometerLogger.Logger.Error(ex, "Unable to delete log file {0}.", (object) log);
            }
            }
            PedometerLogger.Logger.Debug((object) "Logs removed.");
        }), token);
        */
        return default; // temp
    }

    public Task ResetStateAsync(CancellationToken token)
    {
      this.configProvider.Remove(PedometerLogger.LastLoggedReadingKey);
      return this.RemoveReadingsAsync(DateTimeOffset.MaxValue, token);
    }

    Task IDiagnosticsDataProvider.CaptureDiagnosticsDataAsync(
      ZipArchive archive,
      CancellationToken token)
    {
      Assert.ParamIsNotNull((object) archive, nameof (archive));
      return !this.configurationService.GetValue<bool>(PedometerLogger.LogCollectedDataValue) ? (Task) Task.FromResult<bool>(true) : this.sensorCoreLoggerMutex.RunSynchronizedAsync((Func<Task>) (async () =>
      {
        // TODO
        /*
        foreach (IFile file in (await (await this.fileSystemService.GetSensorCoreLogsFolderAsync().ConfigureAwait(false)).GetFilesAsync(token).ConfigureAwait(false)).Where<IFile>((Func<IFile, bool>) (file => PedometerLogger.IsLogFile(file.Name))).ToList<IFile>())
        {
          string[] strArray = new string[2]
          {
            "other",
            file.Name
          };
          using (Stream entryStream = archive.CreateEntry(Path.Combine(strArray)).Open())
            await file.CopyToAsync(entryStream, token).ConfigureAwait(false);
        }
        */
      }), token);
    }

    private DateTimeOffset GetLastLoggedReading() => this.configProvider.GetDateTimeOffset(PedometerLogger.LastLoggedReadingKey, DateTimeOffset.MinValue);

    private void SetLastLoggedReading(DateTimeOffset time)
    {
      this.configProvider.SetDateTimeOffset(PedometerLogger.LastLoggedReadingKey, time);
      PedometerLogger.Logger.Debug("Last logged SensorCore reading was '{0}'.", (object) time);
    }

    private static string CreateLogFileName(DateTimeOffset endTime) => endTime.UtcTicks.ToString() + ".log";

    private static bool IsLogFile(string fileName) => PedometerLogger.ParseDateTimeFromFileName(fileName).HasValue;

    private static DateTimeOffset? ParseDateTimeFromFileName(string fileName)
    {
      if (!StringComparer.OrdinalIgnoreCase.Equals(Path.GetExtension(fileName), ".log"))
        return new DateTimeOffset?();
      long result;
      return long.TryParse(Path.GetFileNameWithoutExtension(fileName), out result) ? new DateTimeOffset?(new DateTimeOffset(result, TimeSpan.Zero)) : new DateTimeOffset?();
    }
  }
}
