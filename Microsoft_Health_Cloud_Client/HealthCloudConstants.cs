// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HealthCloudConstants
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

namespace Microsoft.Health.Cloud.Client
{
  internal static class HealthCloudConstants
  {
    public const string AuthorizationHeader = "Authorization";
    public const string DayIdFormat = "yyyy-MM-dd";
    public const string TimeWithTimeZoneFormat = "yyyy-MM-ddTHH:mm:sszzz";
    public const string JsonMediaType = "application/json";
    public const string EnableEventSharingUrlRelativePath = "share/shareevent";
    public const string SocialTileDisplayUrlRelativePath = "api/socialApi/tileDisplay";
    public const string SocialSignupUrlRelativePath = "api/socialApi/Bind";
    public const string SocialUnbindUrlRelativePath = "/api/socialApi/deleteBinding";
    public const string SocialSiteRelativeUrlPrefix = "social";
    public const string SocialServiceRelativeUrlPrefix = "api/socialApi";
    public const string FacebookApplinkRelativeUrlPrefix = "AppLink/AppLink";

    public static class KdsRestEndpoints
    {
      public static class Users
      {
        public const string Me = "users/me";
        public const string User = "api/v1/user";
        public const string UserMsa = "api/v1/user/live";
      }
    }

    public static class RestEndpoints
    {
      public static class V1
      {
        public const string Notification = "notification/v1";
        public const string Update = "update/v1";
        public const string UserWellnessSchedule = "v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessSchedule";
        public const string UserWellnessPlans = "v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessPlans";
        public const string UserWellnessGoals = "v1/8739fbb7-6bcf-4bb3-a217-c07b0f54ee02/UserWellnessPlans/{0}/goals/{1}";
        public const string UserWellnessPlansFormat = "{0}/{1}/Progress";
      }

      public static class V2
      {
        public const string SensorData = "v2/SensorData";
        public const string ConnectedApps = "v2/connectedapps";
        public const string UploadSensorPayload = "v2/multidevice/UploadSensorPayload";
        public const string Devices = "v2/Devices";
        public const string GolfCourseBase = "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0";
        public const string GolfCourse = "v2/5d784f21-6bd2-4d9d-b9a1-8af7f85839c0/course";
        public const string Weights = "v2/weights/";
      }

      public static class Users
      {
        public const string GetUser = "api/users/GetUser";
        public const string UpdateUser = "api/users/UpdateUser";
      }
    }

    public static class ODataEndpoints
    {
      public static class V1
      {
        public const string Events = "v1/Events";
        public const string EventSummary = "v1/eventsummary";
        public const string RaisedInsights = "v1/RaisedInsights";
        public const string Goals = "v1/Goals";

        public static class Workouts
        {
          public const string Skip = "v1/Workouts/skip";
          public const string Favorites = "v1/Workouts/favorites";
          public const string Favorite = "v1/Workouts/favorite";
          public const string UnFavorite = "v1/Workouts/unfavorite";
          public const string Subscribe = "v1/Workouts/subscribe";
          public const string UnSubscribe = "v1/Workouts/unsubscribe";
          public const string All = "v1/Workouts/all";
          public const string LastSynced = "v1/Workouts/lastsynced";
          public const string WorkoutState = "v1/Workouts/workoutState";
          public const string NextWorkout = "v1/Workouts/nextworkout";
          public const string Featured = "v1/Workouts/featuredworkouts";
          public const string WorkoutDeviceFile = "/v1/workouts/deviceworkout";
        }
      }

      public static class V2
      {
        public const string UserHourlySummary = "v2/UserHourlySummary";
        public const string UserDailySummary = "v2/UserDailySummary";

        public static class Exercise
        {
          public const string GetExerciseTagging = "v2/exercisetagging/getalltags";
          public const string UpdateExerciseTagging = "v2/exercisetagging/uploadtags";
        }
      }
    }
  }
}
