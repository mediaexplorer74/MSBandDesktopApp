// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.ConfigurationValueViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services.Configuration;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public abstract class ConfigurationValueViewModel : HealthViewModelBase
  {
    public static readonly ConfigurationValueOption DefaultOption = new ConfigurationValueOption("<Default>", (object) null);

    protected ConfigurationValueViewModel(
      IConfigurationService configurationService,
      IConfigurationValue value)
    {
      if (configurationService == null)
        throw new ArgumentNullException(nameof (configurationService));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.ConfigurationService = configurationService;
      this.Value = value;
    }

    protected IConfigurationValue Value { get; private set; }

    protected IConfigurationService ConfigurationService { get; private set; }

    public string Category => this.Value.Category;

    public ConfigurationLayout ConfigurationLayout => this.Value.ConfigurationLayout;

    public string Name => this.Value.Name;
  }
}
