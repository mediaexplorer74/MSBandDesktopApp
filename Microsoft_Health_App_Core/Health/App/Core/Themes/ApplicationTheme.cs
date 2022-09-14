// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.ApplicationTheme
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Themes
{
  public sealed class ApplicationTheme
  {
    public ApplicationTheme(
      string name,
      ArgbColor32 color,
      ArgbColor32 colorAccent,
      ArgbColor32 colorBackground,
      ArgbColor32 colorBackgroundSecondary,
      ArgbColor32 colorHeaderBackground,
      ArgbColor32 colorHigh,
      ArgbColor32 colorHighSecondary,
      ArgbColor32 colorLow,
      ArgbColor32 colorLowSecondary,
      ArgbColor32 colorMedium,
      ArgbColor32 colorMediumSecondary,
      ArgbColor32 colorSecondary)
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
            this.Color = color;
            this.ColorAccent = colorAccent;
            this.ColorBackground = colorBackground;
            this.ColorBackgroundSecondary = colorBackgroundSecondary;
            this.ColorHeaderBackground = colorHeaderBackground;
            this.ColorHigh = colorHigh;
            this.ColorHighSecondary = colorHighSecondary;
            this.ColorLow = colorLow;
            this.ColorLowSecondary = colorLowSecondary;
            this.ColorMedium = colorMedium;
            this.ColorMediumSecondary = colorMediumSecondary;
            this.ColorSecondary = colorSecondary;
            break;
          }
          goto case "";
      }
    }

    public string Name { get; private set; }

    public ArgbColor32 Color { get; private set; }

    public ArgbColor32 ColorAccent { get; private set; }

    public ArgbColor32 ColorBackground { get; private set; }

    public ArgbColor32 ColorBackgroundSecondary { get; private set; }

    public ArgbColor32 ColorHeaderBackground { get; private set; }

    public ArgbColor32 ColorHigh { get; private set; }

    public ArgbColor32 ColorHighSecondary { get; private set; }

    public ArgbColor32 ColorLow { get; private set; }

    public ArgbColor32 ColorLowSecondary { get; private set; }

    public ArgbColor32 ColorMedium { get; private set; }

    public ArgbColor32 ColorMediumSecondary { get; private set; }

    public ArgbColor32 ColorSecondary { get; private set; }
  }
}
