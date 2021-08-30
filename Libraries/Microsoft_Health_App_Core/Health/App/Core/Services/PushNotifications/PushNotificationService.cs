// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.PushNotifications.PushNotificationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Coaching;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.PushNotifications
{
  public sealed class PushNotificationService : IPushNotificationService
  {
    public static readonly ConfigurationValue<string> RegistrationId = ConfigurationValue.Create(nameof (PushNotificationService), nameof (RegistrationId), string.Empty);
    private const string PushNotificationServiceCategory = "PushNotificationService";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\PushNotifications\\PushNotificationService.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IEnvironmentService environmentService;
    private readonly IWorkoutsProvider workoutsProvider;
    private readonly IConfigurationService configurationService;
    private readonly ICloudNotificationUpdateService cloudNotificationUpdateService;
    private readonly ISmoothNavService navigationService;

    public PushNotificationService(
      IBandConnectionFactory cargoConnectionFactory,
      IHealthCloudClient healthCloudClient,
      IEnvironmentService environmentService,
      IWorkoutsProvider workoutsProvider,
      IConfigurationService configurationService,
      ICloudNotificationUpdateService cloudNotificationUpdateService,
      ISmoothNavService navigationService)
    {
      Assert.ParamIsNotNull((object) cargoConnectionFactory, nameof (cargoConnectionFactory));
      Assert.ParamIsNotNull((object) healthCloudClient, nameof (healthCloudClient));
      Assert.ParamIsNotNull((object) environmentService, nameof (environmentService));
      Assert.ParamIsNotNull((object) workoutsProvider, nameof (workoutsProvider));
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      Assert.ParamIsNotNull((object) cloudNotificationUpdateService, nameof (cloudNotificationUpdateService));
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.healthCloudClient = healthCloudClient;
      this.environmentService = environmentService;
      this.workoutsProvider = workoutsProvider;
      this.configurationService = configurationService;
      this.cloudNotificationUpdateService = cloudNotificationUpdateService;
      this.navigationService = navigationService;
    }

    public Task NotifyPushNotificationAsync(string content, CancellationToken token)
    {
      Assert.ParamIsNotNullOrEmpty(content, nameof (content));
      PushNotificationRaw content1 = JsonConvert.DeserializeObject<PushNotificationRaw>(content);
      PushNotificationService.Logger.Debug((object) "<FLAG> push notification : notification received");
      return this.NotifyPushNotificationAsync(content1, token);
    }

    public Task NotifyPushNotificationAsync(
      PushNotificationRaw content,
      CancellationToken token)
    {
      Assert.ParamIsNotNull((object) content, nameof (content));
      List<Task> taskList = new List<Task>();
      foreach (PushNotificationMessage notification in (IEnumerable<PushNotificationMessage>) content.Notifications)
      {
        if (notification.TypeId == PushNotificationMessageType.PhoneHome)
        {
          taskList.Add(this.cloudNotificationUpdateService.HandleAvailableUpdatesAsync(token));
          ApplicationTelemetry.PushNotificationReceived(notification.TypeId.ToString());
        }
        else
          throw new ArgumentException(string.Format(string.Format("The '{0}' push notification type is not supported.", new object[1]
          {
            (object) notification.TypeId
          })), nameof (content));
      }
      return Task.WhenAll((IEnumerable<Task>) taskList);
    }

    public void NotifyToastNotification(Uri navigationUri)
    {
      Assert.ParamIsNotNull((object) navigationUri, nameof (navigationUri));
      string scheme = navigationUri.Scheme;
      if (!(scheme == "tile"))
      {
        if (!(scheme == "modal"))
          return;
        this.NavigateToModalDialog(navigationUri);
      }
      else
        this.NavigateToTile(navigationUri);
    }

    public async Task RegisterChannelAsync(string channel, CancellationToken cancellationToken)
    {
      string clientId = this.environmentService.PhoneId;
      string registrationId = this.configurationService.GetValue<string>(PushNotificationService.RegistrationId);
      if (string.IsNullOrEmpty(registrationId))
      {
        string newValue = await this.healthCloudClient.RegisterPushNotificationChannelAsync(clientId, WebUtility.UrlEncode(channel), cancellationToken);
        this.configurationService.SetValue<string>((GenericConfigurationValue<string>) PushNotificationService.RegistrationId, newValue);
      }
      else
        await this.healthCloudClient.UpdatePushNotificationRegistrationAsync(clientId, WebUtility.UrlEncode(channel), registrationId, cancellationToken);
    }

    private void NavigateToTile(Uri navigationUri)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string authority = navigationUri.Authority;
      if (!(authority == "action_plan"))
      {
        if (authority == "socialchallenge")
        {
          str1 = "SocialTileViewModel";
          str2 = "SocialLandingViewModel";
        }
        else
          PushNotificationService.Logger.Debug((object) ("Toast template is not what is expected: " + navigationUri.Authority));
      }
      else
      {
        str1 = "CoachingTileViewModel";
        str2 = "CoachingComingUpViewModel";
      }
      if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
        return;
      IDictionary<string, string> arguments = (IDictionary<string, string>) new Dictionary<string, string>();
      arguments.Add(new KeyValuePair<string, string>("Tile", str1));
      arguments.Add(new KeyValuePair<string, string>("Pivot", str2));
      if (this.navigationService.CurrentJournalEntry.Arguments != null && !(this.navigationService.CurrentJournalEntry.Arguments["Tile"] != str1))
        return;
      this.navigationService.Navigate(typeof (TilesViewModel));
      this.navigationService.Navigate(typeof (TilesViewModel), arguments);
    }

    private void NavigateToModalDialog(Uri navigationUri)
    {
      if (navigationUri.Authority.Equals("GoalRecommendation", StringComparison.OrdinalIgnoreCase))
      {
        string str;
        if (!navigationUri.ParseQuery().TryGetValue("Type", out str) || !(str == HealthAppConstants.ModalDialogs.GoalRecommendationTypes.SleepPlanDuration.ToString()) || (object) this.navigationService.CurrentJournalEntry.ViewModelType == (object) typeof (SleepPlanRecommendationViewModel))
          return;
        this.navigationService.Navigate(typeof (SleepPlanRecommendationViewModel));
      }
      else
      {
        if (!navigationUri.Authority.Equals("SocialChallenge", StringComparison.OrdinalIgnoreCase))
          return;
        this.NavigateToTile(navigationUri);
      }
    }
  }
}
