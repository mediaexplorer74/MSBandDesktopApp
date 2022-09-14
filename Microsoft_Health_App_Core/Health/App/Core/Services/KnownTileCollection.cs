// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.KnownTileCollection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.Bike;
using Microsoft.Health.App.Core.ViewModels.Calories;
using Microsoft.Health.App.Core.ViewModels.Golf;
using Microsoft.Health.App.Core.ViewModels.Hike;
using Microsoft.Health.App.Core.ViewModels.Run;
using Microsoft.Health.App.Core.ViewModels.Sleep;
using Microsoft.Health.App.Core.ViewModels.Steps;
using Microsoft.Health.App.Core.ViewModels.WeightTracking;
using Microsoft.Health.App.Core.ViewModels.Workouts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Services
{
  public sealed class KnownTileCollection : 
    IKnownTileCollection,
    IEnumerable<AppBandTile>,
    IEnumerable
  {
    private static readonly BandColor ThirdPartyBaseColor = new BandColor((byte) 32, (byte) 32, (byte) 32);
    private static readonly AppBandTile StepsTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Steps,
      TileGlyph = "\uE008",
      TileViewModelType = typeof (StepsTileViewModel),
      EnablementText = AppResources.StepsTile_Enablement,
      ShowInSettings = false
    };
    private static readonly AppBandTile CaloriesTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Calories,
      TileGlyph = "\uE009",
      TileViewModelType = typeof (CaloriesTileViewModel),
      EnablementText = AppResources.CaloriesTile_Enablement,
      ShowInSettings = false
    };
    private static readonly AppBandTile MessagingTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Messaging,
      TileGlyph = "\uE071",
      EnablementText = AppResources.MessagingTile_Enablement,
      TileId = Guid.Parse("b4edbc35-027b-4d10-a797-1099cd2ad98a"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Messaging"
      },
      IsDefaultTile = true,
      RequiresSms = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 0 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 0 }
        }
      }
    };
    private static readonly AppBandTile MailTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Mail,
      BandIcon = AppBandIcon.MailTile,
      TileGlyph = "\uE073",
      EnablementText = AppResources.MailTile_Enablement,
      TileId = Guid.Parse("823ba55a-7c98-4261-ad5e-929031289c6e"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Email"
      },
      ShowTile = false,
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            DefaultOrder = 1,
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.MailBadge
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            DefaultOrder = 1,
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.MailBadge,
              AppBandIcon.MailNotification
            }
          }
        }
      }
    };
    private static readonly AppBandTile CortanaTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Cortana,
      BandIcon = AppBandIcon.CortanaTile,
      TileGlyph = "\uE096",
      TileOverlayGlyph = "\uE097",
      TileOverlayGlyphColor = new ArgbColor32(1728053247U),
      EnablementText = AppResources.CortanaTile_Enablement,
      TileId = Guid.Parse("d7fb5ff5-906a-4f2c-8269-dde6a75138c4"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Cortana Instructions"
      },
      ShowTile = false,
      IsDefaultTile = true,
      RequiresCortana = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            DefaultOrder = 2,
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.CortanaBadge
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            DefaultOrder = 2,
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.CortanaBadge,
              AppBandIcon.CortanaNotification
            }
          }
        }
      }
    };
    private static readonly AppBandTile PhoneTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Phone,
      TileGlyph = "\uE078",
      EnablementText = AppResources.PhoneTile_Enablement,
      TileId = Guid.Parse("22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Calls"
      },
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 3 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 3 }
        }
      }
    };
    private static readonly AppBandTile CalendarTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Calendar,
      TileGlyph = "\uE072",
      EnablementText = AppResources.CalendarTile_Enablement,
      TileId = Guid.Parse("ec149021-ce45-40e9-aeee-08f86e4746a7"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Calendar"
      },
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 4 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 4 }
        }
      }
    };
    private static readonly AppBandTile RunTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Run,
      TileGlyph = "\uE004",
      TileViewModelType = typeof (RunTileViewModel),
      EnablementText = AppResources.RunTile_Enablement,
      TileId = Guid.Parse("65bd93db-4293-46af-9a28-bdd6513b4677"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Fitness",
        "Run"
      },
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 5 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 5 }
        }
      }
    };
    private static readonly AppBandTile BikeTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Bike,
      TileGlyph = "\uE133",
      TileViewModelType = typeof (BikeTileViewModel),
      EnablementText = AppResources.BikeTile_Enablement,
      TileId = Guid.Parse("96430fcb-0060-41cb-9de2-e00cac97f85d"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Fitness",
        "Bike"
      },
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 6 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 6 }
        }
      }
    };
    private static readonly AppBandTile HikeTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Hike,
      TileGlyph = "\uE407",
      TileViewModelType = typeof (HikeTileViewModel),
      TileId = Guid.Parse("e9e376af-fa3d-486d-8351-959cc20f4d8f"),
      HasSettings = false,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Fitness",
        "Hike"
      },
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 11 }
        }
      }
    };
    private static readonly AppBandTile GolfTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Golf,
      TileGlyph = "\uE151",
      TileViewModelType = typeof (GolfTileViewModel),
      EnablementText = AppResources.GolfTile_Enablement,
      TileId = Guid.Parse("fb9d005a-c3da-49d4-8e7b-c6f674fc4710"),
      HasSettings = false,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Fitness",
        "Golf"
      },
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 11 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 12 }
        }
      }
    };
    private static readonly AppBandTile ExerciseTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Exercise,
      TileGlyph = "\uE002",
      TileViewModelType = typeof (ExerciseTileViewModel),
      EnablementText = AppResources.ExerciseTile_Enablement,
      TileId = Guid.Parse("a708f02a-03cd-4da0-bb33-be904e6a2924"),
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Fitness",
        "Exercise"
      },
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 7 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 7 }
        }
      }
    };
    private static readonly AppBandTile SleepTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Sleep,
      TileGlyph = "\uE005",
      TileViewModelType = typeof (SleepTileViewModel),
      EnablementText = AppResources.SleepTile_Enablement,
      TileId = Guid.Parse("23e7bc94-f90d-44e0-843f-250910fdf74e"),
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 8 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 8 }
        }
      }
    };
    private static readonly AppBandTile AlarmTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Alarm,
      TileGlyph = "\uE023",
      EnablementText = AppResources.AlarmTile_Enablement,
      TileId = Guid.Parse("d36a92ea-3e85-4aed-a726-2898a6f2769b"),
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 9 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 9 }
        }
      }
    };
    private static readonly AppBandTile GuidedWorkoutResultTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_GuidedWorkout,
      TileGlyph = "\uE003",
      TileViewModelType = typeof (GuidedWorkoutResultTileViewModel),
      EnablementText = AppResources.GuidedWorkoutTile_Enablement,
      TileId = Guid.Parse("0281c878-afa8-40ff-acfd-bca06c5c4922"),
      IsDefaultTile = true,
      DefaultOnSettings = AdminTileSettings.None,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData() { DefaultOrder = 10 }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData() { DefaultOrder = 10 }
        }
      }
    };
    private static readonly AppBandTile WeatherTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Weather,
      BandIcon = AppBandIcon.WeatherClearDay,
      TileGlyph = "\uE079",
      Layouts = (IList<string>) new List<string>()
      {
        "last_updated_0",
        "weather_1"
      },
      EnablementText = AppResources.WeatherTile_Enablement,
      TileId = Guid.Parse("69a39b4e-084b-4b53-9a1b-581826df9e36"),
      RequiresLocationServices = true,
      DefaultOnSettings = AdminTileSettings.None,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.WeatherClearNight,
              AppBandIcon.WeatherCloudy,
              AppBandIcon.WeatherRain,
              AppBandIcon.WeatherLightning,
              AppBandIcon.WeatherRainSnow,
              AppBandIcon.WeatherSnow,
              AppBandIcon.WeatherFog,
              AppBandIcon.WeatherSmoke,
              AppBandIcon.WeatherSquall
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.WeatherClearNight,
              AppBandIcon.WeatherCloudy,
              AppBandIcon.WeatherRain,
              AppBandIcon.WeatherLightning,
              AppBandIcon.WeatherRainSnow,
              AppBandIcon.WeatherSnow,
              AppBandIcon.WeatherFog,
              AppBandIcon.WeatherSmoke,
              AppBandIcon.WeatherSquall
            }
          }
        }
      }
    };
    private static readonly AppBandTile StocksTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Stocks,
      BandIcon = AppBandIcon.StocksTile,
      TileGlyph = "\uE080",
      Layouts = (IList<string>) new List<string>()
      {
        "last_updated_0",
        "finance_down_1",
        "finance_up_2"
      },
      EnablementText = AppResources.StocksTile_Enablement,
      TileId = Guid.Parse("5992928a-bd79-4bb5-9678-f08246d03e68"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Stocks"
      },
      DefaultOnSettings = AdminTileSettings.None,
      DefaultOffSettings = AdminTileSettings.None,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.StocksUp,
              AppBandIcon.StocksDown
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.StocksUp,
              AppBandIcon.StocksDown
            }
          }
        }
      }
    };
    private static readonly AppBandTile UVTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_UV,
      TileGlyph = "\uE091",
      EnablementText = AppResources.UVTile_Enablement,
      TileId = Guid.Parse("59976cf5-15c8-4799-9e31-f34c765a6bd1"),
      IsDefaultTile = false,
      DefaultOnSettings = AdminTileSettings.EnableNotification,
      DefaultOffSettings = AdminTileSettings.None
    };
    private static readonly AppBandTile StarbucksTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Starbucks,
      BandIcon = AppBandIcon.StarbucksTile,
      TileGlyph = "\uE101",
      TileGlyphColor = new ArgbColor32(4278215489U),
      TileOverlayGlyph = "\uE127",
      TileOverlayGlyphColor = new ArgbColor32(uint.MaxValue),
      EnablementText = AppResources.StarbucksTile_Enablement,
      TileId = Guid.Parse("64a29f65-70bb-4f32-99a2-0f250a05d427"),
      ShowTile = false,
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Starbucks"
      },
      Layouts = (IList<string>) new List<string>()
      {
        "starbucks_card_barcode_layout",
        "starbucks_no_card_layout"
      },
      DefaultOnSettings = AdminTileSettings.None,
      DefaultOffSettings = AdminTileSettings.None
    };
    private static readonly AppBandTile FacebookTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Facebook,
      BandIcon = AppBandIcon.FacebookTile,
      TileGlyph = "\uE083",
      EnablementText = AppResources.FacebookTile_Enablement,
      TileId = Guid.Parse("fd06b486-bbda-4da5-9014-124936386237"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Facebook"
      },
      ShowTile = false,
      PaletteOverride = new TileColorPaletteOverride()
      {
        Base = new BandColor?(KnownTileCollection.ThirdPartyBaseColor),
        Highlight = new BandColor?(new BandColor((byte) 70, (byte) 104, (byte) 178))
      },
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.FacebookBadge
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.FacebookBadge,
              AppBandIcon.FacebookNotification
            }
          }
        }
      }
    };
    private static readonly AppBandTile FBMessengerTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_FacebookMessenger,
      BandIcon = AppBandIcon.FacebookMessengerTile,
      TileGlyph = "\uE120",
      EnablementText = AppResources.FacebookMessengerTile_Enablement,
      TileId = Guid.Parse("76b08699-2f2e-9041-96c2-1f4bfc7eab10"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Facebook Messenger"
      },
      ShowTile = false,
      PaletteOverride = new TileColorPaletteOverride()
      {
        Base = new BandColor?(KnownTileCollection.ThirdPartyBaseColor)
      },
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.FacebookMessengerBadge
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.FacebookMessengerBadge,
              AppBandIcon.FacebookMessengerNotification
            }
          }
        }
      }
    };
    private static readonly AppBandTile TwitterTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Twitter,
      BandIcon = AppBandIcon.TwitterTile,
      TileGlyph = "\uE084",
      EnablementText = AppResources.TwitterTile_Enablement,
      TileId = Guid.Parse("2e76a806-f509-4110-9c03-43dd2359d2ad"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Twitter"
      },
      ShowTile = false,
      PaletteOverride = new TileColorPaletteOverride()
      {
        Base = new BandColor?(KnownTileCollection.ThirdPartyBaseColor),
        Highlight = new BandColor?(new BandColor((byte) 0, (byte) 178, (byte) 242))
      },
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.TwitterBadge
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.TwitterBadge,
              AppBandIcon.TwitterNotification
            }
          }
        }
      }
    };
    private static readonly AppBandTile FeedTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Feed,
      BandIcon = AppBandIcon.FeedTile,
      TileGlyph = "\uE187",
      EnablementText = AppResources.FeedTile_Enablement,
      TileId = Guid.Parse("4076b009-0455-4af7-a705-6d4acd45a556"),
      HasSettings = true,
      SettingsPageTaxonomy = (IReadOnlyList<string>) new List<string>()
      {
        "Settings",
        "Manage Tiles",
        "Notifications",
        "Notification Center"
      },
      ShowTile = false,
      DefaultOnSettings = AdminTileSettings.EnableNotification | AdminTileSettings.EnableBadging,
      DefaultOffSettings = AdminTileSettings.EnableBadging,
      BandTileBandClassData = (IDictionary<BandClass, BandTileBandClassData>) new Dictionary<BandClass, BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.FeedBadge
            }
          }
        },
        {
          BandClass.Envoy,
          new BandTileBandClassData()
          {
            ExtraIcons = (IList<AppBandIcon>) new List<AppBandIcon>()
            {
              AppBandIcon.FeedBadge,
              AppBandIcon.FeedNotification
            }
          }
        }
      }
    };
    private static readonly AppBandTile WeightTile = new AppBandTile()
    {
      Title = AppResources.TileTitle_Weight,
      TileGlyph = "\uE203",
      TileViewModelType = typeof (WeightTileViewModel),
      ShowInSettings = false
    };
    private static readonly List<AppBandTile> TilesList = new List<AppBandTile>()
    {
      KnownTileCollection.StepsTile,
      KnownTileCollection.CaloriesTile,
      KnownTileCollection.MessagingTile,
      KnownTileCollection.MailTile,
      KnownTileCollection.CortanaTile,
      KnownTileCollection.PhoneTile,
      KnownTileCollection.CalendarTile,
      KnownTileCollection.RunTile,
      KnownTileCollection.BikeTile,
      KnownTileCollection.HikeTile,
      KnownTileCollection.GolfTile,
      KnownTileCollection.ExerciseTile,
      KnownTileCollection.SleepTile,
      KnownTileCollection.AlarmTile,
      KnownTileCollection.GuidedWorkoutResultTile,
      KnownTileCollection.WeatherTile,
      KnownTileCollection.StocksTile,
      KnownTileCollection.UVTile,
      KnownTileCollection.StarbucksTile,
      KnownTileCollection.FacebookTile,
      KnownTileCollection.FBMessengerTile,
      KnownTileCollection.TwitterTile,
      KnownTileCollection.FeedTile,
      KnownTileCollection.WeightTile
    };
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly ISupportedTileService supportedTileService;

    public KnownTileCollection(
      IDynamicConfigurationService dynamicConfigurationService,
      ISupportedTileService supportedTileService)
    {
      Assert.ParamIsNotNull((object) dynamicConfigurationService, nameof (dynamicConfigurationService));
      Assert.ParamIsNotNull((object) supportedTileService, nameof (supportedTileService));
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.supportedTileService = supportedTileService;
    }

    AppBandTile IKnownTileCollection.StepsTile => KnownTileCollection.StepsTile;

    AppBandTile IKnownTileCollection.CaloriesTile => KnownTileCollection.CaloriesTile;

    AppBandTile IKnownTileCollection.MessagingTile => KnownTileCollection.MessagingTile;

    AppBandTile IKnownTileCollection.MailTile => KnownTileCollection.MailTile;

    AppBandTile IKnownTileCollection.CortanaTile => KnownTileCollection.CortanaTile;

    AppBandTile IKnownTileCollection.PhoneTile => KnownTileCollection.PhoneTile;

    AppBandTile IKnownTileCollection.CalendarTile => KnownTileCollection.CalendarTile;

    AppBandTile IKnownTileCollection.RunTile => KnownTileCollection.RunTile;

    AppBandTile IKnownTileCollection.BikeTile => KnownTileCollection.BikeTile;

    AppBandTile IKnownTileCollection.GolfTile => KnownTileCollection.GolfTile;

    AppBandTile IKnownTileCollection.ExerciseTile => KnownTileCollection.ExerciseTile;

    AppBandTile IKnownTileCollection.SleepTile => KnownTileCollection.SleepTile;

    AppBandTile IKnownTileCollection.AlarmTile => KnownTileCollection.AlarmTile;

    AppBandTile IKnownTileCollection.GuidedWorkoutResultTile => KnownTileCollection.GuidedWorkoutResultTile;

    AppBandTile IKnownTileCollection.WeatherTile => KnownTileCollection.WeatherTile;

    AppBandTile IKnownTileCollection.StocksTile => KnownTileCollection.StocksTile;

    AppBandTile IKnownTileCollection.UVTile => KnownTileCollection.UVTile;

    AppBandTile IKnownTileCollection.StarbucksTile => KnownTileCollection.StarbucksTile;

    AppBandTile IKnownTileCollection.FacebookTile => KnownTileCollection.FacebookTile;

    AppBandTile IKnownTileCollection.FBMessengerTile => KnownTileCollection.FBMessengerTile;

    AppBandTile IKnownTileCollection.TwitterTile => KnownTileCollection.TwitterTile;

    AppBandTile IKnownTileCollection.FeedTile => KnownTileCollection.FeedTile;

    AppBandTile IKnownTileCollection.WeightTile => KnownTileCollection.WeightTile;

    AppBandTile IKnownTileCollection.HikeTile => KnownTileCollection.HikeTile;

    public IList<AppBandTile> DisabledTiles
    {
      get
      {
        List<AppBandTile> appBandTileList = new List<AppBandTile>();
        if (!this.dynamicConfigurationService.Configuration.Features.Starbucks.IsEnabled)
          appBandTileList.Add(KnownTileCollection.StarbucksTile);
        if (!this.dynamicConfigurationService.Configuration.Features.Facebook.IsEnabled)
          appBandTileList.Add(KnownTileCollection.FacebookTile);
        if (!this.dynamicConfigurationService.Configuration.Features.FacebookMessenger.IsEnabled)
          appBandTileList.Add(KnownTileCollection.FBMessengerTile);
        if (!this.dynamicConfigurationService.Configuration.Features.Twitter.IsEnabled)
          appBandTileList.Add(KnownTileCollection.TwitterTile);
        if (!this.dynamicConfigurationService.Configuration.Features.Finance.IsEnabled)
          appBandTileList.Add(KnownTileCollection.StocksTile);
        if (!this.dynamicConfigurationService.Configuration.Features.Hike.IsEnabled)
          appBandTileList.Add(KnownTileCollection.HikeTile);
        return (IList<AppBandTile>) appBandTileList;
      }
    }

    public IEnumerator<AppBandTile> GetEnumerator() => KnownTileCollection.TilesList.Except<AppBandTile>((IEnumerable<AppBandTile>) this.DisabledTiles).Where<AppBandTile>(new Func<AppBandTile, bool>(this.supportedTileService.IsSupportedByPlatform)).GetEnumerator();

    public static bool IsDefaultTile(Guid tileId)
    {
      AppBandTile appBandTile = KnownTileCollection.TilesList.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (t => t.TileId == tileId));
      return (object) appBandTile != null && appBandTile.IsDefaultTile;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
