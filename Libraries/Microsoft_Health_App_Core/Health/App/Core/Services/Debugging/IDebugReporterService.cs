// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Debugging.IDebugReporterService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Debugging
{
  public interface IDebugReporterService
  {
    long SdeCheckElapsed { get; set; }

    void ResetSdeTotalElapsed();

    void RecordEntry(DebugReporterEntry entry, int indentLevel);

    void RecordSyncResult(SyncDebugResult syncResult);

    IList<DebugReporterEntry> GetReport();

    Task SaveAsync(CancellationToken token);

    Task LoadAsync(CancellationToken token);
  }
}
