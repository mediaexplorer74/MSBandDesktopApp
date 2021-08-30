// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Channel.TelemetryBuffer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Channel
{
  internal class TelemetryBuffer
  {
    private const int DefaultCapacity = 500;
    public Action OnFull;
    private readonly object lockObj = new object();
    private int capacity = 500;
    private List<ITelemetry> items;

    internal TelemetryBuffer() => this.items = new List<ITelemetry>();

    public int Capacity
    {
      get => this.capacity;
      set
      {
        if (value < 1)
          this.capacity = 500;
        else
          this.capacity = value;
      }
    }

    public void Enqueue(ITelemetry item)
    {
      if (item == null)
      {
        CoreEventSource.Log.LogVerbose("item is null in TelemetryBuffer.Enqueue");
      }
      else
      {
        lock (this.lockObj)
        {
          this.items.Add(item);
          if (this.items.Count < this.Capacity)
            return;
          Action onFull = this.OnFull;
          if (onFull == null)
            return;
          onFull();
        }
      }
    }

    public IEnumerable<ITelemetry> Dequeue()
    {
      List<ITelemetry> telemetryList = (List<ITelemetry>) null;
      if (this.items.Count > 0)
      {
        lock (this.lockObj)
        {
          if (this.items.Count > 0)
          {
            telemetryList = this.items;
            this.items = new List<ITelemetry>();
          }
        }
      }
      return (IEnumerable<ITelemetry>) telemetryList;
    }
  }
}
