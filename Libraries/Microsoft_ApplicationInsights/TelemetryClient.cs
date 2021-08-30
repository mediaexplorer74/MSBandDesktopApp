// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.TelemetryClient
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Microsoft.ApplicationInsights
{
  public sealed class TelemetryClient
  {
    private readonly TelemetryConfiguration configuration;
    private readonly IDebugOutput debugOutput;
    private TelemetryContext context;
    private ITelemetryChannel channel;

    public TelemetryClient()
      : this(TelemetryConfiguration.Active)
    {
    }

    public TelemetryClient(TelemetryConfiguration configuration)
    {
      if (configuration == null)
      {
        CoreEventSource.Log.TelemetryClientConstructorWithNoTelemetryConfiguration();
        configuration = TelemetryConfiguration.Active;
      }
      this.configuration = configuration;
      this.debugOutput = PlatformSingleton.Current.GetDebugOutput();
    }

    public TelemetryContext Context
    {
      get => LazyInitializer.EnsureInitialized<TelemetryContext>(ref this.context, new Func<TelemetryContext>(this.CreateInitializedContext));
      internal set => this.context = value;
    }

    public string InstrumentationKey
    {
      get => this.Context.InstrumentationKey;
      set => this.Context.InstrumentationKey = value;
    }

    internal ITelemetryChannel Channel
    {
      get
      {
        ITelemetryChannel telemetryChannel = this.channel;
        if (telemetryChannel == null)
        {
          telemetryChannel = this.configuration.TelemetryChannel;
          this.channel = telemetryChannel;
        }
        return telemetryChannel;
      }
      set => this.channel = value;
    }

    public bool IsEnabled() => !this.configuration.DisableTelemetry;

    public void TrackEvent(
      string eventName,
      IDictionary<string, string> properties = null,
      IDictionary<string, double> metrics = null)
    {
      EventTelemetry telemetry = new EventTelemetry(eventName);
      if (properties != null && properties.Count > 0)
        Utils.CopyDictionary<string>(properties, telemetry.Context.Properties);
      if (metrics != null && metrics.Count > 0)
        Utils.CopyDictionary<double>(metrics, telemetry.Metrics);
      this.TrackEvent(telemetry);
    }

    public void TrackEvent(EventTelemetry telemetry)
    {
      if (telemetry == null)
        telemetry = new EventTelemetry();
      this.Track((ITelemetry) telemetry);
    }

    public void TrackTrace(string message) => this.TrackTrace(new TraceTelemetry(message));

    public void TrackTrace(string message, SeverityLevel severityLevel) => this.TrackTrace(new TraceTelemetry(message, severityLevel));

    public void TrackTrace(string message, IDictionary<string, string> properties)
    {
      TraceTelemetry telemetry = new TraceTelemetry(message);
      if (properties != null && properties.Count > 0)
        Utils.CopyDictionary<string>(properties, telemetry.Context.Properties);
      this.TrackTrace(telemetry);
    }

    public void TrackTrace(
      string message,
      SeverityLevel severityLevel,
      IDictionary<string, string> properties)
    {
      TraceTelemetry telemetry = new TraceTelemetry(message, severityLevel);
      if (properties != null && properties.Count > 0)
        Utils.CopyDictionary<string>(properties, telemetry.Context.Properties);
      this.TrackTrace(telemetry);
    }

    public void TrackTrace(TraceTelemetry telemetry)
    {
      telemetry = telemetry ?? new TraceTelemetry();
      this.Track((ITelemetry) telemetry);
    }

    public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
    {
      MetricTelemetry telemetry = new MetricTelemetry(name, value);
      if (properties != null && properties.Count > 0)
        Utils.CopyDictionary<string>(properties, telemetry.Properties);
      this.TrackMetric(telemetry);
    }

    public void TrackMetric(MetricTelemetry telemetry)
    {
      if (telemetry == null)
        telemetry = new MetricTelemetry();
      this.Track((ITelemetry) telemetry);
    }

    public void TrackException(
      Exception exception,
      IDictionary<string, string> properties = null,
      IDictionary<string, double> metrics = null)
    {
      if (exception == null)
        exception = new Exception(Utils.PopulateRequiredStringValue((string) null, "message", typeof (ExceptionTelemetry).FullName));
      ExceptionTelemetry telemetry = new ExceptionTelemetry(exception)
      {
        HandledAt = ExceptionHandledAt.UserCode
      };
      if (properties != null && properties.Count > 0)
        Utils.CopyDictionary<string>(properties, telemetry.Context.Properties);
      if (metrics != null && metrics.Count > 0)
        Utils.CopyDictionary<double>(metrics, telemetry.Metrics);
      this.TrackException(telemetry);
    }

    public void TrackException(ExceptionTelemetry telemetry)
    {
      if (telemetry == null)
        telemetry = new ExceptionTelemetry(new Exception(Utils.PopulateRequiredStringValue((string) null, "message", typeof (ExceptionTelemetry).FullName)))
        {
          HandledAt = ExceptionHandledAt.UserCode
        };
      this.Track((ITelemetry) telemetry);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Track(ITelemetry telemetry)
    {
      if (!this.IsEnabled())
        return;
      string instrumentationKey = this.Context.InstrumentationKey;
      if (string.IsNullOrEmpty(instrumentationKey))
        instrumentationKey = this.configuration.InstrumentationKey;
      if (string.IsNullOrEmpty(instrumentationKey))
        return;
      if (telemetry is ISupportProperties supportProperties)
      {
        if (this.Channel.DeveloperMode)
          supportProperties.Properties.Add("DeveloperMode", "true");
        Utils.CopyDictionary<string>(this.Context.Properties, supportProperties.Properties);
      }
      telemetry.Context.Initialize(this.Context, instrumentationKey);
      foreach (ITelemetryInitializer telemetryInitializer in (IEnumerable<ITelemetryInitializer>) this.configuration.TelemetryInitializers)
      {
        try
        {
          telemetryInitializer.Initialize(telemetry);
        }
        catch (Exception ex)
        {
          CoreEventSource.Log.LogError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Exception while initializing {0}, exception message - {1}", (object) telemetryInitializer.GetType().FullName, (object) ex.ToString()));
        }
      }
      telemetry.Sanitize();
      this.Channel.Send(telemetry);
      if (!this.Channel.DeveloperMode)
        return;
      this.WriteTelemetryToDebugOutput(telemetry);
    }

    public void TrackPageView(string name) => this.Track((ITelemetry) new PageViewTelemetry(name));

    public void TrackPageView(PageViewTelemetry telemetry)
    {
      if (telemetry == null)
        telemetry = new PageViewTelemetry();
      this.Track((ITelemetry) telemetry);
    }

    public void TrackRequest(
      string name,
      DateTimeOffset timestamp,
      TimeSpan duration,
      string responseCode,
      bool success)
    {
      this.Track((ITelemetry) new RequestTelemetry(name, timestamp, duration, responseCode, success));
    }

    public void TrackRequest(RequestTelemetry request) => this.Track((ITelemetry) request);

    public void Flush() => this.Channel.Flush();

    private TelemetryContext CreateInitializedContext()
    {
      TelemetryContext context = new TelemetryContext();
      foreach (IContextInitializer contextInitializer in (IEnumerable<IContextInitializer>) this.configuration.ContextInitializers)
        contextInitializer.Initialize(context);
      return context;
    }

    private void WriteTelemetryToDebugOutput(ITelemetry telemetry)
    {
      using (new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
        this.debugOutput.WriteLine("Application Insights Telemetry: " + JsonSerializer.SerializeAsString(telemetry));
    }
  }
}
