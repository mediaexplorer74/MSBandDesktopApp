// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.NavCommandViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public class NavCommandViewModel : HealthViewModelBase, INavChoiceViewModel, INavListItem
  {
    private HealthCommand actionCommandWrapper;
    private ICommand actionCommand;
    private bool isEnabled;
    private string name;
    private string icon;

    public NavCommandViewModel() => this.IsEnabled = true;

    public ICommand ActionCommand
    {
      get => (ICommand) this.actionCommandWrapper ?? (ICommand) (this.actionCommandWrapper = new HealthCommand((Action) (() =>
      {
        if (this.actionCommand == null)
          return;
        this.actionCommand.Execute((object) null);
      }), (Func<bool>) (() => this.IsEnabled)));
      set => this.SetProperty<ICommand>(ref this.actionCommand, value, nameof (ActionCommand));
    }

    public string Name
    {
      get => this.name;
      set => this.SetProperty<string>(ref this.name, value, nameof (Name));
    }

    public string GlyphIcon
    {
      get => this.icon;
      set => this.SetProperty<string>(ref this.icon, value, nameof (GlyphIcon));
    }

    public bool IsEnabled
    {
      get => this.isEnabled;
      set
      {
        this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled));
        if (this.actionCommandWrapper == null)
          return;
        this.actionCommandWrapper.RaiseCanExecuteChanged();
      }
    }
  }
}
