// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.GenderViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Resources;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class GenderViewModel
  {
    public GenderViewModel(Gender gender) => this.Value = gender;

    public Gender Value { get; set; }

    public string Display
    {
      get
      {
        switch (this.Value)
        {
          case Gender.Male:
            return AppResources.Gender_Male;
          case Gender.Female:
            return AppResources.Gender_Female;
          default:
            return string.Empty;
        }
      }
    }
  }
}
