// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.CoreIoc
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Geolocator.Plugin;
using Geolocator.Plugin.Abstractions;
using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Http;
using Microsoft.Health.App.Core.Http.Clients.LiveLogin;
using Microsoft.Health.App.Core.Mocks;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.App.Core.Providers.Hike;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Services.Diagnostics;
using Microsoft.Health.App.Core.Services.NetPromoterScore;
using Microsoft.Health.App.Core.Services.Pedometer;
using Microsoft.Health.App.Core.Services.PushNotifications;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Services.Test;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.Utilities.Logging;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.Bike;
using Microsoft.Health.App.Core.ViewModels.Calories;
using Microsoft.Health.App.Core.ViewModels.Golf;
using Microsoft.Health.App.Core.ViewModels.Hike;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.Run;
using Microsoft.Health.App.Core.ViewModels.Sleep;
using Microsoft.Health.App.Core.ViewModels.Steps;
using Microsoft.Health.App.Core.ViewModels.Themes;
using Microsoft.Health.App.Core.ViewModels.WeightTracking;
using Microsoft.Health.App.Core.ViewModels.Workouts;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Authentication;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using Microsoft.Health.Cloud.Client.DynamicConfiguration;
using Microsoft.Health.Cloud.Client.Events.Golf;
using Microsoft.Health.Cloud.Client.Events.Golf.Courses;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Services;
using Microsoft.Health.Cloud.Client.Tracing;
using CommonServiceLocator;//using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PCLStorage;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core
{
  public static class CoreIoc
  {
    public static void RegisterAppTypes(UnityContainer container)
    {
      Assert.ParamIsNotNull((object) container, nameof (container));
      CoreIoc.RegisterCommonTypes(container);
      CoreIoc.RegisterAppServices(container);
      CoreIoc.RegisterAppCloudClients(container);
      CoreIoc.RegisterAppProviders(container);
      CoreIoc.RegisterViewModels(container);
      CoreIoc.RegisterOtherAppTypes(container);
    }

    public static void RegisterCommonTypes(UnityContainer container)
    {
      container.RegisterType<IRefreshService, RefreshService>(CoreIoc.Singleton);
      container.RegisterType<IConfigProvider, ConfigProvider>(CoreIoc.Singleton);
      container.RegisterType<IConfig, Microsoft.Health.App.Core.Config.Config>(CoreIoc.Singleton);
      container.RegisterType<IFileObjectStorageService, FileObjectStorageService>(CoreIoc.Singleton);
      container.RegisterType<IFileSystem>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) FileSystem.Current)));
      container.RegisterType<IConnectivity>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) CrossConnectivity.Current)));
      container.RegisterType<INetworkService, NetworkService>(CoreIoc.Singleton);
      container.RegisterType<IGeolocationService, GeolocationService>(CoreIoc.Singleton);
      container.RegisterType<IOobeService, OobeService>(CoreIoc.Singleton);
      container.RegisterType<IGeolocator>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) CrossGeolocator.Current)));
      container.RegisterType<ICultureService, CultureService>(CoreIoc.Singleton);
      container.RegisterType<IRegionService, RegionService>(CoreIoc.Singleton);
      container.RegisterType<ICoachingService, CoachingService>(CoreIoc.Singleton);
      container.RegisterType<IUserService, UserService>(CoreIoc.Singleton);
      container.RegisterType<INotificationCenterConfigurationService, NotificationCenterConfigurationService>(CoreIoc.Singleton);
      container.RegisterType<ISerializationService, JsonSerializationService>(CoreIoc.Singleton);
      container.RegisterType<IOAuthMsaTokenStore, OAuthMsaTokenStore>(CoreIoc.Singleton);
      container.RegisterType<FirmwareUpdateService>(CoreIoc.Singleton);
      container.RegisterType<IFirmwareUpdateService, FirmwareUpdateService>(CoreIoc.Singleton);
      container.RegisterType<IPerfLogger, PerfLogger>(CoreIoc.Singleton);
      container.RegisterType<ISensorDataClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<IHealthCloudClient>().SensorData)));
      container.RegisterType<IDeviceClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<IHealthCloudClient>().Devices)));
      container.RegisterType<IWellnessPlanClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<IHealthCloudClient>().WellnessPlan)));
      container.RegisterType<ICoachingProvider, CoachingProvider>(CoreIoc.Singleton);
      container.RegisterType<IPedometerSyncManager, PedometerSyncManager>(CoreIoc.Singleton);
      container.RegisterType<IPedometerLogger, PedometerLogger>(CoreIoc.Singleton);
      container.RegisterType<IDevice, PedometerDevice>("Pedometer");
      container.RegisterType<ITestConfigurationService, TestConfigurationService>(CoreIoc.Singleton);
      container.RegisterType<IHttpMessageHandlerFactory, HttpMessageHandlerFactory>(CoreIoc.Singleton);
      container.RegisterType<IWeatherService, WeatherService>();
      container.RegisterType<IFinanceService, FinanceService>();
      container.RegisterType<ITileUpdateService, TileUpdateService>();
      container.RegisterType<ITileManagementService, TileManagementService>(CoreIoc.Singleton);
      container.RegisterType<IBandThemeManager, BandThemeManager>(CoreIoc.Singleton);
      container.RegisterType<IKnownTileCollection, KnownTileCollection>(CoreIoc.Singleton);
      container.RegisterType<IMicrosoftBandUserAgentService, MicrosoftBandUserAgentService>(CoreIoc.Singleton);
      container.RegisterType<IUserAgentService>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<IMicrosoftBandUserAgentService>())));
      container.RegisterType<IServiceLocator>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) ServiceLocator.Current)));
      container.RegisterType<IConfigurationService, ConfigurationService>(CoreIoc.Singleton);
      container.RegisterType<ISensorUploader, SensorUploader>(CoreIoc.Singleton);
      container.RegisterType<IHttpTracer>("HealthDiscoveryClient", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) new CloudTracingInterceptor("Health Discovery"))));
      container.RegisterType<IHttpTracer>("HealthCloudClient", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) new CloudTracingInterceptor("Health Cloud"))));
      container.RegisterType<IHttpTracer>("BingHealthAndFitness", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) new CloudTracingInterceptor("Bing Health And Fitness"))));
      container.RegisterType<IDateTimeService, DateTimeService>(CoreIoc.Singleton);
      container.RegisterType<ConnectionInfoProvider>(CoreIoc.Singleton);
      container.RegisterType<IConnectionInfoProvider, ConnectionInfoProvider>(CoreIoc.Singleton);
      container.RegisterType<IKatTrackingService, ConnectionInfoProvider>(CoreIoc.Singleton);
      container.RegisterType<IConnectionInfoStore, ConnectionInfoStore>(CoreIoc.Singleton, (InjectionMember) new InjectionProperty("Caching", (object) true));
      container.RegisterType<IBandConnectionFactory, BandConnectionFactory>(CoreIoc.Singleton);
      container.RegisterType<IBandInfoService, BandInfoService>(CoreIoc.Singleton);
      container.RegisterType<IBandHardwareService, BandHardwareService>(CoreIoc.Singleton);
      container.RegisterType<ICurrentDynamicConfigurationFileStore, CurrentDynamicConfigurationFileStore>(CoreIoc.Singleton);
      container.RegisterType<IDynamicConfigurationFileStore, RemoteDynamicConfigurationFileStore>("RemoteConfigurationFileStore", CoreIoc.Singleton);
      container.RegisterType<IDynamicConfigurationFileStore, DefaultDynamicConfigurationFileStore>("DefaultConfigurationFileStore", CoreIoc.Singleton);
      container.RegisterType<IDynamicConfigurationUpdateService, DynamicConfigurationUpdateService>(CoreIoc.Singleton);
      container.RegisterType<IDynamicConfigurationService, DynamicConfigurationService>(CoreIoc.Singleton);
      container.RegisterInstance<IConfigurationValue>(CurrentDynamicConfigurationFileStore.IsCurrentFileStoreEnabled.UniqueName, (IConfigurationValue) CurrentDynamicConfigurationFileStore.IsCurrentFileStoreEnabled);
      container.RegisterInstance<IConfigurationValue>(RemoteDynamicConfigurationFileStore.IsRemoteFileStoreEnabled.UniqueName, (IConfigurationValue) RemoteDynamicConfigurationFileStore.IsRemoteFileStoreEnabled);
      container.RegisterInstance<IConfigurationValue>(DefaultDynamicConfigurationFileStore.IsDefaultFileStoreEnabled.UniqueName, (IConfigurationValue) DefaultDynamicConfigurationFileStore.IsDefaultFileStoreEnabled);
      container.RegisterType<ICalendarTileAggregationService, CalendarTileAggregationService>();
      container.RegisterType<ICalendarTileUpdateListener>("CalendarUpdate", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<ICalendarService>())));
      container.RegisterType<ICalendarTileUpdateListener>("CoachingPlanUpdate", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<ICoachingService>())));
      container.RegisterType<IEventWaitHandleService, EventWaitHandlerService>(CoreIoc.Singleton);
      container.RegisterType<IDebugReporterService, DebugReporterService>(CoreIoc.Singleton);
      container.RegisterType<IDeepLinkRouter, DeepLinkRouter>(CoreIoc.Singleton);
      CoreIoc.RegisterLiveLoginClient(container);
    }

    private static void RegisterAppServices(UnityContainer container)
    {
      container.RegisterType<ISmoothNavService, SmoothNavService>(CoreIoc.Singleton);
      container.RegisterType<IEventTrackingService, EventTrackingService>(CoreIoc.Singleton);
      container.RegisterType<IFormattingService, FormattingService>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) new FormattingService(c.Resolve<IUserProfileService>()))));
      container.RegisterType<ExerciseSyncService>(CoreIoc.Singleton);
      container.RegisterType<IExerciseSyncService>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<ExerciseSyncService>())));
      container.RegisterType<IGolfSyncService, GolfSyncService>(CoreIoc.Singleton);
      container.RegisterType<IShakeFeedbackService, ShakeFeedbackService>(CoreIoc.Singleton);
      container.RegisterType<IGolfCourseFilterService, GolfCourseFilterService>(CoreIoc.Singleton);
      container.RegisterType<IWhatsNewService, WhatsNewService>(CoreIoc.Singleton);
      container.RegisterType<INetPromoterService, NetPromoterService>(CoreIoc.Singleton);
      container.RegisterType<IHeartRateService, HeartRateService>(CoreIoc.Singleton);
      container.RegisterType<IReportProblemStore, ReportProblemStore>(CoreIoc.Singleton);
      container.RegisterType<INpsStore, NpsStore>(CoreIoc.Singleton);
      container.RegisterType<IFiddlerLogService, FiddlerLogService>(CoreIoc.Singleton);
      container.RegisterType<IAppUpgradeService, AppUpgradeService>(CoreIoc.Singleton);
      container.RegisterType<HttpCacheService>(CoreIoc.Singleton);
      container.RegisterType<IHttpCacheService>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<HttpCacheService>())));
      container.RegisterType<IDebuggableHttpCacheService>(CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<HttpCacheService>())));
      container.RegisterType<ISQLiteSingleConnection, SQLiteSingleConnection>();
      container.RegisterType<IHttpCacheDatabase, HttpCacheDatabase>(CoreIoc.Singleton);
      container.RegisterType<IHttpCacheItem, HttpCacheItem>();
      container.RegisterType<INetPromoterScoreService, NetPromoterScoreService>(CoreIoc.Singleton);
      container.RegisterType<IPushNotificationService, PushNotificationService>(CoreIoc.Singleton);
      container.RegisterType<ICloudNotificationUpdateService, CloudNotificationUpdateService>(CoreIoc.Singleton);
      container.RegisterType<IFreshInstallService, FreshInstallService>(CoreIoc.Singleton);
      container.RegisterType<INetPromoterSettingsService, NetPromoterSettingsService>(CoreIoc.Singleton);
      container.RegisterType<IUserProfileService, UserProfileService>(CoreIoc.Singleton);
      container.RegisterType<IDeviceManager, DeviceManager>(CoreIoc.Singleton);
      container.RegisterType<IDevice, BandDevice>("Band");
      container.RegisterType<IAppUpgradeListener>("FirmwareUpdate", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<FirmwareUpdateService>())));
      container.RegisterType<IAppUpgradeListener>("HttpCache", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<HttpCacheService>())));
      container.RegisterType<IAppUpgradeListener>("ExerciseSync", CoreIoc.Singleton, (InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<ExerciseSyncService>())));
    }

    private static void RegisterViewModels(UnityContainer container)
    {
      container.RegisterType<ApplicationThemeViewModelBase, ApplicationThemeViewModel>(CoreIoc.Singleton);
      container.RegisterType<BandThemeViewModelBase, BandThemeViewModel>(CoreIoc.Singleton);
      container.RegisterType<MetricTileViewModel, StepsTileViewModel>("Steps");
      container.RegisterType<MetricTileViewModel, CaloriesTileViewModel>("Calories");
      container.RegisterType<MetricTileViewModel, GuidedWorkoutCalendarTileViewModel>("GuidedWorkoutsCalendar");
      container.RegisterType<MetricTileViewModel, RunTileViewModel>(EventType.Running.ToString());
      container.RegisterType<MetricTileViewModel, BikeTileViewModel>(EventType.Biking.ToString());
      container.RegisterType<MetricTileViewModel, GolfTileViewModel>(EventType.Golf.ToString());
      container.RegisterType<MetricTileViewModel, SleepTileViewModel>(EventType.Sleeping.ToString());
      container.RegisterType<MetricTileViewModel, ExerciseTileViewModel>(EventType.Workout.ToString());
      container.RegisterType<MetricTileViewModel, GuidedWorkoutResultTileViewModel>(EventType.GuidedWorkout.ToString());
      container.RegisterType<MetricTileViewModel, WorkoutPlanDetailTileViewModel>("WorkoutPlanDetail");
      container.RegisterType<MetricTileViewModel, WorkoutDetailTileViewModel>("WorkoutDetail");
      container.RegisterType<MetricTileViewModel, WeightTileViewModel>("Weight");
      container.RegisterType<MetricTileViewModel, CustomTileViewModel>("Custom");
      container.RegisterType<MetricTileViewModel, HikeTileViewModel>(EventType.Hike.ToString());
      container.RegisterType<IProfileFieldsViewModel, ProfileFieldsViewModel>();
      container.RegisterType<NavShellViewModel>(CoreIoc.Singleton);
    }

    private static void RegisterAppProviders(UnityContainer container)
    {
      container.RegisterType<IInsightsProvider>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => c.Resolve<IConfig>().IsMockInsightsEnabled ? (object) c.Resolve<MockInsightsProvider>() : (object) c.Resolve<InsightsProvider>())));
      container.RegisterType<ISleepProvider, SleepProvider>();
      container.RegisterType<IGoalsProvider, GoalsProvider>();
      container.RegisterType<IRouteBasedExerciseEventProvider<RunEvent>, RunProvider>();
      container.RegisterType<IRouteBasedExerciseEventProvider<BikeEvent>, BikeProvider>();
      container.RegisterType<IExerciseProvider, ExerciseProvider>();
      container.RegisterType<IHistoryProvider, HistoryProvider>();
      container.RegisterType<IBestEventProvider, BestEventProvider>();
      container.RegisterType<IConnectedAppsProvider, ConnectedAppsProvider>();
      container.RegisterType<IWorkoutsProvider, ForegroundWorkoutsProvider>(CoreIoc.Singleton);
      container.RegisterType<IGolfCourseProvider, GolfCourseProvider>(CoreIoc.Singleton);
      container.RegisterType<IGolfRoundProvider, GolfRoundProvider>(CoreIoc.Singleton);
      container.RegisterType<IWeightProvider, WeightProvider>();
      container.RegisterType<IRouteBasedExerciseEventProvider<HikeEvent>, HikeProvider>();
      container.RegisterType<IUserDailySummaryProvider, UserDailySummaryProvider>(CoreIoc.Singleton);
    }

    private static void RegisterAppCloudClients(UnityContainer container)
    {
      CoreIoc.RegisterDiscoveryClient(container);
      CoreIoc.RegisterPodClient(container);
      CoreIoc.RegisterDynamicConfigurationClient(container);
      CoreIoc.RegisterHealthAndFitnessClient(container, true);
      container.RegisterType<IGolfClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<IHealthCloudClient>().Golf)));
      container.RegisterType<IGolfCourseClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) c.Resolve<IHealthCloudClient>().GolfCourses)));
    }

    private static void RegisterDynamicConfigurationClient(UnityContainer container) => container.RegisterType<IDynamicConfigurationClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c =>
    {
      IConnectionInfoProvider connectionInfoProvider = c.Resolve<IConnectionInfoProvider>();
      ITestConfigurationService testService = c.Resolve<ITestConfigurationService>();
      HttpMessageHandler httpMessageHandler = HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging().WithRequestId().WithPodAuthorization(connectionInfoProvider, true).WithCulture(c.Resolve<ICultureService>()).WithUserAgent((IUserAgentService) c.Resolve<IMicrosoftBandUserAgentService>());
      if (testService.IsEnabled)
        httpMessageHandler = httpMessageHandler.WithTestHeader(testService);
      return (object) new DynamicConfigurationClient(httpMessageHandler, (Func<CancellationToken, Task<Uri>>) (async cancellationToken =>
      {
          return (await connectionInfoProvider.GetConnectionInfoAsync(cancellationToken, true).ConfigureAwait(false)).FusEndpoint;
      }));
    })));

    private static void RegisterDiscoveryClient(UnityContainer container) => container.RegisterType<IHealthDiscoveryClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c =>
    {
      IConfig config = c.Resolve<IConfig>();
      ITestConfigurationService testService = c.Resolve<ITestConfigurationService>();
      HttpMessageHandler httpMessageHandler = HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging().WithRequestId().WithMsaAuthorization(c.Resolve<IMsaTokenProvider>(), true).WithCulture(c.Resolve<ICultureService>()).WithRegion(c.Resolve<IRegionService>()).WithUserAgent((IUserAgentService) c.Resolve<IMicrosoftBandUserAgentService>());
      if (testService.IsEnabled)
        httpMessageHandler = httpMessageHandler.WithTestHeader(testService);
      IHealthDiscoveryClient healthDiscoveryClient = (IHealthDiscoveryClient) new HealthDiscoveryClient(httpMessageHandler, (Func<CancellationToken, Task<Uri>>) (t => Task.FromResult<Uri>(new Uri(config.AuthBaseUrl))));
      healthDiscoveryClient.Configuration.Tracing.AddTracingInterceptor(container.Resolve<IHttpTracer>("HealthDiscoveryClient"));
      return (object) healthDiscoveryClient;
    })));

    private static void RegisterPodClient(UnityContainer container) => container.RegisterType<IHealthCloudClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c =>
    {
      IConfig config = c.Resolve<IConfig>();
      IConnectionInfoProvider connectionInfoProvider = c.Resolve<IConnectionInfoProvider>();
      ITestConfigurationService testService = c.Resolve<ITestConfigurationService>();
      HttpMessageHandler httpMessageHandler = HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging().WithRequestId().WithPodAuthorization(connectionInfoProvider, true).WithCulture(c.Resolve<ICultureService>()).WithRegion(c.Resolve<IRegionService>()).WithUserAgent((IUserAgentService) c.Resolve<IMicrosoftBandUserAgentService>());
      if (testService.IsEnabled)
        httpMessageHandler = httpMessageHandler.WithTestHeader(testService);
      IHealthCloudClient healthCloudClient = (IHealthCloudClient) new HealthCloudClient(httpMessageHandler, (Func<CancellationToken, Task<Uri>>) (async cancellationToken => (await connectionInfoProvider.GetConnectionInfoAsync(cancellationToken, true).ConfigureAwait(false)).PodEndpoint), config.IsCachingEnabled ? c.Resolve<IHttpCacheService>() : (IHttpCacheService) null, (Func<CancellationToken, Task<Uri>>) (async cancellationToken => (await connectionInfoProvider.GetConnectionInfoAsync(cancellationToken, true).ConfigureAwait(false)).SocialServiceEndPoint), (Func<CancellationToken, Task<string>>) (async cancellationToken => (await connectionInfoProvider.GetConnectionInfoAsync(cancellationToken, true).ConfigureAwait(false)).PodSecurityToken));
      healthCloudClient.Configuration.Tracing.AddTracingInterceptor(container.Resolve<IHttpTracer>("HealthCloudClient"));
      return (object) healthCloudClient;
    })));

    private static void RegisterHealthAndFitnessClient(UnityContainer container, bool foreground) => container.RegisterType<IBingHealthAndFitnessClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c =>
    {
      IConfig config = c.Resolve<IConfig>();
      IConnectionInfoProvider connectionInfoProvider = c.Resolve<IConnectionInfoProvider>();
      ITestConfigurationService testService = c.Resolve<ITestConfigurationService>();
      HttpMessageHandler httpMessageHandler = HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging().WithPodAuthorization(connectionInfoProvider, false).WithRegion(c.Resolve<IRegionService>());
      if (testService.IsEnabled)
        httpMessageHandler = httpMessageHandler.WithTestHeader(testService);
      BingHealthAndFitnessClient andFitnessClient = new BingHealthAndFitnessClient(httpMessageHandler, c.Resolve<IConnectionInfoProvider>(), config.IsCachingEnabled & foreground ? c.Resolve<IHttpCacheService>() : (IHttpCacheService) null, c.Resolve<ICultureService>());
      andFitnessClient.Configuration.Tracing.AddTracingInterceptor(container.Resolve<IHttpTracer>("BingHealthAndFitness"));
      return (object) andFitnessClient;
    })));

    private static void RegisterLiveLoginClient(UnityContainer container) => container.RegisterType<ILiveLoginClient>((InjectionMember) new InjectionFactory((Func<IUnityContainer, object>) (c => (object) new LiveLoginClient(HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging()))));

    private static void RegisterOtherAppTypes(UnityContainer container)
    {
      container.RegisterType<ISyncInvalidator, ForegroundSyncInvalidator>(CoreIoc.Singleton);
      container.RegisterType<IApplicationThemeManager, ApplicationThemeManager>(CoreIoc.Singleton);
      container.RegisterType<IReportProblemStore, ReportProblemStore>(CoreIoc.Singleton);
      container.RegisterType<IMessageSender, MessageSender>(CoreIoc.Singleton);
    }

        private static LifetimeManager Singleton
        {
            get
            {
                return (LifetimeManager)new ContainerControlledLifetimeManager();
            }
        }
    }
}
