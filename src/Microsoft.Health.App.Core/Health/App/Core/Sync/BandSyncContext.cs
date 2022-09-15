// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.BandSyncContext
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Sync;
using System;

namespace Microsoft.Health.App.Core.Sync
{
  public class BandSyncContext
  {
    public BandSyncContext(SyncType syncType, Action<SyncTaskProgress> progress)
    {
      this.SyncType = syncType;
      this.Progress = progress;
    }

    public Action<SyncTaskProgress> Progress { get; }

    public SyncType SyncType { get; }
  }
}
