// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Channel.InMemoryChannel
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;

namespace Microsoft.ApplicationInsights.Channel
{
  public class InMemoryChannel : ITelemetryChannel, IDisposable
  {
    private readonly TelemetryBuffer buffer;
    private readonly InMemoryTransmitter transmitter;
    private bool developerMode;
    private int bufferSize;

    public InMemoryChannel()
    {
      this.buffer = new TelemetryBuffer();
      this.transmitter = new InMemoryTransmitter(this.buffer);
    }

    internal InMemoryChannel(TelemetryBuffer telemetryBuffer, InMemoryTransmitter transmitter)
    {
      this.buffer = telemetryBuffer;
      this.transmitter = transmitter;
    }

    public bool DeveloperMode
    {
      get => this.developerMode;
      set
      {
        if (value == this.developerMode)
          return;
        if (value)
        {
          this.bufferSize = this.buffer.Capacity;
          this.buffer.Capacity = 1;
        }
        else
          this.buffer.Capacity = this.bufferSize;
        this.developerMode = value;
      }
    }

    public string EndpointAddress
    {
      get => this.transmitter.EndpointAddress.ToString();
      set => this.transmitter.EndpointAddress = new Uri(value);
    }

    public double DataUploadIntervalInSeconds
    {
      get => this.transmitter.SendingInterval.TotalSeconds;
      set => this.transmitter.SendingInterval = TimeSpan.FromSeconds(value);
    }

    public int MaxTelemetryBufferCapacity
    {
      get => this.buffer.Capacity;
      set => this.buffer.Capacity = value;
    }

    public void Send(ITelemetry item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      try
      {
        this.buffer.Enqueue(item);
        CoreEventSource.Log.LogVerbose("TelemetryBuffer.Enqueue succeeded");
      }
      catch (Exception ex)
      {
        CoreEventSource.Log.LogVerbose("TelemetryBuffer.Enqueue failed: ", ex.ToString());
      }
    }

    public void Flush() => this.transmitter.Flush();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (!disposing || this.transmitter == null)
        return;
      this.transmitter.Dispose();
    }
  }
}
