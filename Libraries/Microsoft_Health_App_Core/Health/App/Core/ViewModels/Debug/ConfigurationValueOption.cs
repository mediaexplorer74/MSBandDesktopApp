// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.ConfigurationValueOption
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public sealed class ConfigurationValueOption
  {
    private readonly string displayName;
    private readonly object optionValue;

    public ConfigurationValueOption(string displayName, object optionValue)
    {
      this.displayName = !string.IsNullOrWhiteSpace(displayName) ? displayName : throw new ArgumentNullException(nameof (displayName));
      this.optionValue = optionValue;
    }

    public string DisplayName => this.displayName;

    public object OptionValue => this.optionValue;
  }
}
