// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventOpcode
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  public enum EventOpcode
  {
    Info = 0,
    Start = 1,
    Stop = 2,
    DataCollectionStart = 3,
    DataCollectionStop = 4,
    Extension = 5,
    Reply = 6,
    Resume = 7,
    Suspend = 8,
    Send = 9,
    Receive = 240, // 0x000000F0
  }
}
