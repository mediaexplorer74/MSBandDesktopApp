// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.SyncProgress
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin
{
  public class SyncProgress
  {
    private readonly double percentageCompletion;
    private readonly SyncState state;

    public SyncProgress(double percentageCompletion, SyncState state)
    {
      this.percentageCompletion = percentageCompletion;
      this.state = state;
    }

    public double PercentageCompletion => this.percentageCompletion;

    public SyncState State => this.state;
  }
}
