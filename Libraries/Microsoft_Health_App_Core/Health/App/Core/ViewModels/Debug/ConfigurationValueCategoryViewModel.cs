// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.ConfigurationValueCategoryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class ConfigurationValueCategoryViewModel : HealthViewModelBase
  {
    public ConfigurationValueCategoryViewModel(
      string name,
      IConfigurationService configurationService,
      IEnumerable<IConfigurationValue> values)
    {
      this.Name = name;
      if (!(values is IList<IConfigurationValue> source))
        source = (IList<IConfigurationValue>) values.ToList<IConfigurationValue>();
      this.ConfigurationValues = (IEnumerable<ConfigurationValueViewModel>) source.Select<IConfigurationValue, ConfigurationValueViewModel>((Func<IConfigurationValue, ConfigurationValueViewModel>) (value => ConfigurationValueCategoryViewModel.CreateViewModel(configurationService, value))).ToList<ConfigurationValueViewModel>();
    }

    public string Name { get; private set; }

    public IEnumerable<ConfigurationValueViewModel> ConfigurationValues { get; private set; }

    private static ConfigurationValueViewModel CreateViewModel(
      IConfigurationService configurationService,
      IConfigurationValue value)
    {
      switch (value.ConfigurationLayout)
      {
        case ConfigurationLayout.Boolean:
          return (ConfigurationValueViewModel) new BooleanConfigurationValueViewModel(configurationService, value);
        case ConfigurationLayout.MultipleSelection:
          return (ConfigurationValueViewModel) new MultipleSelectionConfigurationValueViewModel(configurationService, value);
        case ConfigurationLayout.Integer:
          return (ConfigurationValueViewModel) new IntegerConfigurationValueViewModel(configurationService, value);
        case ConfigurationLayout.String:
          return (ConfigurationValueViewModel) new StringConfigurationValueViewModel(configurationService, value);
        default:
          return (ConfigurationValueViewModel) null;
      }
    }
  }
}
