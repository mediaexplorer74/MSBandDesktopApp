// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IGolfSyncService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IGolfSyncService
  {
    bool IsCourseOnBand(string courseId);

    Task SyncCourseToBandAsync(string courseId, string teeId, CancellationToken token);

    SyncedGolfCourse LastSyncedCourse { get; }

    void ClearLastSyncedCourse();
  }
}
