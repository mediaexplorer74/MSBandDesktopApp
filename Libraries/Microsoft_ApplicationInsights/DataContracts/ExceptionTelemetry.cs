// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.ApplicationInsights.DataContracts
{
  public sealed class ExceptionTelemetry : ITelemetry, ISupportProperties
  {
    internal const string TelemetryName = "Exception";
    internal readonly string BaseType = typeof (ExceptionData).Name;
    internal readonly ExceptionData Data;
    private readonly TelemetryContext context;
    private Exception exception;

    public ExceptionTelemetry()
    {
      this.Data = new ExceptionData();
      this.context = new TelemetryContext(this.Data.properties, (IDictionary<string, string>) new Dictionary<string, string>());
      this.HandledAt = ExceptionHandledAt.Unhandled;
    }

    public ExceptionTelemetry(Exception exception)
      : this()
    {
      if (exception == null)
        exception = new Exception(Utils.PopulateRequiredStringValue((string) null, "message", typeof (ExceptionTelemetry).FullName));
      this.Exception = exception;
    }

    public DateTimeOffset Timestamp { get; set; }

    public string Sequence { get; set; }

    public TelemetryContext Context => this.context;

    public ExceptionHandledAt HandledAt
    {
      get => this.ValidateExceptionHandledAt(this.Data.handledAt);
      set => this.Data.handledAt = value.ToString();
    }

    public Exception Exception
    {
      get => this.exception;
      set
      {
        this.exception = value;
        this.UpdateExceptions(value);
      }
    }

    public IDictionary<string, double> Metrics => this.Data.measurements;

    public IDictionary<string, string> Properties => this.Data.properties;

    public Microsoft.ApplicationInsights.DataContracts.SeverityLevel? SeverityLevel
    {
      get => this.Data.severityLevel.TranslateSeverityLevel();
      set => this.Data.severityLevel = value.TranslateSeverityLevel();
    }

    internal IList<ExceptionDetails> Exceptions => this.Data.exceptions;

    void ITelemetry.Sanitize()
    {
      this.Properties.SanitizeProperties();
      this.Metrics.SanitizeMeasurements();
    }

    private static void ConvertExceptionTree(
      Exception exception,
      ExceptionDetails parentExceptionDetails,
      List<ExceptionDetails> exceptions)
    {
      if (exception == null)
        exception = new Exception(Utils.PopulateRequiredStringValue((string) null, "message", typeof (ExceptionTelemetry).FullName));
      ExceptionDetails exceptionDetails = PlatformSingleton.Current.GetExceptionDetails(exception, parentExceptionDetails);
      exceptions.Add(exceptionDetails);
      if (exception is AggregateException aggregateException)
      {
        foreach (Exception innerException in aggregateException.InnerExceptions)
          ExceptionTelemetry.ConvertExceptionTree(innerException, exceptionDetails, exceptions);
      }
      else
      {
        if (exception.InnerException == null)
          return;
        ExceptionTelemetry.ConvertExceptionTree(exception.InnerException, exceptionDetails, exceptions);
      }
    }

    private void UpdateExceptions(Exception exception)
    {
      List<ExceptionDetails> exceptions = new List<ExceptionDetails>();
      ExceptionTelemetry.ConvertExceptionTree(exception, (ExceptionDetails) null, exceptions);
      if (exceptions.Count > 10)
      {
        InnerExceptionCountExceededException exceededException = new InnerExceptionCountExceededException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The number of inner exceptions was {0} which is larger than {1}, the maximum number allowed during transmission. All but the first {1} have been dropped.", (object) exceptions.Count, (object) 10));
        exceptions.RemoveRange(10, exceptions.Count - 10);
        exceptions.Add(PlatformSingleton.Current.GetExceptionDetails((Exception) exceededException, exceptions[0]));
      }
      this.Data.exceptions = (IList<ExceptionDetails>) exceptions;
    }

    private ExceptionHandledAt ValidateExceptionHandledAt(string value)
    {
      ExceptionHandledAt exceptionHandledAt = ExceptionHandledAt.Unhandled;
      if (Enum.IsDefined(typeof (ExceptionHandledAt), (object) value))
        exceptionHandledAt = (ExceptionHandledAt) Enum.Parse(typeof (ExceptionHandledAt), value);
      return exceptionHandledAt;
    }
  }
}
