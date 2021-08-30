// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.ConfigurationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public sealed class ConfigurationService : IConfigurationService
  {
    private readonly IConfigProvider configProvider;
    private readonly Lazy<IEnumerable<IConfigurationValue>> values;

    public ConfigurationService(IConfigProvider configProvider, IServiceLocator serviceLocator)
    {
      if (configProvider == null)
        throw new ArgumentNullException(nameof (configProvider));
      if (serviceLocator == null)
        throw new ArgumentNullException(nameof (serviceLocator));
      this.configProvider = configProvider;
      this.values = new Lazy<IEnumerable<IConfigurationValue>>(new Func<IEnumerable<IConfigurationValue>>(serviceLocator.GetAllInstances<IConfigurationValue>));
    }

    public IEnumerable<IConfigurationValue> Values => this.values.Value;

    public void ClearValue(IConfigurationValue value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.configProvider.Remove(value.UniqueName);
    }

    public T GetValue<T>(ConfigurationValue<T> value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      T actualValue;
      return this.TryGetValue<T>((GenericConfigurationValue<T>) value, out actualValue) ? actualValue : value.GetDefaultValue();
    }

    public Task<T> GetValueAsync<T>(AsyncConfigurationValue<T> value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      T actualValue;
      return this.TryGetValue<T>((GenericConfigurationValue<T>) value, out actualValue) ? Task.FromResult<T>(actualValue) : value.GetDefaultValueAsync();
    }

    public void SetValue<T>(GenericConfigurationValue<T> value, T newValue)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value.IsSerializationFunctionSpecified)
      {
        string str = value.SerializeFunction(newValue);
        this.configProvider.Set<string>(value.UniqueName, str);
      }
      else
        this.configProvider.Set<T>(value.UniqueName, newValue);
    }

    public bool TryGetValue<T>(GenericConfigurationValue<T> value, out T actualValue)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (!value.IsSerializationFunctionSpecified)
        return this.configProvider.TryGetValue<T>(value.UniqueName, out actualValue);
      string str;
      if (this.configProvider.TryGetValue<string>(value.UniqueName, out str))
      {
        actualValue = value.DeserializeFunction(str);
        return true;
      }
      actualValue = default (T);
      return false;
    }
  }
}
