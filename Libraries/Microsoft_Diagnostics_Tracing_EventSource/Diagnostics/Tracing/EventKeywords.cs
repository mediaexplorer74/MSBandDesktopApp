// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventKeywords
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System;

namespace Microsoft.Diagnostics.Tracing
{
  [Flags]
  public enum EventKeywords : long
  {
    None = 0,
    All = -1, // 0xFFFFFFFFFFFFFFFF
    WdiContext = 562949953421312, // 0x0002000000000000
    WdiDiagnostic = 1125899906842624, // 0x0004000000000000
    Sqm = 2251799813685248, // 0x0008000000000000
    AuditFailure = 4503599627370496, // 0x0010000000000000
    AuditSuccess = 9007199254740992, // 0x0020000000000000
    CorrelationHint = 18014398509481984, // 0x0040000000000000
    EventLogClassic = 36028797018963968, // 0x0080000000000000
  }
}
