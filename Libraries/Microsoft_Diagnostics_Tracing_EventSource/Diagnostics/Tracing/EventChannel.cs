// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventChannel
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  public enum EventChannel : byte
  {
    None = 0,
    Admin = 16, // 0x10
    Operational = 17, // 0x11
    Analytic = 18, // 0x12
    Debug = 19, // 0x13
  }
}
