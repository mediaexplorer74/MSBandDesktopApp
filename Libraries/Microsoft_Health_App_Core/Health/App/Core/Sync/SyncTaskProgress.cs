// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.SyncTaskProgress
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;

namespace Microsoft.Health.App.Core.Sync
{
  public class SyncTaskProgress
  {
    public const int ProgressMin = 0;
    public const int ProgressMax = 100;

    public SyncTaskProgress()
    {
    }

    public SyncTaskProgress(double progressWeight, double percentageComplete, ProgressStage stage)
    {
      this.ProgressWeight = progressWeight;
      this.PercentageComplete = percentageComplete;
      this.Stage = stage;
    }

    public double PercentageComplete { get; private set; }

    public double ProgressWeight { get; private set; }

    public ProgressStage Stage { get; private set; }
  }
}
