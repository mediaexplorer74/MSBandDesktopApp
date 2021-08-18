// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ThemeColorPaletteProxy
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band;
using System.Windows.Media;

namespace DesktopSyncApp
{
  public class ThemeColorPaletteProxy
  {
    public ThemeColorPaletteProxy(BandTheme palette) => this.Palette = palette;

    public ThemeColorPaletteProxy(
      BandColor _base,
      BandColor highlight,
      BandColor lowlight,
      BandColor secondaryText,
      BandColor highContrast,
      BandColor muted)
    {
      this.Palette = new BandTheme()
      {
        Base = _base,
        Highlight = highlight,
        Lowlight = lowlight,
        SecondaryText = secondaryText,
        HighContrast = highContrast,
        Muted = muted
      };
    }

    public BandTheme Palette { get; private set; }

    public Color Base => ColorExtensions.ToColor(this.Palette.Base);

    public Color Highlight => ColorExtensions.ToColor(this.Palette.Highlight);

    public Color Lowlight => ColorExtensions.ToColor(this.Palette.Lowlight);

    public Color SecondaryText => ColorExtensions.ToColor(this.Palette.SecondaryText);

    public Color HighContrast => ColorExtensions.ToColor(this.Palette.HighContrast);

    public Color Muted => ColorExtensions.ToColor(this.Palette.Muted);
  }
}
