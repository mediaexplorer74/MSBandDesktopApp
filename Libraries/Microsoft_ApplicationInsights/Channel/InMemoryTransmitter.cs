// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Channel.InMemoryTransmitter
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Channel
{
  internal class InMemoryTransmitter : IDisposable
  {
    private readonly TelemetryBuffer buffer;
    private object sendingLockObj = new object();
    private object disposeLockObj = new object();
    private AutoResetEvent startRunnerEvent;
    private bool enabled = true;
    private bool disposed;
    private TimeSpan sendingInterval = TimeSpan.FromSeconds(30.0);
    private Uri endpointAddress = new Uri("https://dc.services.visualstudio.com/v2/track");

    internal InMemoryTransmitter(TelemetryBuffer buffer)
    {
      this.buffer = buffer;
      this.startRunnerEvent = new AutoResetEvent(false);
      Task.Factory.StartNew(new Action(this.Runner), TaskCreationOptions.LongRunning).ContinueWith((Action<Task>) (task => CoreEventSource.Log.LogVerbose("InMemoryTransmitter: Unhandled exception in runner:" + task.Exception.ToString())), TaskContinuationOptions.OnlyOnFaulted);
      this.buffer.OnFull = new Action(this.OnBufferFull);
    }

    internal Uri EndpointAddress
    {
      get => this.endpointAddress;
      set => Property.Set<Uri>(ref this.endpointAddress, value);
    }

    internal TimeSpan SendingInterval
    {
      get => this.sendingInterval;
      set => this.sendingInterval = value;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal void Flush() => this.DequeueAndSend();

    private void Runner()
    {
      while (this.enabled)
      {
        this.DequeueAndSend();
        CoreEventSource.Log.LogVerbose("InMemoryTransmitter.DequeueAndSend Completed. Will wait");
        this.startRunnerEvent.WaitOne(this.sendingInterval);
      }
    }

    private void OnBufferFull()
    {
      CoreEventSource.Log.LogVerbose("TelemetryBuffer is full.");
      this.startRunnerEvent.Set();
    }

    private void DequeueAndSend()
    {
      lock (this.sendingLockObj)
      {
        IEnumerable<ITelemetry> telemetryItems = this.buffer.Dequeue();
        try
        {
          this.Send(telemetryItems).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
          CoreEventSource.Log.LogError(ex.ToString());
        }
      }
    }

    private async Task Send(IEnumerable<ITelemetry> telemetryItems)
    {
      if (telemetryItems == null || !telemetryItems.Any<ITelemetry>())
      {
        CoreEventSource.Log.LogVerbose("No Telemetry Items passed to Enqueue");
      }
      else
      {
        CoreEventSource.Log.LogVerbose("Telemetry Items passed to Enqueue");
        byte[] data = JsonSerializer.Serialize(telemetryItems);
        Transmission transmission = new Transmission(this.endpointAddress, data, "application/x-json-stream", JsonSerializer.CompressionType);
        await transmission.SendAsync().ConfigureAwait(false);
        CoreEventSource.Log.LogVerbose("InMemoryTransmitter.Send completed");
      }
    }

    private void Dispose(bool disposing)
    {
      lock (this.disposeLockObj)
      {
        if (!disposing || this.disposed)
          return;
        this.enabled = false;
        this.startRunnerEvent.Set();
        this.startRunnerEvent.Dispose();
        this.disposed = true;
      }
    }
  }
}
