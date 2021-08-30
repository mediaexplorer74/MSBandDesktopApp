// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventCommand
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  public enum EventCommand
  {
    Disable = -3, // 0xFFFFFFFD
    Enable = -2, // 0xFFFFFFFE
    SendManifest = -1, // 0xFFFFFFFF
    Update = 0,
  }
}
