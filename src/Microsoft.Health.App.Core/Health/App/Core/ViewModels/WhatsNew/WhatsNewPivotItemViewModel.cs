// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WhatsNew.WhatsNewPivotItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.WhatsNew
{
  public class WhatsNewPivotItemViewModel : HealthObservableObject
  {
    private readonly string name;
    private readonly ICommand navigateCommand;
    private ICommand navigateWithLogCommand;
    private bool isActive;
    private string navButtonText;

    public WhatsNewPivotItemViewModel(
      string name,
      string headerText,
      string subheaderText,
      EmbeddedAsset image,
      WhatsNewPivotColor colorTheme,
      string navigationButtonText = null,
      ICommand navigateCommand = null)
    {
      this.name = name;
      this.HeaderText = headerText;
      this.SubheaderText = subheaderText;
      this.Image = image;
      this.ColorTheme = colorTheme;
      this.NavigationButtonText = navigationButtonText;
      this.navigateCommand = navigateCommand;
    }

    public string HeaderText { get; private set; }

    public string SubheaderText { get; private set; }

    public EmbeddedAsset Image { get; private set; }

    public WhatsNewPivotColor ColorTheme { get; private set; }

    public string NavigationButtonText
    {
      get => this.navButtonText;
      private set => this.SetProperty<string>(ref this.navButtonText, value, nameof (NavigationButtonText));
    }

    public ICommand NavigateCommand => this.navigateWithLogCommand ?? (this.navigateWithLogCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      ApplicationTelemetry.LogWhatsNewLearnMore(this.name);
      if (this.navigateCommand == null)
        return;
      this.navigateCommand.Execute((object) null);
    })));

    public bool IsActive
    {
      get => this.isActive;
      set
      {
        this.SetProperty<bool>(ref this.isActive, value, nameof (IsActive));
        if (!value)
          return;
        ApplicationTelemetry.LogWhatsNewCardView(this.name);
      }
    }
  }
}
