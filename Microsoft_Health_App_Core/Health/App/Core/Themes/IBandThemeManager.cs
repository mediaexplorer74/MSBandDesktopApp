// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.IBandThemeManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Themes
{
  public interface IBandThemeManager : INotifyPropertyChanged
  {
    IEnumerable<BandBackgroundStyle> BackgroundStyles { get; }

    Task SetDeviceTypeAsync();

    void SetDeviceType(BandClass bandClass);

    IEnumerable<BandColorSet> ColorSets { get; }

    IEnumerable<AppBandTheme> Themes { get; }

    AppBandTheme CurrentTheme { get; }

    bool IsThemeNotSaved { get; }

    AppBandTheme DefaultTheme { get; }

    void SwitchActiveColorSet(BandColorSet bandColorSet);

    void SwitchActiveBackgroundStyle(BandBackgroundStyle bandBackgroundStyle);

    void SetCurrentTheme(AppBandTheme bandTheme);

    Task<AppBandTheme> GetCurrentThemeFromBandAsync(
      CancellationToken cancellationToken);

    AppBandTheme RevertTheme();
  }
}
