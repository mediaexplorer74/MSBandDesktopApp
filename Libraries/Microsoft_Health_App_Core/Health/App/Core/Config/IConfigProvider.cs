// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Config.IConfigProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Config
{
  public interface IConfigProvider : ICrossConfigProvider
  {
    T Get<T>(string key, T defaultValue);

    void Set<T>(string key, T value);

    void Remove(string key);

    bool TryGetValue<T>(string key, out T value);

    T GetComplexValue<T>(string key, T defaultValue, bool unprotect = false) where T : class;

    void SetComplexValue<T>(string key, T value, bool protect = false) where T : class;

    T GetEnum<T>(string key, T defaultValue);

    void SetEnum<T>(string key, T value);

    Version GetVersion(string key, Version defaultValue);

    void SetVersion(string key, Version value);

    DateTimeOffset GetDateTimeOffset(string key, DateTimeOffset defaultValue);

    void SetDateTimeOffset(string key, DateTimeOffset value);

    bool TryGetDateTimeOffset(string key, out DateTimeOffset value);

    bool TryGetEnum<T>(string key, out T value);

    Guid GetGuid(string key, Guid defaultValue);

    void SetGuid(string key, Guid value);
  }
}
