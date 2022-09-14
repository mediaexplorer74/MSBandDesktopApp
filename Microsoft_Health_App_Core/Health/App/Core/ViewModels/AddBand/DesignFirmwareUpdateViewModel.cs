// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddBand.DesignFirmwareUpdateViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.AddBand
{
  public class DesignFirmwareUpdateViewModel
  {
    public string Title => "Update in progress";

    public string Subtitle => "Hang tight, this may take a few minutes.";

    public string StepNumberText => "Step 1 of 6";

    public string StepText => "Preparing to update...";

    public double PercentageCompletion => 47.0;

    public FirmwareUpdateViewModel.FirmwareUpdateStatus UpdateStatus => FirmwareUpdateViewModel.FirmwareUpdateStatus.Updating;

    public ICommand CancelCommand => (ICommand) null;

    public ICommand RetryCommand => (ICommand) null;

    public ICommand FinishCommand => (ICommand) null;
  }
}
