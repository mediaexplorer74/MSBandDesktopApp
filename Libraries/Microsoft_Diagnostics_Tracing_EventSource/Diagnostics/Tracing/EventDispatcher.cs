// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventDispatcher
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  internal class EventDispatcher
  {
    internal readonly EventListener m_Listener;
    internal bool[] m_EventEnabled;
    internal EventDispatcher m_Next;

    internal EventDispatcher(EventDispatcher next, bool[] eventEnabled, EventListener listener)
    {
      this.m_Next = next;
      this.m_EventEnabled = eventEnabled;
      this.m_Listener = listener;
    }
  }
}
