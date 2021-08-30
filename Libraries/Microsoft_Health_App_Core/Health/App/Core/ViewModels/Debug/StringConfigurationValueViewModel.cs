// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.StringConfigurationValueViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class StringConfigurationValueViewModel : ConfigurationValueViewModel
  {
    private readonly HealthCommand clearCommand;
    private readonly HealthCommand saveCommand;
    private string currentValue;

    public StringConfigurationValueViewModel(
      IConfigurationService configurationService,
      IConfigurationValue value)
      : base(configurationService, value)
    {
      StringConfigurationValueViewModel configurationValueViewModel = this;
      this.saveCommand = new HealthCommand((Action) (() => value.SetValue(configurationService, (object) configurationValueViewModel.CurrentValue)));
      this.clearCommand = new HealthCommand((Action) (() =>
      {
        value.ClearValue(configurationService);
        configurationValueViewModel.CurrentValue = string.Empty;
      }));
      object actualValue;
      if (value.TryGetValue(configurationService, out actualValue))
        this.CurrentValue = (string) actualValue;
      else
        this.CurrentValue = string.Empty;
    }

    public string CurrentValue
    {
      get => this.currentValue;
      set => this.SetProperty<string>(ref this.currentValue, value, nameof (CurrentValue));
    }

    public ICommand SaveCommand => (ICommand) this.saveCommand;

    public ICommand ClearCommand => (ICommand) this.clearCommand;
  }
}
