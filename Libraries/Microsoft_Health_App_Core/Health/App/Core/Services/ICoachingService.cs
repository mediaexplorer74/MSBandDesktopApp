// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ICoachingService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface ICoachingService
  {
    bool? IsPlanActive { get; }

    bool ComingUpPendingRefresh { get; set; }

    bool TilePendingRefresh { get; set; }

    Task<bool> RefreshIsPlanActiveAsync(CancellationToken token);

    Task SyncSpecialRemindersAsync(CancellationToken token);

    Task ClearSpecialRemindersCacheAsync(CancellationToken token);

    Task SyncGoalsToBandAsync(CancellationToken token);
  }
}
