// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mocks.MockBandConnection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Band.Personalization;
using Microsoft.Band.Tiles;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities.Logging;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Mocks
{
  public class MockBandConnection : IBandConnection, IDisposable
  {
    internal const string SyncTimeMutexName = "KApp.syncTime";
    private const string CurrentBandThemeStorageKey = "MockDeviceCurrentTheme";
    private static readonly Guid MockDeviceGuid = Guid.Parse("43F98124-B389-432F-93C8-5F7681C4A4A1");
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Mocks\\MockBandConnection.cs");
    private static readonly IActivityManager ActivityManager = MockBandConnection.Logger.CreateActivityManager();
    private readonly IConfigProvider configProvider;
    private readonly IBandConnectionSynchronizationService synchronizationService;
    private readonly IMockBandIconFactory bandIconFactory;
    internal const string DummyThirdPartyLink = "http://www.microsoft.com/surface/en-us/support/contact-us/chat?productId=100110817&categoryId=100110825&issueId=100110827&serialNumber=003533724052&lc=1033";

    public MockBandConnection(
      IMessageSender messageSender,
      IConfigProvider configProvider,
      IBandConnectionSynchronizationService synchronizationService,
      IMockBandIconFactory bandIconFactory)
    {
      this.configProvider = configProvider;
      this.synchronizationService = synchronizationService;
      this.bandIconFactory = bandIconFactory;
      this.IsOobeCompleted = true;
    }

    public bool IsDisposed { get; private set; }

    public bool IsOobeCompleted { get; set; }

    public bool IsBackground { get; set; }

    public Task<IBandInfo> GetPrimaryPairedBandAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<IBandInfo>(Level.Debug, (Func<string>) (() => "Getting primary paired Bluetooth device"), (Func<Task<IBandInfo>>) (async () => (await this.GetPairedBandsAsync(cancellationToken))[0]), true);
    }

    public Task<IBandInfo[]> GetPairedBandsAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<IBandInfo[]>(Level.Debug, (Func<string>) (() => "Getting paired Bluetooth devices"), (Func<Task<IBandInfo[]>>) (async () =>
    {
      await Task.Delay(500);
      return new IBandInfo[1]
      {
        (IBandInfo) new MockBandInfo("MockBand", new Guid("883b4e4e-57ec-45e8-b4ed-087d06acdd26"), BandConnectionType.Bluetooth)
      };
    }), true);

    public Task<bool> TryCheckConnectionWorkingAsync() => MockBandConnection.ActivityManager.RunAsActivityAsync<bool>(Level.Info, (Func<string>) (() => "Try Check If Connection To Band Is Working"), (Func<Task<bool>>) (() => Task.FromResult<bool>(true)), true);

    public Task<BandUserProfile> GetUserProfileAsync(
      Func<IUserProfile, Task<BandUserProfile>> profileConversion,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<BandUserProfile>(Level.Info, (Func<string>) (() => "Getting user profile from Band"), (Func<Task<BandUserProfile>>) (async () =>
      {
        await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
        return await profileConversion(this.GetCloudUserProfile());
      }), true);
    }

    public Task<BandUserProfile> GetCloudUserProfileAsync(
      Func<IUserProfile, Task<BandUserProfile>> profileConversion)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<BandUserProfile>(Level.Info, (Func<string>) (() => "Downloading user profile from the Health Cloud"), (Func<Task<BandUserProfile>>) (async () =>
      {
        await Task.Delay(1000).ConfigureAwait(false);
        return await profileConversion(this.GetCloudUserProfile());
      }), true);
    }

    private IUserProfile GetCloudUserProfile()
    {
      DeviceSettings deviceSettings = new DeviceSettings()
      {
        DeviceName = "Mock Band",
        LocaleSettings = new CargoLocaleSettings()
        {
          DateFormat = DisplayDateFormat.MMddyyyy,
          DateSeparator = '-',
          DecimalSeparator = '.',
          DistanceLongUnits = DistanceUnitType.Imperial,
          DistanceShortUnits = DistanceUnitType.Imperial,
          EnergyUnits = EnergyUnitType.Imperial,
          Language = LocaleLanguage.English_US,
          LocaleId = Locale.UnitedStates,
          LocaleName = "United States",
          MassUnits = MassUnitType.Imperial,
          NumberSeparator = ',',
          TemperatureUnits = TemperatureUnitType.Imperial,
          TimeFormat = DisplayTimeFormat.HHmmss,
          VolumeUnits = VolumeUnitType.Imperial
        }
      };
      return (IUserProfile) new MockUserProfile((ushort) 1)
      {
        HasCompletedOOBE = this.IsOobeCompleted,
        Birthdate = new DateTime(1985, 7, 14),
        Gender = Gender.Female,
        Height = (ushort) 1700,
        Weight = 63636U,
        FirstName = "Dummy",
        AllDeviceSettings = (IDictionary<Guid, DeviceSettings>) new Dictionary<Guid, DeviceSettings>()
        {
          {
            MockBandConnection.MockDeviceGuid,
            deviceSettings
          }
        },
        DeviceSettings = deviceSettings,
        ApplicationSettings = new ApplicationSettings()
        {
          PairedDeviceId = MockBandConnection.MockDeviceGuid,
          MeasurementDisplayType = new MeasurementUnitType?(MeasurementUnitType.Imperial),
          TemperatureDisplayType = new TemperatureUnitType?(TemperatureUnitType.Imperial),
          PreferredLocale = "en-US",
          PreferredRegion = "US",
          ThirdPartyPartnersPortalEndpoint = "https://partners.dns-cargo.com/"
        }
      };
    }

    public Task SaveCloudUserProfileAsync(Func<IUserProfile, Task> userProfileModifications) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Uploading the user profile to the Health Cloud"), (Func<Task>) (async () => await Task.Delay(500)));

    public Task PersonalizeBandAsync(
      uint imageId,
      CancellationToken cancellationToken,
      StartStrip startStrip = null,
      BandImage image = null,
      BandTheme color = null,
      IDictionary<Guid, BandTheme> customColors = null)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting user personalization information on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "ImageId",
          (object) imageId
        },
        {
          "StartStrip",
          (object) startStrip
        },
        {
          "BandImage",
          (object) image
        },
        {
          "BandTheme",
          (object) color
        },
        {
          "CustomColors",
          (object) customColors
        }
      }), (Func<Task>) (() =>
      {
        this.configProvider.Set<uint>("MockDeviceCurrentTheme", imageId);
        return Task.Delay(500, cancellationToken);
      }));
    }

    public Task SaveUserProfileAsync(
      Func<IUserProfile, Task> userProfileModifications,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting user profile on the Band and uploading to Health Cloud"), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task SendCalendarEventsAsync(
      IEnumerable<CalendarEvent> calendarEvents,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending calendar updates to the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "CalendarEvents",
          (object) calendarEvents
        }
      }), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task SetSleepNotificationAsync(
      SleepNotification sleepNotification,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending sleep notification to the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "SleepNotification",
          (object) sleepNotification
        }
      }), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task DisableSleepNotificationAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Disabling sleep notification on the band"), (Func<Task>) (() => Task.Delay(100, cancellationToken)));

    public Task SetLightExposureNotificationAsync(
      LightExposureNotification lightExposureNotification,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending light exposure notification to the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "LightExposureNotification",
          (object) lightExposureNotification
        }
      }), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task DisableLightExposureNotificationAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Disabling light exposure notification on the band"), (Func<Task>) (() => Task.Delay(100, cancellationToken)));

    public Task SendFinanceUpdateNotificationsAsync(
      IEnumerable<Stock> stocks,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending finance updates to the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "Stocks",
          (object) stocks
        },
        {
          "TimeStamp",
          (object) timestamp
        }
      }), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task SendWeatherUpdateNotificationsAsync(
      IList<WeatherDay> dailyConditions,
      string location,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending weather updates to the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "DailyConditions",
          (object) dailyConditions
        },
        {
          "Location",
          (object) location
        },
        {
          "TimeStamp",
          (object) timestamp
        }
      }), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task SendStarbucksUpdateNotificationsAsync(
      string cardNumber,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending Starbucks updates to the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "CardNumber",
          (object) cardNumber
        }
      }), (Func<Task>) (() => Task.Delay(500, cancellationToken)));
    }

    public Task<FirmwareVersions> GetBandFirmwareVersionAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<FirmwareVersions>(Level.Info, (Func<string>) (() => "Getting firmware version from Band"), (Func<Task<FirmwareVersions>>) (async () =>
      {
        await Task.Delay(300, cancellationToken).ConfigureAwait(false);
        return new FirmwareVersions()
        {
          ApplicationVersion = new FirmwareVersion(1, 0, 1013, 1, true),
          BootloaderVersion = new FirmwareVersion(1, 0, 1013, 1, true),
          UpdaterVersion = new FirmwareVersion(1, 0, 1013, 1, true),
          PcbId = 9
        };
      }), true);
    }

    public Task NavigateToScreenAsync(CargoScreen screen, CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Navigating Band to screen"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "CargoScreen",
        (object) screen
      }
    }), (Func<Task>) (() => Task.Delay(300)));

    public Task SetOobeStageAsync(OobeStage stage, CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Navigating Band to screen"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "OobeStage",
        (object) stage
      }
    }), (Func<Task>) (() => Task.Delay(300)));

    public Task FinalizeOobeAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Finalizing OOBE"), (Func<Task>) (() => Task.Delay(300)));

    public Task SetMeTileImageAsync(BandImage image) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Set Me tile image on Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "BandImage",
        (object) image
      }
    }), (Func<Task>) (() => Task.Delay(300)));

    public Task<string> GetProductSerialNumberAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<string>(Level.Info, (Func<string>) (() => "Getting serial number from Band"), (Func<Task<string>>) (async () =>
    {
      await Task.Delay(100, cancellationToken).ConfigureAwait(false);
      return "12356419261";
    }), true);

    public Task<IList<AdminBandTile>> GetDefaultTilesAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<IList<AdminBandTile>>(Level.Info, (Func<string>) (() => "Getting default tiles from Band"), (Func<Task<IList<AdminBandTile>>>) (async () =>
      {
        await Task.Delay(100);
        List<BandIcon> bandIconList = new List<BandIcon>()
        {
          this.bandIconFactory.MockBandIcon(1, 1),
          this.bandIconFactory.MockBandIcon(1, 1)
        };
        return (IList<AdminBandTile>) new List<AdminBandTile>()
        {
          new AdminBandTile(Guid.Parse("b4edbc35-027b-4d10-a797-1099cd2ad98a"), "Text", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"), "Phone", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("ec149021-ce45-40e9-aeee-08f86e4746a7"), "Calendar", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("65bd93db-4293-46af-9a28-bdd6513b4677"), "Run", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("96430fcb-0060-41cb-9de2-e00cac97f85d"), "Bike", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("fb9d005a-c3da-49d4-8e7b-c6f674fc4710"), "Golf", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("a708f02a-03cd-4da0-bb33-be904e6a2924"), "Exercise", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("23e7bc94-f90d-44e0-843f-250910fdf74e"), "Sleep", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("d36a92ea-3e85-4aed-a726-2898a6f2769b"), "Alarm", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("0281c878-afa8-40ff-acfd-bca06c5c4922"), "Workouts", AdminTileSettings.None, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("59976cf5-15c8-4799-9e31-f34c765a6bd1"), "UV", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U)
        };
      }), true);
    }

    public Task<StartStrip> GetStartStripAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<StartStrip>(Level.Info, (Func<string>) (() => "Getting start strip from Band"), (Func<Task<StartStrip>>) (async () =>
    {
      await Task.Delay(100, cancellationToken);
      List<BandIcon> bandIconList = new List<BandIcon>()
      {
        this.bandIconFactory.MockBandIcon(46, 46),
        this.bandIconFactory.MockBandIcon(46, 46)
      };
      return new StartStrip((IEnumerable<AdminBandTile>) new List<AdminBandTile>()
      {
        new AdminBandTile(Guid.Parse("b4edbc35-027b-4d10-a797-1099cd2ad98a"), "Text", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("823ba55a-7c98-4261-ad5e-929031289c6e"), "Email", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("d7fb5ff5-906a-4f2c-8269-dde6a75138c4"), "Cortana", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"), "Phone", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("ec149021-ce45-40e9-aeee-08f86e4746a7"), "Calendar", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("65bd93db-4293-46af-9a28-bdd6513b4677"), "Run", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("96430fcb-0060-41cb-9de2-e00cac97f85d"), "Bike", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("a708f02a-03cd-4da0-bb33-be904e6a2924"), "Exercise", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("23e7bc94-f90d-44e0-843f-250910fdf74e"), "Sleep", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("d36a92ea-3e85-4aed-a726-2898a6f2769b"), "Alarm", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("0281c878-afa8-40ff-acfd-bca06c5c4922"), "Workouts", AdminTileSettings.None, (IList<BandIcon>) bandIconList, 0U),
        new AdminBandTile(Guid.Parse("0AC84A11-7AB6-44F2-8BA7-5AB4B5FA0690"), "Third Party 1", AdminTileSettings.None, (IList<BandIcon>) bandIconList, 0U)
        {
          OwnerId = Guid.Parse("0AC84A11-7AB6-44F2-8BA7-5AB4B5FA0690")
        },
        new AdminBandTile(Guid.Parse("6B744212-93F8-49E3-9C96-0C978878A1FF"), "Third Party 2", AdminTileSettings.None, (IList<BandIcon>) bandIconList, 0U)
        {
          OwnerId = Guid.Parse("6B744212-93F8-49E3-9C96-0C978878A1FF")
        }
      });
    }), true);

    public Task<StartStrip> GetStartStripWithoutImagesAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<StartStrip>(Level.Info, (Func<string>) (() => "Getting start strip from Band without images"), (Func<Task<StartStrip>>) (async () =>
      {
        await Task.Delay(100, cancellationToken);
        List<BandIcon> bandIconList = new List<BandIcon>()
        {
          this.bandIconFactory.MockBandIcon(1, 1),
          this.bandIconFactory.MockBandIcon(1, 1)
        };
        return new StartStrip((IEnumerable<AdminBandTile>) new List<AdminBandTile>()
        {
          new AdminBandTile(Guid.Parse("b4edbc35-027b-4d10-a797-1099cd2ad98a"), "Text", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("823ba55a-7c98-4261-ad5e-929031289c6e"), "Email", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("d7fb5ff5-906a-4f2c-8269-dde6a75138c4"), "Cortana", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"), "Phone", AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("ec149021-ce45-40e9-aeee-08f86e4746a7"), "Calendar", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("65bd93db-4293-46af-9a28-bdd6513b4677"), "Run", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("96430fcb-0060-41cb-9de2-e00cac97f85d"), "Bike", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("a708f02a-03cd-4da0-bb33-be904e6a2924"), "Exercise", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("23e7bc94-f90d-44e0-843f-250910fdf74e"), "Sleep", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("d36a92ea-3e85-4aed-a726-2898a6f2769b"), "Alarm", AdminTileSettings.EnableNotification, (IList<BandIcon>) bandIconList, 0U),
          new AdminBandTile(Guid.Parse("0281c878-afa8-40ff-acfd-bca06c5c4922"), "Workouts", AdminTileSettings.None, (IList<BandIcon>) bandIconList, 0U)
        });
      }), true);
    }

    public Task<BandTheme> GetBandThemeAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<BandTheme>(Level.Info, (Func<string>) (() => "Getting device theme from Band"), (Func<Task<BandTheme>>) (async () =>
    {
      await Task.Delay(100, cancellationToken);
      return new BandTheme();
    }), true);

    public Task SetStartStripAsync(StartStrip s, CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting the start strip on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "StartStrip",
        (object) s
      }
    }), (Func<Task>) (async () => await Task.Delay(100)));

    public Task<string[]> GetSmsResponsesAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<string[]>(Level.Info, (Func<string>) (() => "Getting SMS responses from the Band"), (Func<Task<string[]>>) (async () =>
    {
      await Task.Delay(100);
      return (string[]) null;
    }), true);

    public Task SetSmsResponsesAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting SMS responses on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "Response1",
          (object) response1
        },
        {
          "Response2",
          (object) response2
        },
        {
          "Response3",
          (object) response3
        },
        {
          "Response4",
          (object) response4
        }
      }), (Func<Task>) (() => Task.Delay(100)));
    }

    public Task<string[]> GetPhoneCallResponsesAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<string[]>(Level.Info, (Func<string>) (() => "Getting phone call responses from the Band"), (Func<Task<string[]>>) (async () =>
    {
      await Task.Delay(100);
      return (string[]) null;
    }), true);

    public Task SetPhoneCallResponsesAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting phone call responses on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "Response1",
          (object) response1
        },
        {
          "Response2",
          (object) response2
        },
        {
          "Response3",
          (object) response3
        },
        {
          "Response4",
          (object) response4
        }
      }), (Func<Task>) (() => Task.Delay(100)));
    }

    public Task SaveGoalsToBandAsync(Goals goals) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting the user goals on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "Goals",
        (object) goals
      }
    }), (Func<Task>) (() => Task.Delay(500)));

    public Task UpdateGpsEphemerisDataAsync(
      CancellationToken cancellationToken,
      bool forceUpdate)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending ephemeris data to the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "ForceUpdate",
          (object) forceUpdate
        }
      }), (Func<Task>) (() => Task.Delay(100, cancellationToken)));
    }

    public Task UpdateTimeZoneListAsync(CancellationToken cancellationToken, bool forceUpdate) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Updating the time zone list on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "ForceUpdate",
        (object) forceUpdate
      }
    }), (Func<Task>) (() => Task.Delay(100, cancellationToken)));

    public Task SetCurrentTimeAndTimeZoneAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting The Current Time And Timezone On The Band"), (Func<Task>) (() => Task.Delay(100)));

    public Task<AdminTileSettings> GetTileSettingsAsync(Guid id) => MockBandConnection.ActivityManager.RunAsActivityAsync<AdminTileSettings>(Level.Info, (Func<string>) (() => "Getting tile settings from the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "Guid",
        (object) id
      }
    }), (Func<Task<AdminTileSettings>>) (async () =>
    {
      await Task.Delay(100);
      return AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging;
    }), true);

    public Task SetTileSettingsAsync(Guid id, AdminTileSettings settings) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting tile settings on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "Guid",
        (object) id
      },
      {
        "TileSetting",
        (object) settings
      }
    }), (Func<Task>) (() => Task.Delay(100)));

    public Task ImportUserProfileAsync(Func<IUserProfile, Task> userProfileModifications) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Importing The User Profile From The Health Cloud To The Band"), (Func<Task>) (() => Task.Delay(500)));

    public Task<CargoRunDisplayMetrics> GetRunDisplayMetricsAsync() => MockBandConnection.ActivityManager.RunAsActivityAsync<CargoRunDisplayMetrics>(Level.Info, (Func<string>) (() => "Getting run display metrics from the Band"), (Func<Task<CargoRunDisplayMetrics>>) (async () =>
    {
      await Task.Delay(10);
      return new CargoRunDisplayMetrics(new RunDisplayMetricType[7]
      {
        RunDisplayMetricType.Duration,
        RunDisplayMetricType.HeartRate,
        RunDisplayMetricType.Calories,
        RunDisplayMetricType.Distance,
        RunDisplayMetricType.Pace,
        RunDisplayMetricType.None,
        RunDisplayMetricType.None
      });
    }), true);

    public Task SetRunDisplayMetricsAsync(CargoRunDisplayMetrics cargoRunDisplayMetrics) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting run display metrics on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "CargoRunDisplayMetrics",
        (object) cargoRunDisplayMetrics
      }
    }), (Func<Task>) (() => Task.Delay(10)));

    public Task<CargoBikeDisplayMetrics> GetBikeDisplayMetricsAsync() => MockBandConnection.ActivityManager.RunAsActivityAsync<CargoBikeDisplayMetrics>(Level.Info, (Func<string>) (() => "Getting bike display metrics from the Band"), (Func<Task<CargoBikeDisplayMetrics>>) (async () =>
    {
      await Task.Delay(10);
      return new CargoBikeDisplayMetrics(new BikeDisplayMetricType[7]
      {
        BikeDisplayMetricType.Duration,
        BikeDisplayMetricType.HeartRate,
        BikeDisplayMetricType.Calories,
        BikeDisplayMetricType.Distance,
        BikeDisplayMetricType.Speed,
        BikeDisplayMetricType.None,
        BikeDisplayMetricType.None
      });
    }), true);

    public Task SetBikeDisplayMetricsAsync(CargoBikeDisplayMetrics cargoBikeDisplayMetrics) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting bike display metrics on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "CargoBikeDisplayMetrics",
        (object) cargoBikeDisplayMetrics
      }
    }), (Func<Task>) (() => Task.Delay(10)));

    public Task<int> GetBikeSplitDistanceAsync() => MockBandConnection.ActivityManager.RunAsActivityAsync<int>(Level.Info, (Func<string>) (() => "Getting bike split distance from the Band"), (Func<Task<int>>) (async () =>
    {
      await Task.Delay(10);
      return 3;
    }), true);

    public Task SetBikeSplitDistanceAsync(int splitDistance) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting bike split distance on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "SplitDistance",
        (object) splitDistance
      }
    }), (Func<Task>) (() => Task.Delay(10)));

    public Task<uint> GetMaxTileCountAsync() => MockBandConnection.ActivityManager.RunAsActivityAsync<uint>(Level.Info, (Func<string>) (() => "Getting maximum tiles allowed from the Band"), (Func<Task<uint>>) (async () =>
    {
      await Task.Delay(100);
      return 13;
    }), true);

    public Task<int> GetBatteryChargeAsync(CancellationToken token) => MockBandConnection.ActivityManager.RunAsActivityAsync<int>(Level.Info, (Func<string>) (() => "Getting battery charge from the Band"), (Func<Task<int>>) (() => Task.FromResult<int>(60)), true);

    public Task<SyncResultWrapper> SyncBandToCloudAsync(
      CancellationToken cancellationToken,
      Action<SyncProgressWrapper> onSyncProgress,
      bool logsOnly = false)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<SyncResultWrapper>(Level.Info, (Func<string>) (() => "Syncing collected data from the Band to the Health Cloud"), (Func<Task<SyncResultWrapper>>) (() => logsOnly ? this.MockSuccessfulLogsOnlySyncBandToCloudAsync(cancellationToken, onSyncProgress) : MockBandConnection.MockSuccessfulSyncBandToCloudAsync(cancellationToken, onSyncProgress)), true);
    }

    public Task<bool> GetBandOobeCompletedAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<bool>(Level.Info, (Func<string>) (() => "Getting the OOBE completed status from the Band"), (Func<Task<bool>>) (async () =>
    {
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      return false;
    }), true);

    public Task<DeviceProfileStatus> GetBandAndProfileLinkStatusAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<DeviceProfileStatus>(Level.Info, (Func<string>) (() => "Getting the Band and Health Cloud profile link status"), (Func<Task<DeviceProfileStatus>>) (async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
        return new DeviceProfileStatus()
        {
          DeviceLinkStatus = LinkStatus.Matching,
          UserLinkStatus = LinkStatus.Matching
        };
      }), true);
    }

    public Task LinkBandToProfileAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Linking Band To Health Cloud Profile"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken)));

    public Task UnlinkBandFromProfileAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Unlinking band from Health Cloud profile"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken)));

    private static async Task<SyncResultWrapper> MockSuccessfulSyncBandToCloudAsync(
      CancellationToken cancellationToken,
      Action<SyncProgressWrapper> onSyncProgress)
    {
      SyncProgressWrapper syncProgressWrapper = new SyncProgressWrapper()
      {
        State = SyncState.NotStarted,
        PercentageCompletion = 0.0
      };
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.Ephemeris;
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.TimeZoneData;
      syncProgressWrapper.PercentageCompletion = 20.0;
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.DeviceInstrumentation;
      syncProgressWrapper.PercentageCompletion = 30.0;
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.DeviceCrashDump;
      syncProgressWrapper.PercentageCompletion = 40.0;
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.SensorLog;
      syncProgressWrapper.PercentageCompletion = 80.0;
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.Done;
      syncProgressWrapper.PercentageCompletion = 100.0;
      onSyncProgress(syncProgressWrapper);
      return new SyncResultWrapper()
      {
        BandPendingSensorLogBytes = 423,
        DownloadKbitsPerSecond = 2345.0,
        DownloadKbytesPerSecond = 2345.0,
        DownloadedSensorLogBytes = 2345,
        RanToCompletion = true,
        TotalTimeElapsed = 2345,
        UploadKbitsPerSecond = 2345.0,
        UploadKbytesPerSecond = 34257.0,
        UploadedSensorLogBytes = 2345,
        LogFilesProcessing = (IList<LogProcessingStatusWrapper>) new List<LogProcessingStatusWrapper>()
        {
          new LogProcessingStatusWrapper()
          {
            UploadId = string.Empty,
            FileType = LogFileTypes.Sensor
          }
        }
      };
    }

    private async Task<SyncResultWrapper> MockSuccessfulLogsOnlySyncBandToCloudAsync(
      CancellationToken cancellationToken,
      Action<SyncProgressWrapper> onSyncProgress)
    {
      if (!this.IsBackground)
        this.synchronizationService.CancelBackgroundSyncTask();
      SyncProgressWrapper syncProgressWrapper = new SyncProgressWrapper()
      {
        State = SyncState.NotStarted,
        PercentageCompletion = 0.0
      };
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.SensorLog;
      syncProgressWrapper.PercentageCompletion = 80.0;
      onSyncProgress(syncProgressWrapper);
      await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
      syncProgressWrapper.State = SyncState.Done;
      syncProgressWrapper.PercentageCompletion = 100.0;
      onSyncProgress(syncProgressWrapper);
      return new SyncResultWrapper()
      {
        BandPendingSensorLogBytes = 423,
        DownloadKbitsPerSecond = 2345.0,
        DownloadKbytesPerSecond = 2345.0,
        DownloadedSensorLogBytes = 2345,
        RanToCompletion = true,
        TotalTimeElapsed = 2345,
        UploadKbitsPerSecond = 2345.0,
        UploadKbytesPerSecond = 34257.0,
        UploadedSensorLogBytes = 2345,
        LogFilesProcessing = (IList<LogProcessingStatusWrapper>) new List<LogProcessingStatusWrapper>()
        {
          new LogProcessingStatusWrapper()
          {
            FileType = LogFileTypes.Sensor,
            UploadId = "xxx"
          }
        }
      };
    }

    public Task UpdateLogProcessingAsync(
      IList<LogProcessingStatusWrapper> logProcessingStatusWrappers,
      CancellationToken cancellationToken,
      Action<double> onCloudProcessingLogsUpdate,
      bool singleCallback = true)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Subsribing to the log processing status from the Health Cloud"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "LogProcessingStatusWrappers",
          (object) logProcessingStatusWrappers
        },
        {
          "SingleCallback",
          (object) singleCallback
        }
      }), (Func<Task>) (async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken).ConfigureAwait(false);
        onCloudProcessingLogsUpdate(100.0);
      }));
    }

    public Task SendWorkoutToBandAsync(
      Stream workoutStream,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending a guided workout to the Band"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken)));
    }

    public Task SendGolfCourseToBandAsync(
      Stream golfCourseStream,
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending a golf course to the Band"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(4.0), cancellationToken)));
    }

    public Task CheckConnectionWorkingAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Checking if the Band connection is working"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(0.1), cancellationToken)));

    public Task<UpdateInfo> GetLatestAvailableFirmwareVersionAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<UpdateInfo>(Level.Info, (Func<string>) (() => "Getting firmware update information from the Band and the Health Cloud"), (Func<Task<UpdateInfo>>) (() => Task.FromResult<UpdateInfo>(new UpdateInfo()
      {
        FirmwareVersion = "1.1.104.1",
        IsFirmwareUpdateAvailable = false,
        LastUpdateTime = DateTimeOffset.Now
      })), true);
    }

    public Task UpdateFirmwareAsync(
      CancellationToken cancellationToken,
      IProgress<FirmwareUpdateProgressReport> progress = null)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Updating the firmware on the Band from the Health Cloud"), (Func<Task>) (async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
        if (progress != null)
          progress.Report(new FirmwareUpdateProgressReport()
          {
            PercentageCompletion = 0.0,
            FirmwareUpdateState = FirmwareUpdateState.SyncingLog
          });
        await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
        if (progress != null)
          progress.Report(new FirmwareUpdateProgressReport()
          {
            PercentageCompletion = 3.0,
            FirmwareUpdateState = FirmwareUpdateState.DownloadingUpdate
          });
        await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
        if (progress != null)
          progress.Report(new FirmwareUpdateProgressReport()
          {
            PercentageCompletion = 15.0,
            FirmwareUpdateState = FirmwareUpdateState.BootingToUpdateMode
          });
        await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
        if (progress != null)
          progress.Report(new FirmwareUpdateProgressReport()
          {
            PercentageCompletion = 30.0,
            FirmwareUpdateState = FirmwareUpdateState.SendingUpdateToDevice
          });
        await Task.Delay(TimeSpan.FromSeconds(3.0), cancellationToken);
        if (progress != null)
          progress.Report(new FirmwareUpdateProgressReport()
          {
            PercentageCompletion = 80.0,
            FirmwareUpdateState = FirmwareUpdateState.WaitingtoConnectAfterUpdate
          });
        await Task.Delay(TimeSpan.FromSeconds(6.0), cancellationToken);
        if (progress == null)
          return;
        progress.Report(new FirmwareUpdateProgressReport()
        {
          PercentageCompletion = 100.0,
          FirmwareUpdateState = FirmwareUpdateState.Done
        });
      }));
    }

    public Task<uint> GetMeTileIdAsync(CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync<uint>(Level.Info, (Func<string>) (() => "Getting the Me tile ID from the Band"), (Func<Task<uint>>) (() => Task.FromResult<uint>(this.configProvider.Get<uint>("MockDeviceCurrentTheme", uint.MaxValue))), true);

    public Task SyncWebTilesAsync(bool forceSync, CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Syncing all WebTiles"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(4.0), cancellationToken)));

    public Task SyncWebTileAsync(Guid tileId, CancellationToken cancellationToken) => MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Syncing one WebTile"), (Func<Task>) (() => Task.Delay(TimeSpan.FromSeconds(4.0), cancellationToken)));

    public Task<IDynamicBandConstants> GetAppBandConstantsAsync(
      CancellationToken cancellationToken)
    {
      return MockBandConnection.ActivityManager.RunAsActivityAsync<IDynamicBandConstants>(Level.Info, (Func<string>) (() => "Getting the hardware version from the Band"), (Func<Task<IDynamicBandConstants>>) (() => Task.FromResult<IDynamicBandConstants>(AppBandConstants.Envoy)), true);
    }

    public Task<DiagnosticsBandDevice> GetDiagnosticsInfoAsync(
      CancellationToken token)
    {
      DiagnosticsBandDevice result = new DiagnosticsBandDevice();
      result.Type = "Mock Microsoft Band";
      result.SerialNumber = "0000000001";
      result.UniqueId = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE";
      result.Versions = new DiagnosticsBandDeviceVersions()
      {
        PcbId = 4,
        Application = "10.2.2818.0",
        Bootloader = "10.2.2818.1",
        Updater = "10.2.2818.2"
      };
      return Task.FromResult<DiagnosticsBandDevice>(result);
    }

    public Task<string> UploadFileToCloudAsync(
      IFile file,
      LogFileTypes fileType,
      CancellationToken token)
    {
      MockBandConnection.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Uploading file to cloud"), (Func<Task>) (() => Task.Delay(500, token)));
      return Task.FromResult<string>(DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssfff"));
    }

    public Task<IUserProfile> GetUserProfileAsync(
      CancellationToken cancellationToken)
    {
      return Task.FromResult<IUserProfile>((IUserProfile) new MockUserProfile((ushort) 1)
      {
        UserID = Guid.Parse("1FA35A46-5859-458F-94A5-F06ECFFA4BBE"),
        ApplicationSettings = new ApplicationSettings()
      });
    }

    public Task<bool> GetDeviceAndProfileLinkedAsync(
      CancellationToken cancellationToken,
      Guid userId,
      Guid deviceId,
      IBandInfo band,
      bool useCloudClient,
      bool throwExceptions)
    {
      return Task.FromResult<bool>(true);
    }

    public Task SetWorkoutActivitiesAsync(
      IList<WorkoutActivity> activities,
      CancellationToken cancellationToken)
    {
      return (Task) Task.FromResult<bool>(true);
    }

    public Task SendCallNotificationAsync(
      CallNotification callNotification,
      CancellationToken cancellationToken)
    {
      return (Task) Task.FromResult<bool>(true);
    }

    public Task QueueSmsNotificationAsync(
      BandMessage message,
      CancellationToken cancellationToken)
    {
      return (Task) Task.FromResult<bool>(true);
    }

    public void Dispose() => this.IsDisposed = true;

    public Task NotifyOfSuspendAsync(CancellationToken cancellationToken) => (Task) Task.FromResult<bool>(true);

    public void NotifyOfResume()
    {
    }

    public Task<IBandInfo> GetPrimaryPairedBandAsync() => throw new NotImplementedException();

    public Task<IBandInfo[]> GetPairedBandsAsync() => throw new NotImplementedException();

    public Task SendTileMessageAsync(
      Guid tileId,
      TileMessage message,
      CancellationToken cancellationToken)
    {
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
