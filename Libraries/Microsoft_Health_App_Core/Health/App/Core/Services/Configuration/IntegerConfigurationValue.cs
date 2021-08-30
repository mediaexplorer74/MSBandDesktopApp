// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.IntegerConfigurationValue
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Configuration
{
  public class IntegerConfigurationValue : ConfigurationValue<int>
  {
    private readonly Func<Microsoft.Health.App.Core.Utilities.Range<int>> rangeFactory;

    public IntegerConfigurationValue(
      string category,
      string name,
      Func<Microsoft.Health.App.Core.Utilities.Range<int>> rangeFactory,
      Func<int> defaultValueFactory)
      : base(category, name, defaultValueFactory, ConfigurationLayout.Integer)
    {
      this.rangeFactory = rangeFactory;
    }

    public Microsoft.Health.App.Core.Utilities.Range<int> Range => this.rangeFactory();
  }
}
