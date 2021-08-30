// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.ConfigurationValue`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public class ConfigurationValue<T> : GenericConfigurationValue<T>
  {
    private readonly Func<T> defaultValueFactory;

    public ConfigurationValue(
      string category,
      string name,
      ConfigurationLayout configurationLayout)
      : this(category, name, default (T), configurationLayout)
    {
    }

    public ConfigurationValue(
      string category,
      string name,
      T defaultValue,
      ConfigurationLayout configurationLayout)
      : this(category, name, (Func<T>) (() => defaultValue), configurationLayout)
    {
    }

    public ConfigurationValue(
      string category,
      string name,
      Func<T> defaultValueFactory,
      ConfigurationLayout configurationLayout)
      : base(category, name, configurationLayout)
    {
      this.defaultValueFactory = defaultValueFactory != null ? defaultValueFactory : throw new ArgumentNullException(nameof (defaultValueFactory));
    }

    public ConfigurationValue(
      string category,
      string name,
      Func<T> defaultValueFactory,
      ConfigurationLayout configurationLayout,
      Func<T, string> serializeFunc,
      Func<string, T> deserializeFunc)
      : base(category, name, configurationLayout, serializeFunc, deserializeFunc)
    {
      this.defaultValueFactory = defaultValueFactory != null ? defaultValueFactory : throw new ArgumentNullException(nameof (defaultValueFactory));
    }

    public T GetDefaultValue() => this.defaultValueFactory();
  }
}
