// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventSourceException
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System;

namespace Microsoft.Diagnostics.Tracing
{
  public class EventSourceException : Exception
  {
    public EventSourceException()
      : base(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ListenerWriteFailure"))
    {
    }

    public EventSourceException(string message)
      : base(message)
    {
    }

    public EventSourceException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal EventSourceException(Exception innerException)
      : base(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ListenerWriteFailure"), innerException)
    {
    }
  }
}
