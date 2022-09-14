// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.GolfClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Events.Golf
{
  internal sealed class GolfClient : HealthCloudClientBase, IGolfClient
  {
    private readonly IHealthCloudClient cloudClient;
    private readonly IHttpCacheService cacheService;

    public GolfClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHealthCloudClient cloudClient,
      IHttpCacheService cacheService = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
      this.cloudClient = cloudClient != null ? cloudClient : throw new ArgumentNullException(nameof (cloudClient));
      this.cacheService = cacheService;
    }

    public async Task<IReadOnlyList<GolfEvent>> GetTopEventsAsync(
      int count,
      CancellationToken token,
      bool expandSequences = false)
    {
      return (IReadOnlyList<GolfEvent>) new List<GolfEvent>((IEnumerable<GolfEvent>) await this.cloudClient.GetTopEventsAsync<GolfEvent>(count, EventType.Golf, token, expandSequences, useCache: false).ConfigureAwait(false) ?? Enumerable.Empty<GolfEvent>());
    }

    public Task<GolfEvent> GetEventAsync(
      string eventId,
      CancellationToken token,
      bool expandSequences = false,
      bool expandInfo = false,
      bool expandEvidences = false)
    {
      return this.cloudClient.GetEventAsync<GolfEvent>(eventId, token, expandSequences, expandInfo, expandEvidences, false);
    }

    public Task RenameGolfEventAsync(string eventId, string name, CancellationToken token) => this.cloudClient.PatchEventAsync(eventId, name, EventType.Golf, token);

    public async Task<Stream> GetGolfCourseFileAsync(
      string courseId,
      string teeId,
      CancellationToken token)
    {
      if (string.IsNullOrWhiteSpace(courseId))
        throw new ArgumentNullException(nameof (courseId));
      if (string.IsNullOrWhiteSpace(teeId))
        throw new ArgumentNullException(nameof (teeId));
      string relativeUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}", new object[2]
      {
        (object) "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0",
        (object) courseId
      });
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add(nameof (teeId), teeId);
      NameValueCollection parameters = nameValueCollection;
      if (this.cacheService != null)
        await this.cacheService.RemoveTagsAsync("GolfRecent");
      return await this.GetResponseStreamAsync(relativeUrl, token, parameters);
    }
  }
}
