// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.SleepProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class SleepProvider : ISleepProvider
  {
    private readonly IHealthCloudClient healthCloudClient;

    public SleepProvider(IHealthCloudClient healthCloudClient) => this.healthCloudClient = healthCloudClient;

    public async Task<IList<SleepEvent>> GetTopSleepEventsAsync(int count) => await this.healthCloudClient.GetTopEventsAsync<SleepEvent>(count, EventType.Sleeping, CancellationToken.None);

    public async Task<SleepEvent> GetLastSleepEventAsync(bool expand = false)
    {
      IHealthCloudClient healthCloudClient = this.healthCloudClient;
      CancellationToken none = CancellationToken.None;
      bool flag = expand;
      int num1 = expand ? 1 : 0;
      int num2 = flag ? 1 : 0;
      int num3 = expand ? 1 : 0;
      return (await healthCloudClient.GetTopEventsAsync<SleepEvent>(1, EventType.Sleeping, none, num1 != 0, num2 != 0, num3 != 0)).FirstOrDefault<SleepEvent>();
    }

    public async Task<SleepEvent> GetSleepEventAsync(string eventId) => await this.healthCloudClient.GetSleepEventDetailsAsync(eventId, CancellationToken.None);

    public async Task DeleteSleepEventAsync(string eventId) => await this.healthCloudClient.DeleteEventAsync(eventId, EventType.Sleeping, CancellationToken.None);
  }
}
