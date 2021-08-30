// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.DebugSettingsConfigurationPageViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class DebugSettingsConfigurationPageViewModel : DebugSettingsPageViewModelBase
  {
    private IEnumerable<ConfigurationValueCategoryViewModel> categories;

    public DebugSettingsConfigurationPageViewModel(
      IConfigurationService configurationService,
      INetworkService networkService)
      : base(networkService)
    {
      if (configurationService == null)
        throw new ArgumentNullException(nameof (configurationService));
      this.categories = configurationService.Values.GroupBy<IConfigurationValue, string>((Func<IConfigurationValue, string>) (value => value.Category)).Select<IGrouping<string, IConfigurationValue>, ConfigurationValueCategoryViewModel>((Func<IGrouping<string, IConfigurationValue>, ConfigurationValueCategoryViewModel>) (grouping => new ConfigurationValueCategoryViewModel(grouping.Key, configurationService, (IEnumerable<IConfigurationValue>) grouping)));
      this.Header = "Configurations";
      this.SubHeader = "Dynamic config, accessory manager, environment service";
      this.GlyphIcon = "\uE032";
    }

    public IEnumerable<ConfigurationValueCategoryViewModel> Categories
    {
      get => this.categories;
      set => this.SetProperty<IEnumerable<ConfigurationValueCategoryViewModel>>(ref this.categories, value, nameof (Categories));
    }
  }
}
