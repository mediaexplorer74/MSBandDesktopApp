// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.ManifestEnvelope
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  internal struct ManifestEnvelope
  {
    public const int MaxChunkSize = 65280;
    public ManifestEnvelope.ManifestFormats Format;
    public byte MajorVersion;
    public byte MinorVersion;
    public byte Magic;
    public ushort TotalChunks;
    public ushort ChunkNumber;

    public enum ManifestFormats : byte
    {
      SimpleXmlFormat = 1,
    }
  }
}
