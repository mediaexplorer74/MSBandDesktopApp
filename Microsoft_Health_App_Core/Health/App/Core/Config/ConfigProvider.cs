// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Config.ConfigProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Microsoft.Health.App.Core.Config
{
  public class ConfigProvider : IConfigProvider, ICrossConfigProvider
  {
    private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
    {
      ContractResolver = (IContractResolver) new LocaleContractResolver()
    };
    private readonly ICrossConfigProvider crossConfigProvider;
    private readonly ICryptographyService cryptographyService;

    public ConfigProvider(
      ICrossConfigProvider crossConfigProvider,
      ICryptographyService cryptographyService)
    {
      this.crossConfigProvider = crossConfigProvider;
      this.cryptographyService = cryptographyService;
    }

    public T Get<T>(string key, T defaultValue) => this.Get<T>(key, defaultValue, ConfigDomain.User);

    public void Set<T>(string key, T value) => this.Set<T>(key, value, ConfigDomain.User);

    public void Remove(string key) => this.Remove(key, ConfigDomain.User);

    public bool TryGetValue<T>(string key, out T value) => this.TryGetValue<T>(key, out value, ConfigDomain.User);

    public T Get<T>(string key, T defaultValue, ConfigDomain configType)
    {
      T obj;
      return !this.crossConfigProvider.TryGetValue<T>(key, out obj, configType) ? defaultValue : obj;
    }

    public void Set<T>(string key, T value, ConfigDomain configType) => this.crossConfigProvider.Set<T>(key, value, configType);

    public void Remove(string key, ConfigDomain configType) => this.crossConfigProvider.Remove(key, configType);

    public bool TryGetValue<T>(string key, out T value, ConfigDomain configType) => this.crossConfigProvider.TryGetValue<T>(key, out value, configType);

    public void Clear(ConfigDomain configType) => this.crossConfigProvider.Clear(configType);

    public T GetComplexValue<T>(string key, T defaultValue, bool unprotect = false) where T : class
    {
      string result = this.Get<string>(key, (string) null);
      if (string.IsNullOrEmpty(result))
        return defaultValue;
      if (unprotect)
        result = this.cryptographyService.UnprotectAsync(result).Result;
      try
      {
        return JsonConvert.DeserializeObject<T>(result, ConfigProvider.SerializerSettings);
      }
      catch (JsonException ex)
      {
        return defaultValue;
      }
    }

    public void SetComplexValue<T>(string key, T value, bool protect = false) where T : class
    {
      string data = JsonConvert.SerializeObject((object) value, ConfigProvider.SerializerSettings);
      if (protect)
        data = this.cryptographyService.ProtectAsync(data).Result;
      this.Set<string>(key, data);
    }

    public T GetEnum<T>(string key, T defaultValue)
    {
      T obj;
      return this.TryGetEnum<T>(key, out obj) ? obj : defaultValue;
    }

    public void SetEnum<T>(string key, T value) => this.crossConfigProvider.Set<string>(key, value.ToString(), ConfigDomain.User);

    public Version GetVersion(string key, Version defaultValue)
    {
      Version result;
      return Version.TryParse(this.crossConfigProvider.Get<string>(key, (string) null, ConfigDomain.User), out result) ? result : defaultValue;
    }

    public void SetVersion(string key, Version value) => this.crossConfigProvider.Set<string>(key, value.ToString(), ConfigDomain.User);

    public Guid GetGuid(string key, Guid defaultValue)
    {
      Guid result;
      return Guid.TryParse(this.crossConfigProvider.Get<string>(key, (string) null, ConfigDomain.User), out result) ? result : defaultValue;
    }

    public void SetGuid(string key, Guid value) => this.crossConfigProvider.Set<string>(key, value.ToString(), ConfigDomain.User);

    public DateTimeOffset GetDateTimeOffset(string key, DateTimeOffset defaultValue)
    {
      DateTimeOffset dateTimeOffset;
      return this.TryGetDateTimeOffset(key, out dateTimeOffset) ? dateTimeOffset : defaultValue;
    }

    public void SetDateTimeOffset(string key, DateTimeOffset value) => this.crossConfigProvider.Set<string>(key, value.ToString("o"), ConfigDomain.User);

    public bool TryGetDateTimeOffset(string key, out DateTimeOffset value)
    {
      string input;
      return this.crossConfigProvider.TryGetValue<string>(key, out input, ConfigDomain.User) && DateTimeOffset.TryParse(input, out value);
    }

    public bool TryGetEnum<T>(string key, out T value)
    {
      string str;
      if (!this.crossConfigProvider.TryGetValue<string>(key, out str, ConfigDomain.User))
      {
        value = default (T);
        return false;
      }
      if (string.IsNullOrEmpty(str))
      {
        value = default (T);
        return false;
      }
      try
      {
        value = (T) Enum.Parse(typeof (T), str);
        return true;
      }
      catch (InvalidCastException ex)
      {
        value = default (T);
        return false;
      }
    }
  }
}
