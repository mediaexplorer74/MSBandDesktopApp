// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.ApplicationTelemetry
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Sharing;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class ApplicationTelemetry
  {
    private static readonly IReadOnlyList<Tuple<Tuple<int, int>, string>> AgeGroups = (IReadOnlyList<Tuple<Tuple<int, int>, string>>) new List<Tuple<Tuple<int, int>, string>>()
    {
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(17, 24), "18-24"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(25, 29), "25-29"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(30, 34), "30-34"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(35, 39), "35-39"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(40, 44), "40-44"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(45, 49), "45-49"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(50, 54), "50-54"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(55, 59), "55-59"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(60, 64), "60-64"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(65, 69), "65-69"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(70, 74), "70-74"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(75, 79), "75-79"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(80, 84), "80-84"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(85, 89), "85-89"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(90, 94), "90-94"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(95, 99), "95-99"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(100, 104), "100-104"),
      Tuple.Create<Tuple<int, int>, string>(Tuple.Create<int, int>(105, 109), "105-109")
    };
    private static bool foregroundSyncInProgress;

    public static void LogOobeComplete(
      bool isMale,
      DateTime birthDate,
      bool isBandPaired,
      OobePhoneMotionTrackingData motionTrackingData)
    {
      if (motionTrackingData == null)
        return;
      int num = motionTrackingData.PhoneMotionTrackingEnabled ? 1 : 0;
    }

    public static void LogFirmwareUpdateError(string button) => Telemetry.LogEvent("App/Firmware/Something went wrong", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Clicked",
        button
      }
    });

    public static void LogFirmwarePrompt() => Telemetry.LogEvent("App/Firmware/Prompt update");

    public static void LogAddBandChoice(BandClass bandType, bool? continuedThroughUwpPrompt = null) => ApplicationTelemetry.LogAddBandChoice("Band " + (object) (bandType == BandClass.Cargo ? 1 : 2), continuedThroughUwpPrompt);

    public static void LogAddBandChoice(string choice, bool? continuedThroughUwpPrompt = null)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "Type",
          choice
        }
      };
      if (continuedThroughUwpPrompt.HasValue)
        dictionary.Add("UWP", continuedThroughUwpPrompt.Value ? "Continue" : "Cancel");
      Telemetry.LogEvent("App/Add Band/Choose Band", (IDictionary<string, string>) dictionary);
    }

    public static void LogNoBandsFoundChoice(string actionTaken) => Telemetry.LogEvent("App/Add Band/No Bands found", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Action",
        actionTaken
      }
    });

    public static void LogPairBandPageChoice(string actionTaken) => Telemetry.LogEvent("App/Add Band/Pair your Band", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Action",
        actionTaken
      }
    });

    public static void LogMotionTrackingChoice(string actionChosen) => Telemetry.LogEvent("App/Add Band/Motion tracking", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Action",
        actionChosen
      }
    });

    public static void LogPhoneMotionTrackingSettingChanged(bool isEnabled) => Telemetry.LogEvent("Phone/Enable motion tracking", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Tracking state",
        isEnabled ? "Enabled" : "Disabled"
      }
    });

    public static void LogBandRename(bool inOobe) => Telemetry.LogEvent("Settings/Personalize/Rename your band", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "In OOBE",
        inOobe.ToString()
      }
    });

    public static void LogBandThemeChange(string wallpaperName, string colorName, bool inOobe) => Telemetry.LogEvent("Settings/Personalize/Theme", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "In OOBE",
        inOobe.ToString()
      },
      {
        "Theme Wallpaper Name",
        wallpaperName
      },
      {
        "Theme Color Name",
        colorName
      }
    });

    public static void LogProfileUpdated() => Telemetry.LogEvent("Profile/Updated");

    public static void LogTilesChanged(
      int activeTiles,
      bool orderChanged,
      IEnumerable<string> tileInfo)
    {
      Dictionary<string, string> dictionary = tileInfo.ToDictionary<string, string, string>((Func<string, string>) (tile => tile), (Func<string, string>) (_ => true.ToString()));
      dictionary.Add("Changed Order", orderChanged.ToString());
      Telemetry.LogEvent("Settings/Manage Tiles/Change", (IDictionary<string, string>) dictionary, (IDictionary<string, double>) new Dictionary<string, double>()
      {
        {
          "Active Tiles",
          (double) activeTiles
        }
      });
    }

    public static void LogNotificationsToggle() => Telemetry.LogEvent("Settings/Manage Tiles/Notifications Toggle");

    public static ITimedTelemetryEvent TimeSaveTileSettings() => Telemetry.StartTimedEvent("Settings/Manage Tiles/Save Settings");

    public static void LogCallCustomResponseChange() => Telemetry.LogEvent("Settings/Manage Tiles/Edit Custom Responses/Calls");

    public static void LogMessagingCustomResponseChange() => Telemetry.LogEvent("Settings/Manage Tiles/Edit Custom Responses/Messaging");

    public static void LogWatchListChange(int numberOfSymbols) => Telemetry.LogEvent("Settings/Manage Tiles/Edit Watch List", metrics: ((IDictionary<string, double>) new Dictionary<string, double>()
    {
      {
        "Number of Symbols",
        (double) numberOfSymbols
      }
    }));

    public static void LogVipListChange(int numberOfVips) => Telemetry.LogEvent("Settings/Manage Tiles/Edit VIP List", metrics: ((IDictionary<string, double>) new Dictionary<string, double>()
    {
      {
        "Number of VIPs",
        (double) numberOfVips
      }
    }));

    public static void LogRunDataPointsChange() => Telemetry.LogEvent("Fitness/Settings/Band/Manage Tiles/Edit Run Data Points");

    public static void LogBikeDataPointsChange() => Telemetry.LogEvent("Fitness/Settings/Band/Manage Tiles/Edit Bike Data Points");

    public static void LogStarbucksCardAdd() => Telemetry.LogEvent("Settings/Manage Tiles/Starbucks/Send card to band");

    public static void LogShareButtonTap(
      EventType eventType,
      ShareType shareType,
      ShareButtonType shareButtonType)
    {
      Telemetry.LogEvent("Fitness/Share/ButtonTap", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Event Type",
          ApplicationTelemetry.ToString(eventType)
        },
        {
          "Share Type",
          shareType.ToString()
        },
        {
          "Button Type",
          ApplicationTelemetry.ToString(shareButtonType)
        }
      });
    }

    public static void LogShareCharmSelection(
      EventType eventType,
      ShareType shareType,
      string action)
    {
      Telemetry.LogEvent("Fitness/Share/Charm", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Event Type",
          ApplicationTelemetry.ToString(eventType)
        },
        {
          "Share Type",
          shareType.ToString()
        },
        {
          "Action",
          action
        }
      });
    }

    public static void LogShareCancellation(
      EventType eventType,
      ShareType shareType,
      ShareButtonType shareButtonType,
      ShareCancellationType? cancellationType = null,
      string action = null)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "Event Type",
          ApplicationTelemetry.ToString(eventType)
        },
        {
          "Share Type",
          shareType.ToString()
        },
        {
          "Button Type",
          ApplicationTelemetry.ToString(shareButtonType)
        }
      };
      if (cancellationType.HasValue)
        dictionary.Add("Cancellation Type", ApplicationTelemetry.ToString(cancellationType.Value));
      if (action != null)
        dictionary.Add("Action", action);
      Telemetry.LogEvent("Fitness/Share/Cancellation", (IDictionary<string, string>) dictionary);
    }

    public static void LogShareCompletion(
      EventType eventType,
      ShareType shareType,
      ShareButtonType shareButtonType,
      string action = null)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "Event Type",
          ApplicationTelemetry.ToString(eventType)
        },
        {
          "Share Type",
          shareType.ToString()
        },
        {
          "Button Type",
          ApplicationTelemetry.ToString(shareButtonType)
        }
      };
      if (action != null)
        dictionary.Add("Action", action);
      Telemetry.LogEvent("Fitness/Share/Completion", (IDictionary<string, string>) dictionary);
    }

    public static void LogShareFailure(
      EventType eventType,
      ShareType shareType,
      ShareButtonType shareButtonType,
      ShareFailureType failureType,
      string failureDetail,
      string action = null)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "Event Type",
          ApplicationTelemetry.ToString(eventType)
        },
        {
          "Share Type",
          shareType.ToString()
        },
        {
          "Button Type",
          ApplicationTelemetry.ToString(shareButtonType)
        },
        {
          "Failure Type",
          failureType.ToString()
        },
        {
          "Failure Detail",
          failureDetail
        }
      };
      if (action != null)
        dictionary.Add("Action", action);
      Telemetry.LogEvent("Fitness/Share/Failure", (IDictionary<string, string>) dictionary);
    }

    private static string ToString(EventType eventType)
    {
      switch (eventType)
      {
        case EventType.Running:
          return "Run";
        case EventType.Sleeping:
          return "Sleep";
        case EventType.Workout:
          return "Workout";
        case EventType.GuidedWorkout:
          return "Guided Workout";
        case EventType.Biking:
          return "Bike";
        case EventType.Hike:
          return "Hike";
        case EventType.Golf:
          return "Golf";
        default:
          DebugUtilities.Fail("Unrecognized event type: {0}", (object) eventType);
          return eventType.ToString();
      }
    }

    private static string ToString(ShareCancellationType cancellationType)
    {
      switch (cancellationType)
      {
        case ShareCancellationType.IntermediateCancel:
          return "Intermediate Cancel";
        case ShareCancellationType.IntermediateBack:
          return "Intermediate Back";
        case ShareCancellationType.CharmBack:
          return "Charm Back";
        case ShareCancellationType.ShareCancel:
          return "Share Cancel";
        case ShareCancellationType.ShareBack:
          return "Share Back";
        default:
          DebugUtilities.Fail("Unrecognized cancellation type: {0}", (object) cancellationType);
          return cancellationType.ToString();
      }
    }

    private static string ToString(ShareButtonType shareButtonType)
    {
      switch (shareButtonType)
      {
        case ShareButtonType.Top:
          return "Top";
        case ShareButtonType.Bottom:
          return "Bottom";
        case ShareButtonType.IntermediateFacebook:
          return "Intermediate Facebook";
        case ShareButtonType.IntermediateOther:
          return "Intermediate Other";
        default:
          DebugUtilities.Fail("Unrecognized button type: {0}", (object) shareButtonType);
          return shareButtonType.ToString();
      }
    }

    public static void LogSocialFacebookRegistration(bool bound) => Telemetry.LogEvent("Social/Register/Facebook", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Bound",
        bound.ToString()
      }
    });

    public static void LogSocialFailure(SocialFailureType failureType, string failureDetail) => Telemetry.LogEvent("Social/Failure", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Failure Type",
        failureType.ToString()
      },
      {
        "Failure Detail",
        failureDetail
      }
    });

    public static void LogSocialInviteFriend(SocialInviteType inviteType) => Telemetry.LogEvent("Social/Invite friend", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Invite Type",
        inviteType.ToString()
      }
    });

    public static void LogSocialTileRemoval(bool isFeatureOn, SocialRemoveType removeType) => Telemetry.LogEvent("Settings/Manage Tiles/Social", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Toggle",
        isFeatureOn ? "On" : "Off"
      },
      {
        "RemoveType",
        removeType.ToString()
      }
    });

    public static void LogWorkoutPlanFavorite(string workoutPlanId, bool isFavorite) => Telemetry.LogEvent(isFavorite ? "Fitness/Guided Workouts/Favorite" : "Fitness/Guided Workouts/Unfavorite", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Workout Plan ID",
        workoutPlanId
      }
    });

    public static void LogWorkoutPlanSubscription(string workoutPlanId, bool isSubscribed) => Telemetry.LogEvent(isSubscribed ? "Fitness/Guided Workouts/Subscribe" : "Fitness/Guided Workouts/Unsubscribe", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Workout Plan ID",
        workoutPlanId
      }
    });

    public static void LogWorkoutChosen(
      int numberWhenChosen,
      string planName,
      string workoutProvider,
      string howFound)
    {
      Telemetry.LogEvent("Fitness/Guided Workouts/Choose Plan", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Workout Plan Name",
          planName
        },
        {
          "Workout Provider",
          workoutProvider
        },
        {
          "How Found",
          howFound
        }
      }, (IDictionary<string, double>) new Dictionary<string, double>()
      {
        {
          "Number of Results When Plan Is Chosen",
          (double) numberWhenChosen
        }
      });
    }

    public static void LogWorkoutPlanFilterResults(
      int count,
      IReadOnlyDictionary<string, string> filters)
    {
      Telemetry.LogEvent("Fitness/Guided Workouts/Filter Plan Results", (IDictionary<string, string>) filters.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key), (Func<KeyValuePair<string, string>, string>) (pair => pair.Value)), (IDictionary<string, double>) new Dictionary<string, double>()
      {
        {
          "Count",
          (double) count
        }
      });
    }

    public static void LogWorkoutMetricToggle(string metricChosen) => Telemetry.LogEvent("Fitness/Guided Workouts/Post workout details", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Metric Toggle",
        metricChosen
      }
    });

    public static ITimedTelemetryEvent TimeGolfCourseSync(
      string courseId,
      string teeId)
    {
      return Telemetry.StartTimedEvent("Fitness/Golf/Sync", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Course ID",
          courseId
        },
        {
          "Tee ID",
          teeId
        }
      });
    }

    public static void LogGolfFindCourse(string referrer) => Telemetry.LogEvent("Fitness/Golf/FindCourse", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Referrer",
        referrer
      }
    });

    public static void LogGolfBrowseCourse(string referrer) => Telemetry.LogEvent("Fitness/Golf/Browse/Course", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Referrer",
        referrer
      }
    });

    public static void LogGolfWatchIntroVideo() => Telemetry.LogEvent("Fitness/Golf/Intro Video");

    public static void LogWatchedVideo(string exerciseId, string exerciseNumber, string videoId) => Telemetry.LogEvent("Fitness/Guided Workouts/Watch Video", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Exercise ID",
        exerciseId
      },
      {
        "Exercise Number",
        exerciseNumber
      },
      {
        "Video ID",
        videoId
      }
    });

    public static void LogEarlyUpdateSignUp() => Telemetry.LogEvent("Utilities/Early Update/Sign Up");

    public static void LogEarlyUpdateProvision() => Telemetry.LogEvent("Utilities/Early Update/Provision");

    public static void LogEarlyUpdateDeprovision() => Telemetry.LogEvent("Utilities/Early Update/Deprovision");

    public static void LogTileTap(string taxonomy, bool isOpen, bool isFirstTimeUse) => Telemetry.LogEvent("Fitness/Home Tile Tap", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Tile Name",
        taxonomy
      },
      {
        "Action",
        isOpen ? "Open" : "Close"
      },
      {
        "First Time Use",
        isFirstTimeUse ? "Yes" : "No"
      }
    });

    public static void LogTileTap(TileViewModel viewModel, bool isOpen, bool isFirstTimeUse) => ApplicationTelemetry.LogTileTap(string.Join("/", (IEnumerable<string>) ApplicationTelemetry.GetPageTaxonomy((object) viewModel, false)), isOpen, isFirstTimeUse);

    public static void LogPageView(IReadOnlyList<string> taxonomy)
    {
      if (taxonomy == null || !taxonomy.Any<string>() || taxonomy.Count <= 1)
        return;
      Telemetry.LogPageView(string.Join("/", (IEnumerable<string>) taxonomy));
    }

    public static void LogPageView(object viewModel, bool isBackNavigation = false)
    {
      if (viewModel is TilesViewModel tilesViewModel && tilesViewModel.ShowFirstTimeUseControl)
        ApplicationTelemetry.LogFirstTimeUseView(tilesViewModel.SelectedTile.FirstTimeUse.Type);
      else
        ApplicationTelemetry.LogPageView(ApplicationTelemetry.GetPageTaxonomy(viewModel, isBackNavigation));
    }

    public static void LogDeepLinkSuccess(string referrer, string target, string pivot) => Telemetry.LogEvent("App/Deeplink", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Referrer",
        referrer
      },
      {
        "Target",
        target
      },
      {
        "Pivot",
        pivot
      }
    });

    public static void LogDeepLinkError(string referrer, string target, string pivot, string host) => Telemetry.LogEvent("App/Deeplink/Error", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Referrer",
        referrer
      },
      {
        "Target",
        target
      },
      {
        "Pivot",
        pivot
      },
      {
        "Host",
        host
      }
    });

    private static void LogFirstTimeUseView(TileFirstTimeUseViewModel.FirstTimeUseType type)
    {
      string str;
      switch (type)
      {
        case TileFirstTimeUseViewModel.FirstTimeUseType.Bike:
          str = "Bike";
          break;
        case TileFirstTimeUseViewModel.FirstTimeUseType.Exercise:
          str = "Exercise";
          break;
        case TileFirstTimeUseViewModel.FirstTimeUseType.GuidedWorkout:
          str = "Guided Workout/Completed Workout";
          break;
        case TileFirstTimeUseViewModel.FirstTimeUseType.Run:
          str = "Run";
          break;
        case TileFirstTimeUseViewModel.FirstTimeUseType.Sleep:
          str = "Sleep";
          break;
        case TileFirstTimeUseViewModel.FirstTimeUseType.Golf:
          str = "Golf";
          break;
        default:
          str = "Unknown";
          break;
      }
      ApplicationTelemetry.LogPageView((IReadOnlyList<string>) new List<string>()
      {
        "Fitness",
        str,
        "First Time Use"
      });
    }

    public static ITimedTelemetryEvent TimeHomePageLoad() => Telemetry.StartTimedEvent("App/Home");

    public static void LogDeleteAutoDetectedSleep(
      string userInfo,
      string sleepEventId,
      string dogfoodEnvironment)
    {
      Telemetry.LogEvent("Fitness/Sleep/AutoDetect/Delete", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "user info",
          userInfo
        },
        {
          "sleep event ID",
          sleepEventId
        },
        {
          "dogfood environment",
          dogfoodEnvironment
        }
      });
    }

    public static void LogReportAutoDetectedSleepInvalid(
      string userInfo,
      string sleepEventId,
      string dogfoodEnvironment)
    {
      Telemetry.LogEvent("Fitness/Sleep/AutoDetect/Report", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "user info",
          userInfo
        },
        {
          "sleep event ID",
          sleepEventId
        },
        {
          "dogfood environment",
          dogfoodEnvironment
        }
      });
    }

    public static void LogWebTileInstallationSuccess(string webTileName, string source) => Telemetry.LogEvent("App/WebTile/Installation/Success", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "WebTile Name",
        webTileName
      },
      {
        "WebTile Source",
        source
      }
    });

    public static void LogWebTileInstallationFailure(
      string webTileName,
      string source,
      string failureReason)
    {
      Telemetry.LogEvent("App/WebTile/Installation/Failure", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "WebTile Name",
          webTileName
        },
        {
          "WebTile Source",
          source
        },
        {
          "WebTile Install Failure Reason",
          failureReason
        }
      });
    }

    public static void LogWebTileSync(string webTileName, int downloadedBytes, bool tileUpdated) => Telemetry.LogEvent("App/WebTile/Sync", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "WebTile Name",
        webTileName
      },
      {
        "WebTile Sync Downloaded Bytes",
        downloadedBytes.ToString()
      },
      {
        "WebTile Sync Tile Updated",
        tileUpdated.ToString()
      }
    });

    public static void LogWhatsNewView() => Telemetry.LogEvent("App/What's new/Open");

    public static void LogWhatsNewSessionCount(string count) => Telemetry.LogEvent("App/What's new/Sessions", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Sessions",
        count
      }
    });

    public static void LogWhatsNewCardView(string cardName) => Telemetry.LogEvent("App/What's new/Cards", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Card",
        cardName
      }
    });

    public static void LogWhatsNewLearnMore(string cardName) => Telemetry.LogEvent("App/What's new/Learn more", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Learn More",
        cardName
      }
    });

    public static void LogFeedbackEntry(string referrer) => Telemetry.LogEvent("App/Feedback/Launch", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Referrer",
        referrer
      }
    });

    public static void LogFeedbackShakeSetting(bool selected) => Telemetry.LogEvent("App/Feedback/Shake settings", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Value",
        selected ? "On" : "Off"
      }
    });

    public static void LogFeedbackSelection(string selection) => Telemetry.LogEvent("App/Feedback/Selection", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Selection",
        selection
      }
    });

    public static void LogFeedbackSendFailure(string archiveId) => Telemetry.LogEvent("App/Feedback/Send failure", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "ArchiveId",
        archiveId
      }
    });

    public static void LogFeedbackComplete(
      bool imagesProvided,
      bool logsProvided,
      bool descriptionProvided,
      string archiveId,
      string uploadId)
    {
      Telemetry.LogEvent("App/Feedback/Complete", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Images",
          imagesProvided ? "Yes" : "No"
        },
        {
          "Logs",
          logsProvided ? "Yes" : "No"
        },
        {
          "Description",
          descriptionProvided ? "Yes" : "No"
        },
        {
          "ArchiveId",
          archiveId
        },
        {
          "UploadId",
          uploadId
        }
      });
    }

    public static void LogLiveTileEnabledSetting(bool selected) => Telemetry.LogEvent("Settings/Home Tile/Enabled", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Value",
        selected ? "On" : "Off"
      }
    });

    public static void LogLiveTileTransparentSetting(bool selected) => Telemetry.LogEvent("Settings/Home Tile/Transparency", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Value",
        selected ? "On" : "Off"
      }
    });

    public static void LogLiveTileTilesChanged(IDictionary<string, string> tileInfo) => Telemetry.LogEvent("Settings/Home Tile/Enabled Tiles Change", tileInfo);

    public static void LogNetPromoterScoreEntry(
      bool wasManualEntry,
      bool forApp,
      int rating,
      string feedback,
      BandClass? bandType,
      string bandFirmwareVersion)
    {
      string eventName = forApp ? "Feedback/NPS/App/Submitted" : "Feedback/NPS/Band/Submitted";
      string str = !bandType.HasValue ? "None" : (string.IsNullOrWhiteSpace(bandFirmwareVersion) ? "Unknown" : bandFirmwareVersion);
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "Source",
          wasManualEntry ? "Initiate" : "Prompt"
        },
        {
          "Score",
          rating.ToString()
        },
        {
          "Feedback",
          feedback
        },
        {
          "Band Version",
          !bandType.HasValue ? "None" : bandType.ToString()
        },
        {
          "Band FW",
          str
        }
      };
      Telemetry.LogEvent(eventName, (IDictionary<string, string>) dictionary);
    }

    public static void LogNetPromoterScoreDismissed() => Telemetry.LogEvent("Feedback/NPS/Dismissed");

    public static void LogRemoteDynamicConfigFileDownloaded(RegionInfo region, Version version) => Telemetry.LogEvent("Utilities/Dynamic Config/Update", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Region",
        region.ToString()
      },
      {
        "Version",
        version.ToString()
      }
    });

    public static void LogAppUpgradeSuccess(string fromVersion) => Telemetry.LogEvent("Diagnostics/App Upgrade/Success", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Version",
        fromVersion
      }
    });

    public static void LogAppUpgradeFailure(string fromVersion, Exception exception)
    {
      Telemetry.LogEvent("Diagnostics/App Upgrade/Failure", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Version",
          fromVersion
        }
      });
      Telemetry.LogException(exception, (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Version",
          fromVersion
        }
      });
    }

    public static void LogMessageboxHeadlessCall(string title, string message) => Telemetry.LogEvent("Diagnostics/Messagebox/AutoCancel", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Title",
        title
      },
      {
        "Message",
        message
      }
    });

    public static void LogUnhandledHeadlessException(Exception exception)
    {
      Telemetry.LogEvent("Diagnostics/Exceptions/HeadlessException", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Message",
          exception?.StackTrace
        }
      });
      if (exception == null)
        return;
      Telemetry.LogException(exception);
    }

    private static IReadOnlyList<string> GetPageTaxonomy(
      object viewModel,
      bool isBackNavigation)
    {
      IReadOnlyList<string> stringList = (IReadOnlyList<string>) null;
      if (viewModel != null)
      {
        if (viewModel is IPageTaxonomyProvider taxonomyProvider2)
        {
          stringList = taxonomyProvider2.GetPageTaxonomy();
        }
        else
        {
          Type type = !(viewModel is IPageTaxonomyTypeProvider taxonomyTypeProvider3) ? viewModel.GetType() : taxonomyTypeProvider3.GetPageTaxonomyType(isBackNavigation);
          if ((object) type != null)
          {
            PageTaxonomyAttribute taxonomyAttribute = CustomAttributeExtensions.GetCustomAttributes(type.GetTypeInfo(), typeof (PageTaxonomyAttribute), false).Cast<PageTaxonomyAttribute>().FirstOrDefault<PageTaxonomyAttribute>();
            if (taxonomyAttribute != null)
              stringList = (IReadOnlyList<string>) taxonomyAttribute.Taxonomy;
            else
              stringList = (IReadOnlyList<string>) new List<string>()
              {
                type.Name
              };
          }
        }
      }
      return stringList;
    }

    private static string GetAgeGroup(int age)
    {
      Tuple<Tuple<int, int>, string> tuple = ApplicationTelemetry.AgeGroups.FirstOrDefault<Tuple<Tuple<int, int>, string>>((Func<Tuple<Tuple<int, int>, string>, bool>) (g => g.Item1.Item1 <= age && g.Item1.Item2 >= age));
      return tuple == null ? "Unknown" : tuple.Item2;
    }

    public static void ToastNotificationClicked(string launchPath) => Telemetry.LogEvent("Notifications/Toast", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "LaunchPath",
        launchPath
      }
    });

    public static void ToastNotificationReceived(bool optedIn) => Telemetry.LogEvent("Notifications/Registration", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "OptedIn",
        optedIn ? "True" : "False"
      }
    });

    public static void PushNotificationReceived(string type) => Telemetry.LogEvent("Notifications/Data", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "PushType",
        type
      }
    });

    public static void LogWeightAdded(bool saved) => Telemetry.LogEvent("Weight/Add", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Action",
        saved ? "Save" : "Cancel"
      }
    });

    public static void LogWeightDeleted(bool deleted) => Telemetry.LogEvent("Weight/Delete", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Action",
        deleted ? "Delete" : "Cancel"
      }
    });

    public static void LogWeightDialogOpened() => Telemetry.LogEvent("Weight/Add/Add dialog opened");

    public static void LogNewBandDetected(bool userOk) => Telemetry.LogEvent("App/Add Band/New Band detected", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Action",
        userOk ? "Band added" : "Cancel"
      }
    });

    public static void SetSyncInProgress(bool inProgress) => ApplicationTelemetry.foregroundSyncInProgress = inProgress;

    public static void LogSyncError(string failureType, string failureDetail) => Telemetry.LogEvent("Utilities/Sync/Sync Error", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "Failure Type",
        failureType
      },
      {
        "Failure Detail",
        failureDetail
      }
    });

    public static void LogGetLogsFromBand(
      bool isForeground,
      long sensorLogBytes,
      double downloadkBytesPerSecond,
      long duration)
    {
      string str = isForeground ? "ForegroundDownloadKbytesPerSecondFromDevice" : "BackgroundDownloadKbytesPerSecondFromDevice";
      Telemetry.LogEvent("Utilities/Sync/Get log from band", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Sensor Log Bytes",
          sensorLogBytes.ToString()
        },
        {
          str,
          downloadkBytesPerSecond.ToString()
        },
        {
          "Duration",
          duration.ToString()
        }
      });
    }

    public static void LogUploadLogsToCloud(
      bool isForeground,
      long sensorLogBytes,
      double uploadkBytesPerSecond,
      long duration)
    {
      string str = isForeground ? "ForegroundUploadKbytesPerSecondToCloud" : "BackgroundUploadKbytesPerSecondToCloud";
      Telemetry.LogEvent("Utilities/Sync/Log send to cloud", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Sensor Log Bytes",
          sensorLogBytes.ToString()
        },
        {
          str,
          uploadkBytesPerSecond.ToString()
        },
        {
          "Duration",
          duration.ToString()
        }
      });
    }

    public static ITimedTelemetryEvent TimeSyncOperationTime(SyncState syncState)
    {
      switch (syncState)
      {
        case SyncState.NotStarted:
          return (ITimedTelemetryEvent) null;
        case SyncState.CurrentTimeAndTimeZone:
          return Telemetry.StartTimedEvent("Utilities/Sync/Time");
        case SyncState.Ephemeris:
          return Telemetry.StartTimedEvent("Utilities/Sync/Ephemeris");
        case SyncState.TimeZoneData:
          return Telemetry.StartTimedEvent("Utilities/Sync/Time zone list");
        case SyncState.DeviceInstrumentation:
          return Telemetry.StartTimedEvent("Utilities/Sync/Instrumentation");
        case SyncState.DeviceCrashDump:
          return Telemetry.StartTimedEvent("Utilities/Sync/Crash dump");
        case SyncState.UserProfile:
          return Telemetry.StartTimedEvent("Utilities/Sync/User profile");
        case SyncState.SensorLog:
          return (ITimedTelemetryEvent) null;
        case SyncState.WebTiles:
          return Telemetry.StartTimedEvent("Utilities/Sync/Web tiles");
        case SyncState.Done:
          return (ITimedTelemetryEvent) null;
        default:
          return (ITimedTelemetryEvent) null;
      }
    }

    public static ITimedTelemetryEvent TimeTilesUpdateSync() => Telemetry.StartTimedEvent("Utilities/Sync/Tiles");

    public static ITimedTelemetryEvent TimePhoneSensorSync() => Telemetry.StartTimedEvent("Utilities/Sync/Phone sensors");

    public static ITimedTelemetryEvent TimeCloudLogProcessing() => Telemetry.StartTimedEvent("Utilities/Sync/Poll cloud status");

    public static ITimedTelemetryEvent TimeSingleDeviceEnforcementCheck() => !ApplicationTelemetry.foregroundSyncInProgress ? (ITimedTelemetryEvent) null : Telemetry.StartTimedEvent("Utilities/Sync/SDE check");
  }
}
