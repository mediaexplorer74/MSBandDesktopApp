// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.ConfigurationValue
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public abstract class ConfigurationValue : IConfigurationValue
  {
    private readonly string category;
    private readonly string name;
    private readonly Type valueType;

    protected ConfigurationValue(
      string category,
      string name,
      Type valueType,
      ConfigurationLayout configurationLayout)
    {
      if (string.IsNullOrWhiteSpace(category))
        throw new ArgumentNullException(nameof (category));
      if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentNullException(nameof (name));
      if ((object) valueType == null)
        throw new ArgumentNullException(nameof (valueType));
      this.category = category;
      this.name = name;
      this.valueType = valueType;
      this.ConfigurationLayout = configurationLayout;
    }

    public string Category => this.category;

    public string Name => this.name;

    public string UniqueName => ConfigurationValue.CreateKey(this.Category, this.Name);

    public Type ValueType => this.valueType;

    public ConfigurationLayout ConfigurationLayout { get; private set; }

    public void ClearValue(IConfigurationService configurationService) => configurationService.ClearValue((IConfigurationValue) this);

    public void SetValue(IConfigurationService configurationService, object newValue) => this.SetValueCore(configurationService, newValue);

    public bool TryGetValue(IConfigurationService configurationService, out object actualValue) => this.TryGetValueCore(configurationService, out actualValue);

    public static string CreateKey(string category, string name)
    {
      Assert.ParamIsNotNullOrEmpty(category, nameof (category));
      Assert.ParamIsNotNullOrEmpty(name, nameof (name));
      return category + "." + name;
    }

    public static ConfigurationValue<bool> CreateBoolean(
      string category,
      string name,
      bool defaultValue)
    {
      return new ConfigurationValue<bool>(category, name, (Func<bool>) (() => defaultValue), ConfigurationLayout.Boolean);
    }

    public static ConfigurationValue<bool> CreateBoolean(
      string category,
      string name,
      Func<bool> defaultValueFactory)
    {
      return new ConfigurationValue<bool>(category, name, defaultValueFactory, ConfigurationLayout.Boolean);
    }

    public static AsyncConfigurationValue<bool> CreateBoolean(
      string category,
      string name,
      Func<Task<bool>> defaultValueFactory)
    {
      return new AsyncConfigurationValue<bool>(category, name, defaultValueFactory, ConfigurationLayout.Boolean);
    }

    public static IntegerConfigurationValue CreateInteger(
      string category,
      string name,
      Range<int> range,
      int defaultValue)
    {
      return new IntegerConfigurationValue(category, name, (Func<Range<int>>) (() => range), (Func<int>) (() => defaultValue));
    }

    public static IntegerConfigurationValue CreateInteger(
      string category,
      string name,
      Range<int> range,
      Func<int> defaultValueFactory)
    {
      return new IntegerConfigurationValue(category, name, (Func<Range<int>>) (() => range), defaultValueFactory);
    }

    public static IntegerConfigurationValue CreateInteger(
      string category,
      string name,
      Func<Range<int>> rangeFactory,
      Func<int> defaultValueFactory)
    {
      return new IntegerConfigurationValue(category, name, rangeFactory, defaultValueFactory);
    }

    public static MultipleSelectionConfigurationValue CreateMultipleSelectables(
      string category,
      string name,
      Type type,
      Func<IList<Enum>> defaultValueFactory,
      Func<IList<Enum>, string> serializeFunc,
      Func<string, IList<Enum>> deserializeFunc)
    {
      return new MultipleSelectionConfigurationValue(category, name, type, defaultValueFactory, serializeFunc, deserializeFunc);
    }

    public static ConfigurationValue<string> Create(
      string category,
      string name,
      string defaultValue)
    {
      return new ConfigurationValue<string>(category, name, defaultValue, ConfigurationLayout.String);
    }

    public static ConfigurationValue<string> Create(
      string category,
      string name,
      Func<string> defaultValueFactory)
    {
      return new ConfigurationValue<string>(category, name, defaultValueFactory, ConfigurationLayout.String);
    }

    protected abstract void SetValueCore(
      IConfigurationService configurationService,
      object newValue);

    protected abstract bool TryGetValueCore(
      IConfigurationService configurationService,
      out object actualValue);
  }
}
