// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.GenericConfigurationValue`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public abstract class GenericConfigurationValue<T> : ConfigurationValue
  {
    protected GenericConfigurationValue(
      string category,
      string name,
      ConfigurationLayout configurationLayout)
      : base(category, name, typeof (T), configurationLayout)
    {
      this.SerializeFunction = (Func<T, string>) null;
      this.DeserializeFunction = (Func<string, T>) null;
      this.IsSerializationFunctionSpecified = false;
    }

    protected GenericConfigurationValue(
      string category,
      string name,
      ConfigurationLayout configurationLayout,
      Func<T, string> serializeFunc,
      Func<string, T> deserializeFunc)
      : base(category, name, typeof (T), configurationLayout)
    {
      this.SerializeFunction = serializeFunc;
      this.DeserializeFunction = deserializeFunc;
      this.IsSerializationFunctionSpecified = true;
    }

    public bool IsSerializationFunctionSpecified { get; private set; }

    public Func<T, string> SerializeFunction { get; private set; }

    public Func<string, T> DeserializeFunction { get; private set; }

    protected override sealed void SetValueCore(
      IConfigurationService configurationService,
      object newValue)
    {
      configurationService.SetValue<T>(this, (T) newValue);
    }

    protected override sealed bool TryGetValueCore(
      IConfigurationService configurationService,
      out object actualValue)
    {
      T actualValue1;
      if (configurationService.TryGetValue<T>(this, out actualValue1))
      {
        actualValue = (object) actualValue1;
        return true;
      }
      actualValue = (object) null;
      return false;
    }
  }
}
