// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.IConfigurationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public interface IConfigurationService
  {
    IEnumerable<IConfigurationValue> Values { get; }

    void ClearValue(IConfigurationValue value);

    T GetValue<T>(ConfigurationValue<T> value);

    Task<T> GetValueAsync<T>(AsyncConfigurationValue<T> value);

    void SetValue<T>(GenericConfigurationValue<T> value, T newValue);

    bool TryGetValue<T>(GenericConfigurationValue<T> value, out T actualValue);
  }
}
