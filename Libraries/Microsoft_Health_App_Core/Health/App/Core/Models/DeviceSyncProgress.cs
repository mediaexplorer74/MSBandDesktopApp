// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.DeviceSyncProgress
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class DeviceSyncProgress
  {
    public DeviceSyncProgress()
    {
    }

    public DeviceSyncProgress(bool isSyncing, ProgressStage stage, double percentComplete)
    {
      this.IsSyncing = isSyncing;
      this.PercentComplete = percentComplete;
      this.Stage = stage;
    }

    public bool IsSyncing { get; private set; }

    public double PercentComplete { get; private set; }

    public ProgressStage Stage { get; private set; }
  }
}
