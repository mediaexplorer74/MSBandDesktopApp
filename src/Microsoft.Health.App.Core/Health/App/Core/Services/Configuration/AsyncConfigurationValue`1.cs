// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.AsyncConfigurationValue`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public sealed class AsyncConfigurationValue<T> : GenericConfigurationValue<T>
  {
    private readonly Func<Task<T>> defaultValueFactory;

    public AsyncConfigurationValue(
      string category,
      string name,
      Func<Task<T>> defaultValueFactory,
      ConfigurationLayout configurationLayout)
      : base(category, name, configurationLayout)
    {
      this.defaultValueFactory = defaultValueFactory != null ? defaultValueFactory : throw new ArgumentNullException(nameof (defaultValueFactory));
    }

    public AsyncConfigurationValue(
      string category,
      string name,
      Func<Task<T>> defaultValueFactory,
      ConfigurationLayout configurationLayout,
      Func<T, string> serializeFunc,
      Func<string, T> deserializeFunc)
      : base(category, name, configurationLayout, serializeFunc, deserializeFunc)
    {
      this.defaultValueFactory = defaultValueFactory != null ? defaultValueFactory : throw new ArgumentNullException(nameof (defaultValueFactory));
    }

    public Task<T> GetDefaultValueAsync() => this.defaultValueFactory();
  }
}
