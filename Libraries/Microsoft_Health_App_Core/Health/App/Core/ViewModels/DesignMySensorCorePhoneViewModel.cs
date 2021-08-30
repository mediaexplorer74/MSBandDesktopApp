// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.DesignMySensorCorePhoneViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class DesignMySensorCorePhoneViewModel
  {
    public bool IsLoading => false;

    public bool IsEnabled => false;

    public bool CanSetEnablement => false;

    public bool ShowInstructions => !this.CanSetEnablement && !this.IsEnabled;

    public string Status => AppResources.MySensorCorePhoneDisabled;

    public ICommand MotionDataCommand => (ICommand) null;

    public ICommand EnableCommand => (ICommand) null;
  }
}
