// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.ApplicationThemeManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Microsoft.Health.App.Core.Themes
{
  public sealed class ApplicationThemeManager : IApplicationThemeManager, INotifyPropertyChanged
  {
    public static readonly ApplicationTheme Purple = new ApplicationTheme(nameof (Purple), ThemeColors.Purple, ThemeColors.PurpleAccent, ThemeColors.PurpleBackground, ThemeColors.PurpleBackgroundSecondary, ThemeColors.PurpleHeaderBackground, ThemeColors.PurpleHigh, ThemeColors.PurpleHighSecondary, ThemeColors.PurpleLow, ThemeColors.PurpleLowSecondary, ThemeColors.PurpleMedium, ThemeColors.PurpleMediumSecondary, ThemeColors.PurpleSecondary);
    private ApplicationTheme currentTheme;
    private ApplicationTheme defaultTheme;

    public ApplicationThemeManager()
    {
      this.DefaultTheme = ApplicationThemeManager.Purple;
      this.CurrentTheme = this.DefaultTheme;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public ApplicationTheme CurrentTheme
    {
      get => this.currentTheme;
      set
      {
        this.currentTheme = value;
        if (this.currentTheme == value)
          return;
        this.OnPropertyChanged(nameof (CurrentTheme));
      }
    }

    public ApplicationTheme DefaultTheme
    {
      get => this.defaultTheme;
      private set
      {
        this.defaultTheme = value;
        if (this.defaultTheme == value)
          return;
        this.OnPropertyChanged(nameof (DefaultTheme));
      }
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
