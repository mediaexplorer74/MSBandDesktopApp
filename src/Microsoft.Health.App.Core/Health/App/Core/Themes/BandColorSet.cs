// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.BandColorSet
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using System;

namespace Microsoft.Health.App.Core.Themes
{
  public sealed class BandColorSet
  {
    public BandColorSet(
      ushort id,
      string name,
      ArgbColor32 colorBase,
      ArgbColor32 colorHighlight,
      ArgbColor32 colorLowlight,
      ArgbColor32 colorSecondaryText,
      ArgbColor32 colorHighContrast,
      ArgbColor32 colorMuted)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException("a valid non-whitespace name must be provided");
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          if (name.Trim().Length != 0)
          {
            this.Name = name;
            this.Id = id;
            this.ColorBase = colorBase;
            this.ColorHighlight = colorHighlight;
            this.ColorLowlight = colorLowlight;
            this.ColorSecondaryText = colorSecondaryText;
            this.ColorHighContrast = colorHighContrast;
            this.ColorMuted = colorMuted;
            break;
          }
          goto case "";
      }
    }

    public ushort Id { get; private set; }

    public string Name { get; private set; }

    public ArgbColor32 ColorMuted { get; private set; }

    public ArgbColor32 ColorHighContrast { get; private set; }

    public ArgbColor32 ColorSecondaryText { get; private set; }

    public ArgbColor32 ColorLowlight { get; private set; }

    public ArgbColor32 ColorHighlight { get; private set; }

    public ArgbColor32 ColorBase { get; private set; }

    public BandTheme AsTileColorPalette() => new BandTheme()
    {
      Base = this.ColorBase.Color.RgbToBandColor(),
      Highlight = this.ColorHighlight.Color.RgbToBandColor(),
      Lowlight = this.ColorLowlight.Color.RgbToBandColor(),
      SecondaryText = this.ColorSecondaryText.Color.RgbToBandColor(),
      HighContrast = this.ColorHighContrast.Color.RgbToBandColor(),
      Muted = this.ColorMuted.Color.RgbToBandColor()
    };
  }
}
