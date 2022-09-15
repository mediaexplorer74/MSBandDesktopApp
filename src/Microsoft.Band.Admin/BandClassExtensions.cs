// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.BandClassExtensions
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class BandClassExtensions
  {
    internal static BandType ToBandType(this BandClass bandClass)
    {
      switch (bandClass)
      {
        case BandClass.Unknown:
          return BandType.Unknown;
        case BandClass.Cargo:
          return BandType.Cargo;
        case BandClass.Envoy:
          return BandType.Envoy;
        default:
          throw new ArgumentException("Unknown BandClass value.");
      }
    }
  }
}
