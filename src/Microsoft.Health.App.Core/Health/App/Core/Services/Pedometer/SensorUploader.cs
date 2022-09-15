// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Pedometer.SensorUploader
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Pedometer
{
  public class SensorUploader : ISensorUploader, IConfigurationState
  {
    private static readonly string LastCompleteReadingEndTimeKey = ConfigurationValue.CreateKey("SensorCoreUploader", "LastCompleteReadingEndTime");
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Pedometer\\SensorUploader.cs");
    private ISensorDataClient sensorDataClient;
    private IEnvironmentService environmentService;
    private IConfigProvider configProvider;
    private IDateTimeService dateTimeService;

    public SensorUploader(
      ISensorDataClient sensorDataClient,
      IEnvironmentService environmentService,
      IConfigProvider configProvider,
      IDateTimeService dateTimeService)
    {
      Assert.ParamIsNotNull((object) sensorDataClient, nameof (sensorDataClient));
      Assert.ParamIsNotNull((object) environmentService, nameof (environmentService));
      Assert.ParamIsNotNull((object) configProvider, nameof (configProvider));
      Assert.ParamIsNotNull((object) dateTimeService, nameof (dateTimeService));
      this.sensorDataClient = sensorDataClient;
      this.environmentService = environmentService;
      this.configProvider = configProvider;
      this.dateTimeService = dateTimeService;
    }

    public async Task<DateTimeOffset> GetLastCompleteReadingEndTimeAsync(
      TimeSpan duration,
      CancellationToken cancellationToken)
    {
      DateTimeOffset lastCompleteReadingEndTime = DateTimeOffset.MinValue;
      if (this.configProvider.TryGetDateTimeOffset(SensorUploader.LastCompleteReadingEndTimeKey, out lastCompleteReadingEndTime))
        return lastCompleteReadingEndTime;
      SensorData sensorData = await this.sensorDataClient.ReadTopSensorDataAsync(this.environmentService.PhoneId, this.dateTimeService.Now.ToUniversalTime(), 1, cancellationToken).ConfigureAwait(false);
      if (sensorData != null)
      {
        SensorBase sensorBase = ((IEnumerable<IEnumerable<SensorBase>>) new IEnumerable<SensorBase>[5]
        {
          (IEnumerable<SensorBase>) sensorData.Steps,
          (IEnumerable<SensorBase>) sensorData.Calories,
          (IEnumerable<SensorBase>) sensorData.Distances,
          (IEnumerable<SensorBase>) sensorData.HeartRates,
          (IEnumerable<SensorBase>) sensorData.TimezoneChanges
        }).SelectMany<IEnumerable<SensorBase>, SensorBase>((Func<IEnumerable<SensorBase>, IEnumerable<SensorBase>>) (readings => readings)).Where<SensorBase>((Func<SensorBase, bool>) (reading => reading.Timestamp.HasValue)).OrderBy<SensorBase, DateTimeOffset?>((Func<SensorBase, DateTimeOffset?>) (reading => reading.Timestamp)).LastOrDefault<SensorBase>();
        if (sensorBase != null)
          lastCompleteReadingEndTime = !(sensorBase.Duration == duration) ? sensorBase.Timestamp.Value - sensorBase.Duration : sensorBase.Timestamp.Value;
      }
      this.configProvider.SetDateTimeOffset(SensorUploader.LastCompleteReadingEndTimeKey, lastCompleteReadingEndTime);
      return lastCompleteReadingEndTime;
    }

    public async Task UploadReadingsAsync(
      IEnumerable<SensorReading> readings,
      TimeSpan duration,
      IDictionary<string, string> uploadMetadata,
      CancellationToken cancellationToken)
    {
      Assert.ParamIsNotNull((object) readings, nameof (readings));
      SensorUploader.Logger.Info((object) "Uploading readings...");
      List<SensorReading> readingsList = readings.ToList<SensorReading>();
      if (readingsList.Count == 0)
        return;
      SensorUploader.Logger.Debug("There are {0} raw readings...", (object) readingsList.Count);
      DateTimeOffset logStartTime = readingsList[0].StartTime;
      DateTimeOffset now = this.dateTimeService.Now;
      DateTimeOffset universalTime = now.ToUniversalTime();
      SensorData sensorData = new SensorData((IEnumerable<StepsSensor>) readingsList.TakeWhile<SensorReading>((Func<SensorReading, int, bool>) ((reading, index) => index < readingsList.Count - 1)).Where<SensorReading>((Func<SensorReading, bool>) (reading => reading.StepsTaken > 0)).Select<SensorReading, StepsSensor>((Func<SensorReading, StepsSensor>) (reading => SensorUploader.CreateStepsSensorFromReading(logStartTime, reading))).ToList<StepsSensor>())
      {
        LogStartUtcTime = logStartTime.ToUniversalTime(),
        UploadTimeZoneOffset = (short) now.Offset.TotalMinutes,
        DeviceId = this.environmentService.PhoneId,
        OperationId = universalTime.ToUnixTimestamp()
      };
      sensorData.Steps.Add(SensorUploader.CreateStepsSensorFromReading(logStartTime, readingsList.Last<SensorReading>()));
      SensorUploader.Logger.Debug("Uploading {0} filtered reading(s)...", (object) sensorData.Steps.Count);
      try
      {
        string jsonString = this.UploadMetadataToJsonString(uploadMetadata);
        string empty = string.Empty;
        if (uploadMetadata.ContainsKey("HostOS"))
          empty = uploadMetadata["HostOS"];
        await this.sensorDataClient.UploadSensorDataAsync(sensorData, empty, jsonString, cancellationToken).ConfigureAwait(false);
        SensorReading sensorReading = readingsList.Last<SensorReading>();
        DateTimeOffset dateTimeOffset = sensorReading.Duration < duration ? sensorReading.StartTime : sensorReading.StartTime + sensorReading.Duration;
        SensorUploader.Logger.Debug("Last complete reading end time is now '{0}'.", (object) dateTimeOffset);
        this.configProvider.SetDateTimeOffset(SensorUploader.LastCompleteReadingEndTimeKey, dateTimeOffset.ToUniversalTime());
        SensorUploader.Logger.Info((object) "Upload complete.");
      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        SensorUploader.Logger.Info((object) "Completed process and returning.");
      }
    }

    private string UploadMetadataToJsonString(IDictionary<string, string> dict) => "{" + string.Join(",", dict.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (d => string.Format("\"{0}\":\"{1}\"", new object[2]
    {
      (object) d.Key,
      (object) d.Value
    })))) + "}";

    public Task ResetStateAsync(CancellationToken token)
    {
      this.configProvider.Remove(SensorUploader.LastCompleteReadingEndTimeKey);
      return this.environmentService.ResetStateAsync(token);
    }

    private static StepsSensor CreateStepsSensorFromReading(
      DateTimeOffset startTime,
      SensorReading reading)
    {
      StepsSensor stepsSensor = new StepsSensor();
      stepsSensor.Steps = (uint) reading.StepsTaken;
      stepsSensor.Offset = reading.StartTime - startTime;
      stepsSensor.Duration = reading.Duration;
      return stepsSensor;
    }
  }
}
