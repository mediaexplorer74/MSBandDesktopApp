// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.CoreEventSource
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.Diagnostics.Tracing;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  [EventSource(Name = "Microsoft-ApplicationInsights-Core")]
  internal sealed class CoreEventSource : EventSource
  {
    public static readonly CoreEventSource Log = new CoreEventSource();
    private readonly ApplicationNameProvider nameProvider = new ApplicationNameProvider();

    [Event(10, Keywords = (EventKeywords) 4, Level = EventLevel.Verbose, Message = "[msg=Log verbose];[msg={0}]")]
    public void LogVerbose(string msg, string appDomainName = "Incorrect") => this.WriteEvent(10, msg ?? string.Empty, this.nameProvider.Name);

    [Event(20, Keywords = (EventKeywords) 3, Level = EventLevel.Informational, Message = "Diagnostics event throttling has been started for the event {0}")]
    public void DiagnosticsEventThrottlingHasBeenStartedForTheEvent(
      int eventId,
      string appDomainName = "Incorrect")
    {
      this.WriteEvent(20, eventId, this.nameProvider.Name);
    }

    [Event(30, Keywords = (EventKeywords) 3, Level = EventLevel.Informational, Message = "Diagnostics event throttling has been reset for the event {0}, event was fired {1} times during last interval")]
    public void DiagnosticsEventThrottlingHasBeenResetForTheEvent(
      int eventId,
      int executionCount,
      string appDomainName = "Incorrect")
    {
      this.WriteEvent(30, (object) eventId, (object) executionCount, (object) this.nameProvider.Name);
    }

    [Event(40, Keywords = (EventKeywords) 2, Level = EventLevel.Warning, Message = "Scheduler timer dispose failure: {0}")]
    public void DiagnoisticsEventThrottlingSchedulerDisposeTimerFailure(
      string exception,
      string appDomainName = "Incorrect")
    {
      this.WriteEvent(40, exception ?? string.Empty, this.nameProvider.Name);
    }

    [Event(50, Keywords = (EventKeywords) 2, Level = EventLevel.Verbose, Message = "A scheduler timer was created for the interval: {0}")]
    public void DiagnoisticsEventThrottlingSchedulerTimerWasCreated(
      int intervalInMilliseconds,
      string appDomainName = "Incorrect")
    {
      this.WriteEvent(50, intervalInMilliseconds, this.nameProvider.Name);
    }

    [Event(60, Keywords = (EventKeywords) 2, Level = EventLevel.Verbose, Message = "A scheduler timer was removed")]
    public void DiagnoisticsEventThrottlingSchedulerTimerWasRemoved(string appDomainName = "Incorrect") => this.WriteEvent(60, this.nameProvider.Name);

    [Event(70, Level = EventLevel.Warning, Message = "No Telemetry Configuration provided. Using the default TelemetryConfiguration.Active.")]
    public void TelemetryClientConstructorWithNoTelemetryConfiguration(string appDomainName = "Incorrect") => this.WriteEvent(70, this.nameProvider.Name);

    [Event(71, Level = EventLevel.Verbose, Message = "Value for property '{0}' of {1} was not found. Populating it by default.")]
    public void PopulateRequiredStringWithValue(
      string parameterName,
      string telemetryType,
      string appDomainName = "Incorrect")
    {
      this.WriteEvent(71, parameterName ?? string.Empty, telemetryType ?? string.Empty, this.nameProvider.Name);
    }

    [Event(72, Level = EventLevel.Warning, Message = "Invalid duration for Request Telemetry. Setting it to '00:00:00'.")]
    public void RequestTelemetryIncorrectDuration(string appDomainName = "Incorrect") => this.WriteEvent(72, this.nameProvider.Name);

    [Event(80, Level = EventLevel.Verbose, Message = "Telemetry tracking was disabled. Message is dropped.")]
    public void TrackingWasDisabled(string appDomainName = "Incorrect") => this.WriteEvent(80, this.nameProvider.Name);

    [Event(81, Level = EventLevel.Verbose, Message = "Telemetry tracking was enabled. Messages are being logged.")]
    public void TrackingWasEnabled(string appDomainName = "Incorrect") => this.WriteEvent(81, this.nameProvider.Name);

    [Event(90, Keywords = (EventKeywords) 8, Level = EventLevel.Error, Message = "[msg=Log Error];[msg={0}]")]
    public void LogError(string msg, string appDomainName = "Incorrect") => this.WriteEvent(90, msg ?? string.Empty, this.nameProvider.Name);

    [Event(91, Keywords = (EventKeywords) 9, Level = EventLevel.Error, Message = "BuildInfo.config file has incorrect xml structure. Context component version will not be populated. Exception: {0}.")]
    public void BuildInfoConfigBrokenXmlError(string msg, string appDomainName = "Incorrect") => this.WriteEvent(91, msg ?? string.Empty, this.nameProvider.Name);

    public sealed class Keywords
    {
      public const EventKeywords UserActionable = (EventKeywords) 1;
      public const EventKeywords Diagnostics = (EventKeywords) 2;
      public const EventKeywords VerboseFailure = (EventKeywords) 4;
      public const EventKeywords ErrorFailure = (EventKeywords) 8;
    }
  }
}
