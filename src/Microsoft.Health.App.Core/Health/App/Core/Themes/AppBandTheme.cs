// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.AppBandTheme
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System;

namespace Microsoft.Health.App.Core.Themes
{
  public sealed class AppBandTheme
  {
    public AppBandTheme(
      BandBackgroundStyle bandBackgroundStyle,
      BandColorSet bandColorSet,
      BandClass bandClass)
    {
      if (bandBackgroundStyle == null)
        throw new ArgumentNullException(nameof (bandBackgroundStyle));
      if (bandColorSet == null)
        throw new ArgumentNullException(nameof (bandColorSet));
      this.BackgroundStyle = bandBackgroundStyle;
      this.ColorSet = bandColorSet;
      this.BandClass = bandClass;
      switch (bandClass)
      {
        case BandClass.Cargo:
          this.MeTileHeight = 102U;
          this.MeTileWidth = 310U;
          break;
        case BandClass.Envoy:
          this.MeTileHeight = 128U;
          this.MeTileWidth = 310U;
          break;
      }
      this.Id = AppBandTheme.GetThemeId(bandColorSet, bandBackgroundStyle);
    }

    public uint Id { get; private set; }

    public string Name { get; private set; }

    public BandClass BandClass { get; private set; }

    public BandBackgroundStyle BackgroundStyle { get; private set; }

    public BandColorSet ColorSet { get; private set; }

    public uint MeTileHeight { get; private set; }

    public uint MeTileWidth { get; private set; }

    public static uint GetThemeId(
      BandColorSet bandColorSet,
      BandBackgroundStyle bandBackgroundStyle)
    {
      return ((uint) bandColorSet.Id << 16) + (uint) bandBackgroundStyle.Id;
    }
  }
}
