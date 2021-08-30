// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Config.ICrossConfigProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Config
{
  public interface ICrossConfigProvider
  {
    T Get<T>(string key, T defaultValue, ConfigDomain configType);

    void Set<T>(string key, T value, ConfigDomain configType);

    void Remove(string key, ConfigDomain configType);

    bool TryGetValue<T>(string key, out T value, ConfigDomain configType);

    void Clear(ConfigDomain configType);
  }
}
