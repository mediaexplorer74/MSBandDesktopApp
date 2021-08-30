// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.TelemetryEvents
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class TelemetryEvents
  {
    public const string ChooseBand = "App/Add Band/Choose Band";
    public const string NewBandDetected = "App/Add Band/New Band detected";
    public const string OobeMotionTrackingSettingChanged = "App/Add Band/Motion tracking";
    public const string NoBandsFound = "App/Add Band/No Bands found";
    public const string PairBand = "App/Add Band/Pair your Band";
    public const string FeedbackShakeSetting = "App/Feedback/Shake settings";
    public const string FeedbackSendComplete = "App/Feedback/Complete";
    public const string FeedbackEntry = "App/Feedback/Launch";
    public const string FeedbackShake = "App/Feedback/Shake dialog";
    public const string FeedbackSelection = "App/Feedback/Selection";
    public const string FeedbackSendFailure = "App/Feedback/Send failure";
    public const string FeedbackNpsPromptDismissed = "Feedback/NPS/Dismissed";
    public const string FeedbackNpsPromptSubmittedApp = "Feedback/NPS/App/Submitted";
    public const string FeedbackNpsPromptSubmittedBand = "Feedback/NPS/Band/Submitted";
    public const string FirmwareUpdate = "App/Firmware/Update";
    public const string FirmwarePrompt = "App/Firmware/Prompt update";
    public const string FirmwareUpdateDownload = "App/Firmware/Downloading update";
    public const string FirmwareUpdateRebootBand = "App/Firmware/Rebooting band";
    public const string FirmwareUpdateSendToBand = "App/Firmware/Sending to band";
    public const string FirmwareUpdateError = "App/Firmware/Something went wrong";
    public const string WebTileSync = "App/WebTile/Sync";
    public const string WebTileInstallationFailure = "App/WebTile/Installation/Failure";
    public const string WebTileInstallationSuccess = "App/WebTile/Installation/Success";
    public const string WhatsNewCard = "App/What's new/Cards";
    public const string WhatsNewLearnMore = "App/What's new/Learn more";
    public const string WhatsNew = "App/What's new/Open";
    public const string WhatsNewSessions = "App/What's new/Sessions";
    public const string HomeTileTap = "Fitness/Home Tile Tap";
    public const string WorkoutChoosePlan = "Fitness/Guided Workouts/Choose Plan";
    public const string WorkoutFavorite = "Fitness/Guided Workouts/Favorite";
    public const string WorkoutUnfavorite = "Fitness/Guided Workouts/Unfavorite";
    public const string WorkoutFilter = "Fitness/Guided Workouts/Filter Plan Results";
    public const string WorkoutMetricToggle = "Fitness/Guided Workouts/Post workout details";
    public const string WorkoutSync = "Fitness/Guided Workouts/Sync";
    public const string WorkoutSubscribe = "Fitness/Guided Workouts/Subscribe";
    public const string WorkoutUnsubscribe = "Fitness/Guided Workouts/Unsubscribe";
    public const string WorkoutVideoView = "Fitness/Guided Workouts/Watch Video";
    public const string DeleteAutoDetectedSleep = "Fitness/Sleep/AutoDetect/Delete";
    public const string ReportAutoDetectedSleepInvalid = "Fitness/Sleep/AutoDetect/Report";
    public const string EventShareButtonTap = "Fitness/Share/ButtonTap";
    public const string EventShareCharm = "Fitness/Share/Charm";
    public const string EventShareCancellation = "Fitness/Share/Cancellation";
    public const string EventShareCompletion = "Fitness/Share/Completion";
    public const string EventShareFailure = "Fitness/Share/Failure";
    public const string GolfCourseSync = "Fitness/Golf/Sync";
    public const string GolfFindCourse = "Fitness/Golf/FindCourse";
    public const string GolfBrowseCourse = "Fitness/Golf/Browse/Course";
    public const string GolfWatchIntroVideo = "Fitness/Golf/Intro Video";
    public const string LiveTileEnabledChange = "Settings/Home Tile/Enabled";
    public const string LiveTileTransparentChange = "Settings/Home Tile/Transparency";
    public const string LiveTileTilesChange = "Settings/Home Tile/Enabled Tiles Change";
    public const string PushNotification = "Notifications/Data";
    public const string ToastOptIn = "Notifications/Registration";
    public const string ToastClicked = "Notifications/Toast";
    public const string ProfileUpdated = "Profile/Updated";
    public const string TilesChange = "Settings/Manage Tiles/Change";
    public const string SaveTileSettings = "Settings/Manage Tiles/Save Settings";
    public const string CallCustomResponseChange = "Settings/Manage Tiles/Edit Custom Responses/Calls";
    public const string MessagingCustomResponseChange = "Settings/Manage Tiles/Edit Custom Responses/Messaging";
    public const string VipListChange = "Settings/Manage Tiles/Edit VIP List";
    public const string FinanceWatchListChange = "Settings/Manage Tiles/Edit Watch List";
    public const string NotificationToggle = "Settings/Manage Tiles/Notifications Toggle";
    public const string StarbucksCardSent = "Settings/Manage Tiles/Starbucks/Send card to band";
    public const string RenameBand = "Settings/Personalize/Rename your band";
    public const string ThemeChange = "Settings/Personalize/Theme";
    public const string SocialRegisterFacebook = "Social/Register/Facebook";
    public const string SocialFailure = "Social/Failure";
    public const string SocialInviteFriend = "Social/Invite friend";
    public const string SocialRemoval = "Settings/Manage Tiles/Social";
    public const string EarlyUpdateSignUp = "Utilities/Early Update/Sign Up";
    public const string EarlyUpdateProvision = "Utilities/Early Update/Provision";
    public const string EarlyUpdateDeprovision = "Utilities/Early Update/Deprovision";
    public const string AutoSync = "Utilities/Auto sync";
    public const string AppLaunch = "Utilities/Launched";
    public const string AppResume = "Utilities/Resume";
    public const string DynamicConfigUpdate = "Utilities/Dynamic Config/Update";
    public const string Sync = "Utilities/Sync";
    public const string SyncPollCloudStatus = "Utilities/Sync/Poll cloud status";
    public const string SyncSdeCheck = "Utilities/Sync/SDE check";
    public const string SyncCurrentTimeAndTimezone = "Utilities/Sync/Time";
    public const string SyncEphemeris = "Utilities/Sync/Ephemeris";
    public const string SyncTimeZoneList = "Utilities/Sync/Time zone list";
    public const string SyncCrashDump = "Utilities/Sync/Crash dump";
    public const string SyncInstrumentation = "Utilities/Sync/Instrumentation";
    public const string SyncUserProfile = "Utilities/Sync/User profile";
    public const string SyncWebTiles = "Utilities/Sync/Web tiles";
    public const string SyncBandTiles = "Utilities/Sync/Tiles";
    public const string SyncPhoneSensors = "Utilities/Sync/Phone sensors";
    public const string SyncGetLogsFromBand = "Utilities/Sync/Get log from band";
    public const string SyncSendLogsToCloud = "Utilities/Sync/Log send to cloud";
    public const string SyncError = "Utilities/Sync/Sync Error";
    public const string AppUpgradeSuccess = "Diagnostics/App Upgrade/Success";
    public const string AppUpgradeFailure = "Diagnostics/App Upgrade/Failure";
    public const string MessageboxAutoCancel = "Diagnostics/Messagebox/AutoCancel";
    public const string DiagnosticsHeadlessException = "Diagnostics/Exceptions/HeadlessException";
    public const string DeepLink = "App/Deeplink";
    public const string DeepLinkError = "App/Deeplink/Error";
    public const string PhoneMotionTrackingSettingChanged = "Phone/Enable motion tracking";
    public const string HomePageLoad = "App/Home";
    public const string AddWeight = "Weight/Add";
    public const string AddWeightDialogOpened = "Weight/Add/Add dialog opened";
    public const string DeleteWeight = "Weight/Delete";
    public const string RunDataPointsChange = "Fitness/Settings/Band/Manage Tiles/Edit Run Data Points";
    public const string BikeDataPointsChange = "Fitness/Settings/Band/Manage Tiles/Edit Bike Data Points";
  }
}
