// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.HealthAppConstants
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services.ToastNotification;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class HealthAppConstants
  {
    public static string GetWebTileGalleryUrl()
    {
      string str = "Release_External";
      bool flag = true;
      if (str == "Main" || str == "Stabilization")
        return "https://go.microsoft.com/fwlink/?LinkID=619404";
      if (str == "Release")
        return "https://go.microsoft.com/fwlink/?LinkID=619405";
      return flag ? "https://go.microsoft.com/fwlink/?LinkID=619406" : (string) null;
    }

    public static class ApplicationInsights
    {
      private const string WindowsPhoneEngineering = "c5ae9e44-d96c-449e-85a1-7654d65a447b";
      private const string WindowsPhoneDogfooding = "6a89c947-5d3d-4032-ad1b-104dcd3a1f4a";
      private const string WindowsPhonePublic = "6b556c68-0336-40af-ae56-cd8ef6081b80";
      private const string AndroidEngineering = "a72d45ae-fe44-410c-b4dc-ede1d0ac9938";
      private const string AndroidDogfooding = "1f95adcc-2c18-4c52-bb50-9ccea026c2f2";
      private const string AndroidPublic = "85a5b488-0f6a-4e7e-a68b-bf875eee81bf";

      public static string GetWindowsPhoneInstrumentationKey(bool isPublicRelease) => HealthAppConstants.ApplicationInsights.GetInstrumentationKey(isPublicRelease, "c5ae9e44-d96c-449e-85a1-7654d65a447b", "6a89c947-5d3d-4032-ad1b-104dcd3a1f4a", "6b556c68-0336-40af-ae56-cd8ef6081b80");

      public static string GetAndroidInstrumentationKey(bool isPublicRelease) => HealthAppConstants.ApplicationInsights.GetInstrumentationKey(isPublicRelease, "a72d45ae-fe44-410c-b4dc-ede1d0ac9938", "1f95adcc-2c18-4c52-bb50-9ccea026c2f2", "85a5b488-0f6a-4e7e-a68b-bf875eee81bf");

      private static string GetInstrumentationKey(
        bool isPublicRelease,
        string engineeringKey,
        string dogfoodingKey,
        string publicKey)
      {
        string str = "Release_External";
        if (str == "Main" || str == "Stabilization")
          return engineeringKey;
        if (str == "Release")
          return dogfoodingKey;
        if (isPublicRelease)
          return publicKey;
        DebugUtilities.Fail("Unrecognized branch name {0}, could not determine the Application Insights instrumentation key to use. You should update the code to make sure it is still valid.", (object) str);
        return (string) null;
      }
    }

    public static class ErrorHandling
    {
      public const int WebCallRetryUpperBound = 3;
    }

    public static class HockeyApp
    {
      private const string WindowsPhoneEngineering = "c5ae9e44d96c449e85a17654d65a447b";
      private const string WindowsPhoneDogfooding = "6a89c9475d3d4032ad1b104dcd3a1f4a";
      private const string WindowsPhonePublic = "6b556c68033640afae56cd8ef6081b80";

      public static string GetWindowsPhoneInstrumentationKey(bool isPublicRelease) => HealthAppConstants.HockeyApp.GetInstrumentationKey(isPublicRelease, "c5ae9e44d96c449e85a17654d65a447b", "6a89c9475d3d4032ad1b104dcd3a1f4a", "6b556c68033640afae56cd8ef6081b80");

      private static string GetInstrumentationKey(
        bool isPublicRelease,
        string engineeringKey,
        string dogfoodingKey,
        string publicKey)
      {
        return !isPublicRelease ? engineeringKey : publicKey;
      }
    }

    public static class Schemes
    {
      public const string Test = "mshealth-test";
      public const string WebTile = "mshealth-webtile";
      public const string LaunchTileScheme = "tile";
      public const string LaunchModalScheme = "modal";
      public const string MSHealth = "mshealth";
    }

    public static class ModalDialogs
    {
      public const string GoalRecommendation = "GoalRecommendation";
      public const string SocialChallenge = "SocialChallenge";

      public enum GoalRecommendationTypes
      {
        SleepPlanDuration,
      }

      public enum SocialChallengeTypes
      {
        ChallengeInvite,
      }
    }

    public static class ToastTilesMap
    {
      public const string CoachingTileName = "CoachingTileViewModel";
      public const string CoachingTilePivotName = "CoachingComingUpViewModel";
      public const string SocialTileName = "SocialTileViewModel";
      public const string SocialTilePivotName = "SocialLandingViewModel";
    }

    public static class ToastTiles
    {
      public const string ActionPlanTileName = "action_plan";
      public const string SocialChallenge = "socialchallenge";
    }

    public static class ToastPivots
    {
      public const string ActionPlanPivotName = "coming_up";
    }

    public static class Android
    {
      public const string HockeyAppId = "1f95adcc2c184c52bb509ccea026c2f2";
      public const string GcmProjectNumber = "438585859434";
    }

    public static class Urls
    {
      public const string Help = "https://go.microsoft.com/fwlink/?LinkID=506757";
      public const string TermsOfUse = "https://go.microsoft.com/fwlink/?LinkId=507589";
      public const string StarbucksLogin = "https://go.microsoft.com/fwlink/?LinkID=513161";
      public const string StarbucksLoginGb = "https://go.microsoft.com/fwlink/?LinkId=615214";
      public const string HelpAndSupport = "https://go.microsoft.com/fwlink/?LinkID=506763";
      public const string SuggestAnIdea = "https://go.microsoft.com/fwlink/?LinkID=690323";
      public const string GalleryUrlInt = "https://go.microsoft.com/fwlink/?LinkID=619404";
      public const string GalleryUrlStg = "https://go.microsoft.com/fwlink/?LinkID=619405";
      public const string GalleryUrlProd = "https://go.microsoft.com/fwlink/?LinkID=619406";
      public const string Troubleshoot = "https://go.microsoft.com/fwlink/?LinkID=624677";
      public const string GolfRequestCourse = "https://go.microsoft.com/fwlink/?LinkId=625095";
      public const string TileFirstTimeUseLearnMore = "https://go.microsoft.com/fwlink/?LinkID=532705";
      public const string ShareLearnMore = "https://go.microsoft.com/fwlink/?LinkId=718055";
      public const string BandLearnMore = "https://go.microsoft.com/fwlink/?LinkID=532705";
      public const string WorkoutSharing = "https://go.microsoft.com/fwlink/?LinkId=717509";
      public const string ActivityReminders = "https://go.microsoft.com/fwlink/?LinkID=532705 ";
      public const string GpsSaver = "https://go.microsoft.com/fwlink/?LinkID=532705";
      public const string WindowsPrivacyStatement = "https://go.microsoft.com/fwlink/?LinkId=521839";
      public const string WindowsThirdPartyNotices = "https://go.microsoft.com/fwlink/?LinkID=512824";
      public const string WindowsRateAndReview = "https://go.microsoft.com/fwlink/?LinkId=507595";
      public const string AndroidPrivacyStatement = "https://go.microsoft.com/fwlink/?LinkId=507593";
      public const string AndroidRateAndReview = "https://go.microsoft.com/fwlink/?LinkId=507597";
      public const string AndroidThirdPartyNotices = "https://go.microsoft.com/fwlink/?LinkId=512822";
      public const string OAuthLogoutUrl = "https://login.live.com/oauth20_logout.srf";
      public const string OAuthLoginUrlFormat = "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=service::{1}::MBI_SSL&response_type=token&redirect_uri=https://login.live.com/oauth20_desktop.srf";
    }

    public static class Band
    {
      public static class TileIds
      {
        public const string Golf = "fb9d005a-c3da-49d4-8e7b-c6f674fc4710";
        public const string Run = "65bd93db-4293-46af-9a28-bdd6513b4677";
        public const string Bike = "96430fcb-0060-41cb-9de2-e00cac97f85d";
        public const string Sleep = "23e7bc94-f90d-44e0-843f-250910fdf74e";
        public const string Exercise = "a708f02a-03cd-4da0-bb33-be904e6a2924";
        public const string AlarmStopwatch = "d36a92ea-3e85-4aed-a726-2898a6f2769b";
        public const string UV = "59976cf5-15c8-4799-9e31-f34c765a6bd1";
        public const string Weather = "69a39b4e-084b-4b53-9a1b-581826df9e36";
        public const string Finance = "5992928a-bd79-4bb5-9678-f08246d03e68";
        public const string Starbucks = "64a29f65-70bb-4f32-99a2-0f250a05d427";
        public const string GuidedWorkouts = "0281c878-afa8-40ff-acfd-bca06c5c4922";
        public const string Hike = "e9e376af-fa3d-486d-8351-959cc20f4d8f";
      }

      public static class TilePageIds
      {
        public const string WeatherLastUpdatedPage = "76150e97-94cd-4d55-847f-ba7e9fc408e6";
        public const string FinanceLastUpdatedPage = "f375f75e-05b7-42e0-a5ff-9a5c409e9dd9";
        public const string StarbucksLastUpdatedPage = "5fe2048d-7336-684f-901d-dda85118c509";
        public static readonly IList<string> WeatherPages = (IList<string>) new List<string>()
        {
          "b92dcd02-21f2-4208-970a-4bfc9861947e",
          "d0e2bf8a-a2e1-4617-8bba-d60d7b400c34",
          "c05f126f-f5fb-4d9a-88b6-e21c07b475d3",
          "e688378e-ac25-4264-ba53-b3fd9c67eaad",
          "d7b88c9f-5e41-4097-8520-d52c1b2a3b6d",
          "b646345d-1ea4-4872-b2df-b9a610a1b2f6",
          "7fd73cec-edbc-4c3a-b9b4-acde22951131"
        };
        public static readonly IList<string> FinancePages = (IList<string>) new List<string>()
        {
          "efbf331a-fa87-4401-887f-6554d83d6f6f",
          "c8117426-059a-47a7-b7da-dfb76600cd29",
          "9193bbfa-752b-446f-a96b-c841bde04ffd",
          "9126a23f-44f7-44ca-9262-2df577692c16",
          "4f326ae2-75d3-41a7-b41a-8ab11edea571",
          "313ba941-fa5b-4a22-ab35-339396c19973",
          "196021c5-33bb-4aa3-88a7-70eefb5773b1"
        };
      }

      public static class Cargo
      {
        public const int MeTileHeight = 102;
        public const int MeTileWidth = 310;
      }

      public static class Envoy
      {
        public const int MeTileHeight = 128;
        public const int MeTileWidth = 310;
      }
    }

    public static class Goals
    {
      public const int MaxCalories = 10000;
      public const int MinCalories = 1000;
      public const int MinCalorieDelta = 100;
      public const int MaxSteps = 25000;
      public const int MinSteps = 1000;
      public const int MinStepDelta = 500;
      public const int DefaultMaxStepsGoal = 5000;
      public const int DefaultMaxCaloriesGoal = 5000;
    }

    public static class Diagnostics
    {
      public const string FeedbackEmail = "cargodfsupport@microsoft.com";
      public const string DiagnosticsArchiveOtherFolder = "other";
      public const string ApplicationLogName = "App";
      public const string BackgroundLogName = "Background";
      public const string JsonLogFileExtension = ".jsonl";
    }

    public static class FileSystem
    {
      public const string ApplicationFolderName = "Microsoft Health";
      public const string SharingSubfolderName = "Sharing";
      public const string ScreenshotFileName = "Screenshot.png";
      public const string ChartScreenshotFileName = "ChartScreenshot.png";
      public const string SharePngImageFileName = "Share.png";
      public const string ShareJpegImageFileName = "Share.jpg";
      public const string EnhancedShareImageFileName = "EnhancedShare.png";
      public const string NotificationCenterConfigFileName = "NotificationCenterConfiguration.json";
    }

    public static class ToastNotificationIds
    {
      public static readonly ToastNotificationId FirmwareUpdateRequired = new ToastNotificationId("FirmwareUpdate", 9124);
      public static readonly ToastNotificationId CoachingPlanPushNotificationId = new ToastNotificationId("PushNotification", 8734);
    }

    public static class Pedometer
    {
      public const string OSPedometer = "WindowsOSPedometer";
      public const string SensorCore = "WindowsSensorCore";
      public const string Android = "AndroidOSPedometer";
    }

    public static class UploadMetadata
    {
      public const string HostOS = "HostOS";
      public const string HostOSVersion = "HostOSVersion";
      public const string HostAppVersion = "HostAppVersion";
      public const string DeviceVersion = "DeviceVersion";
      public const string PedometerSource = "PedometerSource";
    }
  }
}
