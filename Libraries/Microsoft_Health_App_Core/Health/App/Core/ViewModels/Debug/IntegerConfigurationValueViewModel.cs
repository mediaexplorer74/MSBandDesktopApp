// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.IntegerConfigurationValueViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class IntegerConfigurationValueViewModel : ConfigurationValueViewModel
  {
    public IntegerConfigurationValueViewModel(
      IConfigurationService configurationService,
      IConfigurationValue value)
      : base(configurationService, value)
    {
      IntegerConfigurationValue configurationValue = (IntegerConfigurationValue) value;
      this.CurrentValue = this.ConfigurationService.GetValue<int>((ConfigurationValue<int>) configurationValue);
      this.Minimum = configurationValue.Range.Low;
      this.Maximum = configurationValue.Range.High;
      this.SaveCommand = (ICommand) new HealthCommand((Action) (() => this.Value.SetValue(this.ConfigurationService, (object) this.CurrentValue)));
    }

    public int CurrentValue { get; set; }

    public int Minimum { get; private set; }

    public int Maximum { get; private set; }

    public ICommand SaveCommand { get; private set; }
  }
}
