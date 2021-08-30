// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.BooleanConfigurationValueViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class BooleanConfigurationValueViewModel : ConfigurationValueViewModel
  {
    private static readonly ConfigurationValueOption TrueOption = new ConfigurationValueOption("True", (object) true);
    private static readonly ConfigurationValueOption FalseOption = new ConfigurationValueOption("False", (object) false);
    private static readonly ConfigurationValueOption[] BooleanOptions = new ConfigurationValueOption[3]
    {
      ConfigurationValueViewModel.DefaultOption,
      BooleanConfigurationValueViewModel.TrueOption,
      BooleanConfigurationValueViewModel.FalseOption
    };
    private readonly ConfigurationValueOption[] options;
    private ConfigurationValueOption selectedOption;

    public BooleanConfigurationValueViewModel(
      IConfigurationService configurationService,
      IConfigurationValue value)
      : base(configurationService, value)
    {
      this.options = BooleanConfigurationValueViewModel.BooleanOptions;
      object actualValue;
      if (value.TryGetValue(configurationService, out actualValue))
        this.selectedOption = ((IEnumerable<ConfigurationValueOption>) this.options).First<ConfigurationValueOption>((Func<ConfigurationValueOption, bool>) (option => object.Equals(option.OptionValue, actualValue)));
      else
        this.selectedOption = ConfigurationValueViewModel.DefaultOption;
    }

    public IEnumerable<ConfigurationValueOption> Options => (IEnumerable<ConfigurationValueOption>) this.options;

    public ConfigurationValueOption SelectedOption
    {
      get => this.selectedOption;
      set
      {
        if (value != ConfigurationValueViewModel.DefaultOption)
          this.Value.SetValue(this.ConfigurationService, value.OptionValue);
        else
          this.Value.ClearValue(this.ConfigurationService);
        this.SetProperty<ConfigurationValueOption>(ref this.selectedOption, value, nameof (SelectedOption));
      }
    }
  }
}
