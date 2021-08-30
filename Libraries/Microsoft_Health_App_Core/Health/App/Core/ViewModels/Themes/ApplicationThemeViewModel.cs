// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Themes.ApplicationThemeViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Themes;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.ViewModels.Themes
{
  public class ApplicationThemeViewModel : ApplicationThemeViewModelBase
  {
    private readonly IApplicationThemeManager applicationThemeManager;
    private ApplicationTheme currentTheme;
    private ApplicationTheme defaultTheme;

    public ApplicationThemeViewModel(IApplicationThemeManager applicationThemeManager)
    {
      this.applicationThemeManager = applicationThemeManager;
      this.Current = this.applicationThemeManager.CurrentTheme;
      this.Default = this.applicationThemeManager.DefaultTheme;
      this.applicationThemeManager.PropertyChanged += (PropertyChangedEventHandler) ((sender, args) =>
      {
        if (args.PropertyName == "CurrentTheme")
          this.Current = this.applicationThemeManager.CurrentTheme;
        if (!(args.PropertyName == "DefaultTheme"))
          return;
        this.Default = this.applicationThemeManager.DefaultTheme;
      });
    }

    public override sealed ApplicationTheme Default
    {
      get => this.defaultTheme;
      protected set => this.SetProperty<ApplicationTheme>(ref this.defaultTheme, value, nameof (Default));
    }

    public override sealed ApplicationTheme Current
    {
      get => this.currentTheme;
      protected set => this.SetProperty<ApplicationTheme>(ref this.currentTheme, value, nameof (Current));
    }
  }
}
