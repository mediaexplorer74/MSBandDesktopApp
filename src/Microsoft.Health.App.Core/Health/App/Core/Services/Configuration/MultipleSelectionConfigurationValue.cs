// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.MultipleSelectionConfigurationValue
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public class MultipleSelectionConfigurationValue : ConfigurationValue<IList<Enum>>
  {
    public MultipleSelectionConfigurationValue(string category, string name, Type type)
      : base(category, name, ConfigurationLayout.MultipleSelection)
    {
      this.EnumType = type;
    }

    public MultipleSelectionConfigurationValue(
      string category,
      string name,
      Type type,
      IList<Enum> defaultValue)
      : base(category, name, defaultValue, ConfigurationLayout.MultipleSelection)
    {
      this.EnumType = type;
    }

    public MultipleSelectionConfigurationValue(
      string category,
      string name,
      Type type,
      Func<IList<Enum>> defaultValueFactory)
      : base(category, name, defaultValueFactory, ConfigurationLayout.MultipleSelection)
    {
      this.EnumType = type;
    }

    public MultipleSelectionConfigurationValue(
      string category,
      string name,
      Type type,
      Func<IList<Enum>> defaultValueFactory,
      Func<IList<Enum>, string> serializeFunc,
      Func<string, IList<Enum>> deserializeFunc)
      : base(category, name, defaultValueFactory, ConfigurationLayout.MultipleSelection, serializeFunc, deserializeFunc)
    {
      this.EnumType = type;
    }

    public Type EnumType { get; private set; }
  }
}
