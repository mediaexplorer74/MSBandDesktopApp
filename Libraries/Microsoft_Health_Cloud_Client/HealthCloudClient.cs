// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HealthCloudClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Caching;
using Microsoft.Health.Cloud.Client.Events.Golf;
using Microsoft.Health.Cloud.Client.Events.Golf.Courses;
using Microsoft.Health.Cloud.Client.Exceptions;
using Microsoft.Health.Cloud.Client.Extensions;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Models;
using Microsoft.Health.Cloud.Client.Models.Social;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public class HealthCloudClient : HealthCloudClientBase, IHealthCloudClient
  {
    private readonly Lazy<ISensorDataClient> sensorDataClient;
    private readonly Lazy<IDeviceClient> deviceClient;
    private readonly Func<CancellationToken, Task<Uri>> socialServiceUriSelector;
    private readonly Func<CancellationToken, Task<string>> katTokenSelector;
    private readonly Lazy<IGolfClient> golfClient;
    private readonly Lazy<IGolfCourseClient> golfCourseClient;
    private readonly Lazy<IWellnessPlanClient> wellnessPlanClient;

    public HealthCloudClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null,
      Func<CancellationToken, Task<Uri>> shareServiceUriSelector = null,
      Func<CancellationToken, Task<string>> katTokenSelector = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
      this.sensorDataClient = new Lazy<ISensorDataClient>(new Func<ISensorDataClient>(this.CreateSensorDataClient));
      this.deviceClient = new Lazy<IDeviceClient>(new Func<IDeviceClient>(this.CreateDeviceClient));
      this.socialServiceUriSelector = shareServiceUriSelector;
      this.katTokenSelector = katTokenSelector;
      this.golfClient = new Lazy<IGolfClient>(new Func<IGolfClient>(this.CreateGolfClient));
      this.golfCourseClient = new Lazy<IGolfCourseClient>(new Func<IGolfCourseClient>(this.CreateGolfCourseClient));
      this.wellnessPlanClient = new Lazy<IWellnessPlanClient>(new Func<IWellnessPlanClient>(this.CreateWellnessPlanClient));
    }

    public Task<UserProfile> GetUserProfileAsync(CancellationToken cancellationToken) => this.GetJsonAsync<UserProfile>("api/users/GetUser", cancellationToken, cacheArea: "UserProfile");

    public async Task UpdateUserProfileAsync(
      UserProfile userProfile,
      CancellationToken cancellationToken)
    {
      await this.PutJsonAsync<UserProfile>("api/users/UpdateUser", (NameValueCollection) null, userProfile, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("UserProfile");
    }

    public Task<IList<UserDailySummary>> GetDailySummariesAsync(
      DateTimeOffset start,
      DateTimeOffset end,
      TimeSpan offset,
      CancellationToken cancellationToken,
      string deviceId = null)
    {
      string relativeUrl;
      if (string.IsNullOrWhiteSpace(deviceId))
        relativeUrl = string.Format("{0}(timeZoneUtcOffset={1},startDate='{2}',endDate='{3}')", (object) "v2/UserDailySummary", (object) offset.TotalMinutes, (object) HealthCloudClient.ToDayId(start), (object) HealthCloudClient.ToDayId(end));
      else
        relativeUrl = string.Format("{0}(timeZoneUtcOffset={1},startDate='{2}',endDate='{3}',deviceId='{4}')", (object) "v2/UserDailySummary", (object) offset.TotalMinutes, (object) HealthCloudClient.ToDayId(start), (object) HealthCloudClient.ToDayId(end), (object) deviceId);
      string cacheArea = this.WasSyncedByDate(end.AddDays(-1.0)) ? (string) null : "Sync";
      return this.GetODataJsonAsync<UserDailySummary>(relativeUrl, cancellationToken, cacheArea);
    }

    public Task<IList<UserActivity>> GetUserActivitiesAsync(
      DateTimeOffset start,
      DateTimeOffset end,
      ActivityPeriod activityPeriod,
      CancellationToken cancellationToken,
      string deviceId = null)
    {
      string relativeUrl;
      if (string.IsNullOrWhiteSpace(deviceId))
        relativeUrl = string.Format("{0}(period='{1}')?$filter=TimeOfDay+ge+datetimeoffset'{2}'+and+TimeOfDay+lt+datetimeoffset'{3}'", (object) "v2/UserHourlySummary", (object) HealthCloudClient.ToString(activityPeriod), (object) HealthCloudClientBase.ToIsoDateTime(start), (object) HealthCloudClientBase.ToIsoDateTime(end));
      else
        relativeUrl = string.Format("{0}(period='{1}',deviceId='{2}')?$filter=TimeOfDay+ge+datetimeoffset'{3}'+and+TimeOfDay+lt+datetimeoffset'{4}'", (object) "v2/UserHourlySummary", (object) HealthCloudClient.ToString(activityPeriod), (object) deviceId, (object) HealthCloudClientBase.ToIsoDateTime(start), (object) HealthCloudClientBase.ToIsoDateTime(end));
      string cacheArea = this.WasSyncedByDate((DateTimeOffset) start.Date) ? (string) null : "Sync";
      return this.GetODataJsonAsync<UserActivity>(relativeUrl, cancellationToken, cacheArea);
    }

    public Task<IList<RaisedInsight>> GetRaisedInsightsAsync(
      CancellationToken cancellationToken,
      IEnumerable<InsightDataUsedPivot> dataUsed = null,
      IEnumerable<InsightTimespanPivot> timespan = null,
      IEnumerable<InsightScopePivot> scope = null,
      IEnumerable<InsightCategoryPivot> category = null)
    {
      List<string> stringList = new List<string>();
      if (dataUsed != null)
        stringList.Add(HealthCloudClient.CreateMultiValueParameter<InsightDataUsedPivot>("DataUsed", dataUsed));
      if (timespan != null)
        stringList.Add(HealthCloudClient.CreateMultiValueParameter<InsightTimespanPivot>("Timespan", timespan));
      if (scope != null)
        stringList.Add(HealthCloudClient.CreateMultiValueParameter<InsightScopePivot>("Scope", scope));
      if (category != null)
        stringList.Add(HealthCloudClient.CreateMultiValueParameter<InsightCategoryPivot>("Category", category));
      string relativeUrl = "v1/RaisedInsights";
      if (stringList.Count > 0)
        relativeUrl = relativeUrl + "(" + string.Join(",", (IEnumerable<string>) stringList) + ")";
      return this.GetODataJsonAsync<RaisedInsight>(relativeUrl, cancellationToken, "Insights", (IEnumerable<string>) new string[1]
      {
        "Sync"
      });
    }

    public async Task AcknowledgeInsightAsync(string insightId, CancellationToken token)
    {
      await this.PutAsync(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}(raisedInsightId={1},acknowledged=true)", new object[2]
      {
        (object) "v1/RaisedInsights",
        (object) insightId
      }), token).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("Insights");
    }

    public Task<IList<T>> GetEventsAsync<T>(
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      EventType eventType,
      CancellationToken cancellationToken)
      where T : UserEvent
    {
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(eventType='{0}')?$filter=StartTime+ge+datetimeoffset'{1}'+and+StartTime+le+datetimeoffset'{2}'", new object[3]
      {
        (object) eventType,
        (object) HealthCloudClientBase.ToIsoDateTime(startTime),
        (object) HealthCloudClientBase.ToIsoDateTime(endTime)
      });
      stringBuilder.Append("&$expand=Sequences");
      string[] strArray = new string[1]
      {
        CloudCacheTag.ForEventType(eventType)
      };
      string cacheArea = this.WasSyncedByTime(endTime) ? "Events" : "Sync";
      return this.GetODataJsonAsync<T>(stringBuilder.ToString(), cancellationToken, cacheArea, (IEnumerable<string>) strArray);
    }

    public Task<IList<T>> GetEventsOnAsync<T>(
      EventType eventType,
      DateTimeOffset dayId,
      CancellationToken cancellationToken)
      where T : UserEvent
    {
      dayId = HealthCloudClient.TruncateToDay(dayId);
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(eventType='{0}')?$filter=DayId+eq+datetime'{1}'&$expand=Sequences", new object[2]
      {
        (object) eventType,
        (object) HealthCloudClient.ToDayId(dayId)
      });
      List<string> stringList = new List<string>()
      {
        "EventType=All"
      };
      string cacheArea = this.WasSyncedByDate(dayId) ? "Events" : "Sync";
      return this.GetODataJsonAsync<T>(stringBuilder.ToString(), cancellationToken, cacheArea, (IEnumerable<string>) stringList);
    }

    public Task<IList<T>> GetEventsBetweenAsync<T>(
      DateTimeOffset startDayId,
      DateTimeOffset endDayId,
      EventType eventType,
      CancellationToken cancellationToken)
      where T : UserEvent
    {
      startDayId = HealthCloudClient.TruncateToDay(startDayId);
      endDayId = HealthCloudClient.TruncateToDay(endDayId);
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(eventType='{0}')?$filter=DayId+ge+datetime'{1}'+and+DayId+le+datetime'{2}'&$expand=Sequences", new object[3]
      {
        (object) eventType,
        (object) HealthCloudClient.ToDayId(startDayId),
        (object) HealthCloudClient.ToDayId(endDayId)
      });
      string[] strArray = new string[1]
      {
        CloudCacheTag.ForEventType(eventType)
      };
      string cacheArea = this.WasSyncedByDate(endDayId) ? "Events" : "Sync";
      return this.GetODataJsonAsync<T>(stringBuilder.ToString(), cancellationToken, cacheArea, (IEnumerable<string>) strArray);
    }

    public Task<IList<T>> GetEventsAsync<T>(
      EventType eventType,
      int top,
      DateTimeOffset? beforeDate,
      int splitDistanceCm,
      CancellationToken cancellationToken)
      where T : UserEvent
    {
      string format1 = "(eventType='{0}'{1})?$top={2}";
      string str = splitDistanceCm >= 0 ? ",selectedSplitDistance=" + (object) splitDistanceCm : string.Empty;
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat(format1, new object[3]
      {
        (object) eventType,
        (object) str,
        (object) top
      });
      if (beforeDate.HasValue)
      {
        string format2 = "&$filter=StartTime+lt+datetimeoffset'{0}'";
        beforeDate = new DateTimeOffset?(beforeDate.Value.Subtract(TimeSpan.FromMilliseconds(1.0)));
        stringBuilder.AppendFormat(format2, new object[1]
        {
          (object) HealthCloudClientBase.ToIsoDateTime(beforeDate.Value)
        });
      }
      return this.GetODataJsonAsync<T>(stringBuilder.ToString(), cancellationToken, "Sync", (IEnumerable<string>) new string[1]
      {
        "EventType=All"
      });
    }

    public Task<IList<UsersGoal>> GetBestGoalsAsync(
      CancellationToken cancellationToken,
      GoalType goalType = GoalType.Unknown,
      bool isExpanded = false)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("isExpand", isExpanded.ToString().ToLower());
      nameValueCollection.Add("category", "5");
      NameValueCollection parameters = nameValueCollection;
      switch (goalType)
      {
        case GoalType.Unknown:
          return this.GetJsonAsync<IList<UsersGoal>>("v1/Goals", cancellationToken, parameters, "Sync", (IEnumerable<string>) new string[1]
          {
            "EventType=All"
          });
        case GoalType.SleepGoal:
        case GoalType.GolfGoal:
          return Task.FromResult<IList<UsersGoal>>((IList<UsersGoal>) new List<UsersGoal>());
        case GoalType.RunGoal:
        case GoalType.WorkoutGoal:
        case GoalType.BikeGoal:
        case GoalType.HikeGoal:
          parameters.Add("type", goalType.ToString());
          goto case GoalType.Unknown;
        default:
          throw new ArgumentException("Goal type is not recognized: " + (object) goalType, nameof (goalType));
      }
    }

    public async Task<T> GetEventAsync<T>(
      string eventId,
      CancellationToken cancellationToken,
      bool expandSequences = false,
      bool expandInfo = false,
      bool expandEvidences = false,
      bool useCache = true)
      where T : UserEvent
    {
      if (string.IsNullOrWhiteSpace(eventId))
        return default (T);
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(EventId='{0}')", new object[1]
      {
        (object) eventId
      });
      List<string> stringList = new List<string>();
      if (expandSequences)
        stringList.Add("Sequences");
      if (expandInfo)
        stringList.Add("Info");
      if (expandEvidences)
        stringList.Add("Evidences");
      if (stringList.Count > 0)
        stringBuilder.AppendFormat("?$expand={0}", new object[1]
        {
          (object) string.Join<string>(",", (IEnumerable<string>) stringList)
        });
      string cacheArea = (string) null;
      string[] strArray = (string[]) null;
      if (useCache)
      {
        cacheArea = "Events";
        strArray = new string[1]
        {
          CloudCacheTag.ForEventId(eventId)
        };
      }
      string relativeUrl = stringBuilder.ToString();
      IList<T> objList = await this.GetODataJsonAsync<T>(relativeUrl, cancellationToken, cacheArea, (IEnumerable<string>) strArray).ConfigureAwait(false);
      if (objList == null || objList.Count != 1)
        return default (T);
      T result = objList[0];
      if (!result.IsComplete() && this.CacheService != null)
        await this.CacheService.AddTagAsync(await this.CreateUrlAsync(relativeUrl, cancellationToken).ConfigureAwait(false), "Sync");
      return result;
    }

    public Task<IList<T>> GetTopEventsAsync<T>(
      int count,
      EventType eventType,
      CancellationToken cancellationToken,
      bool expandSequences = false,
      bool expandInfo = false,
      bool expandEvidences = false,
      bool useCache = true)
      where T : UserEvent
    {
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(eventType='{0}')?$top={1}", new object[2]
      {
        (object) eventType,
        (object) count
      });
      List<string> stringList = new List<string>();
      if (expandSequences)
        stringList.Add("Sequences");
      if (expandInfo)
        stringList.Add("Info");
      if (expandEvidences)
        stringList.Add("Evidences");
      if (stringList.Count > 0)
        stringBuilder.AppendFormat("&$expand={0}", new object[1]
        {
          (object) string.Join<string>(",", (IEnumerable<string>) stringList)
        });
      string cacheArea = (string) null;
      string[] strArray = (string[]) null;
      if (useCache)
      {
        cacheArea = "Sync";
        strArray = new string[1]
        {
          CloudCacheTag.ForEventType(eventType)
        };
      }
      return this.GetODataJsonAsync<T>(stringBuilder.ToString(), cancellationToken, cacheArea, (IEnumerable<string>) strArray);
    }

    public Task<T> GetRouteBasedExerciseEventDetailsAsync<T>(
      string eventId,
      int splitDistanceCm,
      CancellationToken cancellationToken)
      where T : RouteBasedExerciseEvent
    {
      return this.GetEventDetailsAsync<T>(eventId, "$expand=Sequences,MapPoints,Info,Evidences", cancellationToken, splitDistanceCm);
    }

    public Task<ExerciseEvent> GetExerciseEventDetailsAsync(
      string eventId,
      CancellationToken cancellationToken)
    {
      return this.GetEventDetailsAsync<ExerciseEvent>(eventId, "$expand=Sequences,Info,Evidences", cancellationToken);
    }

    public Task<SleepEvent> GetSleepEventDetailsAsync(
      string eventId,
      CancellationToken cancellationToken)
    {
      return this.GetEventDetailsAsync<SleepEvent>(eventId, "$expand=Sequences,Info,Evidences", cancellationToken);
    }

    public async Task DeleteEventAsync(
      string eventId,
      EventType eventType,
      CancellationToken cancellationToken)
    {
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(EventId='{0}')", new object[1]
      {
        (object) eventId
      });
      await this.DeleteAsync(stringBuilder.ToString(), cancellationToken).ConfigureAwait(false);
      if (this.CacheService == null)
        return;
      await this.CacheService.RemoveTagsAsync(CloudCacheTag.ForEventId(eventId), CloudCacheTag.ForEventType(eventType), "EventType=All");
    }

    public async Task PatchEventAsync(
      string eventId,
      string name,
      EventType eventType,
      CancellationToken cancellationToken)
    {
      if (name == null)
        name = string.Empty;
      byte[] bytes = Encoding.UTF8.GetBytes(new JObject()
      {
        {
          "Name",
          (JToken) name
        }
      }.ToString());
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      stringBuilder.AppendFormat("(EventId='{0}')", new object[1]
      {
        (object) eventId
      });
      await this.PatchAsync(stringBuilder.ToString(), bytes, cancellationToken).ConfigureAwait(false);
      if (this.CacheService == null)
        return;
      await this.CacheService.RemoveTagsAsync(CloudCacheTag.ForEventId(eventId), CloudCacheTag.ForEventType(eventType), "EventType=All");
    }

    private async Task<T> GetEventDetailsAsync<T>(
      string eventId,
      string queryParam,
      CancellationToken cancellationToken,
      int splitDistanceCm = -1)
      where T : UserEvent
    {
      StringBuilder stringBuilder = new StringBuilder("v1/Events");
      string str = splitDistanceCm >= 0 ? ",selectedSplitDistance=" + (object) splitDistanceCm : string.Empty;
      stringBuilder.AppendFormat("(EventId='{0}'{1})?{2}", new object[3]
      {
        (object) eventId,
        (object) str,
        (object) queryParam
      });
      string[] strArray = new string[1]
      {
        CloudCacheTag.ForEventId(eventId)
      };
      string relativeUrl = stringBuilder.ToString();
      IList<T> objList = await this.GetODataJsonAsync<T>(relativeUrl, cancellationToken, "Events", (IEnumerable<string>) strArray).ConfigureAwait(false);
      T result = objList != null && objList.Count == 1 ? objList[0] : throw new HealthCloudException("Could not find the matching event");
      if (!result.IsComplete() && this.CacheService != null)
        await this.CacheService.AddTagAsync(await this.CreateUrlAsync(relativeUrl, cancellationToken).ConfigureAwait(false), "Sync");
      return result;
    }

    public async Task<ShareUrlsResponseData> EnableEventSharingAsync(
      ShareUrlsRequestFormData formData,
      IMultiPartAttachment thumbnail,
      CancellationToken cancellationToken)
    {
      if (this.socialServiceUriSelector == null)
        throw new InvalidOperationException("Social service Uri selector must not be null");
      if (formData == null)
        throw new ArgumentNullException(nameof (formData));
      if (formData.EventType == null)
        throw new InvalidDataException(string.Format("{0}.{1} must not be null", new object[2]
        {
          (object) nameof (formData),
          (object) "EventType"
        }));
      if (formData.EventId == null)
        throw new InvalidDataException(string.Format("{0}.{1} must not be null", new object[2]
        {
          (object) nameof (formData),
          (object) "EventId"
        }));
      cancellationToken.ThrowIfCancellationRequested();
      Uri socialUri = await this.CreateUrlAsync(this.socialServiceUriSelector, "share/shareevent", cancellationToken).ConfigureAwait(false);
      Uri podUri = await this.CreateUrlAsync(string.Empty, cancellationToken);
      string katToken = await this.katTokenSelector(cancellationToken).ConfigureAwait(false);
      ShareUrlsResponseData urlsResponseData1;
      using (HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Post, socialUri, cancellationToken).ConfigureAwait(false))
      {
        MultipartFormDataContent content = new MultipartFormDataContent("------------------sodugcisjebdfcu99--");
        content.AddStringFormDataAttachmentIfNotNull("PodHostBaseUri", podUri.ToString());
        content.AddStringFormDataAttachmentIfNotNull("KatToken", string.Format("WRAP access_token=\"{0}\"", new object[1]
        {
          (object) katToken
        }));
        content.AddStringFormDataAttachmentIfNotNull("EventType", formData.EventType);
        content.AddStringFormDataAttachmentIfNotNull("EventId", formData.EventId);
        content.AddStringFormDataAttachmentIfNotNull("Title", formData.Title);
        content.AddStringFormDataAttachmentIfNotNull("Location", formData.Location);
        content.AddStringFormDataAttachmentIfNotNull("MetricLabel1", formData.MetricLabel1);
        content.AddStringFormDataAttachmentIfNotNull("MetricContent1", formData.MetricContent1);
        content.AddStringFormDataAttachmentIfNotNull("MetricUnit1", formData.MetricUnit1);
        content.AddStringFormDataAttachmentIfNotNull("MetricLabel2", formData.MetricLabel2);
        content.AddStringFormDataAttachmentIfNotNull("MetricContent2", formData.MetricContent2);
        content.AddStringFormDataAttachmentIfNotNull("MetricUnit2", formData.MetricUnit2);
        content.AddStringFormDataAttachmentIfNotNull("MetricLabel3", formData.MetricLabel3);
        content.AddStringFormDataAttachmentIfNotNull("MetricContent3", formData.MetricContent3);
        content.AddStringFormDataAttachmentIfNotNull("MetricUnit3", formData.MetricUnit3);
        content.AddStringFormDataAttachmentIfNotNull("MetricLabel4", formData.MetricLabel4);
        content.AddStringFormDataAttachmentIfNotNull("MetricContent4", formData.MetricContent4);
        content.AddStringFormDataAttachmentIfNotNull("MetricUnit4", formData.MetricUnit4);
        content.AddStringFormDataAttachmentIfNotNull("SubLabel1", formData.SubLabel1);
        content.AddStringFormDataAttachmentIfNotNull("SubContent1", formData.SubContent1);
        content.AddStringFormDataAttachmentIfNotNull("SubUnit1", formData.SubUnit1);
        content.AddStringFormDataAttachmentIfNotNull("TwitterDescription", formData.TwitterDescription);
        content.AddStringFormDataAttachmentIfNotNull("ActivityName", formData.ActivityName);
        if (thumbnail != null)
          content.Add(thumbnail.Content, "ActivityThumbnail", thumbnail.FileName);
        request.Content = (HttpContent) content;
        ShareUrlsResponseData urlsResponseData2 = await this.GetJsonAsync<ShareUrlsResponseData>(request, cancellationToken).ConfigureAwait(false);
        urlsResponseData1 = urlsResponseData2.IsSuccess ? urlsResponseData2 : throw new HealthCloudServerException("Uri service returned non-success status");
      }
      return urlsResponseData1;
    }

    public async Task<IList<GoalTemplate>> GetGoalTemplatesAsync(
      CancellationToken cancellationToken)
    {
      string relativeUrl = string.Format("{0}/templates", new object[1]
      {
        (object) "v1/Goals"
      });
      try
      {
        return await this.GetJsonAsync<IList<GoalTemplate>>(relativeUrl, cancellationToken, cacheArea: "Goals").ConfigureAwait(false);
      }
      catch (NotFoundException ex)
      {
        return (IList<GoalTemplate>) new List<GoalTemplate>();
      }
    }

    public async Task<UsersGoal> GetUsersGoalAsync(
      string id,
      CancellationToken cancellationToken)
    {
      string relativeUrl = string.Format("{0}/{1}/", new object[2]
      {
        (object) "v1/Goals",
        (object) id
      });
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("isExpand", "false");
      NameValueCollection parameters = nameValueCollection;
      try
      {
        return await this.GetJsonAsync<UsersGoal>(relativeUrl, cancellationToken, parameters, "Goals").ConfigureAwait(false);
      }
      catch (NotFoundException ex)
      {
        return (UsersGoal) null;
      }
    }

    public async Task<IList<UsersGoal>> GetUsersGoalsAsync(
      GoalType type,
      CancellationToken cancellationToken,
      GoalStatus status = GoalStatus.Active,
      GoalCategory category = GoalCategory.PersonalGoals,
      bool shouldExpand = false)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add(nameof (status), ((int) status).ToString());
      nameValueCollection.Add("isExpand", shouldExpand.ToString().ToLower());
      int num = (int) category;
      nameValueCollection.Add(nameof (category), num.ToString());
      num = (int) type;
      nameValueCollection.Add(nameof (type), num.ToString());
      NameValueCollection parameters = nameValueCollection;
      try
      {
        return await this.GetJsonAsync<IList<UsersGoal>>("v1/Goals", cancellationToken, parameters, "Goals").ConfigureAwait(false);
      }
      catch (NotFoundException ex)
      {
        return (IList<UsersGoal>) new List<UsersGoal>();
      }
    }

    public Task AddUsersGoalsAsync(
      ICollection<UsersGoalCreation> adds,
      CancellationToken cancellationToken)
    {
      return (Task) this.PostJsonAsync<ICollection<UsersGoalCreation>>("v1/Goals", (NameValueCollection) null, adds, cancellationToken);
    }

    public async Task UpdateUsersGoalAsync(
      ICollection<UsersGoalUpdate> updates,
      CancellationToken cancellationToken)
    {
      await this.PutJsonAsync<ICollection<UsersGoalUpdate>>("v1/Goals", (NameValueCollection) null, updates, cancellationToken).ConfigureAwait(false);
      await this.CacheService.RemoveTagsAsync("Goals");
    }

    public Task<IList<FavoriteWorkout>> GetFavoriteWorkoutsAsync(
      CancellationToken cancellationToken)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("statusFilter", "All");
      NameValueCollection parameters = nameValueCollection;
      return this.GetJsonAsync<IList<FavoriteWorkout>>("v1/Workouts/favorites", cancellationToken, parameters, "WorkoutFavorites");
    }

    public async Task FavoriteWorkoutAsync(
      string workoutPlanId,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/favorite";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      HttpResponseMessage httpResponseMessage = await this.PostJsonAsync<string>(relativeUrl, parameters, string.Empty, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("WorkoutFavorites");
    }

    public async Task UnFavoriteWorkoutAsync(
      string workoutPlanId,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/unfavorite";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      await this.DeleteAsync(relativeUrl, cancellationToken, parameters).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("WorkoutFavorites");
    }

    public async Task SubscribeWorkoutAsync(
      string workoutPlanId,
      CancellationToken cancellationToken)
    {
      await this.PutJsonAsync<string[]>("v1/Workouts/subscribe", (NameValueCollection) null, new string[1]
      {
        workoutPlanId
      }, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("WorkoutFavorites");
    }

    public async Task UnsubscribeWorkoutAsync(
      string workoutPlanId,
      CancellationToken cancellationToken)
    {
      await this.PutJsonAsync<string[]>("v1/Workouts/unsubscribe", (NameValueCollection) null, new string[1]
      {
        workoutPlanId
      }, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("WorkoutFavorites");
    }

    public Task<IList<WorkoutStatus>> GetWorkoutsInPlanAsync(
      string workoutPlanId,
      string instanceId,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/all";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      parameters.Add(nameof (instanceId), instanceId);
      return this.GetJsonAsync<IList<WorkoutStatus>>(relativeUrl, cancellationToken, parameters, "WorkoutPlan", (IEnumerable<string>) new string[1]
      {
        "Sync"
      });
    }

    public Task<WorkoutStatus> GetLastSyncedWorkoutAsync(
      CancellationToken cancellationToken)
    {
      return this.GetJsonAsync<WorkoutStatus>("v1/Workouts/lastsynced", cancellationToken);
    }

    public Task SetLastSyncedWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/lastsynced";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      parameters.Add(nameof (workoutIndex), workoutIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add(nameof (weekId), weekId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add(nameof (dayId), dayId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add(nameof (workoutPlanInstanceId), workoutPlanInstanceId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return (Task) this.PostJsonAsync<string>(relativeUrl, parameters, string.Empty, cancellationToken);
    }

    public async Task SkipWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int week,
      int day,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/skip";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      parameters.Add(nameof (workoutIndex), workoutIndex.ToString());
      parameters.Add(nameof (week), week.ToString());
      parameters.Add(nameof (day), day.ToString());
      HttpResponseMessage httpResponseMessage = await this.PostJsonAsync<string>(relativeUrl, parameters, string.Empty, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("WorkoutPlan");
    }

    public Task<IList<FeaturedWorkout>> GetFeaturedWorkoutsAsync(
      int age,
      string gender,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/featuredworkouts";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (age), age.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add(nameof (gender), gender);
      return this.GetJsonAsync<IList<FeaturedWorkout>>(relativeUrl, cancellationToken, parameters, "WorkoutGeneral");
    }

    public Task<WorkoutState> GetWorkoutStateAsync(
      CancellationToken cancellationToken)
    {
      return this.GetJsonAsync<WorkoutState>("v1/Workouts/workoutState", cancellationToken);
    }

    public Task SetNextWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "v1/Workouts/nextworkout";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      parameters.Add(nameof (workoutIndex), workoutIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add(nameof (weekId), weekId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      parameters.Add(nameof (dayId), dayId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return (Task) this.PostJsonAsync<string>(relativeUrl, parameters, string.Empty, cancellationToken);
    }

    public Task<Stream> GetWorkoutBandFileAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      CancellationToken cancellationToken)
    {
      string relativeUrl = "/v1/workouts/deviceworkout";
      NameValueCollection parameters = new NameValueCollection();
      parameters.Add(nameof (workoutPlanId), workoutPlanId);
      parameters.Add("workoutindex", workoutIndex.ToString());
      parameters.Add(nameof (weekId), weekId.ToString());
      parameters.Add(nameof (dayId), dayId.ToString());
      parameters.Add(nameof (workoutPlanInstanceId), workoutPlanInstanceId.ToString());
      return this.GetResponseStreamAsync(relativeUrl, cancellationToken, parameters);
    }

    public async Task<string> RegisterPushNotificationChannelAsync(
      string clientId,
      string channelId,
      CancellationToken cancellationToken)
    {
      return (await (await this.PostJsonAsync<string>(string.Format("{0}/{1}/?psnsi={2}", new object[3]
      {
        (object) "notification/v1",
        (object) clientId,
        (object) channelId
      }), (NameValueCollection) null, string.Empty, cancellationToken).ConfigureAwait(false)).Content.ReadAsStringAsync()).Replace("\"", string.Empty);
    }

    public async Task UpdatePushNotificationRegistrationAsync(
      string clientId,
      string channelId,
      string registrationId,
      CancellationToken cancellationToken)
    {
      await this.PutJsonAsync<string>(string.Format("{0}/{1}/{2}/?psnsi={3}", (object) "notification/v1", (object) clientId, (object) registrationId, (object) channelId), (NameValueCollection) null, string.Empty, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeletePushNotificationRegistrationAsync(
      string clientId,
      string channelId,
      string registrationId,
      CancellationToken cancellationToken)
    {
      await this.DeleteAsync(string.Format("{0}/{1}/{2}/?psnsi={3}", (object) "notification/v1", (object) clientId, (object) registrationId, (object) channelId), cancellationToken);
    }

    public Task<UpdatesResponse> GetUpdatesAsync(
      string clientId,
      CancellationToken cancellationToken)
    {
      return this.GetJsonAsync<UpdatesResponse>(string.Format("{0}/{1}", new object[2]
      {
        (object) "update/v1",
        (object) clientId
      }), cancellationToken);
    }

    public async Task DeleteUpdatesAsync(
      string clientId,
      string updateIds,
      CancellationToken cancellationToken)
    {
      await this.DeleteAsync(string.Format("{0}/{1}/?updateids={2}", new object[3]
      {
        (object) "update/v1",
        (object) clientId,
        (object) updateIds
      }), cancellationToken);
    }

    public async Task<IList<ExerciseTag>> GetExerciseTagsAsync(
      CancellationToken cancellationToken)
    {
      return await this.GetJsonAsync<IList<ExerciseTag>>("v2/exercisetagging/getalltags", cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateExerciseTagsAsync(
      IList<ExerciseTag> exerciseTags,
      CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage = await this.PostJsonAsync<IList<ExerciseTag>>("v2/exercisetagging/uploadtags", (NameValueCollection) null, exerciseTags, cancellationToken).ConfigureAwait(false);
    }

    public async Task<SocialTileStatusResponse> GetSocialTileDisplayAsync(
      string facebookToken,
      string facebookUserId,
      int timezoneOffsetMinutes,
      CancellationToken cancellationToken,
      bool forceCacheUpdate = false)
    {
      cancellationToken.ThrowIfCancellationRequested();
      NameValueCollection nameValueCollection1 = new NameValueCollection();
      NameValueCollection nameValueCollection2 = nameValueCollection1;
      Uri uri = await this.BaseUriSelector(cancellationToken);
      nameValueCollection2.Add("poolUrl", uri.ToString());
      nameValueCollection1.Add(nameof (facebookToken), facebookToken ?? string.Empty);
      nameValueCollection1.Add(nameof (facebookUserId), facebookUserId ?? string.Empty);
      nameValueCollection1.Add(nameof (timezoneOffsetMinutes), timezoneOffsetMinutes.ToString());
      NameValueCollection parameters = nameValueCollection1;
      nameValueCollection2 = (NameValueCollection) null;
      nameValueCollection1 = (NameValueCollection) null;
      SocialTileStatusResponse tileStatusResponse;
      using (HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Get, await this.CreateUrlAsync(this.socialServiceUriSelector, "api/socialApi/tileDisplay", cancellationToken, parameters).ConfigureAwait(false), cancellationToken).ConfigureAwait(false))
        tileStatusResponse = await this.GetJsonAsync<SocialTileStatusResponse>(request, cancellationToken, "Sync", forceCacheUpdate: forceCacheUpdate).ConfigureAwait(false);
      return tileStatusResponse;
    }

    public async Task SignUpForSocialEngagementAsync(
      string facebookAccessToken,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (HttpRequestMessage request1 = await this.CreateHttpRequestAsync(HttpMethod.Post, await this.CreateUrlAsync(this.socialServiceUriSelector, "api/socialApi/Bind", cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false))
      {
        HttpRequestMessage request = request1;
        SocialSignupRequest body = new SocialSignupRequest();
        body.FacebookAccessToken = facebookAccessToken;
        SocialSignupRequest socialSignupRequest = body;
        Uri uri = await this.BaseUriSelector(cancellationToken);
        socialSignupRequest.PoolUrl = uri;
        request.SetJsonContent<SocialSignupRequest>(body);
        request = (HttpRequestMessage) null;
        socialSignupRequest = (SocialSignupRequest) null;
        body = (SocialSignupRequest) null;
        HttpResponseMessage httpResponseMessage = await this.SendAsync(request1, cancellationToken);
      }
    }

    public async Task<HttpResponseMessage> UnbindForSocialEngagementAsync(
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      HttpResponseMessage httpResponseMessage;
      using (HttpRequestMessage request1 = await this.CreateHttpRequestAsync(HttpMethod.Post, await this.CreateUrlAsync(this.socialServiceUriSelector, "/api/socialApi/deleteBinding", cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false))
      {
        HttpRequestMessage request = request1;
        SocialSignupRequest body = new SocialSignupRequest();
        SocialSignupRequest socialSignupRequest = body;
        Uri uri = await this.BaseUriSelector(cancellationToken);
        socialSignupRequest.PoolUrl = uri;
        request.SetJsonContent<SocialSignupRequest>(body);
        request = (HttpRequestMessage) null;
        socialSignupRequest = (SocialSignupRequest) null;
        body = (SocialSignupRequest) null;
        httpResponseMessage = await this.SendAsync(request1, cancellationToken);
      }
      return httpResponseMessage;
    }

    public Task<Uri> GetSocialServiceUrlAsync(
      string relativeUrl,
      CancellationToken cancellationToken,
      NameValueCollection parameters = null)
    {
      if (this.socialServiceUriSelector == null)
        throw new InvalidOperationException("Social service Uri selector must not be null");
      return this.CreateUrlAsync(this.socialServiceUriSelector, string.Format("{0}/{1}", new object[2]
      {
        (object) "api/socialApi",
        (object) relativeUrl
      }), cancellationToken, parameters);
    }

    public async Task<bool> IsSocialSiteAvailableAsync(
      string urlSuffix,
      CancellationToken cancellationToken)
    {
      try
      {
        using (HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Get, await this.GetSocialSiteUrlAsync(urlSuffix, cancellationToken), cancellationToken).ConfigureAwait(false))
          return (await this.SendAsync(request, cancellationToken).ConfigureAwait(false)).IsSuccessStatusCode;
      }
      catch
      {
        return false;
      }
    }

    public Task<Uri> GetSocialSiteUrlAsync(
      string urlSuffix,
      CancellationToken cancellationToken)
    {
      if (this.socialServiceUriSelector == null)
        throw new InvalidOperationException("Social service Uri selector must not be null");
      return this.CreateUrlAsync(this.socialServiceUriSelector, "social" + (urlSuffix ?? string.Empty), cancellationToken);
    }

    public Task<Uri> GetFacebookAppLinkUrlAsync(CancellationToken cancellationToken) => this.CreateUrlAsync(this.socialServiceUriSelector, "AppLink/AppLink", cancellationToken);

    public Task<ConnectedAppsResponse> GetConnectedAppsAsync(
      CancellationToken cancellationToken)
    {
      return this.GetJsonAsync<ConnectedAppsResponse>("v2/connectedapps", cancellationToken);
    }

    public async Task<UserWeight> GetAllWeightsAsync(
      CancellationToken cancellationToken)
    {
      return await this.GetJsonAsync<UserWeight>("v2/weights/", cancellationToken, cacheArea: "Weights", cacheTags: ((IEnumerable<string>) new string[2]
      {
        "UserProfile",
        "Sync"
      })).ConfigureAwait(false);
    }

    public async Task<UserWeight> GetTopWeightsAsync(
      int count,
      CancellationToken cancellationToken)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      nameValueCollection.Add("top", count.ToString());
      NameValueCollection parameters = nameValueCollection;
      return await this.GetJsonAsync<UserWeight>("v2/weights/", cancellationToken, parameters, "Weights", (IEnumerable<string>) new string[2]
      {
        "UserProfile",
        "Sync"
      }).ConfigureAwait(false);
    }

    public async Task PostWeightsAsync(
      UserWeight userWeight,
      CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage = await this.PostJsonAsync<UserWeight>("v2/weights/", (NameValueCollection) null, userWeight, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("UserProfile");
      await this.RemoveCacheItemAsync("Weights");
    }

    public async Task DeleteWeightsAsync(
      UserWeight userWeight,
      CancellationToken cancellationToken)
    {
      await this.DeleteAsync<UserWeight>("v2/weights/", (NameValueCollection) null, userWeight, cancellationToken).ConfigureAwait(false);
      await this.RemoveCacheItemAsync("UserProfile");
      await this.RemoveCacheItemAsync("Weights");
    }

    public ISensorDataClient SensorData => this.sensorDataClient.Value;

    private ISensorDataClient CreateSensorDataClient() => (ISensorDataClient) new SensorDataClient(this.MessageHandler, this.BaseUriSelector, this.CacheService);

    public IDeviceClient Devices => this.deviceClient.Value;

    private IDeviceClient CreateDeviceClient() => (IDeviceClient) new DeviceClient(this.MessageHandler, this.BaseUriSelector, this.CacheService);

    public IGolfClient Golf => this.golfClient.Value;

    private IGolfClient CreateGolfClient() => (IGolfClient) new GolfClient(this.MessageHandler, this.BaseUriSelector, (IHealthCloudClient) this, this.CacheService);

    public IGolfCourseClient GolfCourses => this.golfCourseClient.Value;

    private IGolfCourseClient CreateGolfCourseClient() => (IGolfCourseClient) new GolfCourseClient(this.MessageHandler, this.BaseUriSelector, this.CacheService);

    public IWellnessPlanClient WellnessPlan => this.wellnessPlanClient.Value;

    public Task<Uri> GetBaseUriAsync()
    {
      if (this.BaseUriSelector == null)
        throw new InvalidOperationException("Base Uri selector must not be null");
      return this.BaseUriSelector(CancellationToken.None);
    }

    public Task<string> GetKatTokenAsync()
    {
      if (this.katTokenSelector == null)
        throw new InvalidOperationException("Kat token selector must not be null");
      return this.katTokenSelector(CancellationToken.None);
    }

    private IWellnessPlanClient CreateWellnessPlanClient() => (IWellnessPlanClient) new WellnessPlanClient(this.MessageHandler, this.BaseUriSelector, this.CacheService);

    private bool WasSyncedByTime(DateTimeOffset time) => this.CacheService != null && time < this.CacheService.LastInvalidatedSyncTime;

    private bool WasSyncedByDate(DateTimeOffset day) => this.WasSyncedByTime(day.AddDays(1.0));

    private async Task RemoveCacheItemAsync(string tag)
    {
      if (this.CacheService == null)
        return;
      await this.CacheService.RemoveTagsAsync(tag);
    }

    private static string CreateMultiValueParameter<T>(string name, IEnumerable<T> values) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}='{1}'", new object[2]
    {
      (object) name,
      (object) string.Join<T>("|", values)
    });

    private static string ToDayId(DateTimeOffset date) => date.ToString("yyyy-MM-dd");

    private static DateTimeOffset TruncateToDay(DateTimeOffset date) => date.Hour == 0 && date.Minute == 0 && date.Second == 0 && date.Millisecond == 0 ? date : new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset);

    private static string ToString(ActivityPeriod period)
    {
      if (period == ActivityPeriod.Hour)
        return "h";
      if (period == ActivityPeriod.Minute)
        return "m";
      throw new NotSupportedException("Unsupported period " + (object) period);
    }
  }
}
