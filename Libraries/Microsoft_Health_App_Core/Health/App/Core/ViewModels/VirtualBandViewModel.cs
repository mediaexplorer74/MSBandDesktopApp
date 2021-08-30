// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.VirtualBandViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Themes;
using System;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class VirtualBandViewModel : HealthObservableObject
  {
    private IBandThemeManager bandThemeManager;
    private bool useDefault;
    private BandClass bandClass;

    public VirtualBandViewModel(IBandThemeManager bandThemeManager, bool useDefault)
    {
      this.bandThemeManager = bandThemeManager;
      this.bandClass = bandThemeManager.CurrentTheme.BandClass;
      this.bandThemeManager.PropertyChanged += new PropertyChangedEventHandler(this.OnThemeManagerPropertyChanged);
      this.useDefault = useDefault;
    }

    private void OnThemeManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "CurrentTheme") || this.useDefault)
        return;
      this.RaisePropertyChanged("Color");
      this.RaisePropertyChanged("Theme");
      if (this.bandClass == this.bandThemeManager.CurrentTheme.BandClass)
        return;
      this.bandClass = this.bandThemeManager.CurrentTheme.BandClass;
      this.RaisePropertyChanged("BandClass");
      this.RaisePropertyChanged("PresenterMargin");
      this.RaisePropertyChanged("PresenterHeight");
    }

    public DateTimeOffset CurrentTime => DateTimeOffset.Now.ToLocalTime();

    public ArgbColor32 Color => !this.useDefault ? this.bandThemeManager.CurrentTheme.ColorSet.ColorBase : this.bandThemeManager.DefaultTheme.ColorSet.ColorBase;

    public AppBandTheme Theme => !this.useDefault ? this.bandThemeManager.CurrentTheme : this.bandThemeManager.DefaultTheme;

    public BandClass BandClass => this.bandThemeManager.DefaultTheme.BandClass;

    public string PresenterMargin => this.bandThemeManager.DefaultTheme.BandClass != BandClass.Envoy ? "93 43 0 0" : "93 33 0 0";

    public int PresenterHeight => this.bandThemeManager.DefaultTheme.BandClass != BandClass.Envoy ? 48 : 55;
  }
}
