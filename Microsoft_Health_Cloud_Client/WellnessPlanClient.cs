// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WellnessPlanClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  internal class WellnessPlanClient : HealthCloudClientBase, IWellnessPlanClient
  {
    public WellnessPlanClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
    }

    public async Task<WellnessSchedule> GetScheduleAsync(
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      CancellationToken token,
      bool bypassCache = false)
    {
      try
      {
        string cacheArea = bypassCache ? (string) null : "WellnessSchedule";
        string[] strArray1;
        if (!bypassCache)
          strArray1 = new string[1]{ "Sync" };
        else
          strArray1 = (string[]) null;
        string[] strArray2 = strArray1;
        NameValueCollection parameters = new NameValueCollection();
        parameters.Add("utcStartTime", startTime.ToString("o"));
        parameters.Add("utcEndTime", endTime.ToString("o"));
        return await this.GetJsonAsync<WellnessSchedule>("v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessSchedule", token, parameters, cacheArea, (IEnumerable<string>) strArray2).ConfigureAwait(false);
      }
      catch (NotFoundException ex)
      {
        return (WellnessSchedule) null;
      }
    }

    public async Task PutScheduleAsync(
      WellnessSchedule schedule,
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      CancellationToken token)
    {
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add("utcStartTime", startTime.ToString("o"));
      parameters.Add("utcEndTime", endTime.ToString("o"));
      await this.PutJsonAsync<WellnessSchedule>("v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessSchedule", parameters, schedule, token).ConfigureAwait(false);
      if (this.CacheService == null)
        return;
      await this.CacheService.RemoveTagsAsync("WellnessSchedule");
    }

    public async Task<UserWellnessPlanProgress> GetPlanProgressAsync(
      string planId,
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      CancellationToken cancellationToken)
    {
      string relativeUrl = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/{1}/Progress", new object[2]
      {
        (object) "v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessPlans",
        (object) planId
      });
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add(nameof (startTime), startTime.ToString("o"));
      nameValueCollection.Add(nameof (endTime), endTime.ToString("o"));
      NameValueCollection parameters = nameValueCollection;
      return await this.GetJsonAsync<UserWellnessPlanProgress>(relativeUrl, cancellationToken, parameters, "Sync");
    }

    public Task<UserWellnessPlansResponse> GetPlansAsync(
      CancellationToken cancellationToken,
      WellnessPlanType? planType = null,
      IList<string> includes = null,
      WellnessPlanStatus? status = null)
    {
      NameValueCollection parameters = new NameValueCollection();
      if (planType.HasValue)
        parameters.Add("wellnessPlanTypes", planType.Value.ToString());
      if (includes != null && includes.Any<string>())
        parameters.Add("planIncludes", string.Join(",", (IEnumerable<string>) includes));
      if (status.HasValue)
        parameters.Add("wellnessPlanStatus", status.Value.ToString());
      return this.GetJsonAsync<UserWellnessPlansResponse>("v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessPlans", cancellationToken, parameters, "WellnessPlan", (IEnumerable<string>) new string[1]
      {
        "Sync"
      });
    }

    public async Task UpdatePlanGoalAsync(
      string planId,
      string goalId,
      WellnessGoalUpdate update,
      CancellationToken token)
    {
      await this.PatchJsonAsync<WellnessGoalUpdate>(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessPlans/{0}/goals/{1}", new object[2]
      {
        (object) planId,
        (object) goalId
      }), (NameValueCollection) null, update, token).ConfigureAwait(false);
      if (this.CacheService == null)
        return;
      await this.CacheService.RemoveTagsAsync("WellnessPlan", "WellnessSchedule");
    }
  }
}
