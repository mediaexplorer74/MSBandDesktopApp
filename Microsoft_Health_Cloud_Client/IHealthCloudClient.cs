// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.IHealthCloudClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Configuration;
using Microsoft.Health.Cloud.Client.Events.Golf;
using Microsoft.Health.Cloud.Client.Events.Golf.Courses;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Models;
using Microsoft.Health.Cloud.Client.Models.Social;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public interface IHealthCloudClient
  {
    IHttpTracingConfiguration Configuration { get; }

    Task AddUsersGoalsAsync(
      ICollection<UsersGoalCreation> adds,
      CancellationToken cancellationToken);

    Task DeleteEventAsync(
      string eventId,
      EventType eventType,
      CancellationToken cancellationToken);

    Task FavoriteWorkoutAsync(string workoutPlanId, CancellationToken cancellationToken);

    Task<IList<UsersGoal>> GetBestGoalsAsync(
      CancellationToken cancellationToken,
      GoalType goalType = GoalType.Unknown,
      bool isExpanded = false);

    Task<IList<UserDailySummary>> GetDailySummariesAsync(
      DateTimeOffset start,
      DateTimeOffset end,
      TimeSpan offset,
      CancellationToken cancellationToken,
      string deviceId = null);

    Task<T> GetEventAsync<T>(
      string eventId,
      CancellationToken cancellationToken,
      bool expandSequences = false,
      bool expandInfo = false,
      bool expandEvidences = false,
      bool useCache = true)
      where T : UserEvent;

    Task<IList<T>> GetEventsAsync<T>(
      EventType eventType,
      int top,
      DateTimeOffset? beforeDate,
      int splitDistanceCm,
      CancellationToken cancellationToken)
      where T : UserEvent;

    Task<IList<T>> GetEventsAsync<T>(
      DateTimeOffset startTime,
      DateTimeOffset endTime,
      EventType eventType,
      CancellationToken cancellationToken)
      where T : UserEvent;

    Task<IList<T>> GetEventsBetweenAsync<T>(
      DateTimeOffset startDayId,
      DateTimeOffset endDayId,
      EventType eventType,
      CancellationToken cancellationToken)
      where T : UserEvent;

    Task<IList<T>> GetEventsOnAsync<T>(
      EventType eventType,
      DateTimeOffset dayId,
      CancellationToken cancellationToken)
      where T : UserEvent;

    Task<ExerciseEvent> GetExerciseEventDetailsAsync(
      string eventId,
      CancellationToken cancellationToken);

    Task<T> GetRouteBasedExerciseEventDetailsAsync<T>(
      string eventId,
      int splitDistanceCm,
      CancellationToken cancellationToken)
      where T : RouteBasedExerciseEvent;

    Task<IList<FavoriteWorkout>> GetFavoriteWorkoutsAsync(
      CancellationToken cancellationToken);

    Task<IList<FeaturedWorkout>> GetFeaturedWorkoutsAsync(
      int age,
      string gender,
      CancellationToken cancellationToken);

    Task<IList<GoalTemplate>> GetGoalTemplatesAsync(
      CancellationToken cancellationToken);

    Task<WorkoutStatus> GetLastSyncedWorkoutAsync(
      CancellationToken cancellationToken);

    Task<IList<RaisedInsight>> GetRaisedInsightsAsync(
      CancellationToken cancellationToken,
      IEnumerable<InsightDataUsedPivot> dataUsed = null,
      IEnumerable<InsightTimespanPivot> timespan = null,
      IEnumerable<InsightScopePivot> scope = null,
      IEnumerable<InsightCategoryPivot> category = null);

    Task<SleepEvent> GetSleepEventDetailsAsync(
      string eventId,
      CancellationToken cancellationToken);

    Task<IList<T>> GetTopEventsAsync<T>(
      int count,
      EventType eventType,
      CancellationToken cancellationToken,
      bool expandSequences = false,
      bool expandInfo = false,
      bool expandEvidences = false,
      bool useCache = true)
      where T : UserEvent;

    Task<IList<UserActivity>> GetUserActivitiesAsync(
      DateTimeOffset start,
      DateTimeOffset end,
      ActivityPeriod activityPeriod,
      CancellationToken cancellationToken,
      string deviceId = null);

    Task<UserProfile> GetUserProfileAsync(CancellationToken cancellationToken);

    Task<UsersGoal> GetUsersGoalAsync(string id, CancellationToken cancellationToken);

    Task<IList<UsersGoal>> GetUsersGoalsAsync(
      GoalType type,
      CancellationToken cancellationToken,
      GoalStatus status = GoalStatus.Active,
      GoalCategory category = GoalCategory.PersonalGoals,
      bool shouldExpand = false);

    Task<Stream> GetWorkoutBandFileAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      CancellationToken cancellationToken);

    Task<IList<WorkoutStatus>> GetWorkoutsInPlanAsync(
      string workoutPlanId,
      string instanceId,
      CancellationToken cancellationToken);

    Task<WorkoutState> GetWorkoutStateAsync(CancellationToken cancellationToken);

    Task SetNextWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      CancellationToken cancellationToken);

    Task PatchEventAsync(
      string eventId,
      string name,
      EventType eventType,
      CancellationToken cancellationToken);

    Task SetLastSyncedWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      CancellationToken cancellationToken);

    Task SkipWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int week,
      int day,
      CancellationToken cancellationToken);

    Task SubscribeWorkoutAsync(string workoutPlanId, CancellationToken cancellationToken);

    Task UnFavoriteWorkoutAsync(string workoutPlanId, CancellationToken cancellationToken);

    Task UnsubscribeWorkoutAsync(string workoutPlanId, CancellationToken cancellationToken);

    Task UpdateUserProfileAsync(UserProfile userProfile, CancellationToken cancellationToken);

    Task UpdateUsersGoalAsync(
      ICollection<UsersGoalUpdate> updates,
      CancellationToken cancellationToken);

    ISensorDataClient SensorData { get; }

    IDeviceClient Devices { get; }

    IGolfClient Golf { get; }

    IGolfCourseClient GolfCourses { get; }

    Task<string> RegisterPushNotificationChannelAsync(
      string clientId,
      string channelId,
      CancellationToken cancellationToken);

    Task UpdatePushNotificationRegistrationAsync(
      string clientId,
      string channelId,
      string registrationId,
      CancellationToken cancellationToken);

    Task DeletePushNotificationRegistrationAsync(
      string clientId,
      string channelId,
      string registrationId,
      CancellationToken cancellationToken);

    Task<UpdatesResponse> GetUpdatesAsync(
      string clientId,
      CancellationToken cancellationToken);

    Task DeleteUpdatesAsync(
      string clientId,
      string updateIds,
      CancellationToken cancellationToken);

    IWellnessPlanClient WellnessPlan { get; }

    Task<ConnectedAppsResponse> GetConnectedAppsAsync(
      CancellationToken cancellationToken);

    Task AcknowledgeInsightAsync(string insightId, CancellationToken token);

    Task<IList<ExerciseTag>> GetExerciseTagsAsync(
      CancellationToken cancellationToken);

    Task UpdateExerciseTagsAsync(
      IList<ExerciseTag> exerciseTags,
      CancellationToken cancellationToken);

    Task<UserWeight> GetAllWeightsAsync(CancellationToken cancellationToken);

    Task<UserWeight> GetTopWeightsAsync(
      int count,
      CancellationToken cancellationToken);

    Task PostWeightsAsync(UserWeight userWeight, CancellationToken cancellationToken);

    Task DeleteWeightsAsync(UserWeight userWeight, CancellationToken cancellationToken);

    Task<ShareUrlsResponseData> EnableEventSharingAsync(
      ShareUrlsRequestFormData formData,
      IMultiPartAttachment thumbnail,
      CancellationToken cancellationToken);

    Task<SocialTileStatusResponse> GetSocialTileDisplayAsync(
      string facebookToken,
      string facebookUserId,
      int timezoneOffsetMinutes,
      CancellationToken cancellationToken,
      bool forceCacheUpdate = false);

    Task SignUpForSocialEngagementAsync(
      string facebookAccessToken,
      CancellationToken cancellationToken);

    Task<HttpResponseMessage> UnbindForSocialEngagementAsync(
      CancellationToken cancellationToken);

    Task<Uri> GetSocialSiteUrlAsync(string urlSuffix, CancellationToken cancellationToken);

    Task<Uri> GetSocialServiceUrlAsync(
      string relativeUrl,
      CancellationToken cancellationToken,
      NameValueCollection parameters = null);

    Task<bool> IsSocialSiteAvailableAsync(string urlSuffix, CancellationToken cancellationToken);

    Task<Uri> GetFacebookAppLinkUrlAsync(CancellationToken cancellationToken);

    Task<Uri> GetBaseUriAsync();

    Task<string> GetKatTokenAsync();
  }
}
